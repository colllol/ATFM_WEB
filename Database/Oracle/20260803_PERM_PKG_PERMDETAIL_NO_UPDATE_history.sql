-- Toi uu transaction cua PERMDETAIL_NO_UPDATE sau khi da co trigger lich su.
-- ConvertPermNoToSchedule goi spRenderSchedule_No co COMMIT noi bo;
-- script nay giu nguyen hanh vi do de tranh anh huong chuc nang khac.

SET SERVEROUTPUT ON;

DECLARE
    l_ddl          CLOB;
    l_cursor       INTEGER;
    l_scope_marker VARCHAR2(100) := 'PROCEDURE PERMDETAIL_NO_UPDATE(';
    l_new_marker   VARCHAR2(100) := '-- Per-flight changes are audited by TRG_T_PERMDETAIL_NO_ACTION_HIS.';

    PROCEDURE REPLACE_BETWEEN
    (
        io_text        IN OUT NOCOPY CLOB,
        p_scope_marker IN VARCHAR2,
        p_start_marker IN VARCHAR2,
        p_end_marker   IN VARCHAR2,
        p_replacement  IN VARCHAR2
    )
    IS
        l_scope_pos  PLS_INTEGER;
        l_start_pos  PLS_INTEGER;
        l_end_pos    PLS_INTEGER;
        l_prefix_len PLS_INTEGER;
        l_suffix_len PLS_INTEGER;
        l_new_text   CLOB;
    BEGIN
        l_scope_pos := DBMS_LOB.INSTR(io_text, p_scope_marker, 1, 1);
        l_start_pos := DBMS_LOB.INSTR(io_text, p_start_marker, l_scope_pos, 1);
        l_end_pos := DBMS_LOB.INSTR(io_text, p_end_marker, l_start_pos, 1);

        IF l_scope_pos = 0 OR l_start_pos = 0 OR l_end_pos = 0 THEN
            RAISE_APPLICATION_ERROR(-20001, 'Cannot locate PERMDETAIL_NO_UPDATE patch markers.');
        END IF;

        DBMS_LOB.CREATETEMPORARY(l_new_text, TRUE);
        l_prefix_len := l_start_pos - 1;
        IF l_prefix_len > 0 THEN
            DBMS_LOB.COPY(l_new_text, io_text, l_prefix_len, 1, 1);
        END IF;
        IF p_replacement IS NOT NULL THEN
            DBMS_LOB.WRITEAPPEND(l_new_text, LENGTH(p_replacement), p_replacement);
        END IF;
        l_suffix_len := DBMS_LOB.GETLENGTH(io_text) - l_end_pos + 1;
        DBMS_LOB.COPY(
            l_new_text,
            io_text,
            l_suffix_len,
            DBMS_LOB.GETLENGTH(l_new_text) + 1,
            l_end_pos
        );
        io_text := l_new_text;
    END REPLACE_BETWEEN;
BEGIN
    SELECT DBMS_METADATA.GET_DDL('PACKAGE_BODY', 'PERM_PKG')
      INTO l_ddl
      FROM DUAL;

    IF DBMS_LOB.INSTR(l_ddl, l_new_marker, 1, 1) > 0 THEN
        DBMS_OUTPUT.PUT_LINE('PERMDETAIL_NO_UPDATE is already optimized.');
        RETURN;
    END IF;

    REPLACE_BETWEEN(
        l_ddl,
        l_scope_marker,
        'INSERT INTO T_ACTIONHISTORY(',
        '      UPDATE T_PERMDETAIL_NO e',
        '      ' || l_new_marker || CHR(10)
    );

    REPLACE_BETWEEN(
        l_ddl,
        l_scope_marker,
        'WHERE e.ID = P_ID returning FLIGHT_PK into vFLIGHT_PK;',
        '      /*----close',
        'WHERE e.ID = P_ID returning FLIGHT_PK into vFLIGHT_PK;' || CHR(10)
    );

    REPLACE_BETWEEN(
        l_ddl,
        l_scope_marker,
        '      P_RETURN_CODE := P_ID;',
        '  END PERMDETAIL_NO_UPDATE;',
          '      P_RETURN_CODE := P_ID;' || CHR(10)
        || '      COMMIT;' || CHR(10)
        || '    EXCEPTION' || CHR(10)
        || '    WHEN OTHERS THEN' || CHR(10)
        || '      ROLLBACK;' || CHR(10)
        || '      PROCESS_PKG.ADD_ERROR_LOG(''DIC_PKG.PERMDETAIL_NO_UPDATE'',' || CHR(10)
        || '                               SQLCODE || TO_CHAR(sysdate, ''FMMON DDth, YYYY''),' || CHR(10)
        || '                               SUBSTR(SQLERRM, 1, 200));' || CHR(10)
        || '      P_RETURN_CODE := -1;' || CHR(10)
    );

    l_cursor := DBMS_SQL.OPEN_CURSOR;
    BEGIN
        DBMS_SQL.PARSE(l_cursor, l_ddl, DBMS_SQL.NATIVE);
        DBMS_SQL.CLOSE_CURSOR(l_cursor);
    EXCEPTION
        WHEN OTHERS THEN
            IF DBMS_SQL.IS_OPEN(l_cursor) THEN
                DBMS_SQL.CLOSE_CURSOR(l_cursor);
            END IF;
            RAISE;
    END;

    DBMS_OUTPUT.PUT_LINE('Optimized PERM_PKG.PERMDETAIL_NO_UPDATE.');
END;
/

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERM_PKG'
 ORDER BY OBJECT_TYPE;

SELECT LINE, POSITION, TEXT
  FROM USER_ERRORS
 WHERE NAME = 'PERM_PKG'
 ORDER BY SEQUENCE;
