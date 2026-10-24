# -*- coding: utf-8 -*-
# Kiem tra bang/sequence/package phu thuoc truoc khi tao cac package moi.
import oracledb

from db_config import get_connection_info

user, password, dsn = get_connection_info()
conn = oracledb.connect(user=user, password=password, dsn=dsn)
cur = conn.cursor()

tables = ["T_FINISHED_FLIGHTS", "T_DAY_FLIGHTS_GOINGON", "T_DAY_FLIGHTS_CANCEL",
          "T_DAY_FLIGHTS", "T_DAY_FLIGHTS_GOINGON032024", "T_T_DAY_FLIGHTS_GOINGON",
          "T_KHH", "T_SLOT_AERO", "T_SLOT_COMPARE_RUN", "T_SLOT_COMPARE_RESULT",
          "M_OPER", "M_AERO"]
cur.execute("SELECT TABLE_NAME FROM USER_TABLES WHERE TABLE_NAME IN (%s)"
            % ",".join("'%s'" % t for t in tables))
found = {r[0] for r in cur.fetchall()}
for t in tables:
    print(("OK  " if t in found else "MISS") + " TABLE " + t)

cur.execute("SELECT SEQUENCE_NAME FROM USER_SEQUENCES WHERE SEQUENCE_NAME IN ('SEQ_SLOT_COMPARE_RUN','SEQ_SLOT_COMPARE_RESULT')")
print("Sequences:", [r[0] for r in cur.fetchall()])

cur.execute("SELECT OBJECT_NAME, OBJECT_TYPE, STATUS FROM USER_OBJECTS WHERE OBJECT_NAME IN ('KHBHDBN_COMPARE_PKG','PROCESS_PKG','NOTIFICATION_PKG') ORDER BY 1,2")
for r in cur.fetchall():
    print("PKG:", r)

conn.close()
