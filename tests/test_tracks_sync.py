import unittest
from unittest.mock import patch

from tools.TracksSyncPython import main


class FakeCursor:
    def __init__(self, rows=None, rowcount=0, one=(1,)):
        self.rows = rows or []
        self.rowcount = rowcount
        self.one = one
        self.executed = []
        self.batch_sql = ""
        self.batch_data = []
        self.closed = False

    def execute(self, sql, params=None):
        self.executed.append((sql, params))

    def executemany(self, sql, data):
        self.batch_sql = sql
        self.batch_data = data

    def fetchone(self):
        return self.one

    def fetchall(self):
        return self.rows

    def close(self):
        self.closed = True


class FakeConnection:
    def __init__(self, cursor):
        self.fake_cursor = cursor
        self.commits = 0

    def cursor(self):
        return self.fake_cursor

    def commit(self):
        self.commits += 1


def candidate(update_text, lat, lon):
    return main.TrackCandidate(
        callsign="HVN123",
        flight_date="23-07-2026",
        update_text=update_text,
        status=1,
        lat=lat,
        lon=lon,
    )


def flight():
    return main.FlightInfo(
        callsign="HVN123",
        flight_date="23-07-2026",
        from_airp="VVNB",
        to_airp="VVTS",
        etd="0100",
        eta="0300",
        permtype="LD",
    )


def log_row(lat=21.0285, lon=105.8542):
    return main.LogRow(
        callsign="HVN123",
        from_airp="VVNB",
        to_airp="VVTS",
        etd="0100",
        eta="0300",
        status=1,
        flight_date="23-07-2026",
        updated_at_utc="2026-07-23 01:02:03",
        permtype="LD",
        lat=lat,
        lon=lon,
    )


class TracksSyncTests(unittest.TestCase):
    def test_dedupe_tracks_keeps_latest_coordinates(self):
        rows = [
            candidate("2026-07-23 01:00:00", 20.0, 105.0),
            candidate("2026-07-23 01:05:00", 21.0, 106.0),
        ]

        result = main.dedupe_tracks(rows)

        self.assertEqual(1, len(result))
        self.assertEqual((21.0, 106.0), (result[0].lat, result[0].lon))

    def test_build_log_rows_copies_coordinates(self):
        track = candidate("2026-07-23 01:05:00", 21.25, 106.75)

        rows = main.build_log_rows([track], {(track.callsign, track.flight_date): flight()})

        self.assertEqual(1, len(rows))
        self.assertEqual((21.25, 106.75), (rows[0].lat, rows[0].lon))

    def test_ensure_schema_registers_lat_and_lon_migrations(self):
        cursor = FakeCursor()
        conn = FakeConnection(cursor)

        with patch.object(main, "add_column_if_missing") as add_column:
            main.ensure_schema(conn)

        calls = [call.args for call in add_column.call_args_list]
        self.assertIn((cursor, "T_TRACKS_LOG", "LAT", "LAT NUMBER"), calls)
        self.assertIn((cursor, "T_TRACKS_LOG", "LON", "LON NUMBER"), calls)
        create_table_sql = cursor.executed[0][0]
        self.assertIn("LAT NUMBER NOT NULL", create_table_sql)
        self.assertIn("LON NUMBER NOT NULL", create_table_sql)
        self.assertEqual(1, conn.commits)

    def test_enforce_coordinate_constraints_keeps_legacy_null_rows(self):
        cursor = FakeCursor(rows=[("LAT", "Y"), ("LON", "Y")], one=(3,))
        conn = FakeConnection(cursor)

        remaining_null = main.enforce_coordinate_constraints(conn)

        statements = [sql for sql, _ in cursor.executed]
        self.assertEqual(3, remaining_null)
        self.assertEqual(1, len(statements))
        self.assertIn("SELECT COUNT(*)", statements[0])
        self.assertFalse(any(sql.startswith("DELETE") for sql in statements))
        self.assertFalse(any(sql.startswith("ALTER TABLE") for sql in statements))
        self.assertEqual(0, conn.commits)
        self.assertTrue(cursor.closed)

    def test_enforce_coordinate_constraints_sets_not_null_when_complete(self):
        cursor = FakeCursor(rows=[("LAT", "Y"), ("LON", "Y")], one=(0,))
        conn = FakeConnection(cursor)

        remaining_null = main.enforce_coordinate_constraints(conn)

        statements = [sql for sql, _ in cursor.executed]
        self.assertEqual(0, remaining_null)
        self.assertIn("ALTER TABLE T_TRACKS_LOG MODIFY (LAT NOT NULL, LON NOT NULL)", statements[2])
        self.assertEqual(1, conn.commits)
        self.assertTrue(cursor.closed)

    def test_enforce_coordinate_constraints_is_idempotent(self):
        cursor = FakeCursor(rows=[("LAT", "N"), ("LON", "N")], one=(0,))
        conn = FakeConnection(cursor)

        remaining_null = main.enforce_coordinate_constraints(conn)

        statements = [sql for sql, _ in cursor.executed]
        self.assertEqual(0, remaining_null)
        self.assertFalse(any(sql.startswith("ALTER TABLE") for sql in statements))
        self.assertEqual(0, conn.commits)
        self.assertTrue(cursor.closed)

    def test_merge_updates_only_coordinates_and_inserts_all_fields(self):
        cursor = FakeCursor()
        conn = FakeConnection(cursor)
        row = log_row()

        written = main.merge_logs(conn, [row])

        matched_sql = cursor.batch_sql.split("WHEN MATCHED THEN UPDATE SET", 1)[1].split(
            "WHEN NOT MATCHED", 1
        )[0]
        self.assertIn("target.LAT = source.LAT", matched_sql)
        self.assertIn("target.LON = source.LON", matched_sql)
        for preserved_column in (
            "target.FROM_AIRP",
            "target.TO_AIRP",
            "target.ETD",
            "target.ETA",
            "target.STATUS",
            "target.UPDATED_AT_UTC",
            "target.PERMTYPE",
        ):
            self.assertNotIn(preserved_column, matched_sql)
        self.assertIn("PERMTYPE, LAT, LON", cursor.batch_sql)
        self.assertEqual(21.0285, cursor.batch_data[0]["lat"])
        self.assertEqual(105.8542, cursor.batch_data[0]["lon"])
        self.assertEqual(1, written)
        self.assertEqual(1, conn.commits)
        self.assertTrue(cursor.closed)

    def test_read_existing_log_keys_returns_only_requested_keys(self):
        cursor = FakeCursor(
            rows=[
                ("HVN123", "23-07-2026"),
                ("hvn123", "23-07-2026"),
                ("HVN123 ", "23-07-2026"),
                ("HVN123-OLD", "23-07-2026"),
                ("OTHER", "23-07-2026"),
            ]
        )
        conn = FakeConnection(cursor)

        result = main.read_existing_log_keys(conn, [log_row()])

        self.assertEqual({("HVN123", "23-07-2026")}, result)

    def test_verify_existing_row_compares_latest_coordinates_only(self):
        cursor = FakeCursor(
            rows=[
                (
                    "HVN123",
                    "23-07-2026",
                    "OLD_FROM",
                    "OLD_TO",
                    "OLD_ETD",
                    "OLD_ETA",
                    2,
                    "OLD_UPDATED",
                    "O/F",
                    21.0285,
                    105.8542,
                )
            ]
        )
        conn = FakeConnection(cursor)
        sink = main.MessageSink()
        messages = []
        sink.write = messages.append

        main.verify_logs(conn, [log_row()], sink, report_extra=False)

        self.assertIn("dung 1, sai 0, thieu 0", messages[-1])

    def test_verify_inserted_row_compares_all_fields(self):
        cursor = FakeCursor(
            rows=[
                (
                    "HVN123",
                    "23-07-2026",
                    "WRONG_FROM",
                    "VVTS",
                    "0100",
                    "0300",
                    1,
                    "2026-07-23 01:02:03",
                    "LD",
                    21.0285,
                    105.8542,
                )
            ]
        )
        conn = FakeConnection(cursor)
        sink = main.MessageSink()
        messages = []
        sink.write = messages.append
        key = ("HVN123", "23-07-2026")

        main.verify_logs(conn, [log_row()], sink, report_extra=False, full_row_keys={key})

        self.assertIn("dung 0, sai 1, thieu 0", messages[-1])

    def test_verify_rejects_null_coordinates(self):
        cursor = FakeCursor(
            rows=[
                (
                    "HVN123",
                    "23-07-2026",
                    "VVNB",
                    "VVTS",
                    "0100",
                    "0300",
                    1,
                    "2026-07-23 01:02:03",
                    "LD",
                    None,
                    105.8542,
                )
            ]
        )
        conn = FakeConnection(cursor)
        sink = main.MessageSink()
        messages = []
        sink.write = messages.append

        main.verify_logs(conn, [log_row()], sink, report_extra=False)

        self.assertIn("dung 0, sai 1, thieu 0", messages[-1])


if __name__ == "__main__":
    unittest.main()
