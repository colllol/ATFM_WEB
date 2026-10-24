import json
import socket
import tempfile
import time
import unittest
from pathlib import Path
from urllib.request import urlopen

from tools.AdsBUnified.main import FlightTrackingApiServer


class AdsBUnifiedTests(unittest.TestCase):
    def test_api_starts_and_stops_with_unified_controller(self):
        with socket.socket() as listener:
            listener.bind(("127.0.0.1", 0))
            port = listener.getsockname()[1]

        config = {
            "postgres": {
                "host": "127.0.0.1",
                "port": 5432,
                "database": "test",
                "username": "test",
                "password": "test",
                "sslmode": "disable",
                "connect_timeout": 1,
            },
            "server": {"host": "127.0.0.1", "port": port, "threads": 2},
            "api": {"max_lookback_days": 7},
        }

        with tempfile.TemporaryDirectory() as directory:
            config_path = Path(directory) / "FlightTrackingApi.local.json"
            config_path.write_text(json.dumps(config), encoding="utf-8")
            api = FlightTrackingApiServer(config_path)
            endpoint = api.start()
            try:
                response = None
                for _ in range(30):
                    try:
                        with urlopen(endpoint + "/health/live", timeout=1) as result:
                            response = json.loads(result.read().decode("utf-8"))
                        break
                    except OSError:
                        time.sleep(0.1)
                self.assertEqual({"status": "ok"}, response)
            finally:
                api.stop()

            self.assertFalse(api.thread.is_alive())


if __name__ == "__main__":
    unittest.main()
