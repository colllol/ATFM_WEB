-- ============================================================================
-- Toi uu A_TEST_SEARCH.GetExportMOVEFINISH va loc dung ngay P_DATE.
-- Oracle 12.1/19c; chay tren schema ATFM bang DBeaver Alt+X.
--
-- P_DATE co dinh dang DD-MM-YYYY.
-- Script khong dung lenh SET cua SQL*Plus de tuong thich Oracle/JDBC.
-- Package specification da co dung chu ky:
--   GetExportMOVEFINISH(P_USER VARCHAR2, P_DATE VARCHAR2, P_OUT_CURSOR OUT T_CURSOR)
-- ============================================================================

DECLARE
    c_package_name CONSTANT VARCHAR2(30) := 'A_TEST_SEARCH';
    c_start_marker CONSTANT VARCHAR2(100) :=
        'PROCEDURE GETEXPORTMOVEFINISH(';
    c_end_marker CONSTANT VARCHAR2(100) :=
        'END GETEXPORTMOVEFINISH;';
    c_index_name CONSTANT VARCHAR2(30) :=
        'IX_TDFG_MOVEFINISH_DATE';

    v_original_body  CLOB;
    v_upper_body     CLOB;
    v_new_body       CLOB;
    v_implementation VARCHAR2(32767);
    v_start_pos      PLS_INTEGER;
    v_end_pos        PLS_INTEGER;
    v_suffix_pos     PLS_INTEGER;
    v_source_length  PLS_INTEGER;
    v_dest_offset    PLS_INTEGER;
    v_error_count    PLS_INTEGER;
    v_count          PLS_INTEGER;
    v_package_changed BOOLEAN := FALSE;
    v_index_created   BOOLEAN := FALSE;

    PROCEDURE execute_clob_ddl(p_ddl IN CLOB) IS
        v_cursor INTEGER;
    BEGIN
        v_cursor := DBMS_SQL.OPEN_CURSOR;
        BEGIN
            -- Voi DDL, DBMS_SQL.PARSE thuc thi cau lenh ngay khi parse.
            DBMS_SQL.PARSE(v_cursor, p_ddl, DBMS_SQL.NATIVE);
            DBMS_SQL.CLOSE_CURSOR(v_cursor);
        EXCEPTION
            WHEN OTHERS THEN
                IF DBMS_SQL.IS_OPEN(v_cursor) THEN
                    DBMS_SQL.CLOSE_CURSOR(v_cursor);
                END IF;
                RAISE;
        END;
    END execute_clob_ddl;

    PROCEDURE restore_original_package IS
    BEGIN
        IF v_package_changed AND v_original_body IS NOT NULL THEN
            execute_clob_ddl(v_original_body);
            v_package_changed := FALSE;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(
                'WARNING: Khong khoi phuc duoc A_TEST_SEARCH: ' || SQLERRM
            );
    END restore_original_package;
BEGIN
    -- Precheck de tranh thay nham package/schema.
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = c_package_name
       AND OBJECT_TYPE = 'PACKAGE BODY'
       AND STATUS = 'VALID';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20891, 'A_TEST_SEARCH package body khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
       AND COLUMN_NAME IN ('MOVEFINISH', 'FLIGHTDATE');

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20892,
            'Thieu cot T_DAY_FLIGHTS_GOINGON.MOVEFINISH/FLIGHTDATE'
        );
    END IF;

    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    v_original_body := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY',
        c_package_name,
        USER
    );
    v_upper_body := UPPER(v_original_body);

    v_start_pos := DBMS_LOB.INSTR(v_upper_body, c_start_marker, 1, 1);
    v_end_pos := DBMS_LOB.INSTR(
        v_upper_body,
        c_end_marker,
        v_start_pos,
        1
    );

    IF v_start_pos = 0 OR v_end_pos = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20893,
            'Khong tim thay GetExportMOVEFINISH trong package body'
        );
    END IF;

    v_implementation := q'~
Procedure GetExportMOVEFINISH
(
    P_USER       VARCHAR2,
    P_DATE       VARCHAR2,
    P_OUT_CURSOR OUT T_CURSOR
)
IS
    v_user_id     T_USERS.USERID%TYPE;
    v_flight_date DATE;
BEGIN
    IF TRIM(P_DATE) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'P_DATE is required');
    END IF;

    BEGIN
        v_flight_date := TO_DATE(TRIM(P_DATE), 'FXDD-MM-YYYY');
    EXCEPTION
        WHEN OTHERS THEN
            RAISE_APPLICATION_ERROR(
                -20002,
                'P_DATE must use DD-MM-YYYY format'
            );
    END;

    v_flight_date := TRUNC(v_flight_date);

    OPEN P_OUT_CURSOR FOR
        SELECT
            g.OPER_ID,
            g.FLIGHTNBR,
            g.REGISTRATION,
            g.CRAFT_TYPE,
            g.PURPOSE,
            g.PERMTYPE,
            g.FROM_AIRP,
            g.TO_AIRP,
            g.FLIGHTDATE,
            g.ATD,
            g.ATA,
            g.VIA,
            g.REMARK,
            g.LASTUSER,
            g.ETD,
            g.ETA,
            g.EOBT
        FROM T_DAY_FLIGHTS_GOINGON g
        WHERE g.MOVEFINISH = 1
          AND g.FLIGHTDATE >= v_flight_date
          AND g.FLIGHTDATE <  v_flight_date + 1
        ORDER BY g.OPER_ID, g.FLIGHTNBR, g.FLIGHT_ID;

    -- T_USERS hien rat nho; MAX tranh NO_DATA_FOUND lam hong luong export.
    SELECT MAX(u.USERID)
      INTO v_user_id
      FROM T_USERS u
     WHERE UPPER(TRIM(u.USERNAME)) = UPPER(TRIM(P_USER));

    IF v_user_id IS NOT NULL THEN
        INSERT INTO T_ACTIONHISTORY
        (
            USERID,
            FULLNAME,
            HOSTIP,
            DATEMODIFY,
            ACTIONSCODE,
            NEWS_ID,
            NOTES,
            MENU_ID
        )
        VALUES
        (
            v_user_id,
            P_USER,
            '',
            SYS_EXTRACT_UTC(SYSTIMESTAMP),
            'ExportMOVEFINISH',
            0,
            'FLIGHTDATE=' || TO_CHAR(v_flight_date, 'DD-MM-YYYY'),
            0
        );

        COMMIT;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'GetExportMOVEFINISH',
                SQLCODE,
                SUBSTR(
                    SQLERRM || CHR(10) ||
                    DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                    1,
                    200
                )
            );
        EXCEPTION
            WHEN OTHERS THEN
                NULL;
        END;
        RAISE;
END GetExportMOVEFINISH;~';

    v_source_length := DBMS_LOB.GETLENGTH(v_original_body);
    v_suffix_pos := v_end_pos + LENGTH(c_end_marker);

    DBMS_LOB.CREATETEMPORARY(v_new_body, TRUE);

    IF v_start_pos > 1 THEN
        DBMS_LOB.COPY(
            v_new_body,
            v_original_body,
            v_start_pos - 1,
            1,
            1
        );
    END IF;

    DBMS_LOB.WRITEAPPEND(
        v_new_body,
        LENGTH(v_implementation),
        v_implementation
    );

    IF v_suffix_pos <= v_source_length THEN
        v_dest_offset := DBMS_LOB.GETLENGTH(v_new_body) + 1;
        DBMS_LOB.COPY(
            v_new_body,
            v_original_body,
            v_source_length - v_suffix_pos + 1,
            v_dest_offset,
            v_suffix_pos
        );
    END IF;

    execute_clob_ddl(v_new_body);
    v_package_changed := TRUE;

    SELECT COUNT(*)
      INTO v_error_count
      FROM USER_ERRORS
     WHERE NAME = c_package_name
       AND TYPE = 'PACKAGE BODY';

    IF v_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20894,
            'A_TEST_SEARCH con ' || v_error_count || ' loi bien dich'
        );
    END IF;

    -- Chi tao index khi chua co index bat dau bang (MOVEFINISH, FLIGHTDATE).
    SELECT COUNT(*)
      INTO v_count
      FROM
      (
          SELECT c.INDEX_NAME
            FROM USER_IND_COLUMNS c
           WHERE c.TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
           GROUP BY c.INDEX_NAME
          HAVING MAX(
                     CASE
                         WHEN c.COLUMN_POSITION = 1
                          AND c.COLUMN_NAME = 'MOVEFINISH'
                         THEN 1 ELSE 0
                     END
                 ) = 1
             AND MAX(
                     CASE
                         WHEN c.COLUMN_POSITION = 2
                          AND c.COLUMN_NAME = 'FLIGHTDATE'
                         THEN 1 ELSE 0
                     END
                 ) = 1
      );

    IF v_count = 0 THEN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_INDEXES
         WHERE INDEX_NAME = c_index_name;

        IF v_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20895,
                'Index name ' || c_index_name || ' da duoc su dung'
            );
        END IF;

        EXECUTE IMMEDIATE
            'CREATE INDEX ' || c_index_name ||
            ' ON T_DAY_FLIGHTS_GOINGON (MOVEFINISH, FLIGHTDATE) ONLINE';
        v_index_created := TRUE;

        DBMS_STATS.GATHER_INDEX_STATS(USER, c_index_name);
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'DEPLOY OK: A_TEST_SEARCH.GetExportMOVEFINISH loc theo P_DATE.'
    );
    IF v_index_created THEN
        DBMS_OUTPUT.PUT_LINE('CREATED INDEX: ' || c_index_name);
    ELSE
        DBMS_OUTPUT.PUT_LINE('INDEX OK: da co index tuong duong.');
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        IF v_index_created THEN
            BEGIN
                EXECUTE IMMEDIATE 'DROP INDEX ' || c_index_name;
            EXCEPTION
                WHEN OTHERS THEN
                    NULL;
            END;
        END IF;

        restore_original_package;
        RAISE;
END;
/

-- Verify package, chu ky API va index.
DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'A_TEST_SEARCH'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(-20896, 'A_TEST_SEARCH khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'A_TEST_SEARCH'
       AND OBJECT_NAME = 'GETEXPORTMOVEFINISH'
       AND ARGUMENT_NAME IN ('P_USER', 'P_DATE', 'P_OUT_CURSOR');

    IF v_count <> 3 THEN
        RAISE_APPLICATION_ERROR(-20897, 'Sai chu ky GetExportMOVEFINISH');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM
      (
          SELECT c.INDEX_NAME
            FROM USER_IND_COLUMNS c
           WHERE c.TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
           GROUP BY c.INDEX_NAME
          HAVING MAX(
                     CASE
                         WHEN c.COLUMN_POSITION = 1
                          AND c.COLUMN_NAME = 'MOVEFINISH'
                         THEN 1 ELSE 0
                     END
                 ) = 1
             AND MAX(
                     CASE
                         WHEN c.COLUMN_POSITION = 2
                          AND c.COLUMN_NAME = 'FLIGHTDATE'
                         THEN 1 ELSE 0
                     END
                 ) = 1
      );

    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20898, 'Chua co index MOVEFINISH, FLIGHTDATE');
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: package/body, arguments va index hop le.');
END;
/

SELECT POSITION, ARGUMENT_NAME, IN_OUT, DATA_TYPE
  FROM USER_ARGUMENTS
 WHERE PACKAGE_NAME = 'A_TEST_SEARCH'
   AND OBJECT_NAME = 'GETEXPORTMOVEFINISH'
 ORDER BY SEQUENCE;

SELECT i.INDEX_NAME,
       i.STATUS,
       c.COLUMN_POSITION,
       c.COLUMN_NAME
  FROM USER_INDEXES i
  JOIN USER_IND_COLUMNS c
    ON c.INDEX_NAME = i.INDEX_NAME
   AND c.TABLE_NAME = i.TABLE_NAME
 WHERE i.TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
   AND c.COLUMN_NAME IN ('MOVEFINISH', 'FLIGHTDATE')
 ORDER BY i.INDEX_NAME, c.COLUMN_POSITION;
