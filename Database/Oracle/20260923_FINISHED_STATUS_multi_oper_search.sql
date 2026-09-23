-- ============================================================================
-- Ho tro SEARCH ADV theo nhieu OPER cho FINISHED_STATUS_PKG.GET_FINISHED_FLIGHTS.
--
-- Quy uoc API:
--   OPER_ID = |HVN|VJC|VNA|  => exact match bat ky OPER nao trong danh sach.
--   OPER_ID khac dinh dang tren => giu nguyen LIKE %OPER_ID% cu.
-- Khong doi chu ky package, khong anh huong client/chuc nang cu.
-- Chay bang schema ATFM qua SQL*Plus/SQLcl hoac DBeaver Execute Script.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

DECLARE
    c_package_name CONSTANT VARCHAR2(30) := 'FINISHED_STATUS_PKG';
    c_marker       CONSTANT VARCHAR2(100) :=
        '-- 2026-09-23: multi-OPER exact search';
    l_body          CLOB;
    l_cursor        INTEGER;
    l_error_count   PLS_INTEGER;
    l_invalid_count PLS_INTEGER;

    PROCEDURE parse_ddl(p_ddl IN CLOB) IS
    BEGIN
        l_cursor := DBMS_SQL.OPEN_CURSOR;
        DBMS_SQL.PARSE(l_cursor, p_ddl, DBMS_SQL.NATIVE);
        DBMS_SQL.CLOSE_CURSOR(l_cursor);
        l_cursor := NULL;
    EXCEPTION
        WHEN OTHERS THEN
            IF l_cursor IS NOT NULL AND DBMS_SQL.IS_OPEN(l_cursor) THEN
                DBMS_SQL.CLOSE_CURSOR(l_cursor);
            END IF;
            RAISE;
    END parse_ddl;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM, 'SQLTERMINATOR', FALSE
    );
    l_body := DBMS_METADATA.GET_DDL('PACKAGE_BODY', c_package_name, USER);

    IF DBMS_LOB.INSTR(l_body, c_marker) > 0 THEN
        DBMS_OUTPUT.PUT_LINE('FINISHED_STATUS_PKG: da co ho tro multi-OPER.');
    ELSE
        IF REGEXP_INSTR(
               l_body, 'v_oper_id[[:space:]]+VARCHAR2\(4000\);', 1, 1, 0, 'c'
           ) = 0
           OR REGEXP_INSTR(
               l_body,
               'v_oper_id[[:space:]]*:=[[:space:]]*NULLIF\(UPPER\(TRIM\(OPER_ID\)\),[[:space:]]*''''\);',
               1, 1, 0, 'c'
           ) = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20997,
                'Khong tim thay marker v_oper_id de patch an toan'
            );
        END IF;

        l_body := REGEXP_REPLACE(
            l_body,
            'v_oper_id[[:space:]]+VARCHAR2\(4000\);',
            'v_oper_id VARCHAR2(4000);' || CHR(10)
            || '        v_oper_id_list VARCHAR2(4000);',
            1, 1, 'c'
        );
        l_body := REGEXP_REPLACE(
            l_body,
            'v_oper_id[[:space:]]*:=[[:space:]]*NULLIF\(UPPER\(TRIM\(OPER_ID\)\),[[:space:]]*''''\);',
            'v_oper_id := NULLIF(UPPER(TRIM(OPER_ID)), '''');' || CHR(10)
            || '        ' || c_marker || CHR(10)
            || '        IF SUBSTR(v_oper_id, 1, 1) = ''|'' THEN' || CHR(10)
            || '            v_oper_id_list := TRIM(BOTH ''|'' FROM v_oper_id);' || CHR(10)
            || '        END IF;',
            1, 1, 'c'
        );

        l_body := REGEXP_REPLACE(
            l_body,
            'v_oper_id[[:space:]]+IS[[:space:]]+NULL[[:space:]]+OR[[:space:]]+'
            || 'UPPER\(t\.OPER_ID\)[[:space:]]+LIKE[[:space:]]+'
            || '''%''[[:space:]]+\|\|[[:space:]]+v_oper_id[[:space:]]+\|\|[[:space:]]+''%''',
            'v_oper_id IS NULL' || CHR(10)
            || '                            OR (v_oper_id_list IS NOT NULL' || CHR(10)
            || '                                AND INSTR(''|'' || v_oper_id_list || ''|'',' || CHR(10)
            || '                                    ''|'' || UPPER(TRIM(t.OPER_ID)) || ''|'') > 0)' || CHR(10)
            || '                            OR (v_oper_id_list IS NULL' || CHR(10)
            || '                                AND UPPER(t.OPER_ID) LIKE ''%'' || v_oper_id || ''%'')',
            1,
            1,
            'c'
        );

        IF DBMS_LOB.INSTR(l_body, c_marker) = 0
           OR DBMS_LOB.INSTR(l_body, 'v_oper_id_list') = 0 THEN
            RAISE_APPLICATION_ERROR(-20998, 'Patch multi-OPER khong thanh cong');
        END IF;

        parse_ddl(l_body);
        DBMS_OUTPUT.PUT_LINE('FINISHED_STATUS_PKG: da them ho tro multi-OPER.');
    END IF;

    SELECT COUNT(*)
      INTO l_invalid_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = c_package_name
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS <> 'VALID';
    SELECT COUNT(*)
      INTO l_error_count
      FROM USER_ERRORS
     WHERE NAME = c_package_name;

    IF l_invalid_count > 0 OR l_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20999,
            'FINISHED_STATUS_PKG khong VALID; invalid=' || l_invalid_count
            || ', errors=' || l_error_count
        );
    END IF;
    DBMS_OUTPUT.PUT_LINE('VERIFY OK: FINISHED_STATUS_PKG VALID.');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'FINISHED_STATUS_PKG'
 ORDER BY OBJECT_TYPE;
