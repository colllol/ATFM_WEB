import argparse
import json
import logging
import os
import shutil
import sys
from copy import deepcopy
from datetime import date, datetime, time, timedelta, timezone
from pathlib import Path
from typing import Any, Callable, Dict, List, Optional

import psycopg
from flask import Flask, jsonify, request
from waitress import serve


LOGGER = logging.getLogger("flight_tracking_api")
CONFIG_FILE_NAME = "TracksSync.local.json"
DATE_FORMAT = "%Y-%m-%d"

LATEST_TRACKS_SQL = """
    SELECT DISTINCT ON (normalized_callsign)
           flight_id_current,
           normalized_callsign,
           updated_at_timestamp,
           last_lat,
           last_lon,
           COALESCE(last_track_deg, 0)
    FROM (
        SELECT flight_id_current,
               UPPER(TRIM(SPLIT_PART(COALESCE(flight_id_current, ''), '-', 1))) normalized_callsign,
               NULLIF(TRIM(updated_at_utc), '')::timestamp updated_at_timestamp,
               last_lat,
               last_lon,
               last_track_deg
        FROM public.tracks
        WHERE flight_id_current IS NOT NULL
          AND updated_at_utc IS NOT NULL
          AND last_lat IS NOT NULL
          AND last_lon IS NOT NULL
          AND NULLIF(TRIM(updated_at_utc), '')::timestamp >= %s
          AND NULLIF(TRIM(updated_at_utc), '')::timestamp < %s
    ) source
    WHERE normalized_callsign <> ''
    ORDER BY normalized_callsign, updated_at_timestamp DESC
"""

DEFAULT_CONFIG: Dict[str, Any] = {
    "postgres": {
        "host": "",
        "port": 5432,
        "database": "postgres",
        "username": "",
        "password": "",
        "sslmode": "prefer",
        "connect_timeout": 10,
    },
    "flight_tracking_api": {
        "host": "0.0.0.0",
        "port": 5088,
        "threads": 8,
        "max_lookback_days": 7,
    },
}


def executable_directory() -> Path:
    if getattr(sys, "frozen", False):
        return Path(sys.executable).resolve().parent
    return Path(__file__).resolve().parent


def resource_directory() -> Path:
    bundle_path = getattr(sys, "_MEIPASS", None)
    return Path(bundle_path) if bundle_path else Path(__file__).resolve().parent


def config_candidates() -> List[Path]:
    paths: List[Path] = []
    environment_path = os.getenv("TRACKS_CONFIG")
    if environment_path:
        paths.append(Path(environment_path))
    paths.append(executable_directory() / CONFIG_FILE_NAME)
    if not getattr(sys, "frozen", False):
        paths.append(Path(__file__).resolve().parents[2] / "prjApplication" / "App_Data" / CONFIG_FILE_NAME)
    return paths


def default_config_path() -> Path:
    return config_candidates()[0]


def merge_config(target: Dict[str, Any], source: Dict[str, Any]) -> Dict[str, Any]:
    for key, value in source.items():
        if isinstance(value, dict) and isinstance(target.get(key), dict):
            merge_config(target[key], value)
        else:
            target[key] = value
    return target


def load_config(path: Optional[Path] = None) -> Dict[str, Any]:
    config = deepcopy(DEFAULT_CONFIG)
    config_path = path
    if config_path is None:
        config_path = next((candidate for candidate in config_candidates() if candidate.exists()), default_config_path())
    if config_path.exists():
        with config_path.open("r", encoding="utf-8-sig") as stream:
            loaded = json.load(stream)
        if not isinstance(loaded, dict):
            raise ValueError("File cấu hình phải chứa một JSON object.")
        # Đọc được file FlightTrackingApi.local.json cũ trong giai đoạn chuyển đổi.
        if "server" in loaded and "flight_tracking_api" not in loaded:
            loaded["flight_tracking_api"] = dict(loaded["server"])
            loaded["flight_tracking_api"].update(loaded.get("api", {}))
        merge_config(config, loaded)

    environment_values = {
        ("postgres", "host"): os.getenv("TRACKS_PG_HOST") or os.getenv("FLIGHT_API_PG_HOST"),
        ("postgres", "port"): os.getenv("TRACKS_PG_PORT") or os.getenv("FLIGHT_API_PG_PORT"),
        ("postgres", "database"): os.getenv("TRACKS_PG_DATABASE") or os.getenv("FLIGHT_API_PG_DATABASE"),
        ("postgres", "username"): os.getenv("TRACKS_PG_USERNAME") or os.getenv("FLIGHT_API_PG_USERNAME"),
        ("postgres", "password"): os.getenv("TRACKS_PG_PASSWORD") or os.getenv("FLIGHT_API_PG_PASSWORD"),
        ("postgres", "sslmode"): os.getenv("FLIGHT_API_PG_SSLMODE"),
        ("flight_tracking_api", "host"): os.getenv("FLIGHT_API_HOST"),
        ("flight_tracking_api", "port"): os.getenv("FLIGHT_API_PORT"),
    }
    for (section, key), value in environment_values.items():
        if value not in (None, ""):
            config[section][key] = value

    config["postgres"]["port"] = int(config["postgres"]["port"])
    config["postgres"]["connect_timeout"] = int(config["postgres"]["connect_timeout"])
    config["flight_tracking_api"]["port"] = int(config["flight_tracking_api"]["port"])
    config["flight_tracking_api"]["threads"] = int(config["flight_tracking_api"]["threads"])
    config["flight_tracking_api"]["max_lookback_days"] = int(config["flight_tracking_api"]["max_lookback_days"])
    config["_config_source"] = str(config_path.resolve()) if config_path.exists() else "environment/default"
    return config


def validate_config(config: Dict[str, Any]) -> None:
    postgres = config["postgres"]
    missing = [key for key in ("host", "database", "username", "password") if not str(postgres.get(key, "")).strip()]
    if missing:
        raise ValueError("Thiếu cấu hình PostgreSQL: " + ", ".join(missing) + ".")
    if not 1 <= int(postgres["port"]) <= 65535:
        raise ValueError("postgres.port không hợp lệ.")
    api = config["flight_tracking_api"]
    if not 1 <= int(api["port"]) <= 65535:
        raise ValueError("flight_tracking_api.port không hợp lệ.")
    if not 1 <= int(api["threads"]) <= 128:
        raise ValueError("flight_tracking_api.threads phải từ 1 đến 128.")
    if not 0 <= int(api["max_lookback_days"]) <= 366:
        raise ValueError("flight_tracking_api.max_lookback_days phải từ 0 đến 366.")


def connection_parameters(config: Dict[str, Any]) -> Dict[str, Any]:
    postgres = config["postgres"]
    return {
        "host": postgres["host"],
        "port": postgres["port"],
        "dbname": postgres["database"],
        "user": postgres["username"],
        "password": postgres["password"],
        "sslmode": postgres.get("sslmode", "prefer"),
        "connect_timeout": postgres.get("connect_timeout", 10),
    }


def fetch_latest_tracks(config: Dict[str, Any], requested_date: date) -> List[Dict[str, Any]]:
    from_date = datetime.combine(requested_date, time.min)
    to_date = from_date + timedelta(days=1)
    with psycopg.connect(**connection_parameters(config)) as connection:
        with connection.cursor() as cursor:
            cursor.execute(LATEST_TRACKS_SQL, (from_date, to_date))
            rows = cursor.fetchall()

    return [
        {
            "flightIdCurrent": str(row[0] or ""),
            "callsign": str(row[1] or ""),
            "updatedAtUtc": row[2].strftime("%Y-%m-%dT%H:%M:%S"),
            "latitude": float(row[3]),
            "longitude": float(row[4]),
            "heading": float(row[5] or 0),
        }
        for row in rows
    ]


def check_database(config: Dict[str, Any]) -> None:
    with psycopg.connect(**connection_parameters(config)) as connection:
        with connection.cursor() as cursor:
            cursor.execute("SELECT 1")
            cursor.fetchone()


def create_app(
    config: Dict[str, Any],
    track_loader: Callable[[Dict[str, Any], date], List[Dict[str, Any]]] = fetch_latest_tracks,
    database_checker: Callable[[Dict[str, Any]], None] = check_database,
) -> Flask:
    app = Flask(__name__)
    app.config["ATFM_FLIGHT_CONFIG"] = config

    @app.get("/health/live")
    def health_live():
        return jsonify(status="ok")

    @app.get("/health/ready")
    def health_ready():
        try:
            database_checker(config)
            return jsonify(status="ready")
        except Exception:
            LOGGER.exception("Không thể kết nối PostgreSQL khi kiểm tra readiness.")
            return jsonify(status="not_ready"), 503

    @app.get("/api/v1/tracks")
    def tracks():
        raw_date = (request.args.get("date") or "").strip()
        if raw_date:
            try:
                requested_date = datetime.strptime(raw_date, DATE_FORMAT).date()
            except ValueError:
                return jsonify(error="Tham số date phải có định dạng yyyy-MM-dd."), 400
        else:
            requested_date = datetime.now(timezone.utc).date()

        today = datetime.now(timezone.utc).date()
        earliest = today - timedelta(days=config["flight_tracking_api"]["max_lookback_days"])
        if requested_date < earliest or requested_date > today:
            return jsonify(error=f"Chỉ được truy vấn từ {earliest:%Y-%m-%d} đến {today:%Y-%m-%d}."), 400

        try:
            flights = track_loader(config, requested_date)
        except Exception:
            LOGGER.exception("Không thể đọc public.tracks cho ngày %s.", requested_date)
            return jsonify(error="PostgreSQL tạm thời không sẵn sàng."), 503

        return jsonify(
            day=requested_date.strftime(DATE_FORMAT),
            serverTimeUtc=datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
            count=len(flights),
            flights=flights,
        )

    return app


def write_initial_config(target: Path) -> None:
    if target.exists():
        raise FileExistsError(f"File đã tồn tại: {target}")
    target.parent.mkdir(parents=True, exist_ok=True)
    example_path = resource_directory() / "TracksSync.sample.json"
    if example_path.exists():
        shutil.copyfile(example_path, target)
    else:
        with target.open("w", encoding="utf-8") as stream:
            json.dump(DEFAULT_CONFIG, stream, ensure_ascii=False, indent=2)
            stream.write("\n")


def parse_arguments(argv: Optional[List[str]] = None) -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="ATFM Flight Tracking PostgreSQL API")
    parser.add_argument("--config", type=Path, help="Đường dẫn file cấu hình JSON.")
    parser.add_argument("--init-config", action="store_true", help="Tạo file cấu hình mẫu cạnh EXE rồi thoát.")
    parser.add_argument("--check", action="store_true", help="Kiểm tra cấu hình và kết nối PostgreSQL rồi thoát.")
    parser.add_argument("--host", help="Ghi đè địa chỉ HTTP lắng nghe.")
    parser.add_argument("--port", type=int, help="Ghi đè cổng HTTP lắng nghe.")
    return parser.parse_args(argv)


def configure_console_encoding() -> None:
    for stream in (sys.stdout, sys.stderr):
        reconfigure = getattr(stream, "reconfigure", None)
        if reconfigure:
            try:
                reconfigure(encoding="utf-8", errors="replace")
            except (OSError, ValueError):
                pass


def main(argv: Optional[List[str]] = None) -> int:
    configure_console_encoding()
    logging.basicConfig(level=logging.INFO, format="%(asctime)s %(levelname)s %(message)s")
    args = parse_arguments(argv)
    config_path = args.config

    if args.init_config:
        target_path = config_path or default_config_path()
        write_initial_config(target_path)
        print(f"Đã tạo file cấu hình: {target_path}")
        return 0

    try:
        config = load_config(config_path)
        if args.host:
            config["flight_tracking_api"]["host"] = args.host
        if args.port:
            config["flight_tracking_api"]["port"] = args.port
        validate_config(config)
        if args.check:
            check_database(config)
            print("Cấu hình hợp lệ và kết nối PostgreSQL thành công.")
            return 0
    except Exception as exc:
        LOGGER.error("Không thể khởi động: %s", exc)
        return 2

    server = config["flight_tracking_api"]
    LOGGER.info("Flight Tracking API đang lắng nghe tại http://%s:%s", server["host"], server["port"])
    serve(create_app(config), host=server["host"], port=server["port"], threads=server["threads"])
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
