-- ============================================================================
-- Sua loi scope tham so trong FINISHED_STATUS_PKG.GET_FINISHED_FLIGHTS va
-- FINISHED_COMPARE_PKG.COUNT_GOINGON.
--
-- FLIGHTNBR trung ten cot FLIGHTNBR cua bang. Trong SQL, Oracle hieu
-- FLIGHTNBR la cot, lam dieu kien LIKE tu so sanh mot cot voi chinh no.
-- Script them v_flight_nbr va dung bien nay cho dieu kien CALLSIGN.
-- Chay tren schema ATFM bang SQL*Plus/SQLcl hoac DBeaver Execute Script.
-- Co the chay lai an toan.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    c_declare_old CONSTANT VARCHAR2(1000) :=
        '        v_filter_date DATE;';
    c_declare_new CONSTANT VARCHAR2(1000) :=
        '        v_filter_date DATE;' || CHR(10)
        || '        v_flight_nbr   VARCHAR2(4000);';
    c_assignment_marker CONSTANT VARCHAR2(1000) :=
        '        IF TRIM(WHECONDITION) IS NOT NULL THEN' || CHR(10)
        || '            v_filter_date := PARSE_DATE(WHECONDITION);' || CHR(10)
        || '        END IF;';
    c_assignment_new CONSTANT VARCHAR2(1000) :=
        '        IF TRIM(WHECONDITION) IS NOT NULL THEN' || CHR(10)
        || '            v_filter_date := PARSE_DATE(WHECONDITION);' || CHR(10)
        || '        END IF;' || CHR(10) || CHR(10)
        || '        v_flight_nbr := NULLIF(UPPER(TRIM(FLIGHTNBR)), '''');';
    c_filter_old CONSTANT VARCHAR2(1000) :=
        '                         TRIM(FLIGHTNBR) IS NULL' || CHR(10)
        || '                         OR UPPER(t.FLIGHTNBR) LIKE' || CHR(10)
        || '                            ''%'' || UPPER(TRIM(FLIGHTNBR)) || ''%''';
    c_filter_exact CONSTANT VARCHAR2(1000) :=
        '                         UPPER(TRIM(t.FLIGHTNBR)) =' || CHR(10)
        || '                            UPPER(TRIM(FLIGHTNBR))';
    c_filter_prefix CONSTANT VARCHAR2(1000) :=
        '                         UPPER(t.FLIGHTNBR) LIKE' || CHR(10)
        || '                            UPPER(TRIM(FLIGHTNBR)) || ''%''';
    c_filter_new CONSTANT VARCHAR2(1000) :=
        '                         v_flight_nbr IS NULL' || CHR(10)
        || '                         OR UPPER(TRIM(t.FLIGHTNBR)) LIKE' || CHR(10)
        || '                            ''%'' || v_flight_nbr || ''%''';

    PROCEDURE execute_ddl(p_ddl IN CLOB) IS
        v_cursor INTEGER;
    BEGIN
        v_cursor := DBMS_SQL.OPEN_CURSOR;
        BEGIN
            DBMS_SQL.PARSE(v_cursor, p_ddl, DBMS_SQL.NATIVE);
            DBMS_SQL.CLOSE_CURSOR(v_cursor);
        EXCEPTION
            WHEN OTHERS THEN
                IF DBMS_SQL.IS_OPEN(v_cursor) THEN
                    DBMS_SQL.CLOSE_CURSOR(v_cursor);
                END IF;
                RAISE;
        END;
    END execute_ddl;

    PROCEDURE patch_body(p_package_name IN VARCHAR2) IS
        v_ddl CLOB;
    BEGIN
        v_ddl := DBMS_METADATA.GET_DDL('PACKAGE_BODY', p_package_name, USER);

        IF DBMS_LOB.INSTR(v_ddl, c_filter_new) > 0 THEN
            DBMS_OUTPUT.PUT_LINE(p_package_name || ': da sua scope FLIGHTNBR.');
            RETURN;
        END IF;

        IF DBMS_LOB.INSTR(v_ddl, c_declare_old) = 0
           OR DBMS_LOB.INSTR(v_ddl, c_assignment_marker) = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20931,
                p_package_name || ': khong tim thay marker source de cap nhat an toan'
            );
        END IF;

        v_ddl := REPLACE(v_ddl, c_declare_old, c_declare_new);
        v_ddl := REPLACE(v_ddl, c_assignment_marker, c_assignment_new);

        IF DBMS_LOB.INSTR(v_ddl, c_filter_old) > 0 THEN
            v_ddl := REPLACE(v_ddl, c_filter_old, c_filter_new);
        ELSIF DBMS_LOB.INSTR(v_ddl, c_filter_exact) > 0 THEN
            v_ddl := REPLACE(v_ddl, c_filter_exact, c_filter_new);
        ELSIF DBMS_LOB.INSTR(v_ddl, c_filter_prefix) > 0 THEN
            v_ddl := REPLACE(v_ddl, c_filter_prefix, c_filter_new);
        ELSE
            RAISE_APPLICATION_ERROR(
                -20933,
                p_package_name || ': khong tim thay dieu kien CALLSIGN de cap nhat'
            );
        END IF;
        execute_ddl(v_ddl);

        DBMS_OUTPUT.PUT_LINE(p_package_name || ': da cap nhat scope FLIGHTNBR.');
    END patch_body;

    v_invalid_count PLS_INTEGER;
    v_error_count   PLS_INTEGER;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    patch_body('FINISHED_STATUS_PKG');
    patch_body('FINISHED_COMPARE_PKG');

    SELECT COUNT(*)
      INTO v_invalid_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('FINISHED_STATUS_PKG', 'FINISHED_COMPARE_PKG')
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS <> 'VALID';

    SELECT COUNT(*)
      INTO v_error_count
      FROM USER_ERRORS
     WHERE NAME IN ('FINISHED_STATUS_PKG', 'FINISHED_COMPARE_PKG');

    IF v_invalid_count > 0 OR v_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20932,
            'Package compile khong thanh cong; invalid=' || v_invalid_count
            || ', errors=' || v_error_count
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: CALLSIGN dung gia tri tham so truyen vao.');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('FINISHED_STATUS_PKG', 'FINISHED_COMPARE_PKG')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT NAME, TYPE, LINE, POSITION, TEXT
  FROM USER_ERRORS
 WHERE NAME IN ('FINISHED_STATUS_PKG', 'FINISHED_COMPARE_PKG')
 ORDER BY NAME, SEQUENCE;

-- Kiem tra sau khi chay script (ket qua phai < tong ngay neu co loc HVN):
-- SELECT COUNT(*)
--   FROM T_FINISHED_FLIGHTS t
--  WHERE t.ISACCEPTED = 1
--    AND t.PERMTYPE = 'LD'
--    AND t.FLIGHTDATE >= DATE '2026-06-20'
--    AND t.FLIGHTDATE <  DATE '2026-06-21'
--    AND UPPER(TRIM(t.FLIGHTNBR)) LIKE '%HVN%';
