-- ============================================================================
-- Rollback logic thong bao ATFM/DPLKL cua make_finished_flights_news4day.
-- Khoi phuc chinh xac MAKE_FINISHED package body truoc khi deploy.
-- Khong xoa ban ghi T_NOTIFICATION da phat sinh.
-- ============================================================================

DECLARE
    c_deployment_id CONSTANT VARCHAR2(80) :=
        '20260818_MAKE_FINISHED_DPLKL_NOTIFY_V1';
    v_body  CLOB;
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
        RAISE_APPLICATION_ERROR(-20971, 'Script chi duoc chay tren schema ATFM');
    END IF;

    SELECT DDL_TEXT
      INTO v_body
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_deployment_id
       AND OBJECT_NAME = 'MAKE_FINISHED'
       AND OBJECT_TYPE = 'PACKAGE_BODY';

    execute_clob_ddl(v_body);

    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'MAKE_FINISHED'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(-20972, 'MAKE_FINISHED khong VALID sau rollback');
    END IF;

    DBMS_OUTPUT.PUT_LINE('ROLLBACK OK: restored MAKE_FINISHED package body.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(
            -20973,
            'Khong tim thay backup ' || c_deployment_id
        );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'MAKE_FINISHED'
 ORDER BY OBJECT_TYPE;
