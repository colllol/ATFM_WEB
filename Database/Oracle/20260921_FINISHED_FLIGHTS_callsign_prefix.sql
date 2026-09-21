-- ============================================================================
-- Dong bo quy tac tim CALLSIGN cua danh sach Finished Flights.
--
-- Truoc: LIKE '%' || gia_tri || '%' (co the khop CALLSIGN co gia tri o giua chuoi).
-- Sau : so sanh bang, bo qua khoang trang va phan biet hoa/thuong.
--       AAR736 chi khop AAR736, khong khop AAR7361/AAR1736.
--
-- Cap nhat ca FINISHED_STATUS_PKG (danh sach) va FINISHED_COMPARE_PKG
-- (doi chieu GOINGON) de hai so lieu dung cung dieu kien.
-- Chay bang SQL*Plus/SQLcl hoac DBeaver Execute Script tren schema ATFM.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    c_old_filter CONSTANT VARCHAR2(1000) :=
        'UPPER(t.FLIGHTNBR) LIKE' || CHR(10)
        || '                            ''%'' || UPPER(TRIM(FLIGHTNBR)) || ''%''';
    c_new_filter CONSTANT VARCHAR2(1000) :=
        'UPPER(TRIM(t.FLIGHTNBR)) =' || CHR(10)
        || '                            UPPER(TRIM(FLIGHTNBR))';
    c_prefix_filter CONSTANT VARCHAR2(1000) :=
        'UPPER(t.FLIGHTNBR) LIKE' || CHR(10)
        || '                            UPPER(TRIM(FLIGHTNBR)) || ''%''';

    PROCEDURE parse_ddl(p_ddl IN CLOB) IS
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
    END parse_ddl;

    PROCEDURE update_package_body(p_package_name IN VARCHAR2) IS
        v_ddl CLOB;
    BEGIN
        v_ddl := DBMS_METADATA.GET_DDL('PACKAGE_BODY', p_package_name, USER);

        IF DBMS_LOB.INSTR(v_ddl, c_new_filter) > 0 THEN
            DBMS_OUTPUT.PUT_LINE(p_package_name || ': da dung dieu kien CALLSIGN chinh xac.');
            RETURN;
        END IF;

        IF DBMS_LOB.INSTR(v_ddl, c_old_filter) > 0 THEN
            v_ddl := REPLACE(v_ddl, c_old_filter, c_new_filter);
        ELSIF DBMS_LOB.INSTR(v_ddl, c_prefix_filter) > 0 THEN
            v_ddl := REPLACE(v_ddl, c_prefix_filter, c_new_filter);
        ELSE
            RAISE_APPLICATION_ERROR(
                -20921,
                p_package_name || ': khong tim thay dieu kien CALLSIGN cu de cap nhat'
            );
        END IF;

        parse_ddl(v_ddl);

        DBMS_OUTPUT.PUT_LINE(p_package_name || ': da cap nhat CALLSIGN chinh xac.');
    END update_package_body;

    v_invalid_count PLS_INTEGER;
    v_error_count   PLS_INTEGER;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    update_package_body('FINISHED_STATUS_PKG');
    update_package_body('FINISHED_COMPARE_PKG');

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
            -20922,
            'Package sau cap nhat khong VALID; invalid='
            || v_invalid_count || ', compile errors=' || v_error_count
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: 2 package VALID; CALLSIGN dung exact search.');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('FINISHED_STATUS_PKG', 'FINISHED_COMPARE_PKG')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

-- Ket qua mong doi khi nhap AAR736: chi CALLSIGN bang AAR736.
-- SELECT FLIGHTNBR
--   FROM T_FINISHED_FLIGHTS
--  WHERE ISACCEPTED = 1
--    AND UPPER(TRIM(FLIGHTNBR)) = 'AAR736';
