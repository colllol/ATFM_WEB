import argparse
import cryptography  # PyInstaller: oracledb thin mode can goi goi ma hoa nay dong.
import getpass  # PyInstaller: oracledb nap dong module nay khi khoi tao.
import json
import os
import sys
import traceback
from dataclasses import dataclass
from datetime import datetime, timezone
from pathlib import Path
from typing import Dict, Iterable, List, Optional, Sequence, Tuple

# python-oracledb Thin Mode nap cac nhanh cryptography nay bang import dong.
# Import truc tiep de ban PyInstaller one-file dong goi du dependency.
from cryptography import x509
from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC


DEFAULT_GEOJSON_PATH = r"C:\Users\Admin\Downloads\vietnam-fir-VVHM-VVHN.geojson"
DATE_DISPLAY_FORMAT = "%d-%m-%Y"
WATERMARK_FORMAT = "%Y-%m-%d %H:%M:%S"


@dataclass
class TrackCandidate:
    callsign: str
    flight_date: str
    update_text: str
    status: int


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
                    )
                )
    sink.write(f"Doc public.tracks: {len(rows)} dong nam trong FIR can doi chieu.")
    return dedupe_tracks(rows), max_updated


def dedupe_tracks(rows: Iterable[TrackCandidate]) -> List[TrackCandidate]:
    best: Dict[Tuple[str, str], TrackCandidate] = {}
    for row in rows:
        key = (row.callsign, row.flight_date)
        old = best.get(key)
        if old is None or row.update_text >= old.update_text:
            best[key] = row
    return list(best.values())


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
                    PERMTYPE VARCHAR2(50)
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


def read_oracle_flights(conn, cfg: Dict, keys: Sequence[Tuple[str, str]], sink: MessageSink) -> Dict[Tuple[str, str], FlightInfo]:
    if not keys:
        return {}
    table = cfg.get("oracle", {}).get("flight_table", "T_DAY_FLIGHTS_GOINGON")
    result: Dict[Tuple[str, str], FlightInfo] = {}
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
                    if not is_flight_route_valid(flight):
                        invalid_route_count += 1
                        continue
                    collected.setdefault((flight.callsign, flight.flight_date), []).append(flight)
        if invalid_route_count:
            sink.write(
                f"Bo qua {invalid_route_count} dong co PERMTYPE khong phai O/F "
                "nhung thieu FROM_AIRP hoac TO_AIRP."
            )
        ambiguous_examples: List[str] = []
        ambiguous_count = 0
        for key, matches in collected.items():
            if len(matches) == 1:
                result[key] = matches[0]
            else:
                ambiguous_count += 1
                if len(ambiguous_examples) < 20:
                    ambiguous_examples.append(f"{key[0]}/{key[1]} ({len(matches)} dong)")
        if ambiguous_count:
            sink.write(
                "Bo qua "
                f"{ambiguous_count} callsign/ngay bi trung trong T_DAY_FLIGHTS_GOINGON; "
                "public.tracks khong co du route/time de chon chinh xac."
            )
            for item in ambiguous_examples:
                sink.write(f"  - {item}")
    finally:
        cur.close()
    return result


def chunks(values: Sequence[str], size: int) -> Iterable[Sequence[str]]:
    for start in range(0, len(values), size):
        yield values[start : start + size]


def is_flight_route_valid(flight: FlightInfo) -> bool:
    permtype = (flight.permtype or "").strip().upper()
    if permtype == "O/F":
        return True
    return bool((flight.from_airp or "").strip() and (flight.to_airp or "").strip())


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
                   :permtype PERMTYPE
            FROM DUAL
        ) source
        ON (
            target.CALLSIGN = source.CALLSIGN
            AND target."DATE" = source."DATE"
        )
        WHEN MATCHED THEN UPDATE SET
            target.FROM_AIRP = source.FROM_AIRP,
            target.TO_AIRP = source.TO_AIRP,
            target.ETD = source.ETD,
            target.ETA = source.ETA,
            target.STATUS = source.STATUS,
            target.UPDATED_AT_UTC = source.UPDATED_AT_UTC,
            target.PERMTYPE = source.PERMTYPE
        WHEN NOT MATCHED THEN INSERT
            (TRLOG_ID, CALLSIGN, FROM_AIRP, TO_AIRP, ETD, ETA, STATUS, "DATE", UPDATED_AT_UTC, PERMTYPE)
        VALUES
            (T_TRACKS_LOG_SEQ.NEXTVAL, source.CALLSIGN, source.FROM_AIRP, source.TO_AIRP, source.ETD, source.ETA,
             source.STATUS, source."DATE", source.UPDATED_AT_UTC, source.PERMTYPE)
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
            }
            for row in rows
        ]
        cur.executemany(sql, data)
        conn.commit()
        return len(rows)
    finally:
        cur.close()


def build_log_rows(candidates: Sequence[TrackCandidate], flights: Dict[Tuple[str, str], FlightInfo]) -> List[LogRow]:
    rows: List[LogRow] = []
    for item in candidates:
        flight = flights.get((item.callsign, item.flight_date))
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
    sink.write(f"Can doi chieu Oracle: {len(keys)} callsign/ngay.")
    with oracle_connect(cfg) as conn:
        ensure_schema(conn)
        flights = read_oracle_flights(conn, cfg, keys, sink)
        rows = build_log_rows(candidates, flights)
        missing = len(keys) - len(flights)
        sink.write(f"Match T_DAY_FLIGHTS_GOINGON: {len(flights)}; khong match: {missing}.")
        if mode == "check":
            sink.write(f"Kiem tra xong. Neu ghi se cap nhat {len(rows)} dong T_TRACKS_LOG.")
        elif mode == "sync":
            written = merge_logs(conn, rows)
            write_watermark(cfg, max_updated)
            sink.write(f"Da ghi/cap nhat {written} dong T_TRACKS_LOG.")
            if max_updated:
                sink.write(f"Watermark moi: {max_updated.strftime(WATERMARK_FORMAT)}")
        elif mode == "verify":
            verify_logs(conn, rows, sink)
        elif mode == "all":
            sink.write(f"[1/3] Kiem tra doi chieu xong: {len(rows)} dong hop le.")
            written = merge_logs(conn, rows)
            sink.write(f"[2/3] Da ghi/cap nhat {written} dong T_TRACKS_LOG.")
            verify_logs(conn, rows, sink, report_extra=False)
            sink.write("[3/3] Da xac minh cac dong trong chu ky.")
            write_watermark(cfg, max_updated)
            if max_updated:
                sink.write(f"Watermark moi: {max_updated.strftime(WATERMARK_FORMAT)}")
        else:
            raise ValueError(f"Mode khong hop le: {mode}")


def verify_logs(conn, expected: Sequence[LogRow], sink: MessageSink, report_extra: bool = True) -> None:
    if not expected:
        sink.write("Khong co dong du kien de xac minh.")
        return
    expected_map = {
        (row.callsign, row.flight_date): (
            row.from_airp,
            row.to_airp,
            row.etd,
            row.eta,
            str(row.status),
            row.updated_at_utc,
            row.permtype,
        )
        for row in expected
    }
    actual_map: Dict[Tuple[str, str], Tuple[str, ...]] = {}
    duplicate_count = 0
    cur = conn.cursor()
    try:
        cur.execute(
            '''
            SELECT CALLSIGN, "DATE", FROM_AIRP, TO_AIRP, ETD, ETA,
                   STATUS, UPDATED_AT_UTC, PERMTYPE
            FROM T_TRACKS_LOG
            '''
        )
        for found in cur.fetchall():
            key = (normalize_callsign(found[0]), as_text(found[1]))
            if key in actual_map:
                duplicate_count += 1
            actual_map[key] = tuple(as_text(value) for value in found[2:])
    finally:
        cur.close()

    expected_keys = set(expected_map)
    actual_keys = set(actual_map)
    missing_keys = expected_keys - actual_keys
    extra_keys = actual_keys - expected_keys if report_extra else set()
    common_keys = expected_keys & actual_keys
    mismatch_keys = {key for key in common_keys if expected_map[key] != actual_map[key]}
    ok = len(common_keys) - len(mismatch_keys)
    sink.write(
        "Xac minh T_TRACKS_LOG: "
        f"dung {ok}, sai {len(mismatch_keys)}, thieu {len(missing_keys)}, "
        f"thua {len(extra_keys)}, trung khoa {duplicate_count}."
    )


def run_gui() -> None:
    import threading
    import tkinter as tk
    from tkinter import messagebox, scrolledtext

    stop_event = threading.Event()
    state = {"busy": False, "auto": False}

    class TkSink(MessageSink):
        def __init__(self, widget):
            self.widget = widget

        def write(self, text: str) -> None:
            if stop_event.is_set():
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
                execute(mode, full=full, sink=sink)
            except Exception:
                sink.write(traceback.format_exc())
            finally:
                if not stop_event.is_set():
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

        state["auto"] = True
        state["busy"] = True
        set_manual_controls(False)
        auto_button.configure(text="Auto đang chạy", state="disabled")
        interval_entry.configure(state="disabled")

        def auto_worker():
            iteration = 0
            while not stop_event.is_set():
                iteration += 1
                sink.write("\n========== AUTO - LƯỢT %d ==========" % iteration)
                root.after(0, lambda current=iteration: auto_status.set(f"Đang chạy lượt {current}..."))
                try:
                    execute("all", full=False, sink=sink)
                    sink.write("Auto lượt %d đã hoàn thành." % iteration)
                except Exception:
                    sink.write("Auto lượt %d gặp lỗi:\n%s" % (iteration, traceback.format_exc()))

                if stop_event.is_set():
                    break
                for remaining in range(interval, 0, -1):
                    if stop_event.is_set():
                        break
                    root.after(0, lambda value=remaining, current=iteration: update_auto_countdown(value, current))
                    if stop_event.wait(1):
                        break

        threading.Thread(target=auto_worker, daemon=True).start()

    def close_app() -> None:
        stop_event.set()
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
        tk.Button(bar, text="Kiểm tra đối chiếu", command=lambda: start("check"), bg="#2d8ac4", **button_style),
        tk.Button(bar, text="Ghi T_TRACKS_LOG", command=lambda: start("sync"), bg="#14966d", **button_style),
        tk.Button(bar, text="Xác minh kết quả", command=lambda: start("verify"), bg="#7059b8", **button_style),
    ]
    for button in manual_buttons:
        button.pack(side="left", padx=(0, 8))

    auto_button = tk.Button(bar, text="Auto", command=start_auto, bg="#d9822b", **button_style)
    auto_button.pack(side="left", padx=(0, 8))
    tk.Label(bar, text="Chu kỳ (giây):", font=("Segoe UI", 9, "bold"), fg="#294c68").pack(side="left", padx=(5, 6))
    interval_value = tk.StringVar(value="10")
    interval_entry = tk.Entry(bar, textvariable=interval_value, width=7, justify="center", font=("Segoe UI", 10))
    interval_entry.pack(side="left", ipady=7)
    auto_status = tk.StringVar(value="Sẵn sàng")
    tk.Label(root, textvariable=auto_status, anchor="w", padx=18, pady=5, fg="#47677d", font=("Segoe UI", 9)).pack(fill="x")
    log = scrolledtext.ScrolledText(root, bg="#102531", fg="#e8f7ff", insertbackground="white", font=("Consolas", 10))
    log.pack(fill="both", expand=True, padx=18, pady=12)
    sink = TkSink(log)
    root.mainloop()


def main() -> None:
    configure_console_encoding()
    parser = argparse.ArgumentParser(description="ATFM FIR Tracks Logger")
    parser.add_argument("--mode", choices=["check", "sync", "verify", "all", "gui"], default="gui")
    parser.add_argument("--full", action="store_true", help="Bo qua watermark va doc lai toan bo public.tracks.")
    args = parser.parse_args()
    if args.mode == "gui":
        run_gui()
    else:
        execute(args.mode, full=args.full)


if __name__ == "__main__":
    main()
