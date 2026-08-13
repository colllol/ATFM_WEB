-- ============================================================================
-- DEPLOY CANCEL PERMISSION SC V2 TREN ORACLE MAY CHU
-- Schema du kien: ATFM
--
-- Cach chay khuyen nghi bang SQL*Plus/SQLcl, tu thu muc Database/Oracle:
--   sqlplus ATFM@//HOST:1521/SERVICE @20260813_ImportsPermSC_LD_V2_server_deploy.sql
--
-- Cac file con phai nam cung thu muc voi file nay:
--   20260813_T_PERMSC_IMP_permnbr_8.sql
--   20260812_T_MENUS_ImportsPermSC_LD_V2.sql
--   20260813_PERM_IMP_V2_PKG_apply_cancellation.sql
--
-- Script co the chay lai. Cac object cu PERM_IMP_PKG va
-- ConvertPermScToSchedule_Delete khong bi thay doi.
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

PROMPT ========================================================================
PROMPT 1. PRECHECK SCHEMA VA DOI TUONG PHU THUOC
PROMPT ========================================================================

DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;

    PROCEDURE require_table(p_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*) INTO v_count
          FROM USER_TABLES
         WHERE TABLE_NAME = UPPER(p_name);

        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10) || '- TABLE ' || UPPER(p_name);
        END IF;
    END require_table;

    PROCEDURE require_object
    (
        p_name IN VARCHAR2,
        p_type IN VARCHAR2
    ) IS
    BEGIN
        SELECT COUNT(*) INTO v_count
          FROM USER_OBJECTS
         WHERE OBJECT_NAME = UPPER(p_name)
           AND OBJECT_TYPE = UPPER(p_type)
           AND STATUS = 'VALID';

        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10)
                || '- ' || UPPER(p_type) || ' ' || UPPER(p_name);
        END IF;
    END require_object;
BEGIN
    require_table('T_PERMSC_IMP');
    require_table('T_PERMMASTER_SC');
    require_table('T_PERMDETAIL_SC');
    require_table('T_SCHEDULE_DAYFLIGHTS');
    require_table('M_CRAFT_TYPE');
    require_table('T_MENUS');
    require_table('T_USERMENU');
    require_table('T_USERS');

    require_object('FNCREATEPERMSC_NBR_ID', 'FUNCTION');
    require_object('PROCESS_PKG', 'PACKAGE');

    SELECT COUNT(*) INTO v_count
      FROM T_USERS
     WHERE LOWER(TRIM(USERNAME)) = 'admin';

    IF v_count = 0 THEN
        v_missing := v_missing || CHR(10) || '- USER admin';
    END IF;

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20851,
            'Thieu doi tuong phu thuoc:' || SUBSTR(v_missing, 1, 1800)
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('PRECHECK OK. Schema=' || USER);
END;
/

PROMPT ========================================================================
PROMPT 2. MO RONG T_PERMSC_IMP.PERM NBR LEN 8 KY TU
PROMPT ========================================================================

@@20260813_T_PERMSC_IMP_permnbr_8.sql

PROMPT ========================================================================
PROMPT 3. TAO/CAP NHAT MENU VA QUYEN ADMIN
PROMPT ========================================================================

@@20260812_T_MENUS_ImportsPermSC_LD_V2.sql

PROMPT ========================================================================
PROMPT 4. TRIEN KHAI PERM_IMP_V2_PKG VA INDEX TOI UU XAC NHAN HUY
PROMPT Luu y: tao index ONLINE tren bang lich lon co the mat vai phut.
PROMPT ========================================================================

@@20260813_PERM_IMP_V2_PKG_apply_cancellation.sql

PROMPT ========================================================================
PROMPT 5. VERIFY SAU TRIEN KHAI
PROMPT ========================================================================

DECLARE
    v_count       PLS_INTEGER;
    v_char_length PLS_INTEGER;
BEGIN
    SELECT CHAR_LENGTH
      INTO v_char_length
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_PERMSC_IMP'
       AND COLUMN_NAME = 'PERMNBR';

    IF v_char_length < 8 THEN
        RAISE_APPLICATION_ERROR(
            -20852,
            'T_PERMSC_IMP.PERM NBR chua dat 8 ky tu'
        );
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'PERM_IMP_V2_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20853,
            'PERM_IMP_V2_PKG co object khong VALID'
        );
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM USER_ERRORS
     WHERE NAME = 'PERM_IMP_V2_PKG';

    IF v_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20854,
            'PERM_IMP_V2_PKG con ' || v_count || ' loi bien dich'
        );
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TSDF_CANCEL_MATCH_V2'
       AND STATUS = 'VALID';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(
            -20855,
            'IX_TSDF_CANCEL_MATCH_V2 khong VALID'
        );
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'PERM_IMP_V2_PKG'
       AND OBJECT_NAME = 'APPLY_CANCELLATIONS'
       AND ARGUMENT_NAME IN ('P_STAGING_IDS', 'P_OUT');

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20856,
            'Sai chu ky PERM_IMP_V2_PKG.APPLY_CANCELLATIONS'
        );
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM T_MENUS m
      JOIN T_USERMENU um ON um.MENU_ID = m.ID
      JOIN T_USERS u ON u.USERID = um.USER_ID
     WHERE LOWER(TRIM(m.MENUURL)) =
           LOWER('Tool/ImportsPermSC_LD_V2.aspx')
       AND LOWER(TRIM(u.USERNAME)) = 'admin'
       AND NVL(m.ISDISPLAY, 0) = 1
       AND NVL(um.R_ADD, 0) = 1
       AND NVL(um.R_EDIT, 0) = 1
       AND NVL(um.R_PUB, 0) = 1;

    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20857,
            'Menu hoac quyen admin chua day du'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: column=8, package/body VALID, index VALID, menu admin OK.'
    );
END;
/

COLUMN OBJECT_NAME FORMAT A35
COLUMN OBJECT_TYPE FORMAT A20
COLUMN STATUS FORMAT A10

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERM_IMP_V2_PKG'
 ORDER BY OBJECT_TYPE;

SELECT INDEX_NAME, STATUS, LAST_ANALYZED
  FROM USER_INDEXES
 WHERE INDEX_NAME = 'IX_TSDF_CANCEL_MATCH_V2';

SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, CHAR_LENGTH
  FROM USER_TAB_COLUMNS
 WHERE TABLE_NAME = 'T_PERMSC_IMP'
   AND COLUMN_NAME = 'PERMNBR';

SELECT m.ID AS MENU_ID,
       m.MENUNAME,
       m.MENUURL,
       u.USERNAME,
       um.R_EDIT,
       um.R_DEL,
       um.R_ADD,
       um.R_PUB
  FROM T_MENUS m
  JOIN T_USERMENU um ON um.MENU_ID = m.ID
  JOIN T_USERS u ON u.USERID = um.USER_ID
 WHERE LOWER(TRIM(m.MENUURL)) =
       LOWER('Tool/ImportsPermSC_LD_V2.aspx')
   AND LOWER(TRIM(u.USERNAME)) = 'admin';

PROMPT ========================================================================
PROMPT DEPLOY CANCEL PERMISSION SC V2 HOAN TAT
PROMPT File log: 20260813_ImportsPermSC_LD_V2_server_deploy.log
PROMPT Sau do publish/copy cac file web cua ImportsPermSC_LD_V2 len IIS.
PROMPT ========================================================================

SPOOL OFF
EXIT SUCCESS

