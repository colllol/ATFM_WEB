-- ============================================================================
-- DEPLOY CANCEL PERMISSION SC V2 TREN ORACLE MAY CHU
-- Tach hoan toan staging V2 sang T_PERMSC_CANCEL_V2.
-- KHONG ALTER/INSERT/UPDATE/DELETE T_PERMSC_IMP; chuc nang cu giu nguyen.
--
-- SQL*Plus/SQLcl (chay tu thu muc Database/Oracle):
--   sqlplus ATFM@//HOST:1521/SERVICE @20260813_ImportsPermSC_LD_V2_server_deploy.sql
--
-- File con bat buoc:
--   20260813_T_PERMSC_CANCEL_V2.sql
--   20260812_T_MENUS_ImportsPermSC_LD_V2.sql
--   20260813_PERM_IMP_V2_PKG_apply_cancellation.sql
--
-- Luu y: script nay chi trien khai DATABASE. Sau khi SQL thanh cong, can
-- publish/copy ban web moi (ASPX/DLL/JS/CSS) len IIS; thay doi giao dien
-- nhu kich thuoc popup khong the trien khai bang SQL.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET ECHO ON
SET FEEDBACK ON
SET VERIFY OFF
SET TIMING ON
SET LINESIZE 240
SET PAGESIZE 200
WHENEVER OSERROR EXIT FAILURE ROLLBACK
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

SPOOL 20260813_ImportsPermSC_LD_V2_server_deploy.log

PROMPT 1. PRECHECK DOI TUONG NGHIEP VU CU (CHI DOC/KIEM TRA)
DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;
    PROCEDURE require_table(p_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*) INTO v_count FROM USER_TABLES
         WHERE TABLE_NAME = UPPER(p_name);
        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10) || '- TABLE ' || UPPER(p_name);
        END IF;
    END;
    PROCEDURE require_object(p_name IN VARCHAR2, p_type IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*) INTO v_count FROM USER_OBJECTS
         WHERE OBJECT_NAME = UPPER(p_name)
           AND OBJECT_TYPE = UPPER(p_type)
           AND STATUS = 'VALID';
        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10) || '- ' || UPPER(p_type)
                || ' ' || UPPER(p_name);
        END IF;
    END;
BEGIN
    IF UPPER(USER) <> 'ATFM' THEN
        RAISE_APPLICATION_ERROR(
            -20850,
            'Sai schema. Phai ket noi bang schema ATFM; schema hien tai=' || USER
        );
    END IF;

    require_table('T_PERMMASTER_SC');
    require_table('T_PERMDETAIL_SC');
    require_table('T_SCHEDULE_DAYFLIGHTS');
    require_table('M_CRAFT_TYPE');
    require_table('M_OPER');
    require_table('T_MENUS');
    require_table('T_USERMENU');
    require_table('T_USERS');
    require_object('FNCREATEPERMSC_NBR_ID', 'FUNCTION');
    require_object('PROCESS_PKG', 'PACKAGE');

    SELECT COUNT(*) INTO v_count FROM T_USERS
     WHERE LOWER(TRIM(USERNAME)) = 'admin';
    IF v_count = 0 THEN v_missing := v_missing || CHR(10) || '- USER admin'; END IF;

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(-20851,
            'Thieu doi tuong phu thuoc:' || SUBSTR(v_missing, 1, 1800));
    END IF;
    DBMS_OUTPUT.PUT_LINE('PRECHECK OK. Schema=' || USER);
END;
/

PROMPT 2. TAO BANG STAGING RIENG T_PERMSC_CANCEL_V2
@@20260813_T_PERMSC_CANCEL_V2.sql

PROMPT 3. TAO/CAP NHAT MENU VA QUYEN ADMIN
@@20260812_T_MENUS_ImportsPermSC_LD_V2.sql

PROMPT 4. TRIEN KHAI PERM_IMP_V2_PKG VA INDEX TOI UU
@@20260813_PERM_IMP_V2_PKG_apply_cancellation.sql

PROMPT 5. VERIFY: KHONG YEU CAU THAY DOI T_PERMSC_IMP
DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'T_PERMSC_CANCEL_V2'
       AND OBJECT_TYPE = 'TABLE' AND STATUS = 'VALID';
    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20852, 'T_PERMSC_CANCEL_V2 khong VALID');
    END IF;

    SELECT COUNT(*) INTO v_count FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'SEQ_T_PERMSC_CANCEL_V2'
       AND OBJECT_TYPE = 'SEQUENCE' AND STATUS = 'VALID';
    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20853, 'SEQ_T_PERMSC_CANCEL_V2 khong VALID');
    END IF;

    SELECT COUNT(*) INTO v_count FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'PERM_IMP_V2_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY') AND STATUS = 'VALID';
    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(-20854, 'PERM_IMP_V2_PKG co object khong VALID');
    END IF;

    SELECT COUNT(*) INTO v_count FROM USER_ERRORS
     WHERE NAME = 'PERM_IMP_V2_PKG';
    IF v_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20855,
            'PERM_IMP_V2_PKG con ' || v_count || ' loi bien dich');
    END IF;

    SELECT COUNT(*) INTO v_count FROM USER_INDEXES
     WHERE INDEX_NAME IN
       ('IX_TPSC_V2_USER_STATUS', 'IX_TPSC_V2_BATCH',
        'IX_TPSC_V2_MATCH', 'IX_TSDF_CANCEL_MATCH_V2')
       AND STATUS = 'VALID';
    IF v_count <> 4 THEN
        RAISE_APPLICATION_ERROR(-20856,
            'Index V2 chua day du/VALID. So index VALID=' || v_count);
    END IF;

    SELECT COUNT(*) INTO v_count FROM USER_PROCEDURES
     WHERE OBJECT_NAME = 'PERM_IMP_V2_PKG'
       AND PROCEDURE_NAME IN
       ('INSERT_CANCELLATION', 'REFRESH_VALIDATION', 'SEARCH_PENDING',
        'GET_CANCELLATION_MATCHES', 'DELETE_CANCELLATIONS',
        'APPLY_CANCELLATIONS');
    IF v_count <> 6 THEN
        RAISE_APPLICATION_ERROR(-20857,
            'PERM_IMP_V2_PKG thieu procedure. So procedure=' || v_count);
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM T_MENUS m
      JOIN T_USERMENU um ON um.MENU_ID = m.ID
      JOIN T_USERS u ON u.USERID = um.USER_ID
     WHERE LOWER(TRIM(m.MENUURL)) = LOWER('Tool/ImportsPermSC_LD_V2.aspx')
       AND LOWER(TRIM(u.USERNAME)) = 'admin'
       AND NVL(m.ISDISPLAY, 0) = 1
       AND NVL(um.R_ADD, 0) = 1
       AND NVL(um.R_EDIT, 0) = 1
       AND NVL(um.R_PUB, 0) = 1;
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20858, 'Menu hoac quyen admin chua day du');
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: staging V2 rieng, package/body VALID, 4 index VALID, menu OK.');
END;
/

COLUMN OBJECT_NAME FORMAT A35
COLUMN OBJECT_TYPE FORMAT A20
COLUMN STATUS FORMAT A10
SELECT OBJECT_NAME, OBJECT_TYPE, STATUS FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
 ('T_PERMSC_CANCEL_V2', 'SEQ_T_PERMSC_CANCEL_V2', 'PERM_IMP_V2_PKG')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT INDEX_NAME, STATUS FROM USER_INDEXES
 WHERE INDEX_NAME IN
 ('IX_TPSC_V2_USER_STATUS', 'IX_TPSC_V2_BATCH',
  'IX_TPSC_V2_MATCH', 'IX_TSDF_CANCEL_MATCH_V2')
 ORDER BY INDEX_NAME;

PROMPT DEPLOY CANCEL PERMISSION SC V2 HOAN TAT
PROMPT T_PERMSC_IMP VA PERM_IMP_PKG CU KHONG BI THAY DOI.
PROMPT Sau do publish/copy cac thanh phan web sau len IIS:
PROMPT - Tool/ImportsPermSC_LD_V2.aspx
PROMPT - Tool/ImportsPermSC_LD_V2.js
PROMPT - Tool/ImportsPermSC_LD_V2.css
PROMPT - Tool/ImportsPermSC_LD_V2.fix.css
PROMPT - bin/prjApplication.dll (chua code-behind da bien dich)
PROMPT Thay doi width/padding popup la thay doi web, khong can chay lai SQL.

SPOOL OFF
EXIT SUCCESS
