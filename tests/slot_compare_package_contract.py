from pathlib import Path

source = Path("Database/Oracle/20260911_SLOT_COMPARE_PKG.sql").read_text(encoding="utf-8")
assert "ETD_ETA ETD, CARRIE OPER_HINT" in source
assert "ETD_ETA ETD, NVL(AERO, CARRIE) OPER_HINT" not in source
perm = source[source.rfind("PROCEDURE GET_SOURCE_PERM"):source.rfind("PROCEDURE GET_OPERATOR_MAP")]
assert "AND PERMNBR IS NOT NULL" in perm
assert "UPPER(TRIM(PERMNBR)) <> 'NOPERM'" in perm
print("PASS SLOT_COMPARE_PKG source contract checks")
