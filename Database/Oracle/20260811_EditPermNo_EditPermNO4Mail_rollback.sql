-- ============================================================================
-- Rollback the latest execution of:
--   20260811_EditPermNo_EditPermNO4Mail_deploy.sql
--
-- Restores PERM_PKG package body and the previous approval package DDL.
-- Drops IX_PERMDETAIL_NO_PERM_ID_ID only when it did not exist before.
-- Does not delete schedule rows created by Accepted actions after deployment.
-- ============================================================================

DECLARE
    v_run_id             VARCHAR2(40);
    v_ddl                CLOB;
    v_existed_before     NUMBER;

    PROCEDURE execute_ddl(p_ddl IN CLOB) IS
        v_ddl_cursor INTEGER;
    BEGIN
        IF p_ddl IS NULL THEN
            RETURN;
        END IF;

        v_ddl_cursor := DBMS_SQL.OPEN_CURSOR;
        BEGIN
            DBMS_SQL.PARSE(v_ddl_cursor, p_ddl, DBMS_SQL.NATIVE);
            DBMS_SQL.CLOSE_CURSOR(v_ddl_cursor);
        EXCEPTION
            WHEN OTHERS THEN
                IF DBMS_SQL.IS_OPEN(v_ddl_cursor) THEN
                    DBMS_SQL.CLOSE_CURSOR(v_ddl_cursor);
                END IF;
                RAISE;
        END;
    END execute_ddl;

    PROCEDURE get_backup
    (
        p_object_name  IN  VARCHAR2,
        p_object_type  IN  VARCHAR2,
        p_existed      OUT NUMBER,
        p_ddl          OUT CLOB
    ) IS
    BEGIN
        SELECT EXISTED_BEFORE, DDL_TEXT
          INTO p_existed, p_ddl
          FROM T_ATFM_DEPLOY_DDL_BACKUP
         WHERE RUN_ID = v_run_id
           AND OBJECT_NAME = UPPER(p_object_name)
           AND OBJECT_TYPE = UPPER(p_object_type);
    END get_backup;
BEGIN
    SELECT MAX(RUN_ID)
      INTO v_run_id
      FROM T_ATFM_DEPLOY_DDL_BACKUP
     WHERE DEPLOYMENT_NAME = 'EDIT_PERM_NO_AND_MAIL';

    IF v_run_id IS NULL THEN
        RAISE_APPLICATION_ERROR(-20861, 'No Edit PermNo deployment backup found');
    END IF;

    -- Restore PERM_PKG body first.
    get_backup('PERM_PKG', 'PACKAGE BODY', v_existed_before, v_ddl);
    IF v_existed_before = 1 THEN
        execute_ddl(v_ddl);
    END IF;

    -- Restore or remove the approval package.
    get_backup('PERM_NO_APPROVAL_PKG', 'PACKAGE', v_existed_before, v_ddl);
    IF v_existed_before = 1 THEN
        execute_ddl(v_ddl);

        get_backup('PERM_NO_APPROVAL_PKG', 'PACKAGE BODY', v_existed_before, v_ddl);
        IF v_existed_before = 1 THEN
            execute_ddl(v_ddl);
        END IF;
    ELSE
        BEGIN
            EXECUTE IMMEDIATE 'DROP PACKAGE PERM_NO_APPROVAL_PKG';
        EXCEPTION
            WHEN OTHERS THEN
                IF SQLCODE <> -4043 THEN
                    RAISE;
                END IF;
        END;
    END IF;

    -- Drop the named index only when this deployment created it.
    get_backup('IX_PERMDETAIL_NO_PERM_ID_ID', 'INDEX', v_existed_before, v_ddl);
    IF v_existed_before = 0 THEN
        BEGIN
            EXECUTE IMMEDIATE 'DROP INDEX IX_PERMDETAIL_NO_PERM_ID_ID';
        EXCEPTION
            WHEN OTHERS THEN
                IF SQLCODE <> -1418 THEN
                    RAISE;
                END IF;
        END;
    END IF;

    DBMS_OUTPUT.PUT_LINE('Rollback completed. RUN_ID=' || v_run_id);
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('PERM_PKG', 'PERM_NO_APPROVAL_PKG')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT NAME, TYPE, LINE, POSITION, TEXT
  FROM USER_ERRORS
 WHERE NAME IN ('PERM_PKG', 'PERM_NO_APPROVAL_PKG')
 ORDER BY NAME, TYPE, SEQUENCE;
