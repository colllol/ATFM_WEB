# -*- coding: utf-8 -*-
# Deploy ADSB_PERFORMANCE_PKG truc tiep vao database ATFM.
import oracledb
import re
import sys

from db_config import get_connection_info

def run_script(cursor, path):
    with open(path, encoding="utf-8") as f:
        text = f.read()
    # Bo cac lenh SQL*Plus
    text = re.sub(r"^SET\s+.*$", "", text, flags=re.MULTILINE)
    # Tach theo dong chi chua dau '/'
    blocks = re.split(r"^\s*/\s*$", text, flags=re.MULTILINE)
    for block in blocks:
        stmt = block.strip()
        if not stmt:
            continue
        # Cac cau SELECT kiem tra cuoi file: chay tung cau (ket thuc ';')
        if stmt.upper().startswith("SELECT"):
            for sel in [s.strip() for s in stmt.split(";") if s.strip()]:
                cursor.execute(sel)
                rows = cursor.fetchall()
                print("  SELECT ->", rows if rows else "(khong co dong)")
        else:
            cursor.execute(stmt)
            print("  OK:", stmt.splitlines()[0][:80] if not stmt.upper().startswith("--") else "block")

def print_dbms_output(cursor):
    line = cursor.var(str)
    status = cursor.var(int)
    while True:
        cursor.callproc("DBMS_OUTPUT.GET_LINE", (line, status))
        if status.getvalue() != 0:
            break
        print("  [DBMS_OUTPUT]", line.getvalue())

def main(path):
    user, password, dsn = get_connection_info()
    conn = oracledb.connect(user=user, password=password, dsn=dsn)
    cursor = conn.cursor()
    cursor.callproc("DBMS_OUTPUT.ENABLE", [None])
    try:
        run_script(cursor, path)
    finally:
        print_dbms_output(cursor)
    conn.commit()
    conn.close()
    print("HOAN TAT:", path)

if __name__ == "__main__":
    main(sys.argv[1])
