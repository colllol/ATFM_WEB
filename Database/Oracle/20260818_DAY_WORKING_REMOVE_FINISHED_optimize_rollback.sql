-- ============================================================================
-- Rollback toi uu DAY_WORKING.REMOVE_FINISHED_FLIGHS_DELETE ngay 18-08-2026.
-- Chay bang DBeaver Execute SQL Script (Alt+X) tren schema ATFM.
-- Khoi phuc chinh xac hai package body da luu trong T_PACKAGE_SOURCE_BACKUP.
-- Khong dao nguoc du lieu nghiep vu da duoc xu ly sau khi deploy.
-- ============================================================================

DECLARE
    c_deployment_id CONSTANT VARCHAR2(80) :=
        '20260818_DAY_WORKING_REMOVE_FINISHED_V1';

    v_day_body  CLOB;
    v_make_body CLOB;
    v_count     PLS_INTEGER;

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
    IF UPPER(USER) <> 'ATFM' THEN
        RAISE_APPLICATION_ERROR(-20921, 'Script chi duoc chay tren schema ATFM');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_PACKAGE_SOURCE_BACKUP';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20922, 'Khong co bang source backup');
    END IF;

    SELECT DDL_TEXT
      INTO v_day_body
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_deployment_id
       AND OBJECT_NAME = 'DAY_WORKING'
       AND OBJECT_TYPE = 'PACKAGE_BODY';

    SELECT DDL_TEXT
      INTO v_make_body
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_deployment_id
       AND OBJECT_NAME = 'MAKE_FINISHED'
       AND OBJECT_TYPE = 'PACKAGE_BODY';

    execute_clob_ddl(v_day_body);
    execute_clob_ddl(v_make_body);

    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 4 THEN
        RAISE_APPLICATION_ERROR(
            -20923,
            'Rollback package xong nhung co object khong VALID'
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TDFG_FLIGHTDATE_MOVEFINISH';

    IF v_count = 1 THEN
        EXECUTE IMMEDIATE 'DROP INDEX IX_TDFG_FLIGHTDATE_MOVEFINISH';
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'ROLLBACK OK: restored DAY_WORKING and MAKE_FINISHED package bodies.'
    );
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(
            -20924,
            'Khong tim thay source backup cho deployment ' || c_deployment_id
        );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT INDEX_NAME, STATUS
  FROM USER_INDEXES
 WHERE INDEX_NAME = 'IX_TDFG_FLIGHTDATE_MOVEFINISH';
