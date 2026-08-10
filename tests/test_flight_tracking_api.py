import unittest
from datetime import datetime, timezone
from pathlib import Path
from tempfile import TemporaryDirectory

from tools.FlightTrackingApiPython import main


def config():
    value = main.deepcopy(main.DEFAULT_CONFIG)
    value["postgres"].update(
        host="db.internal",
        username="reader",
        password="secret",
    )
    return value


class FlightTrackingApiTests(unittest.TestCase):
    def test_live_does_not_require_api_key(self):
        app = main.create_app(config())
        response = app.test_client().get("/health/live")

        self.assertEqual(200, response.status_code)
        self.assertEqual({"status": "ok"}, response.get_json())

    def test_tracks_returns_expected_contract_without_api_key(self):
        now = datetime.now(timezone.utc)
        rows = [
            {
                "flightIdCurrent": "HVN123-20260810",
                "callsign": "HVN123",
                "updatedAtUtc": now.strftime("%Y-%m-%dT%H:%M:%S"),
                "latitude": 21.0285,
                "longitude": 105.8542,
                "heading": 180.0,
            }
        ]
        app = main.create_app(config(), track_loader=lambda _config, _date: rows)

        response = app.test_client().get(f"/api/v1/tracks?date={now:%Y-%m-%d}")
        payload = response.get_json()

        self.assertEqual(200, response.status_code)
        self.assertEqual(1, payload["count"])
        self.assertEqual("HVN123", payload["flights"][0]["callsign"])

    def test_tracks_rejects_invalid_date(self):
        app = main.create_app(config())
        response = app.test_client().get("/api/v1/tracks?date=10-08-2026")

        self.assertEqual(400, response.status_code)

    def test_ready_returns_503_when_database_is_unavailable(self):
        def fail(_config):
            raise RuntimeError("offline")

        app = main.create_app(config(), database_checker=fail)
        response = app.test_client().get("/health/ready")

        self.assertEqual(503, response.status_code)
        self.assertEqual({"status": "not_ready"}, response.get_json())

    def test_validate_config_requires_database_credentials(self):
        with self.assertRaisesRegex(ValueError, "host"):
            main.validate_config(main.deepcopy(main.DEFAULT_CONFIG))

    def test_loads_tracks_sync_shared_config(self):
        with TemporaryDirectory() as directory:
            path = Path(directory) / "TracksSync.local.json"
            path.write_text(
                '{"postgres":{"host":"db","username":"reader","password":"secret"},'
                '"flight_tracking_api":{"port":5099}}',
                encoding="utf-8",
            )

            loaded = main.load_config(path)

        self.assertEqual("db", loaded["postgres"]["host"])
        self.assertEqual(5099, loaded["flight_tracking_api"]["port"])


if __name__ == "__main__":
    unittest.main()
