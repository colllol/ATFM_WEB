-- DEPRECATED: GET_CANCELLATION_MATCHES hien nam trong
-- 20260813_PERM_IMP_V2_PKG_apply_cancellation.sql va doc T_PERMSC_CANCEL_V2.
-- File nay chi kiem tra, khong CREATE OR REPLACE package de tranh ghi de
-- chu ky day du cua PERM_IMP_V2_PKG.

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count
      FROM USER_PROCEDURES
     WHERE OBJECT_NAME = 'PERM_IMP_V2_PKG'
       AND PROCEDURE_NAME = 'GET_CANCELLATION_MATCHES';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(
            -20861,
            'Hay chay 20260813_ImportsPermSC_LD_V2_server_deploy.sql'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'GET_CANCELLATION_MATCHES da duoc trien khai trong package V2 day du.'
    );
END;
/
