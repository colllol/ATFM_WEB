-- ============================================================================
-- Mo rong SEARCH ADV theo nhieu FROM, TO, VIA, PURPOSE.
--
-- API van dung cac tham so GET_FINISHED_FLIGHTS hien co:
--   FROM_AIRP / TO_AIRP / PURPOSE: |VALUE1|VALUE2|
--   VIA:                         |TOKEN1|TOKEN2|
-- Moi field nhieu gia tri la OR trong chinh field do; cac field khac la AND.
-- Neu khong co dau | o dau/cuoi, dieu kien tim kiem cu duoc giu nguyen.
-- Can trien khai sau 20260923_FINISHED_STATUS_multi_oper_search.sql.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

DECLARE
    c_package_name CONSTANT VARCHAR2(30) := 'FINISHED_STATUS_PKG';
    c_marker       CONSTANT VARCHAR2(100) :=
        '-- 2026-09-24: multi-FROM-TO-VIA-PURPOSE search';
    c_where_marker CONSTANT VARCHAR2(100) :=
        '-- 2026-09-24: multi-filter predicates';
    l_original      CLOB;
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

    PROCEDURE restore_original IS
    BEGIN
        parse_ddl(l_original);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('WARNING: khong restore duoc package cu: ' || SQLERRM);
    END restore_original;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM, 'SQLTERMINATOR', FALSE
    );
    l_original := DBMS_METADATA.GET_DDL('PACKAGE_BODY', c_package_name, USER);

    IF DBMS_LOB.INSTR(l_original, c_marker) > 0 THEN
        DBMS_OUTPUT.PUT_LINE('FINISHED_STATUS_PKG: da co multi FROM/TO/VIA/PURPOSE.');
    ELSE
        IF DBMS_LOB.INSTR(l_original, 'v_oper_id_list') = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20991,
                'Can trien khai multi-OPER search truoc script nay'
            );
        END IF;

        l_body := REGEXP_REPLACE(
            l_original,
            'v_oper_id_list[[:space:]]+VARCHAR2\(4000\);',
            'v_oper_id_list VARCHAR2(4000);' || CHR(10)
            || '        v_from_airp_list VARCHAR2(4000);' || CHR(10)
            || '        v_to_airp_list VARCHAR2(4000);' || CHR(10)
            || '        v_via_list VARCHAR2(4000);' || CHR(10)
            || '        v_purpose_list VARCHAR2(4000);',
            1, 1, 'c'
        );

        l_body := REGEXP_REPLACE(
            l_body,
            'v_from_airp[[:space:]]*:=[[:space:]]*NULLIF\(UPPER\(TRIM\(FROM_AIRP\)\),[[:space:]]*''''\);',
            'v_from_airp := NULLIF(UPPER(TRIM(FROM_AIRP)), '''');' || CHR(10)
            || '        ' || c_marker || CHR(10)
            || '        v_from_airp_list := NULL;' || CHR(10)
            || '        IF SUBSTR(v_from_airp, 1, 1) = ''|'' THEN' || CHR(10)
            || '            v_from_airp_list := TRIM(BOTH ''|'' FROM v_from_airp);' || CHR(10)
            || '            v_from_airp := NULL;' || CHR(10)
            || '        END IF;',
            1, 1, 'c'
        );
        l_body := REGEXP_REPLACE(
            l_body,
            'v_to_airp[[:space:]]*:=[[:space:]]*NULLIF\(UPPER\(TRIM\(TO_AIRP\)\),[[:space:]]*''''\);',
            'v_to_airp := NULLIF(UPPER(TRIM(TO_AIRP)), '''');' || CHR(10)
            || '        v_to_airp_list := NULL;' || CHR(10)
            || '        IF SUBSTR(v_to_airp, 1, 1) = ''|'' THEN' || CHR(10)
            || '            v_to_airp_list := TRIM(BOTH ''|'' FROM v_to_airp);' || CHR(10)
            || '            v_to_airp := NULL;' || CHR(10)
            || '        END IF;',
            1, 1, 'c'
        );
        l_body := REGEXP_REPLACE(
            l_body,
            'v_purpose[[:space:]]*:=[[:space:]]*NULLIF\(UPPER\(TRIM\(PURPOSE\)\),[[:space:]]*''''\);',
            'v_purpose := NULLIF(UPPER(TRIM(PURPOSE)), '''');' || CHR(10)
            || '        v_purpose_list := NULL;' || CHR(10)
            || '        IF SUBSTR(v_purpose, 1, 1) = ''|'' THEN' || CHR(10)
            || '            v_purpose_list := TRIM(BOTH ''|'' FROM v_purpose);' || CHR(10)
            || '            v_purpose := NULL;' || CHR(10)
            || '        END IF;',
            1, 1, 'c'
        );
        l_body := REGEXP_REPLACE(
            l_body,
            'v_via[[:space:]]*:=[[:space:]]*NULLIF\(UPPER\(TRIM\(VIA\)\),[[:space:]]*''''\);',
            'v_via := NULLIF(UPPER(TRIM(VIA)), '''');' || CHR(10)
            || '        v_via_list := NULL;' || CHR(10)
            || '        IF SUBSTR(v_via, 1, 1) = ''|'' THEN' || CHR(10)
            || '            v_via_list := TRIM(BOTH ''|'' FROM v_via);' || CHR(10)
            || '            v_via := NULL;' || CHR(10)
            || '        END IF;',
            1, 1, 'c'
        );

        IF REGEXP_INSTR(
            l_body,
            'v_from_airp_list[[:space:]]+IS[[:space:]]+NULL[[:space:]]+OR[[:space:]]+INSTR',
            1, 1, 0, 'c'
        ) = 0 THEN
            l_body := REGEXP_REPLACE(
                l_body,
                'AND[[:space:]]*\([[:space:]]*v_validhours[[:space:]]+IS[[:space:]]+NULL',
                'AND (' || CHR(10)
                || '                      ' || c_where_marker || CHR(10)
                || '                      v_from_airp_list IS NULL OR INSTR(' || CHR(10)
                || '                          ''|'' || v_from_airp_list || ''|'',' || CHR(10)
                || '                          ''|'' || UPPER(TRIM(t.FROM_AIRP)) || ''|'') > 0' || CHR(10)
                || '                  )' || CHR(10)
                || '                  AND (v_to_airp_list IS NULL OR INSTR(' || CHR(10)
                || '                           ''|'' || v_to_airp_list || ''|'',' || CHR(10)
                || '                           ''|'' || UPPER(TRIM(t.TO_AIRP)) || ''|'') > 0)' || CHR(10)
                || '                  AND (v_purpose_list IS NULL OR INSTR(' || CHR(10)
                || '                           ''|'' || v_purpose_list || ''|'',' || CHR(10)
                || '                           ''|'' || UPPER(TRIM(t.PURPOSE)) || ''|'') > 0)' || CHR(10)
                || '                  AND (v_via_list IS NULL OR EXISTS (' || CHR(10)
                || '                      SELECT 1 FROM (' || CHR(10)
                || '                          SELECT REGEXP_SUBSTR(v_via_list, ''[^|]+'', 1, LEVEL) VIA_TOKEN' || CHR(10)
                || '                            FROM DUAL' || CHR(10)
                || '                          CONNECT BY LEVEL <= REGEXP_COUNT(v_via_list, ''\|'') + 1' || CHR(10)
                || '                      ) via_tokens' || CHR(10)
                || '                       WHERE UPPER(t.VIA) LIKE ''%'' || via_tokens.VIA_TOKEN || ''%''))' || CHR(10)
                || '                  AND (v_validhours IS NULL',
                1, 1, 'c'
            );
        END IF;

        IF DBMS_LOB.INSTR(l_body, c_marker) = 0
           OR DBMS_LOB.INSTR(l_body, c_where_marker) = 0
           OR DBMS_LOB.INSTR(l_body, 'v_from_airp_list') = 0
           OR DBMS_LOB.INSTR(l_body, 'v_to_airp_list') = 0
           OR DBMS_LOB.INSTR(l_body, 'v_via_list') = 0
           OR DBMS_LOB.INSTR(l_body, 'v_purpose_list') = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20992,
                'Khong tao duoc patch multi-filter; package chua thay doi'
            );
        END IF;

        BEGIN
            parse_ddl(l_body);
        EXCEPTION
            WHEN OTHERS THEN
                restore_original;
                RAISE;
        END;
        DBMS_OUTPUT.PUT_LINE('FINISHED_STATUS_PKG: da them multi FROM/TO/VIA/PURPOSE.');
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
            -20993,
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
