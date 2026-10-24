from __future__ import annotations

import argparse
import copy
import os
import sys
import threading
import time
from pathlib import Path
from typing import Any, Dict, Optional

from waitress import create_server

TOOLS_ROOT = Path(__file__).resolve().parents[1]
if str(TOOLS_ROOT) not in sys.path:
    sys.path.insert(0, str(TOOLS_ROOT))

from FlightTrackingApiPython import main as flight_api  # noqa: E402
from TracksSyncPython import main as tracks_sync  # noqa: E402


class FlightTrackingApiServer:
    """Chạy Flight Tracking API trong cùng tiến trình với giao diện ADS-B."""

    def __init__(self, config_path: Optional[Path] = None) -> None:
        self.config_path = config_path or flight_api.default_config_path()
        self.config: Optional[Dict[str, Any]] = None
        self.server: Any = None
        self.thread: Optional[threading.Thread] = None

    def _load_config(self) -> Dict[str, Any]:
        config = flight_api.load_config(self.config_path)
        if self.config_path.exists():
            return config

        # Khi chỉ có TracksSync.local.json, tái sử dụng kết nối PostgreSQL của tool ADS-B.
        tracks_config = tracks_sync.load_config()
        postgres = tracks_config.get("postgres", {})
        environment_keys = {
            "host": "FLIGHT_API_PG_HOST",
            "port": "FLIGHT_API_PG_PORT",
            "database": "FLIGHT_API_PG_DATABASE",
            "username": "FLIGHT_API_PG_USERNAME",
            "password": "FLIGHT_API_PG_PASSWORD",
        }
        for key, environment_key in environment_keys.items():
            if not os.getenv(environment_key) and postgres.get(key) not in (None, ""):
                config["postgres"][key] = postgres[key]
        return config

    def start(self) -> str:
        self.config = self._load_config()
        flight_api.validate_config(self.config)
        server_config = self.config["server"]
        self.server = create_server(
            flight_api.create_app(self.config),
            host=server_config["host"],
            port=server_config["port"],
            threads=server_config["threads"],
        )
        self.thread = threading.Thread(
            target=self.server.run,
            name="ATFM-FlightTrackingApi",
            daemon=True,
        )
        self.thread.start()
        return "http://127.0.0.1:{0}".format(server_config["port"])

    def stop(self) -> None:
        if self.server is not None:
            self.server.close()
        if self.thread is not None:
            self.thread.join(timeout=5)


def parse_arguments(argv: Optional[list[str]] = None) -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="ATFM ADS-B: FIR Tracks Logger + Flight Tracking API trong một EXE."
    )
    parser.add_argument(
        "--api-config",
        type=Path,
        help="Đường dẫn FlightTrackingApi.local.json; mặc định là file cạnh EXE.",
    )
    parser.add_argument(
        "--api-only",
        action="store_true",
        help="Chỉ chạy API nền (dùng cho kiểm tra/dịch vụ).",
    )
    parser.add_argument(
        "--no-api",
        action="store_true",
        help="Chỉ mở Tracks Logger, không tự khởi chạy API.",
    )
    return parser.parse_args(argv)


def show_api_warning(message: str) -> None:
    try:
        import tkinter as tk
        from tkinter import messagebox

        root = tk.Tk()
        root.withdraw()
        messagebox.showwarning("ATFM ADS-B", message, parent=root)
        root.destroy()
    except Exception:
        print(message, file=sys.stderr)


def run_api_only(api_server: FlightTrackingApiServer) -> int:
    try:
        endpoint = api_server.start()
    except Exception as exc:
        print("Không thể khởi động Flight Tracking API: {0}".format(exc), file=sys.stderr)
        return 2
    print("Flight Tracking API đang chạy tại {0}".format(endpoint))
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        return 0
    finally:
        api_server.stop()


def main(argv: Optional[list[str]] = None) -> int:
    tracks_sync.configure_console_encoding()
    args = parse_arguments(argv)
    if args.api_only and args.no_api:
        raise SystemExit("Không thể dùng --api-only cùng --no-api.")

    api_server = FlightTrackingApiServer(args.api_config)
    if args.api_only:
        return run_api_only(api_server)

    api_started = False
    if not args.no_api:
        try:
            endpoint = api_server.start()
            api_started = True
            print("Flight Tracking API đã tự khởi chạy tại {0}".format(endpoint))
        except Exception as exc:
            show_api_warning(
                "Không thể tự khởi chạy Flight Tracking API.\n\n"
                "Tracks Logger vẫn được mở, nhưng bản đồ theo dõi sẽ không có API nền.\n\n"
                "Chi tiết: {0}".format(exc)
            )

    try:
        tracks_sync.run_gui()
    finally:
        if api_started:
            api_server.stop()
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
