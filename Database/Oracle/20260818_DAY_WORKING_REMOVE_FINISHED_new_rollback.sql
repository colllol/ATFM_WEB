-- ============================================================================
-- Rollback rieng ban tach nhanh NEW ngay 18-08-2026.
-- Khoi phuc trang thai ngay truoc khi chay script ..._new.sql.
-- Khong xoa IX_TDFG_FLIGHTDATE_MOVEFINISH vi index da ton tai truoc ban nay.
-- Chay bang DBeaver Execute SQL Script (Alt+X), schema ATFM.
-- ============================================================================

DECLARE
    c_deployment_id CONSTANT VARCHAR2(80) :=
        '20260818_DAY_WORKING_REMOVE_FINISHED_NEW_V2';

    v_day_spec CLOB;
    v_day_body CLOB;
    v_make_body CLOB;
    v_count PLS_INTEGER;

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
        RAISE_APPLICATION_ERROR(-20951, 'Script chi duoc chay tren schema ATFM');
    END IF;

    SELECT DDL_TEXT
      INTO v_day_spec
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_deployment_id
       AND OBJECT_NAME = 'DAY_WORKING'
       AND OBJECT_TYPE = 'PACKAGE_SPEC';

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

    execute_clob_ddl(v_day_spec);
    execute_clob_ddl(v_day_body);
    execute_clob_ddl(v_make_body);

    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 4 THEN
        RAISE_APPLICATION_ERROR(-20952, 'Rollback xong nhung package khong VALID');
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'ROLLBACK OK: restored state before NEW branch deployment.'
    );
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(
            -20953,
            'Khong tim thay backup ' || c_deployment_id
        );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;
