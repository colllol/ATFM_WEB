-- Rollback A_TEST_SEARCH.GetExportMOVEFINISH ve logic cu.
-- Chay tren schema ATFM bang DBeaver Alt+X.
-- Luu y: rollback khoi phuc viec loc UTC hien tai - 1 va SELECT *.

DECLARE
    c_package_name CONSTANT VARCHAR2(30) := 'A_TEST_SEARCH';
    c_start_marker CONSTANT VARCHAR2(100) :=
        'PROCEDURE GETEXPORTMOVEFINISH(';
    c_end_marker CONSTANT VARCHAR2(100) :=
        'END GETEXPORTMOVEFINISH;';

    v_original_body  CLOB;
    v_upper_body     CLOB;
    v_new_body       CLOB;
    v_implementation VARCHAR2(32767);
    v_start_pos      PLS_INTEGER;
    v_end_pos        PLS_INTEGER;
    v_suffix_pos     PLS_INTEGER;
    v_source_length  PLS_INTEGER;
    v_dest_offset    PLS_INTEGER;

    PROCEDURE execute_clob_ddl(p_ddl IN CLOB) IS
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
    END execute_clob_ddl;
BEGIN
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
        RAISE_APPLICATION_ERROR(-20901, 'Khong tim thay GetExportMOVEFINISH');
    END IF;

    v_implementation := q'~
Procedure GetExportMOVEFINISH
(
    P_USER       VARCHAR2,
    P_DATE       VARCHAR2,
    P_OUT_CURSOR OUT T_CURSOR
)
IS
    ax NUMBER;
BEGIN
    OPEN P_OUT_CURSOR FOR
        SELECT *
          FROM T_DAY_FLIGHTS_GOINGON
         WHERE MOVEFINISH = 1
           AND FLIGHTDATE = TO_CHAR(
                   SYS_EXTRACT_UTC(SYSTIMESTAMP) - 1,
                   'DD-MON-YY'
               );

    SELECT USERID
      INTO ax
      FROM T_USERS
     WHERE USERNAME = P_USER;

    INSERT INTO T_ACTIONHISTORY
    (
        USERID, FULLNAME, HOSTIP, DATEMODIFY,
        ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
    )
    VALUES
    (
        ax, P_USER, '', SYS_EXTRACT_UTC(SYSTIMESTAMP),
        'ExportMOVEFINISH', 0, 'ExportMOVEFINISH', 0
    );
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        PROCESS_PKG.ADD_ERROR_LOG(
            'GetExportMOVEFINISH',
            SQLCODE,
            SUBSTR(SQLERRM, 1, 200)
        );
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

    DBMS_OUTPUT.PUT_LINE('ROLLBACK OK: GetExportMOVEFINISH da ve logic cu.');
END;
/

-- Index khong anh huong logic cu. Neu chac chan khong can dung lai thi bo comment:
-- DROP INDEX IX_TDFG_MOVEFINISH_DATE;

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'A_TEST_SEARCH'
 ORDER BY OBJECT_TYPE;
