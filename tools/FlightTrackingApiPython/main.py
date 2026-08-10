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
from flasgger import Swagger
from flask import Flask, jsonify, redirect, request
from waitress import serve


LOGGER = logging.getLogger("flight_tracking_api")
CONFIG_FILE_NAME = "FlightTrackingApi.local.json"
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
    "server": {
        "host": "0.0.0.0",
        "port": 5088,
        "threads": 8,
    },
    "api": {
        "max_lookback_days": 7,
    },
}

SWAGGER_CONFIG = {
    "headers": [],
    "specs": [
        {
            "endpoint": "openapi",
            "route": "/openapi.json",
            "rule_filter": lambda rule: True,
            "model_filter": lambda tag: True,
        }
    ],
    "static_url_path": "/swagger-static",
    "swagger_ui": True,
    "specs_route": "/swagger/",
}

SWAGGER_TEMPLATE = {
    "swagger": "2.0",
    "info": {
        "title": "ATFM Flight Tracking API",
        "description": "API đọc vị trí chuyến bay mới nhất từ PostgreSQL public.tracks. Không yêu cầu X-API-Key.",
        "version": "1.0.0",
    },
    "basePath": "/",
    "schemes": ["http"],
    "tags": [
        {"name": "Health", "description": "Trạng thái tiến trình và kết nối PostgreSQL."},
        {"name": "Flight Tracking", "description": "Dữ liệu vị trí chuyến bay."},
    ],
    "definitions": {
        "HealthStatus": {
            "type": "object",
            "required": ["status"],
            "properties": {"status": {"type": "string", "example": "ok"}},
        },
        "ErrorResponse": {
            "type": "object",
            "required": ["error"],
            "properties": {"error": {"type": "string"}},
        },
        "TrackPosition": {
            "type": "object",
            "required": ["flightIdCurrent", "callsign", "updatedAtUtc", "latitude", "longitude", "heading"],
            "properties": {
                "flightIdCurrent": {"type": "string", "example": "HVN123-20260810"},
                "callsign": {"type": "string", "example": "HVN123"},
                "updatedAtUtc": {"type": "string", "format": "date-time", "example": "2026-08-10T08:30:00"},
                "latitude": {"type": "number", "format": "double", "example": 21.0285},
                "longitude": {"type": "number", "format": "double", "example": 105.8542},
                "heading": {"type": "number", "format": "double", "example": 180.0},
            },
        },
        "TracksResponse": {
            "type": "object",
            "required": ["day", "serverTimeUtc", "count", "flights"],
            "properties": {
                "day": {"type": "string", "format": "date", "example": "2026-08-10"},
                "serverTimeUtc": {"type": "string", "format": "date-time"},
                "count": {"type": "integer", "example": 1},
                "flights": {
                    "type": "array",
                    "items": {"$ref": "#/definitions/TrackPosition"},
                },
            },
        },
    },
}


def executable_directory() -> Path:
    if getattr(sys, "frozen", False):
        return Path(sys.executable).resolve().parent
    return Path(__file__).resolve().parent


def resource_directory() -> Path:
    bundle_path = getattr(sys, "_MEIPASS", None)
    return Path(bundle_path) if bundle_path else Path(__file__).resolve().parent


def default_config_path() -> Path:
    return executable_directory() / CONFIG_FILE_NAME


def merge_config(target: Dict[str, Any], source: Dict[str, Any]) -> Dict[str, Any]:
    for key, value in source.items():
        if isinstance(value, dict) and isinstance(target.get(key), dict):
            merge_config(target[key], value)
        else:
            target[key] = value
    return target


def load_config(path: Optional[Path] = None) -> Dict[str, Any]:
    config = deepcopy(DEFAULT_CONFIG)
    config_path = path or default_config_path()
    if config_path.exists():
        with config_path.open("r", encoding="utf-8") as stream:
            loaded = json.load(stream)
        if not isinstance(loaded, dict):
            raise ValueError("File cấu hình phải chứa một JSON object.")
        merge_config(config, loaded)

    environment_values = {
        ("postgres", "host"): os.getenv("FLIGHT_API_PG_HOST"),
        ("postgres", "port"): os.getenv("FLIGHT_API_PG_PORT"),
        ("postgres", "database"): os.getenv("FLIGHT_API_PG_DATABASE"),
        ("postgres", "username"): os.getenv("FLIGHT_API_PG_USERNAME"),
        ("postgres", "password"): os.getenv("FLIGHT_API_PG_PASSWORD"),
        ("postgres", "sslmode"): os.getenv("FLIGHT_API_PG_SSLMODE"),
        ("server", "host"): os.getenv("FLIGHT_API_HOST"),
        ("server", "port"): os.getenv("FLIGHT_API_PORT"),
    }
    for (section, key), value in environment_values.items():
        if value not in (None, ""):
            config[section][key] = value

    config["postgres"]["port"] = int(config["postgres"]["port"])
    config["postgres"]["connect_timeout"] = int(config["postgres"]["connect_timeout"])
    config["server"]["port"] = int(config["server"]["port"])
    config["server"]["threads"] = int(config["server"]["threads"])
    config["api"]["max_lookback_days"] = int(config["api"]["max_lookback_days"])
    return config


def validate_config(config: Dict[str, Any]) -> None:
    postgres = config["postgres"]
    missing = [key for key in ("host", "database", "username", "password") if not str(postgres.get(key, "")).strip()]
    if missing:
        raise ValueError("Thiếu cấu hình PostgreSQL: " + ", ".join(missing) + ".")
    if not 1 <= int(postgres["port"]) <= 65535:
        raise ValueError("postgres.port không hợp lệ.")
    if not 1 <= int(config["server"]["port"]) <= 65535:
        raise ValueError("server.port không hợp lệ.")
    if not 1 <= int(config["server"]["threads"]) <= 128:
        raise ValueError("server.threads phải từ 1 đến 128.")
    if not 0 <= int(config["api"]["max_lookback_days"]) <= 366:
        raise ValueError("api.max_lookback_days phải từ 0 đến 366.")


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
    Swagger(app, config=SWAGGER_CONFIG, template=SWAGGER_TEMPLATE)

    @app.get("/")
    def index():
        return redirect("/swagger/", code=302)

    @app.get("/health/live")
    def health_live():
        """Kiểm tra tiến trình API đang chạy.
        ---
        tags: [Health]
        responses:
          200:
            description: Tiến trình đang hoạt động.
            schema:
              $ref: '#/definitions/HealthStatus'
        """
        return jsonify(status="ok")

    @app.get("/health/ready")
    def health_ready():
        """Kiểm tra kết nối PostgreSQL.
        ---
        tags: [Health]
        responses:
          200:
            description: PostgreSQL sẵn sàng.
            schema:
              $ref: '#/definitions/HealthStatus'
          503:
            description: PostgreSQL chưa sẵn sàng.
            schema:
              $ref: '#/definitions/HealthStatus'
        """
        try:
            database_checker(config)
            return jsonify(status="ready")
        except Exception:
            LOGGER.exception("Không thể kết nối PostgreSQL khi kiểm tra readiness.")
            return jsonify(status="not_ready"), 503

    @app.get("/api/v1/tracks")
    def tracks():
        """Lấy vị trí mới nhất của từng callsign trong ngày.
        ---
        tags: [Flight Tracking]
        parameters:
          - name: date
            in: query
            type: string
            format: date
            required: false
            description: Ngày UTC theo định dạng yyyy-MM-dd; mặc định là hôm nay.
        responses:
          200:
            description: Danh sách vị trí chuyến bay.
            schema:
              $ref: '#/definitions/TracksResponse'
          400:
            description: Ngày sai định dạng hoặc ngoài khoảng cho phép.
            schema:
              $ref: '#/definitions/ErrorResponse'
          503:
            description: PostgreSQL tạm thời chưa sẵn sàng.
            schema:
              $ref: '#/definitions/ErrorResponse'
        """
        raw_date = (request.args.get("date") or "").strip()
        if raw_date:
            try:
                requested_date = datetime.strptime(raw_date, DATE_FORMAT).date()
            except ValueError:
                return jsonify(error="Tham số date phải có định dạng yyyy-MM-dd."), 400
        else:
            requested_date = datetime.now(timezone.utc).date()

        today = datetime.now(timezone.utc).date()
        earliest = today - timedelta(days=config["api"]["max_lookback_days"])
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
    example_path = resource_directory() / "FlightTrackingApi.example.json"
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
    config_path = args.config or default_config_path()

    if args.init_config:
        write_initial_config(config_path)
        print(f"Đã tạo file cấu hình: {config_path}")
        return 0

    try:
        config = load_config(config_path)
        if args.host:
            config["server"]["host"] = args.host
        if args.port:
            config["server"]["port"] = args.port
        validate_config(config)
        if args.check:
            check_database(config)
            print("Cấu hình hợp lệ và kết nối PostgreSQL thành công.")
            return 0
    except Exception as exc:
        LOGGER.error("Không thể khởi động: %s", exc)
        return 2

    server = config["server"]
    LOGGER.info("Flight Tracking API đang lắng nghe tại http://%s:%s", server["host"], server["port"])
    serve(create_app(config), host=server["host"], port=server["port"], threads=server["threads"])
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
