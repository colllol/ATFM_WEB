-- ============================================================================
-- DEPLOY SUA NGAT DONG VA RANH GIOI PART KHBH TREN ORACLE MAY CHU
-- Schema: ATFM
--
-- Chay bang SQL*Plus/SQLcl tai thu muc Database/Oracle:
--   sqlplus ATFM@//HOST:1521/SERVICE @20260923_KHBH_PART_boundary_server_deploy.sql
--
-- Yeu cau 2 file cung thu muc:
--   20260923_CATDONG69KYTU_word_wrap.sql
--   20260923_PERMISSION2TEXT_part_boundary.sql
--   20260923_GEN_OVER_FLIGHTS_new_v2.sql
--
-- Tac dong:
--   1. CATDONG69KYTU ngat toi da 69 ky tu theo ranh gioi tu.
--   2. PERMISSION2TEXT.FLIGHT2TEXT_new chi tach PART tai ranh gioi chuyen bay.
--   3. Over-flight cua FLIGHT2TEXT_new dung GEN_OVER_FLIGHTS_new_v2 toi uu.
-- Khong ghi/sua du lieu nghiep vu.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET ECHO ON
SET FEEDBACK ON
SET VERIFY OFF
SET TIMING ON
SET LINESIZE 240
SET PAGESIZE 200

WHENEVER OSERROR EXIT FAILURE ROLLBACK
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

SPOOL 20260923_KHBH_PART_boundary_server_deploy.log

PROMPT ========================================================================
PROMPT 1. PRECHECK
PROMPT ========================================================================

DECLARE
    l_missing VARCHAR2(4000);
    l_count   PLS_INTEGER;

    PROCEDURE require_valid_object
    (
        p_name IN VARCHAR2,
        p_type IN VARCHAR2
    ) IS
    BEGIN
        SELECT COUNT(*)
          INTO l_count
          FROM USER_OBJECTS
         WHERE OBJECT_NAME = UPPER(p_name)
           AND OBJECT_TYPE = UPPER(p_type)
           AND STATUS = 'VALID';

        IF l_count = 0 THEN
            l_missing := l_missing || CHR(10)
                || '- ' || UPPER(p_type) || ' ' || UPPER(p_name);
        END IF;
    END require_valid_object;
BEGIN
    require_valid_object('P_FLY', 'PACKAGE');
    require_valid_object('P_FLY', 'PACKAGE BODY');
    require_valid_object('PERMISSION2TEXT', 'PACKAGE');
    require_valid_object('PERMISSION2TEXT', 'PACKAGE BODY');
    require_valid_object('CHUANDONG69KYTU', 'FUNCTION');

    IF l_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20981,
            'Thieu doi tuong phu thuoc:' || SUBSTR(l_missing, 1, 1800)
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('PRECHECK OK. Schema=' || USER);
END;
/

PROMPT ========================================================================
PROMPT 2. CAP NHAT HAM CATDONG69KYTU VA BIEN DICH P_FLY
PROMPT ========================================================================

@@20260923_CATDONG69KYTU_word_wrap.sql

PROMPT ========================================================================
PROMPT 3. CAP NHAT PERMISSION2TEXT.FLIGHT2TEXT_NEW
PROMPT ========================================================================

@@20260923_PERMISSION2TEXT_part_boundary.sql

PROMPT ========================================================================
PROMPT 4. CAP NHAT GEN_OVER_FLIGHTS_NEW_V2 CHO OVER-FLIGHT
PROMPT ========================================================================

@@20260923_GEN_OVER_FLIGHTS_new_v2.sql

PROMPT ========================================================================
PROMPT 5. VERIFY CUOI CUNG
PROMPT ========================================================================

DECLARE
    l_invalid_count PLS_INTEGER;
    l_error_count   PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO l_invalid_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('CATDONG69KYTU', 'P_FLY', 'PERMISSION2TEXT')
       AND STATUS <> 'VALID';

    SELECT COUNT(*)
      INTO l_error_count
      FROM USER_ERRORS
     WHERE NAME IN ('CATDONG69KYTU', 'P_FLY', 'PERMISSION2TEXT');

    IF l_invalid_count > 0 OR l_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20982,
            'Verify khong dat: invalid=' || l_invalid_count
            || ', compile_errors=' || l_error_count
        );
    END IF;

    SELECT COUNT(*)
      INTO l_error_count
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'P_FLY'
       AND OBJECT_NAME = 'GEN_OVER_FLIGHTS_NEW_V2'
       AND ARGUMENT_NAME IN ('DATE_FLY', 'PARTS_NUMBER', 'PARTS_CONTENT', 'PART_ID');

    IF l_error_count <> 4 THEN
        RAISE_APPLICATION_ERROR(
            -20983,
            'Khong tim thay du chu ky P_FLY.GEN_OVER_FLIGHTS_NEW_V2'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: CATDONG69KYTU, P_FLY, PERMISSION2TEXT deu VALID; V2 OK.'
    );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('CATDONG69KYTU', 'P_FLY', 'PERMISSION2TEXT')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

PROMPT ========================================================================
PROMPT DEPLOY KHBH PART BOUNDARY HOAN TAT
PROMPT Log: 20260923_KHBH_PART_boundary_server_deploy.log
PROMPT ========================================================================

SPOOL OFF
EXIT SUCCESS
