import argparse
import cryptography  # PyInstaller: oracledb thin mode can goi goi ma hoa nay dong.
import getpass  # PyInstaller: oracledb nap dong module nay khi khoi tao.
import json
import math
import os
import re
import subprocess
import sys
import time
import traceback
import urllib.error
import urllib.request
from dataclasses import dataclass
from datetime import datetime, timezone
from pathlib import Path
from typing import Dict, Iterable, List, Optional, Sequence, Set, Tuple

# python-oracledb Thin Mode nap cac nhanh cryptography nay bang import dong.
# Import truc tiep de ban PyInstaller one-file dong goi du dependency.
from cryptography import x509
from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC


DEFAULT_GEOJSON_PATH = r"C:\Users\Admin\Downloads\vietnam-fir-VVHM-VVHN.geojson"
DATE_DISPLAY_FORMAT = "%d-%m-%Y"
WATERMARK_FORMAT = "%Y-%m-%d %H:%M:%S"
UNKNOWN_FLIGHT_TYPE = "OTHER"


@dataclass
class TrackCandidate:
    callsign: str
    flight_date: str
    update_text: str
    status: int
    lat: float
    lon: float
    time_in: str = ""
    time_out: str = ""
    first_status: int = 0


@dataclass
class FlightInfo:
    callsign: str
    flight_date: str
    from_airp: str
    to_airp: str
    etd: str
    eta: str
    permtype: str


@dataclass
class LogRow:
    callsign: str
    from_airp: str
    to_airp: str
    etd: str
    eta: str
    status: int
    flight_date: str
    updated_at_utc: str
    permtype: str
    lat: float
    lon: float
    time_in: str = ""
    time_out: str = ""
    first_status: int = 0


@dataclass
class FlightLookup:
    flights: Dict[Tuple[str, str], FlightInfo]
    unknown_keys: Set[Tuple[str, str]]
    skipped_keys: Set[Tuple[str, str]]


class MessageSink:
    def write(self, text: str) -> None:
        print(text)


def configure_console_encoding() -> None:
    """Tranh CLI bi dung khi Windows dang dung code page khong ho tro tieng Viet."""
    for stream in (sys.stdout, sys.stderr):
        reconfigure = getattr(stream, "reconfigure", None)
        if callable(reconfigure):
            try:
                reconfigure(encoding="utf-8", errors="replace")
            except (OSError, ValueError):
                pass


def app_root() -> Path:
    if getattr(sys, "frozen", False):
        return Path(sys.executable).resolve().parent
    return Path(__file__).resolve().parent


def project_root() -> Path:
    if getattr(sys, "frozen", False):
        # EXE duoc build vao <project>/tools/dist. Van ho tro ca cau truc
        # <project>/dist cu de co the tim App_Data khi nang cap tai cho.
        for candidate in (app_root().parent, app_root().parent.parent):
            if (candidate / "prjApplication").is_dir():
                return candidate
        return app_root().parent
    return Path(__file__).resolve().parents[2]


def config_candidates() -> List[Path]:
    env_path = os.environ.get("TRACKS_CONFIG")
    paths: List[Path] = []
    if env_path:
        paths.append(Path(env_path))
    paths.append(app_root() / "TracksSync.local.json")
    paths.append(project_root() / "prjApplication" / "App_Data" / "TracksSync.local.json")
    return paths


def load_config() -> Dict:
    cfg = {
        "postgres": {
            "host": os.environ.get("TRACKS_PG_HOST", ""),
            "port": int(os.environ.get("TRACKS_PG_PORT", "5432")),
            "database": os.environ.get("TRACKS_PG_DATABASE", "postgres"),
            "username": os.environ.get("TRACKS_PG_USERNAME", "postgres"),
            "password": os.environ.get("TRACKS_PG_PASSWORD", ""),
            "sslmode": os.environ.get("TRACKS_PG_SSLMODE", "prefer"),
            "connect_timeout": int(os.environ.get("TRACKS_PG_CONNECT_TIMEOUT", "10")),
        },
        "oracle": {
            "host": os.environ.get("TRACKS_ORA_HOST", ""),
            "port": int(os.environ.get("TRACKS_ORA_PORT", "1521")),
            "service_name": os.environ.get("TRACKS_ORA_SERVICE", "PDBORCL"),
            "username": os.environ.get("TRACKS_ORA_USERNAME", "atfm"),
            "password": os.environ.get("TRACKS_ORA_PASSWORD", ""),
            "flight_table": os.environ.get("TRACKS_ORA_FLIGHT_TABLE", "T_DAY_FLIGHTS_GOINGON"),
        },
        "fir": {"geojson_path": os.environ.get("TRACKS_FIR_GEOJSON", DEFAULT_GEOJSON_PATH)},
        "flight_tracking_api": {
            "host": os.environ.get("FLIGHT_API_HOST", "0.0.0.0"),
            "port": int(os.environ.get("FLIGHT_API_PORT", "5088")),
            "threads": 8,
            "max_lookback_days": 7,
        },
        "watermark_path": os.environ.get("TRACKS_WATERMARK", ""),
    }
    config_source = "environment/default"
    for path in config_candidates():
        if path.exists():
            with path.open("r", encoding="utf-8-sig") as handle:
                file_cfg = json.load(handle)
            deep_update(cfg, file_cfg)
            config_source = str(path.resolve())
            break
    if not cfg.get("fir", {}).get("geojson_path"):
        bundled_geojson = project_root() / "prjApplication" / "App_Data" / "vietnam-fir-VVHM-VVHN.geojson"
        cfg["fir"]["geojson_path"] = str(bundled_geojson) if bundled_geojson.exists() else DEFAULT_GEOJSON_PATH
    cfg["_config_source"] = config_source
    return cfg


def deep_update(target: Dict, source: Dict) -> None:
    for key, value in source.items():
        if isinstance(value, dict) and isinstance(target.get(key), dict):
            deep_update(target[key], value)
        else:
            target[key] = value


def watermark_path(cfg: Dict) -> Path:
    configured = cfg.get("watermark_path")
    if configured:
        return Path(configured)
    return app_root() / "TracksSync.watermark.log"


def read_watermark(cfg: Dict) -> Optional[datetime]:
    path = watermark_path(cfg)
    if not path.exists():
        return None
    text = path.read_text(encoding="utf-8").strip()
    if not text:
        return None
    for fmt in ("%Y-%m-%d %H:%M:%S.%f", WATERMARK_FORMAT, "%Y-%m-%dT%H:%M:%S"):
        try:
            return datetime.strptime(text, fmt)
        except ValueError:
            continue
    raise ValueError(f"Watermark khong dung dinh dang: {text}")


def write_watermark(cfg: Dict, value: Optional[datetime]) -> None:
    if value is None:
        return
    path = watermark_path(cfg)
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(value.strftime(WATERMARK_FORMAT), encoding="utf-8")


def normalize_callsign(value: object) -> str:
    raw = str(value or "").strip()
    if not raw:
        return ""
    return raw.split("-", 1)[0].strip().upper()


def parse_datetime(value: object) -> Optional[datetime]:
    if value is None:
        return None
    if isinstance(value, datetime):
        dt = value
    else:
        text = str(value).strip()
        if not text:
            return None
        text = text.replace("Z", "+00:00")
        try:
            dt = datetime.fromisoformat(text)
        except ValueError:
            for fmt in ("%Y-%m-%d %H:%M:%S.%f%z", "%Y-%m-%d %H:%M:%S%z", "%Y-%m-%d %H:%M:%S.%f", "%Y-%m-%d %H:%M:%S"):
                try:
                    dt = datetime.strptime(text, fmt)
                    break
                except ValueError:
                    dt = None
            if dt is None:
                raise
    if dt.tzinfo is not None:
        dt = dt.astimezone(timezone.utc).replace(tzinfo=None)
    return dt


def display_date(value: datetime) -> str:
    return value.strftime(DATE_DISPLAY_FORMAT)


def oracle_date_key(value: object) -> str:
    if isinstance(value, datetime):
        return display_date(value)
    text = str(value or "").strip()
    if not text:
        return ""
    for fmt in ("%d-%m-%Y", "%d/%m/%Y", "%Y-%m-%d %H:%M:%S", "%Y-%m-%d"):
        try:
            return datetime.strptime(text[:19] if "%H" in fmt else text[:10], fmt).strftime(DATE_DISPLAY_FORMAT)
        except ValueError:
            continue
    return text[:10]


def as_text(value: object) -> str:
    if value is None:
        return ""
    return str(value).strip()


def load_fir_polygons(path: str) -> Dict[str, List[List[List[Tuple[float, float]]]]]:
    geo_path = Path(path)
    if not geo_path.exists():
        raise FileNotFoundError(f"Khong tim thay file FIR geojson: {geo_path}")
    with geo_path.open("r", encoding="utf-8") as handle:
        data = json.load(handle)
    polygons: Dict[str, List[List[List[Tuple[float, float]]]]] = {"VVHN": [], "VVHM": []}
    for feature in data.get("features", []):
        props = feature.get("properties") or {}
        fir_id = str(props.get("id") or props.get("name") or "").upper()
        if "VVHN" in fir_id:
            target = "VVHN"
        elif "VVHM" in fir_id:
            target = "VVHM"
        else:
            continue
        geom = feature.get("geometry") or {}
        coords = geom.get("coordinates") or []
        if geom.get("type") == "Polygon":
            polygons[target].append(convert_polygon(coords))
        elif geom.get("type") == "MultiPolygon":
            polygons[target].extend(convert_polygon(poly) for poly in coords)
    if not polygons["VVHN"] or not polygons["VVHM"]:
        raise ValueError("GeoJSON phai co du 2 vung VVHN va VVHM.")
    return polygons


def convert_polygon(raw_polygon: Sequence) -> List[List[Tuple[float, float]]]:
    rings: List[List[Tuple[float, float]]] = []
    for ring in raw_polygon:
        rings.append([(float(point[0]), float(point[1])) for point in ring])
    return rings


def point_on_segment(point: Tuple[float, float], a: Tuple[float, float], b: Tuple[float, float], eps: float = 1e-9) -> bool:
    px, py = point
    ax, ay = a
    bx, by = b
    length_sq = (bx - ax) ** 2 + (by - ay) ** 2
    # GeoJSON co the lap hai dinh lien tiep. Doan co do dai 0 chi chua
    # chinh dinh do; neu khong xu ly rieng thi moi diem deu bi coi la tren bien.
    if length_sq <= eps * eps:
        return (px - ax) ** 2 + (py - ay) ** 2 <= eps * eps
    cross = (px - ax) * (by - ay) - (py - ay) * (bx - ax)
    if abs(cross) > eps:
        return False
    dot = (px - ax) * (bx - ax) + (py - ay) * (by - ay)
    if dot < -eps:
        return False
    return dot <= length_sq + eps


def point_in_ring(point: Tuple[float, float], ring: Sequence[Tuple[float, float]]) -> Tuple[bool, bool]:
    x, y = point
    inside = False
    for idx in range(len(ring)):
        a = ring[idx]
        b = ring[(idx + 1) % len(ring)]
        if point_on_segment(point, a, b):
            return True, True
        xi, yi = a
        xj, yj = b
        intersects = ((yi > y) != (yj > y)) and (x < (xj - xi) * (y - yi) / ((yj - yi) or 1e-30) + xi)
        if intersects:
            inside = not inside
    return inside, False


def point_in_polygon(point: Tuple[float, float], polygon: Sequence[Sequence[Tuple[float, float]]]) -> Tuple[bool, bool]:
    outer_inside, outer_boundary = point_in_ring(point, polygon[0])
    if outer_boundary:
        return True, True
    if not outer_inside:
        return False, False
    for hole in polygon[1:]:
        hole_inside, hole_boundary = point_in_ring(point, hole)
        if hole_boundary:
            return True, True
        if hole_inside:
            return False, False
    return True, False


def classify_fir(lat: object, lon: object, polygons: Dict[str, List[List[List[Tuple[float, float]]]]]) -> Optional[int]:
    if lat is None or lon is None:
        return None
    point = (float(lon), float(lat))
    hits: Dict[str, bool] = {}
    boundaries: Dict[str, bool] = {}
    for fir_id, fir_polygons in polygons.items():
        hits[fir_id] = False
        boundaries[fir_id] = False
        for polygon in fir_polygons:
            inside, boundary = point_in_polygon(point, polygon)
            if inside:
                hits[fir_id] = True
            if boundary:
                boundaries[fir_id] = True
            if hits[fir_id] and boundaries[fir_id]:
                break
    if boundaries.get("VVHN") or (hits.get("VVHN") and hits.get("VVHM")):
        return 1
    if boundaries.get("VVHM"):
        return 2
    if hits.get("VVHN"):
        return 1
    if hits.get("VVHM"):
        return 2
    return None


def import_postgres():
    try:
        import psycopg

        return psycopg
    except ImportError:
        pass
    try:
        import psycopg2
    except ImportError as exc:
        raise RuntimeError("Thieu thu vien psycopg hoac psycopg2/psycopg2-binary de ket noi PostgreSQL.") from exc
    return psycopg2


def import_oracle():
    try:
        import oracledb

        return oracledb
    except ImportError:
        try:
            import cx_Oracle

            return cx_Oracle
        except ImportError as exc:
            raise RuntimeError("Thieu thu vien oracledb hoac cx_Oracle de ket noi Oracle.") from exc


def pg_connect(cfg: Dict):
    psycopg2 = import_postgres()
    pg = cfg["postgres"]
    if not pg.get("host") or not (pg.get("username") or pg.get("user")) or not pg.get("password"):
        raise RuntimeError(
            "Cấu hình PostgreSQL chưa đầy đủ (host/username/password). "
            "Hãy đặt TracksSync.local.json cạnh file EXE hoặc trong prjApplication/App_Data."
        )
    return psycopg2.connect(
        host=pg.get("host"),
        port=int(pg.get("port", 5432)),
        dbname=pg.get("database"),
        user=pg.get("username") or pg.get("user"),
        password=pg.get("password"),
        sslmode=pg.get("sslmode", "prefer"),
        connect_timeout=int(pg.get("connect_timeout", 10)),
    )


def oracle_connect(cfg: Dict):
    db = import_oracle()
    ora = cfg["oracle"]
    user = ora.get("username") or ora.get("user") or "atfm"
    password = ora.get("password") or ""
    if ora.get("connection_string"):
        text = ora["connection_string"]
        lower = text.lower()
        if "user id=" in lower and "password=" in lower:
            user = extract_conn_part(text, "User Id") or user
            password = extract_conn_part(text, "Password") or password
            dsn = extract_data_source(text)
            return db.connect(user=user, password=password, dsn=dsn)
        return db.connect(text)
    if hasattr(db, "makedsn"):
        dsn = db.makedsn(ora.get("host"), int(ora.get("port", 1521)), service_name=ora.get("service_name", "PDBORCL"))
    else:
        dsn = f"{ora.get('host')}:{ora.get('port', 1521)}/{ora.get('service_name', 'PDBORCL')}"
    return db.connect(user=user, password=password, dsn=dsn)


def extract_conn_part(text: str, key: str) -> str:
    marker = key.lower() + "="
    for part in text.split(";"):
        if part.strip().lower().startswith(marker):
            return part.split("=", 1)[1].strip()
    return ""


def extract_data_source(text: str) -> str:
    key = "data source="
    lower = text.lower()
    start = lower.find(key)
    if start < 0:
        return text
    start += len(key)
    end = lower.find(";user", start)
    if end < 0:
        end = len(text)
    return text[start:end].strip()


def flight_api_base_url(cfg: Dict) -> str:
    api = cfg.get("flight_tracking_api", {})
    host = str(api.get("host") or "0.0.0.0").strip()
    if host in ("0.0.0.0", "::", "[::]", "localhost"):
        host = "127.0.0.1"
    return "http://%s:%s" % (host, int(api.get("port", 5088)))


def request_api_json(url: str, timeout: float = 3.0) -> Dict:
    try:
        with urllib.request.urlopen(url, timeout=timeout) as response:
            content = response.read().decode("utf-8")
            return json.loads(content)
    except urllib.error.HTTPError as exc:
        try:
            content = exc.read().decode("utf-8", errors="replace")
        except Exception:
            content = ""
        raise RuntimeError("API tra HTTP %s: %s" % (exc.code, content[:300])) from exc
    except (OSError, ValueError) as exc:
        raise RuntimeError("Khong goi duoc %s: %s" % (url, exc)) from exc


def flight_api_is_running(cfg: Dict, timeout: float = 1.5) -> bool:
    try:
        result = request_api_json(flight_api_base_url(cfg) + "/health/live", timeout=timeout)
        return str(result.get("status", "")).lower() == "ok"
    except Exception:
        return False


def flight_api_command(cfg: Dict) -> List[str]:
    config_source = str(cfg.get("_config_source") or "")
    config_args = ["--config", config_source] if config_source and Path(config_source).is_file() else []
    if getattr(sys, "frozen", False):
        executable = app_root() / "ATFM-FlightTrackingApi.exe"
        if not executable.is_file():
            raise RuntimeError("Khong tim thay ATFM-FlightTrackingApi.exe canh ATFM-TracksSync.exe.")
        return [str(executable)] + config_args

    script = project_root() / "tools" / "FlightTrackingApiPython" / "main.py"
    if not script.is_file():
        raise RuntimeError("Khong tim thay tools/FlightTrackingApiPython/main.py.")
    return [sys.executable, str(script)] + config_args


def start_flight_api_hidden(cfg: Dict, sink: Optional[MessageSink] = None, wait_seconds: float = 12.0) -> bool:
    sink = sink or MessageSink()
    base_url = flight_api_base_url(cfg)
    if flight_api_is_running(cfg):
        sink.write("Flight Tracking API da chay tai %s." % base_url)
        return False

    command = flight_api_command(cfg)
    creation_flags = 0
    startup_info = None
    if os.name == "nt":
        creation_flags = subprocess.CREATE_NO_WINDOW | subprocess.CREATE_NEW_PROCESS_GROUP
        startup_info = subprocess.STARTUPINFO()
        startup_info.dwFlags |= subprocess.STARTF_USESHOWWINDOW
        startup_info.wShowWindow = 0

    process = subprocess.Popen(
        command,
        cwd=str(app_root()),
        env=os.environ.copy(),
        stdin=subprocess.DEVNULL,
        stdout=subprocess.DEVNULL,
        stderr=subprocess.DEVNULL,
        close_fds=True,
        creationflags=creation_flags,
        startupinfo=startup_info,
    )
    deadline = time.monotonic() + wait_seconds
    while time.monotonic() < deadline:
        if flight_api_is_running(cfg, timeout=0.5):
            sink.write("Da khoi dong ngam Flight Tracking API tai %s (PID %s)." % (base_url, process.pid))
            return True
        exit_code = process.poll()
        if exit_code is not None:
            raise RuntimeError("Flight Tracking API dung khi khoi dong (exit code %s)." % exit_code)
        time.sleep(0.25)
    raise RuntimeError("Flight Tracking API khong san sang sau %.0f giay." % wait_seconds)


def test_configuration(sink: Optional[MessageSink] = None) -> None:
    sink = sink or MessageSink()
    cfg = load_config()
    sink.write("========== TEST CAU HINH ==========")
    sink.write("File cau hinh chung: %s" % cfg.get("_config_source", "environment/default"))

    with pg_connect(cfg) as connection:
        with connection.cursor() as cursor:
            cursor.execute("SELECT 1")
            cursor.fetchone()
    sink.write("[OK] PostgreSQL %s:%s/%s" % (
        cfg["postgres"].get("host"),
        cfg["postgres"].get("port", 5432),
        cfg["postgres"].get("database"),
    ))

    with oracle_connect(cfg) as connection:
        cursor = connection.cursor()
        try:
            cursor.execute("SELECT 1 FROM DUAL")
            cursor.fetchone()
        finally:
            cursor.close()
    sink.write("[OK] Oracle va thong tin dang nhap.")

    geojson_path = cfg.get("fir", {}).get("geojson_path")
    load_fir_polygons(geojson_path)
    sink.write("[OK] GeoJSON FIR: %s" % geojson_path)

    start_flight_api_hidden(cfg, sink=sink)
    ready = request_api_json(flight_api_base_url(cfg) + "/health/ready", timeout=12)
    if str(ready.get("status", "")).lower() != "ready":
        raise RuntimeError("Flight Tracking API chua ready: %s" % ready)
    sink.write("[OK] Flight Tracking API: %s" % flight_api_base_url(cfg))
    sink.write("Tat ca cau hinh deu hop le.")


def read_tracks(cfg: Dict, polygons: Dict, sink: MessageSink, full: bool = False) -> Tuple[List[TrackCandidate], Optional[datetime]]:
    since = None if full else read_watermark(cfg)
    rows: List[TrackCandidate] = []
    max_updated: Optional[datetime] = since
    query = (
        "SELECT flight_id_current, NULLIF(TRIM(updated_at_utc), '')::timestamp, last_lat, last_lon "
        "FROM public.tracks "
        "WHERE last_lat IS NOT NULL AND last_lon IS NOT NULL "
    )
    params: List[object] = []
    if since is not None:
        query += "AND NULLIF(TRIM(updated_at_utc), '')::timestamp > %s "
        params.append(since)
    query += "ORDER BY NULLIF(TRIM(updated_at_utc), '')::timestamp"
    with pg_connect(cfg) as conn:
        with conn.cursor() as cur:
            cur.execute(query, params)
            for flight_id, updated_at, lat, lon in cur:
                updated_dt = parse_datetime(updated_at)
                if updated_dt is None:
                    continue
                max_updated = updated_dt if max_updated is None or updated_dt > max_updated else max_updated
                status = classify_fir(lat, lon, polygons)
                if status is None:
                    continue
                callsign = normalize_callsign(flight_id)
                if not callsign:
                    continue
                rows.append(
                    TrackCandidate(
                        callsign=callsign,
                        flight_date=display_date(updated_dt),
                        update_text=updated_dt.strftime(WATERMARK_FORMAT),
                        status=status,
                        lat=float(lat),
                        lon=float(lon),
                        time_in=updated_dt.strftime(WATERMARK_FORMAT),
                        first_status=status,
                    )
                )
    sink.write(f"Doc public.tracks: {len(rows)} dong nam trong FIR can doi chieu.")
    return dedupe_tracks(rows), max_updated


def dedupe_tracks(rows: Iterable[TrackCandidate]) -> List[TrackCandidate]:
    grouped: Dict[Tuple[str, str], List[TrackCandidate]] = {}
    for row in rows:
        grouped.setdefault((row.callsign, row.flight_date), []).append(row)
    result: List[TrackCandidate] = []
    for group in grouped.values():
        ordered = sorted(group, key=lambda item: item.update_text)
        latest = ordered[-1]
        first = ordered[0]
        time_out = ""
        for item in ordered[1:]:
            if item.status != first.status:
                time_out = item.update_text
                break
        result.append(
            TrackCandidate(
                callsign=latest.callsign,
                flight_date=latest.flight_date,
                update_text=latest.update_text,
                status=latest.status,
                lat=latest.lat,
                lon=latest.lon,
                time_in=first.time_in or first.update_text,
                time_out=time_out,
                first_status=first.status,
            )
        )
    return result


def ensure_schema(conn) -> None:
    cur = conn.cursor()
    try:
        cur.execute(
            """
            BEGIN
                EXECUTE IMMEDIATE 'CREATE TABLE T_TRACKS_LOG (
                    TRLOG_ID NUMBER PRIMARY KEY,
                    CALLSIGN VARCHAR2(50),
                    FROM_AIRP VARCHAR2(20),
                    TO_AIRP VARCHAR2(20),
                    ETD VARCHAR2(20),
                    ETA VARCHAR2(20),
                    STATUS NUMBER(1),
                    "DATE" VARCHAR2(10),
                    UPDATED_AT_UTC VARCHAR2(19),
                    PERMTYPE VARCHAR2(50),
                    TIME_IN VARCHAR2(19),
                    TIME_OUT VARCHAR2(19),
                    LAT NUMBER NOT NULL,
                    LON NUMBER NOT NULL
                )';
            EXCEPTION
                WHEN OTHERS THEN
                    IF SQLCODE != -955 THEN RAISE; END IF;
            END;
            """
        )
        cur.execute(
            """
            BEGIN
                EXECUTE IMMEDIATE 'CREATE SEQUENCE T_TRACKS_LOG_SEQ START WITH 1 INCREMENT BY 1 NOCACHE';
            EXCEPTION
                WHEN OTHERS THEN
                    IF SQLCODE != -955 THEN RAISE; END IF;
            END;
            """
        )
        cur.execute(
            """
            BEGIN
                EXECUTE IMMEDIATE 'CREATE OR REPLACE TRIGGER T_TRACKS_LOG_BI
                BEFORE INSERT ON T_TRACKS_LOG
                FOR EACH ROW
                WHEN (NEW.TRLOG_ID IS NULL)
                BEGIN
                    SELECT T_TRACKS_LOG_SEQ.NEXTVAL INTO :NEW.TRLOG_ID FROM DUAL;
                END;';
            END;
            """
        )
        add_column_if_missing(cur, "T_TRACKS_LOG", "DATE", '"DATE" VARCHAR2(10)')
        add_column_if_missing(cur, "T_TRACKS_LOG", "UPDATED_AT_UTC", "UPDATED_AT_UTC VARCHAR2(19)")
        add_column_if_missing(cur, "T_TRACKS_LOG", "PERMTYPE", "PERMTYPE VARCHAR2(50)")
        add_column_if_missing(cur, "T_TRACKS_LOG", "TIME_IN", "TIME_IN VARCHAR2(19)")
        add_column_if_missing(cur, "T_TRACKS_LOG", "TIME_OUT", "TIME_OUT VARCHAR2(19)")
        add_column_if_missing(cur, "T_TRACKS_LOG", "LAT", "LAT NUMBER")
        add_column_if_missing(cur, "T_TRACKS_LOG", "LON", "LON NUMBER")
        ensure_unique_log_key(cur)
        conn.commit()
    finally:
        cur.close()


def add_column_if_missing(cur, table_name: str, column_name: str, ddl: str) -> None:
    cur.execute(
        "SELECT COUNT(*) FROM USER_TAB_COLUMNS WHERE TABLE_NAME = :table_name AND COLUMN_NAME = :column_name",
        {"table_name": table_name.upper(), "column_name": column_name.upper()},
    )
    if int(cur.fetchone()[0]) == 0:
        cur.execute(f"ALTER TABLE {table_name} ADD ({ddl})")


def enforce_coordinate_constraints(conn) -> int:
    cur = conn.cursor()
    try:
        cur.execute("SELECT COUNT(*) FROM T_TRACKS_LOG WHERE LAT IS NULL OR LON IS NULL")
        remaining_null = int(cur.fetchone()[0])
        if remaining_null:
            return remaining_null
        cur.execute(
            """
            SELECT COLUMN_NAME, NULLABLE
            FROM USER_TAB_COLUMNS
            WHERE TABLE_NAME = 'T_TRACKS_LOG'
              AND COLUMN_NAME IN ('LAT', 'LON')
            """
        )
        nullable_columns = sorted(
            column_name
            for column_name, nullable in cur.fetchall()
            if nullable == "Y" and column_name in ("LAT", "LON")
        )
        if nullable_columns:
            definitions = ", ".join(f"{column_name} NOT NULL" for column_name in nullable_columns)
            cur.execute(f"ALTER TABLE T_TRACKS_LOG MODIFY ({definitions})")
            conn.commit()
        return 0
    finally:
        cur.close()


def ensure_unique_log_key(cur) -> None:
    cur.execute(
        "SELECT COUNT(*) FROM USER_INDEXES WHERE INDEX_NAME = 'UX_TRACKS_LOG_CALL_DATE'"
    )
    if int(cur.fetchone()[0]) > 0:
        return
    cur.execute(
        '''
        SELECT COUNT(*)
        FROM (
            SELECT CALLSIGN, "DATE"
            FROM T_TRACKS_LOG
            GROUP BY CALLSIGN, "DATE"
            HAVING COUNT(*) > 1
        )
        '''
    )
    duplicate_count = int(cur.fetchone()[0])
    if duplicate_count:
        raise RuntimeError(
            f"T_TRACKS_LOG co {duplicate_count} khoa CALLSIGN/DATE bi trung; "
            "can xu ly du lieu trung truoc khi tao unique index."
        )
    cur.execute(
        'CREATE UNIQUE INDEX UX_TRACKS_LOG_CALL_DATE ON T_TRACKS_LOG (CALLSIGN, "DATE")'
    )


def read_oracle_flights(
    conn,
    cfg: Dict,
    keys: Sequence[Tuple[str, str]],
    sink: MessageSink,
    track_times: Dict[Tuple[str, str], datetime],
) -> FlightLookup:
    if not keys:
        return FlightLookup({}, set(), set())
    table = cfg.get("oracle", {}).get("flight_table", "T_DAY_FLIGHTS_GOINGON")
    result: Dict[Tuple[str, str], FlightInfo] = {}
    seen_keys: Set[Tuple[str, str]] = set()
    skipped_keys: Set[Tuple[str, str]] = set()
    grouped: Dict[str, List[str]] = {}
    for callsign, flight_date in keys:
        grouped.setdefault(flight_date, []).append(callsign)
    collected: Dict[Tuple[str, str], List[FlightInfo]] = {}
    invalid_route_count = 0
    cur = conn.cursor()
    try:
        for flight_date, callsigns in grouped.items():
            unique_callsigns = sorted(set(callsigns))
            for chunk in chunks(unique_callsigns, 800):
                binds = {f"c{i}": value for i, value in enumerate(chunk)}
                binds["flight_date"] = flight_date
                in_clause = ", ".join(f":c{i}" for i in range(len(chunk)))
                cur.execute(
                    f"""
                    SELECT FLIGHTNBR, FLIGHTDATE, FROM_AIRP, TO_AIRP, ETD, ETA, PERMTYPE
                    FROM {table}
                    WHERE TRUNC(FLIGHTDATE) = TO_DATE(:flight_date, 'DD-MM-YYYY')
                      AND UPPER(TRIM(FLIGHTNBR)) IN ({in_clause})
                    ORDER BY FLIGHTNBR, FLIGHTDATE DESC
                    """,
                    binds,
                )
                for match in cur.fetchall():
                    flight = row_to_flight(match)
                    key = (flight.callsign, flight.flight_date)
                    seen_keys.add(key)
                    if not is_flight_route_valid(flight):
                        invalid_route_count += 1
                        flight = FlightInfo(
                            callsign=flight.callsign,
                            flight_date=flight.flight_date,
                            from_airp=flight.from_airp,
                            to_airp=flight.to_airp,
                            etd=flight.etd,
                            eta=flight.eta,
                            permtype=UNKNOWN_FLIGHT_TYPE,
                        )
                    collected.setdefault(key, []).append(flight)
        if invalid_route_count:
            sink.write(
                f"Danh dau {invalid_route_count} dong thieu FROM_AIRP hoac TO_AIRP "
                f"la {UNKNOWN_FLIGHT_TYPE} (chuyen bay khac)."
            )
        ambiguous_examples: List[str] = []
        skipped_duplicate_count = 0
        resolved_by_time = 0
        for key in keys:
            matches = collected.get(key, [])
            if not matches:
                if key in seen_keys:
                    skipped_keys.add(key)
                continue
            if len(matches) == 1:
                result[key] = matches[0]
            else:
                matching = [
                    flight
                    for flight in matches
                    if flight_time_contains(track_times.get(key), flight)
                ]
                if len(matching) == 1:
                    result[key] = matching[0]
                    resolved_by_time += 1
                    continue
                skipped_keys.add(key)
                skipped_duplicate_count += 1
                if len(ambiguous_examples) < 20:
                    reason = "khong co khoang ETD-ETA phu hop" if not matching else "nhieu khoang ETD-ETA cung phu hop"
                    ambiguous_examples.append(f"{key[0]}/{key[1]} ({len(matches)} dong: {reason})")
        if resolved_by_time:
            sink.write(f"Da chon {resolved_by_time} dong trung theo gio updated_at_utc nam trong ETD-ETA.")
        if ambiguous_examples:
            sink.write(
                "Bo qua "
                f"{skipped_duplicate_count} callsign/ngay khong chon duoc duy nhat trong T_DAY_FLIGHTS_GOINGON; "
                "public.tracks khong co khoang ETD-ETA phu hop hoac bi chong lan."
            )
            for item in ambiguous_examples:
                sink.write(f"  - {item}")
    finally:
        cur.close()
    unknown_keys = set(keys) - seen_keys
    if unknown_keys:
        sink.write(
            f"Khong tim thay {len(unknown_keys)} callsign/ngay trong T_DAY_FLIGHTS_GOINGON; "
            f"se ghi {UNKNOWN_FLIGHT_TYPE} (chuyen bay khac)."
        )
    if skipped_keys:
        sink.write(f"Bo qua {len(skipped_keys)} callsign/ngay co du lieu nhung khong xac dinh duoc chuyen bay.")
    return FlightLookup(result, unknown_keys, skipped_keys)


def chunks(values: Sequence[str], size: int) -> Iterable[Sequence[str]]:
    for start in range(0, len(values), size):
        yield values[start : start + size]


def is_flight_route_valid(flight: FlightInfo) -> bool:
    return bool((flight.from_airp or "").strip() and (flight.to_airp or "").strip())


def flight_time_minutes(value: object) -> Optional[int]:
    text = as_text(value).upper().replace("UTC", "").strip().rstrip("+")
    if not text:
        return None
    match = re.fullmatch(r"(\d{1,2}):?(\d{2})(?::\d{2})?", text)
    if not match:
        return None
    hour, minute = int(match.group(1)), int(match.group(2))
    if hour > 23 or minute > 59:
        return None
    return hour * 60 + minute


def flight_time_contains(updated_at: Optional[datetime], flight: FlightInfo) -> bool:
    if updated_at is None:
        return False
    etd = flight_time_minutes(flight.etd)
    eta = flight_time_minutes(flight.eta)
    if etd is None or eta is None:
        return False
    current = updated_at.hour * 60 + updated_at.minute
    if etd <= eta:
        return etd <= current <= eta
    return current >= etd or current <= eta


def unknown_flight(candidate: TrackCandidate) -> FlightInfo:
    return FlightInfo(
        callsign=candidate.callsign,
        flight_date=candidate.flight_date,
        from_airp="",
        to_airp="",
        etd="",
        eta="",
        permtype=UNKNOWN_FLIGHT_TYPE,
    )


def row_to_flight(row: Sequence[object]) -> FlightInfo:
    return FlightInfo(
        callsign=normalize_callsign(row[0]),
        flight_date=oracle_date_key(row[1]),
        from_airp=as_text(row[2]),
        to_airp=as_text(row[3]),
        etd=as_text(row[4]),
        eta=as_text(row[5]),
        permtype=as_text(row[6]),
    )


def read_existing_log_keys(conn, rows: Sequence[LogRow]) -> Set[Tuple[str, str]]:
    requested = {(row.callsign, row.flight_date) for row in rows}
    if not requested:
        return set()
    cur = conn.cursor()
    try:
        cur.execute('SELECT CALLSIGN, "DATE" FROM T_TRACKS_LOG')
        return {
            (as_text(callsign), as_text(flight_date))
            for callsign, flight_date in cur.fetchall()
        } & requested
    finally:
        cur.close()


def merge_logs(conn, rows: Sequence[LogRow]) -> int:
    if not rows:
        return 0
    sql = """
        MERGE INTO T_TRACKS_LOG target
        USING (
            SELECT :callsign CALLSIGN,
                   :flight_date "DATE",
                   :from_airp FROM_AIRP,
                   :to_airp TO_AIRP,
                   :etd ETD,
                   :eta ETA,
                   :status STATUS,
                   :updated_at_utc UPDATED_AT_UTC,
                   :permtype PERMTYPE,
                   :time_in TIME_IN,
                   :time_out TIME_OUT,
                   :first_status FIRST_STATUS,
                   :lat LAT,
                   :lon LON
            FROM DUAL
        ) source
        ON (
            target.CALLSIGN = source.CALLSIGN
            AND target."DATE" = source."DATE"
        )
        WHEN MATCHED THEN UPDATE SET
            target.LAT = source.LAT,
            target.LON = source.LON,
            target.UPDATED_AT_UTC = source.UPDATED_AT_UTC,
            target.FROM_AIRP = CASE WHEN NVL(UPPER(TRIM(target.PERMTYPE)), 'OTHER') = 'OTHER' AND source.PERMTYPE <> 'OTHER' THEN source.FROM_AIRP ELSE target.FROM_AIRP END,
            target.TO_AIRP = CASE WHEN NVL(UPPER(TRIM(target.PERMTYPE)), 'OTHER') = 'OTHER' AND source.PERMTYPE <> 'OTHER' THEN source.TO_AIRP ELSE target.TO_AIRP END,
            target.ETD = CASE WHEN NVL(UPPER(TRIM(target.PERMTYPE)), 'OTHER') = 'OTHER' AND source.PERMTYPE <> 'OTHER' THEN source.ETD ELSE target.ETD END,
            target.ETA = CASE WHEN NVL(UPPER(TRIM(target.PERMTYPE)), 'OTHER') = 'OTHER' AND source.PERMTYPE <> 'OTHER' THEN source.ETA ELSE target.ETA END,
            target.STATUS = CASE WHEN NVL(UPPER(TRIM(target.PERMTYPE)), 'OTHER') = 'OTHER' AND source.PERMTYPE <> 'OTHER' THEN source.STATUS ELSE target.STATUS END,
            target.PERMTYPE = CASE WHEN NVL(UPPER(TRIM(target.PERMTYPE)), 'OTHER') = 'OTHER' AND source.PERMTYPE <> 'OTHER' THEN source.PERMTYPE ELSE target.PERMTYPE END,
            target.TIME_IN = CASE
                WHEN target.TIME_IN IS NULL OR source.TIME_IN < target.TIME_IN THEN source.TIME_IN
                ELSE target.TIME_IN
            END,
            target.TIME_OUT = CASE
                WHEN target.TIME_OUT IS NOT NULL THEN target.TIME_OUT
                WHEN source.TIME_OUT IS NOT NULL THEN source.TIME_OUT
                WHEN target.STATUS IS NOT NULL AND source.FIRST_STATUS <> target.STATUS THEN source.TIME_IN
                ELSE target.TIME_OUT
            END
        WHEN NOT MATCHED THEN INSERT
            (TRLOG_ID, CALLSIGN, FROM_AIRP, TO_AIRP, ETD, ETA, STATUS, "DATE", UPDATED_AT_UTC, PERMTYPE, LAT, LON, TIME_IN, TIME_OUT)
        VALUES
            (T_TRACKS_LOG_SEQ.NEXTVAL, source.CALLSIGN, source.FROM_AIRP, source.TO_AIRP, source.ETD, source.ETA,
             source.STATUS, source."DATE", source.UPDATED_AT_UTC, source.PERMTYPE, source.LAT, source.LON,
             source.TIME_IN, source.TIME_OUT)
    """
    cur = conn.cursor()
    try:
        data = [
            {
                "callsign": row.callsign,
                "flight_date": row.flight_date,
                "from_airp": row.from_airp,
                "to_airp": row.to_airp,
                "etd": row.etd,
                "eta": row.eta,
                "status": row.status,
                "updated_at_utc": row.updated_at_utc,
                "permtype": row.permtype,
                "time_in": row.time_in,
                "time_out": row.time_out,
                "first_status": row.first_status,
                "lat": row.lat,
                "lon": row.lon,
            }
            for row in rows
        ]
        cur.executemany(sql, data)
        conn.commit()
        return len(rows)
    finally:
        cur.close()


def build_log_rows(
    candidates: Sequence[TrackCandidate],
    flights: Dict[Tuple[str, str], FlightInfo],
    unknown_keys: Optional[Set[Tuple[str, str]]] = None,
) -> List[LogRow]:
    unknown_keys = unknown_keys or set()
    rows: List[LogRow] = []
    for item in candidates:
        key = (item.callsign, item.flight_date)
        flight = flights.get(key)
        if flight is None and key in unknown_keys:
            flight = unknown_flight(item)
        if flight is None:
            continue
        rows.append(
            LogRow(
                callsign=flight.callsign,
                from_airp=flight.from_airp,
                to_airp=flight.to_airp,
                etd=flight.etd,
                eta=flight.eta,
                status=item.status,
                flight_date=item.flight_date,
                updated_at_utc=item.update_text,
                permtype=flight.permtype,
                lat=item.lat,
                lon=item.lon,
                time_in=item.time_in or item.update_text,
                time_out=item.time_out,
                first_status=item.first_status or item.status,
            )
        )
    return rows


def execute(mode: str, full: bool = False, sink: Optional[MessageSink] = None) -> None:
    sink = sink or MessageSink()
    cfg = load_config()
    pg = cfg.get("postgres", {})
    sink.write(f"Cấu hình: {cfg.get('_config_source', 'environment/default')}")
    sink.write(
        "PostgreSQL: %s:%s/%s"
        % (pg.get("host") or "(trống)", pg.get("port", 5432), pg.get("database") or "(trống)")
    )
    polygons = load_fir_polygons(cfg["fir"]["geojson_path"])
    sink.write("Dang doc tracks va phan vung FIR...")
    candidates, max_updated = read_tracks(cfg, polygons, sink, full=full)
    keys = sorted({(row.callsign, row.flight_date) for row in candidates})
    track_times = {
        (row.callsign, row.flight_date): parse_datetime(row.update_text)
        for row in candidates
    }
    sink.write(f"Can doi chieu Oracle: {len(keys)} callsign/ngay.")
    with oracle_connect(cfg) as conn:
        ensure_schema(conn)
        lookup = read_oracle_flights(conn, cfg, keys, sink, track_times)
        rows = build_log_rows(candidates, lookup.flights, lookup.unknown_keys)
        sink.write(
            f"Match T_DAY_FLIGHTS_GOINGON: {len(lookup.flights)}; "
            f"khong match: {len(lookup.unknown_keys)}; bo qua: {len(lookup.skipped_keys)}."
        )
        if mode == "check":
            sink.write(f"Kiem tra xong. Neu ghi se them moi/cap nhat toa do {len(rows)} dong T_TRACKS_LOG.")
        elif mode == "sync":
            written = merge_logs(conn, rows)
            if full:
                remaining_null = enforce_coordinate_constraints(conn)
                if remaining_null:
                    sink.write(
                        f"Con {remaining_null} dong log cu thieu LAT/LON; giu nguyen de xu ly co chu dich."
                    )
                else:
                    sink.write("LAT/LON da day du va dat NOT NULL.")
            write_watermark(cfg, max_updated)
            sink.write(f"Da them moi/cap nhat toa do {written} dong T_TRACKS_LOG.")
            if max_updated:
                sink.write(f"Watermark moi: {max_updated.strftime(WATERMARK_FORMAT)}")
        elif mode == "verify":
            verify_logs(conn, rows, sink)
        elif mode == "all":
            sink.write(f"[1/3] Kiem tra doi chieu xong: {len(rows)} dong hop le.")
            existing_keys = read_existing_log_keys(conn, rows)
            written = merge_logs(conn, rows)
            sink.write(f"[2/3] Da them moi/cap nhat toa do {written} dong T_TRACKS_LOG.")
            if full:
                remaining_null = enforce_coordinate_constraints(conn)
                if remaining_null:
                    sink.write(
                        f"Con {remaining_null} dong log cu thieu LAT/LON; giu nguyen de xu ly co chu dich."
                    )
                else:
                    sink.write("LAT/LON da day du va dat NOT NULL.")
            inserted_keys = {(row.callsign, row.flight_date) for row in rows} - existing_keys
            verify_logs(conn, rows, sink, report_extra=False, full_row_keys=inserted_keys)
            sink.write("[3/3] Da xac minh cac dong trong chu ky.")
            write_watermark(cfg, max_updated)
            if max_updated:
                sink.write(f"Watermark moi: {max_updated.strftime(WATERMARK_FORMAT)}")
        else:
            raise ValueError(f"Mode khong hop le: {mode}")


def verify_logs(
    conn,
    expected: Sequence[LogRow],
    sink: MessageSink,
    report_extra: bool = True,
    full_row_keys: Optional[Set[Tuple[str, str]]] = None,
) -> None:
    if not expected:
        sink.write("Khong co dong du kien de xac minh.")
        return
    expected_map = {
        (row.callsign, row.flight_date): row
        for row in expected
    }
    full_row_keys = full_row_keys or set()
    actual_map: Dict[Tuple[str, str], Tuple[object, ...]] = {}
    duplicate_count = 0
    cur = conn.cursor()
    try:
        cur.execute(
            '''
            SELECT CALLSIGN, "DATE", FROM_AIRP, TO_AIRP, ETD, ETA,
                   STATUS, UPDATED_AT_UTC, PERMTYPE, TIME_IN, TIME_OUT, LAT, LON
            FROM T_TRACKS_LOG
            '''
        )
        for found in cur.fetchall():
            key = (normalize_callsign(found[0]), as_text(found[1]))
            if key in actual_map:
                duplicate_count += 1
            actual_map[key] = tuple(found[2:])
    finally:
        cur.close()

    expected_keys = set(expected_map)
    actual_keys = set(actual_map)
    missing_keys = expected_keys - actual_keys
    extra_keys = actual_keys - expected_keys if report_extra else set()
    common_keys = expected_keys & actual_keys
    mismatch_keys = {
        key for key in common_keys
        if not log_row_matches(expected_map[key], actual_map[key], key in full_row_keys)
    }
    ok = len(common_keys) - len(mismatch_keys)
    sink.write(
        "Xac minh T_TRACKS_LOG: "
        f"dung {ok}, sai {len(mismatch_keys)}, thieu {len(missing_keys)}, "
        f"thua {len(extra_keys)}, trung khoa {duplicate_count}."
    )


def as_optional_float(value: object) -> Optional[float]:
    if value is None or not str(value).strip():
        return None
    return float(value)


def coordinates_match(
    expected: Tuple[float, float],
    actual: Tuple[Optional[float], Optional[float]],
) -> bool:
    if actual[0] is None or actual[1] is None:
        return False
    return math.isclose(expected[0], actual[0], rel_tol=0.0, abs_tol=1e-9) and math.isclose(
        expected[1], actual[1], rel_tol=0.0, abs_tol=1e-9
    )


def log_row_matches(expected: LogRow, actual: Sequence[object], compare_all: bool) -> bool:
    if len(actual) not in (9, 11):
        return False
    coordinate_offset = 9 if len(actual) == 11 else 7
    actual_coordinates = (as_optional_float(actual[coordinate_offset]), as_optional_float(actual[coordinate_offset + 1]))
    if not coordinates_match((expected.lat, expected.lon), actual_coordinates):
        return False
    if not compare_all:
        return True
    expected_values = (
        expected.from_airp,
        expected.to_airp,
        expected.etd,
        expected.eta,
        str(expected.status),
        expected.updated_at_utc,
        expected.permtype,
    )
    if len(actual) == 11:
        expected_values += (expected.time_in, expected.time_out)
        actual_values = actual[:9]
    else:
        actual_values = actual[:7]
    return expected_values == tuple(as_text(value) for value in actual_values)


def run_gui() -> None:
    import threading
    import tkinter as tk
    from tkinter import messagebox, scrolledtext

    app_stop_event = threading.Event()
    auto_stop_event = threading.Event()
    state = {"busy": False, "auto": False}

    class TkSink(MessageSink):
        def __init__(self, widget):
            self.widget = widget

        def write(self, text: str) -> None:
            if app_stop_event.is_set():
                return
            try:
                self.widget.after(0, lambda: append_log(self.widget, text))
            except tk.TclError:
                pass

    def append_log(widget, text: str) -> None:
        widget.insert("end", text + "\n")
        widget.see("end")

    def set_manual_controls(enabled: bool) -> None:
        value = "normal" if enabled else "disabled"
        for button in manual_buttons:
            button.configure(state=value)
        if not state["auto"]:
            auto_button.configure(state=value)
            interval_entry.configure(state=value)
        stop_button.configure(state="normal" if state["auto"] else "disabled")

    def finish_manual() -> None:
        state["busy"] = False
        set_manual_controls(True)
        auto_status.set("Sẵn sàng")

    def start(mode: str, full: bool = False) -> None:
        if state["busy"] or state["auto"]:
            return
        state["busy"] = True
        set_manual_controls(False)
        auto_status.set("Đang xử lý...")

        def worker():
            try:
                sink.write("Đang chạy, vui lòng chờ...")
                if mode == "config":
                    test_configuration(sink=sink)
                else:
                    execute(mode, full=full, sink=sink)
            except Exception:
                sink.write(traceback.format_exc())
            finally:
                if not app_stop_event.is_set():
                    root.after(0, finish_manual)

        threading.Thread(target=worker, daemon=True).start()

    def update_auto_countdown(remaining: int, iteration: int) -> None:
        auto_status.set(f"Lượt {iteration} hoàn tất · chạy lại sau {remaining} giây")

    def start_auto() -> None:
        if state["busy"] or state["auto"]:
            return
        try:
            interval = int(interval_value.get().strip())
            if interval <= 0:
                raise ValueError
        except ValueError:
            messagebox.showwarning("Chu kỳ không hợp lệ", "Vui lòng nhập số giây là số nguyên lớn hơn 0.")
            interval_entry.focus_set()
            return

        auto_stop_event.clear()
        state["auto"] = True
        state["busy"] = True
        set_manual_controls(False)
        auto_button.configure(text="Auto đang chạy", state="disabled")
        stop_button.configure(state="normal")
        interval_entry.configure(state="disabled")

        def auto_worker():
            iteration = 0
            try:
                while not app_stop_event.is_set() and not auto_stop_event.is_set():
                    iteration += 1
                    sink.write("\n========== AUTO - LƯỢT %d ==========" % iteration)
                    root.after(0, lambda current=iteration: auto_status.set(f"Đang chạy lượt {current}..."))
                    try:
                        execute("all", full=False, sink=sink)
                        sink.write("Auto lượt %d đã hoàn thành." % iteration)
                    except Exception:
                        sink.write("Auto lượt %d gặp lỗi:\n%s" % (iteration, traceback.format_exc()))

                    if app_stop_event.is_set() or auto_stop_event.is_set():
                        break
                    for remaining in range(interval, 0, -1):
                        if app_stop_event.is_set() or auto_stop_event.is_set():
                            break
                        root.after(0, lambda value=remaining, current=iteration: update_auto_countdown(value, current))
                        if auto_stop_event.wait(1):
                            break
            finally:
                if not app_stop_event.is_set():
                    root.after(0, finish_auto)

        threading.Thread(target=auto_worker, daemon=True).start()

    def stop_auto() -> None:
        if not state["auto"] or auto_stop_event.is_set():
            return
        auto_stop_event.set()
        stop_button.configure(state="disabled")
        auto_status.set("Đang dừng Auto sau khi lượt hiện tại hoàn tất...")
        sink.write("Đã yêu cầu dừng Auto; lượt đang xử lý sẽ hoàn tất trước khi dừng.")

    def finish_auto() -> None:
        state["auto"] = False
        state["busy"] = False
        auto_button.configure(text="Auto")
        set_manual_controls(True)
        auto_status.set("Auto đã dừng")
        sink.write("Auto đã dừng.")

    def close_app() -> None:
        app_stop_event.set()
        auto_stop_event.set()
        root.destroy()

    root = tk.Tk()
    root.title("ATFM FIR Tracks Logger")
    root.geometry("1120x720")
    root.protocol("WM_DELETE_WINDOW", close_app)
    header = tk.Label(root, text="ATFM - FIR Tracks Logger", font=("Segoe UI", 18, "bold"), bg="#155b83", fg="white", pady=18)
    header.pack(fill="x")
    desc = tk.Label(root, text="public.tracks -> FIR VVHN/VVHM -> T_DAY_FLIGHTS_GOINGON -> T_TRACKS_LOG", anchor="w", padx=18, pady=12)
    desc.pack(fill="x")
    bar = tk.Frame(root)
    bar.pack(fill="x", padx=18, pady=(0, 4))
    button_style = {"fg": "white", "padx": 16, "pady": 10, "font": ("Segoe UI", 9, "bold"), "relief": "flat", "cursor": "hand2"}
    manual_buttons = [
        tk.Button(bar, text="Test cấu hình", command=lambda: start("config"), bg="#526f85", **button_style),
        tk.Button(bar, text="Kiểm tra đối chiếu", command=lambda: start("check"), bg="#2d8ac4", **button_style),
        tk.Button(bar, text="Ghi T_TRACKS_LOG", command=lambda: start("sync"), bg="#14966d", **button_style),
        tk.Button(bar, text="Xác minh kết quả", command=lambda: start("verify"), bg="#7059b8", **button_style),
    ]
    for button in manual_buttons:
        button.pack(side="left", padx=(0, 8))

    auto_button = tk.Button(bar, text="Auto", command=start_auto, bg="#d9822b", **button_style)
    auto_button.pack(side="left", padx=(0, 8))
    stop_button = tk.Button(bar, text="Stop", command=stop_auto, bg="#c74444", state="disabled", **button_style)
    stop_button.pack(side="left", padx=(0, 8))
    tk.Label(bar, text="Chu kỳ (giây):", font=("Segoe UI", 9, "bold"), fg="#294c68").pack(side="left", padx=(5, 6))
    interval_value = tk.StringVar(value="10")
    interval_entry = tk.Entry(bar, textvariable=interval_value, width=7, justify="center", font=("Segoe UI", 10))
    interval_entry.pack(side="left", ipady=7)
    auto_status = tk.StringVar(value="Sẵn sàng")
    tk.Label(root, textvariable=auto_status, anchor="w", padx=18, pady=5, fg="#47677d", font=("Segoe UI", 9)).pack(fill="x")
    log = scrolledtext.ScrolledText(root, bg="#102531", fg="#e8f7ff", insertbackground="white", font=("Consolas", 10))
    log.pack(fill="both", expand=True, padx=18, pady=12)
    sink = TkSink(log)

    def start_api_on_open() -> None:
        try:
            cfg = load_config()
            sink.write("Đang kiểm tra Flight Tracking API nền...")
            start_flight_api_hidden(cfg, sink=sink)
        except Exception:
            sink.write("Không thể tự khởi động Flight Tracking API:\n%s" % traceback.format_exc())

    threading.Thread(target=start_api_on_open, daemon=True).start()
    root.mainloop()


def main() -> None:
    configure_console_encoding()
    parser = argparse.ArgumentParser(description="ATFM FIR Tracks Logger")
    parser.add_argument("--mode", choices=["config", "check", "sync", "verify", "all", "gui"], default="gui")
    parser.add_argument("--full", action="store_true", help="Bo qua watermark va doc lai toan bo public.tracks.")
    args = parser.parse_args()
    if args.mode == "gui":
        run_gui()
    elif args.mode == "config":
        test_configuration()
    else:
        execute(args.mode, full=args.full)


if __name__ == "__main__":
    main()
