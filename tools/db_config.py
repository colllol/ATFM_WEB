# -*- coding: utf-8 -*-
# Doc thong tin ket noi Oracle cho cac script tools/.
# Uu tien bien moi truong ATFM_DB_USER / ATFM_DB_PASSWORD / ATFM_DB_DSN;
# neu thieu thi tu dong lay tu connection string SlotsOracle trong Web.config.
import os
import re


def get_connection_info():
    user = os.environ.get("ATFM_DB_USER")
    password = os.environ.get("ATFM_DB_PASSWORD")
    dsn = os.environ.get("ATFM_DB_DSN")
    if user and password and dsn:
        return user, password, dsn

    web_config = os.path.join(os.path.dirname(__file__), "..", "prjApplication", "Web.config")
    with open(web_config, encoding="utf-8") as f:
        content = f.read()
    match = re.search(
        r'name="SlotsOracle"[^>]*connectionString="Data Source=([^;]+);User Id=([^;]+);Password=([^;"]+);?"',
        content,
    )
    if not match:
        raise SystemExit("Khong tim thay connection string SlotsOracle trong Web.config "
                         "va thieu bien moi truong ATFM_DB_USER/ATFM_DB_PASSWORD/ATFM_DB_DSN.")
    return user or match.group(2), password or match.group(3), dsn or match.group(1)
