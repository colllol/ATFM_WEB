# -*- coding: utf-8 -*-
# Goi thu tat ca procedure cua cac package moi voi du lieu that (chi doc).
import datetime
import oracledb

from db_config import get_connection_info

user, password, dsn = get_connection_info()
conn = oracledb.connect(user=user, password=password, dsn=dsn)


def call(name, args):
    cur = conn.cursor()
    out = cur.var(oracledb.DB_TYPE_CURSOR)
    cur.callproc(name, list(args) + [out])
    rows = out.getvalue().fetchall()
    print("%-40s -> %d dong" % (name, len(rows)))
    return rows


today = datetime.date.today()
d7 = (today - datetime.timedelta(days=7)).strftime("%Y-%m-%d")
d1 = (today - datetime.timedelta(days=1)).strftime("%Y-%m-%d")
t = today.strftime("%Y-%m-%d")

# FLIGHT_STATUS_PKG
rows = call("FLIGHT_STATUS_PKG.GET_STATUS", [d7, d1, None, None, 0])
states = {}
for r in rows:
    states[r[10]] = states.get(r[10], 0) + 1
print("   trang thai:", states)
call("FLIGHT_STATUS_PKG.GET_STATUS", [t, t, None, None, 1])
call("FLIGHT_STATUS_PKG.GET_CANCELLED", [d7, d1])
call("FLIGHT_STATUS_PKG.GET_OPERATORS", [d7, t])

# DELAY_ALERT_PKG
call("DELAY_ALERT_PKG.GET_TODAY_FLIGHTS", [])

# DAYPLAN_COMPARE_PKG
call("DAYPLAN_COMPARE_PKG.GET_DAY_FLIGHTS", [t, None, None])
call("DAYPLAN_COMPARE_PKG.GET_AIRPORTS", [])
call("DAYPLAN_COMPARE_PKG.GET_OPERATORS", [])

# TRACKING_MAP_PKG
rows = call("TRACKING_MAP_PKG.GET_FLIGHT_META", [t, None])
if rows:
    subset = ",".join(sorted({r[0] for r in rows if r[0]})[:5])
    r2 = call("TRACKING_MAP_PKG.GET_FLIGHT_META", [t, subset])
    print("   loc theo 5 callsign dau:", subset, "->", len(r2), "dong")

# SLOT_COMPARE_PKG
rows = call("SLOT_COMPARE_PKG.GET_DEFAULT_DATE", [])
cmp_date = rows[0][0]
print("   ngay so sanh mac dinh:", cmp_date)
call("SLOT_COMPARE_PKG.GET_OPERATORS", [])
if cmp_date:
    ds = cmp_date.strftime("%Y-%m-%d")
    call("SLOT_COMPARE_PKG.GET_SUMMARY", [ds, None])
    call("SLOT_COMPARE_PKG.GET_RESULTS", [ds, "KQ1", None, 50, 0])
    call("SLOT_COMPARE_PKG.GET_SOURCE_KHH", [ds])
    call("SLOT_COMPARE_PKG.GET_SOURCE_SLOT", [ds])
    call("SLOT_COMPARE_PKG.GET_SOURCE_PERM", [ds])
call("SLOT_COMPARE_PKG.GET_OPERATOR_MAP", [])
call("SLOT_COMPARE_PKG.GET_AIRPORT_MAP", [])

conn.close()
print("SMOKE TEST HOAN TAT")
