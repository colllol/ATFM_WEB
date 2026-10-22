import unittest
from datetime import date, datetime, timedelta, timezone
from unittest.mock import MagicMock, patch

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
    def test_swagger_ui_and_openapi_spec_are_available(self):
        app = main.create_app(config())
        client = app.test_client()

        redirect_response = client.get("/")
        ui_response = client.get("/swagger/")
        spec_response = client.get("/openapi.json")
        specification = spec_response.get_json()

        self.assertEqual(302, redirect_response.status_code)
        self.assertEqual("/swagger/", redirect_response.headers["Location"])
        self.assertEqual(200, ui_response.status_code)
        self.assertIn(b"swagger-ui", ui_response.data.lower())
        self.assertEqual(200, spec_response.status_code)
        self.assertIn("/health/live", specification["paths"])
        self.assertIn("/health/ready", specification["paths"])
        self.assertIn("/api/v1/tracks", specification["paths"])

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

    def test_tracks_date_is_vietnam_day_and_query_uses_utc_bounds(self):
        requested_day = date(2026, 10, 22)
        connection = MagicMock()
        cursor = MagicMock()
        connection.__enter__.return_value = connection
        connection.cursor.return_value.__enter__.return_value = cursor
        cursor.fetchall.return_value = []

        with patch.object(main.psycopg, "connect", return_value=connection):
            rows = main.fetch_latest_tracks(config(), requested_day)

        self.assertEqual([], rows)
        parameters = cursor.execute.call_args.args[1]
        self.assertEqual(datetime(2026, 10, 21, 17, 0), parameters[0])
        self.assertEqual(datetime(2026, 10, 22, 17, 0), parameters[1])

    def test_tracks_endpoint_accepts_vietnam_date_during_utc_previous_day(self):
        utc_now = datetime(2026, 10, 21, 18, 0, tzinfo=timezone.utc)
        local_day = (utc_now + timedelta(hours=7)).date()
        app = main.create_app(
            config(),
            track_loader=lambda _config, requested_date: [
                {"flightIdCurrent": "HVN123-20261022", "callsign": "HVN123",
                 "updatedAtUtc": "2026-10-22T00:30:00", "latitude": 21.0,
                 "longitude": 105.0, "heading": 180.0}
            ] if requested_date == local_day else [],
        )

        with patch.object(main, "datetime") as clock:
            clock.now.return_value = utc_now
            clock.strptime = datetime.strptime
            clock.combine = datetime.combine
            clock.min = datetime.min
            response = app.test_client().get(f"/api/v1/tracks?date={local_day:%Y-%m-%d}")

        self.assertEqual(200, response.status_code)
        self.assertEqual(local_day.strftime("%Y-%m-%d"), response.get_json()["day"])
        self.assertEqual(1, response.get_json()["count"])


if __name__ == "__main__":
    unittest.main()
