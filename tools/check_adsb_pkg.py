# -*- coding: utf-8 -*-
# Kiem tra du lieu that trong T_TRACKS_LOG va goi thu GET_LOGS.
import oracledb

from db_config import get_connection_info

user, password, dsn = get_connection_info()
conn = oracledb.connect(user=user, password=password, dsn=dsn)
cur = conn.cursor()
cur.execute(
    'SELECT COUNT(*), MIN(TO_DATE("DATE", \'DD-MM-YYYY\')), MAX(TO_DATE("DATE", \'DD-MM-YYYY\')) '
    "FROM T_TRACKS_LOG WHERE REGEXP_LIKE(\"DATE\", '^[0-9]{2}-[0-9]{2}-[0-9]{4}$')"
)
total, dmin, dmax = cur.fetchone()
print("Tong dong T_TRACKS_LOG:", total, "| Min:", dmin, "| Max:", dmax)

if total:
    c2 = conn.cursor()
    out = c2.var(oracledb.DB_TYPE_CURSOR)
    c2.callproc(
        "ADSB_PERFORMANCE_PKG.GET_LOGS",
        [dmin.strftime("%d-%m-%Y"), dmax.strftime("%d-%m-%Y"), "ALL", None, out],
    )
    rows = out.getvalue().fetchall()
    print("GET_LOGS toan khoang:", len(rows), "dong")
    if rows:
        print("Dong dau:", rows[0])

    c3 = conn.cursor()
    out2 = c3.var(oracledb.DB_TYPE_CURSOR)
    c3.callproc(
        "ADSB_PERFORMANCE_PKG.GET_OPERATORS",
        [dmin.strftime("%d-%m-%Y"), dmax.strftime("%d-%m-%Y"), None, out2],
    )
    opers = [r[0] for r in out2.getvalue().fetchall()]
    print("GET_OPERATORS toan khoang:", len(opers), "hang:", opers[:10])
conn.close()
