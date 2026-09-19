-- ============================================================================
-- Trien khai tron bo chuc nang Live Fire Message tren schema ATFM.
-- Chay bang SQL*Plus hoac SQLcl tu thu muc chua file nay:
--   sqlplus ATFM@//HOST:1521/SERVICE @20260810_LiveFireMessage_deploy_all.sql
--
-- Script con bat buoc phai nam cung thu muc:
--   20260809_T_LIVE_FIRE_MESSAGE.sql
--   20260809_T_MENUS_LiveFireMessage.sql
--   20260809_T_MENUS_LiveFireMessageAccepted.sql
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF
SET ECHO ON
SET FEEDBACK ON
SET VERIFY OFF
SET TIMING ON
SET LINESIZE 240
SET PAGESIZE 200
WHENEVER OSERROR EXIT FAILURE ROLLBACK
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

SPOOL 20260810_LiveFireMessage_deploy_all.log

PROMPT ========================================================================
PROMPT 1. PRECHECK SCHEMA VA CAC DOI TUONG PHU THUOC
PROMPT ========================================================================

DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;

    PROCEDURE require_table(p_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TABLES
         WHERE TABLE_NAME = UPPER(p_name);

        IF v_count = 0 THEN
            v_missing := v_missing || ', TABLE ' || UPPER(p_name);
        END IF;
    END require_table;

    PROCEDURE require_object(p_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_OBJECTS
         WHERE OBJECT_NAME = UPPER(p_name)
           AND STATUS = 'VALID';

        IF v_count = 0 THEN
            v_missing := v_missing || ', OBJECT ' || UPPER(p_name);
        END IF;
    END require_object;
BEGIN
    require_table('T_USERS');
    require_table('T_MENUS');
    require_table('T_USERMENU');
    require_table('T_ACTIONHISTORY');
    require_table('T_PLAN_MESSAGE');
    require_object('PROCESS_PKG');

    SELECT COUNT(*)
      INTO v_count
      FROM T_USERS
     WHERE LOWER(TRIM(USERNAME)) = 'admin';

    IF v_count = 0 THEN
        v_missing := v_missing || ', USER admin';
    END IF;

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20801,
            'Thieu doi tuong phu thuoc:' || LTRIM(v_missing, ',')
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('Precheck OK. Schema=' || USER);
END;
/

PROMPT ========================================================================
PROMPT 2. TAO BANG, SEQUENCE, INDEX VA LIVE_FIRE_MESSAGE_PKG
PROMPT ========================================================================

@@20260809_T_LIVE_FIRE_MESSAGE.sql

PROMPT ========================================================================
PROMPT 3. TAO MENU NHAP LIEU VA CAP QUYEN ADMIN
PROMPT ========================================================================

@@20260809_T_MENUS_LiveFireMessage.sql

PROMPT ========================================================================
PROMPT 4. TAO MENU DUYET VA CAP QUYEN ADMIN
PROMPT ========================================================================

@@20260809_T_MENUS_LiveFireMessageAccepted.sql

PROMPT ========================================================================
PROMPT 5. KIEM TRA KET QUA VA DUNG NGAY NEU KHONG DAT
PROMPT ========================================================================

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE
         (
             OBJECT_NAME = 'T_LIVE_FIRE_MESSAGE'
             AND OBJECT_TYPE = 'TABLE'
             AND STATUS = 'VALID'
         )
         OR
         (
             OBJECT_NAME = 'SEQ_T_LIVE_FIRE_MESSAGE'
             AND OBJECT_TYPE = 'SEQUENCE'
             AND STATUS = 'VALID'
         )
         OR
         (
             OBJECT_NAME = 'LIVE_FIRE_MESSAGE_PKG'
             AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
             AND STATUS = 'VALID'
         );

    IF v_count <> 4 THEN
        RAISE_APPLICATION_ERROR(
            -20802,
            'Live Fire Message co object khong VALID. So object VALID=' || v_count
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_ERRORS
     WHERE NAME = 'LIVE_FIRE_MESSAGE_PKG';

    IF v_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20803,
            'LIVE_FIRE_MESSAGE_PKG con ' || v_count || ' loi bien dich'
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM T_MENUS m
      JOIN T_USERMENU um
        ON um.MENU_ID = m.ID
      JOIN T_USERS u
        ON u.USERID = um.USER_ID
     WHERE LOWER(TRIM(u.USERNAME)) = 'admin'
       AND
       (
           (
               LOWER(TRIM(m.MENUURL)) =
                   LOWER('MessManagement/LiveFireMessage.aspx')
               AND NVL(um.R_ADD, 0) = 1
               AND NVL(um.R_EDIT, 0) = 1
               AND NVL(um.R_PUB, 0) = 1
           )
           OR
           (
               LOWER(TRIM(m.MENUURL)) =
                   LOWER('MessManagement/LiveFireMessageAccepted.aspx')
               AND NVL(um.R_PUB, 0) = 1
           )
       );

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20804,
            'Menu hoac quyen admin chua day du. So menu dat yeu cau=' || v_count
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: 4 object VALID, package khong loi, 2 menu admin hop le.');
END;
/

COLUMN OBJECT_NAME FORMAT A35
COLUMN OBJECT_TYPE FORMAT A20
COLUMN STATUS FORMAT A10

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
       (
           'T_LIVE_FIRE_MESSAGE',
           'SEQ_T_LIVE_FIRE_MESSAGE',
           'LIVE_FIRE_MESSAGE_PKG'
       )
 ORDER BY OBJECT_TYPE, OBJECT_NAME;

COLUMN MENUNAME FORMAT A32
COLUMN MENUURL FORMAT A55
COLUMN USERNAME FORMAT A20

SELECT m.ID,
       m.MENUNAME,
       m.MENUURL,
       u.USERNAME,
       um.R_ADD,
       um.R_EDIT,
       um.R_PUB
  FROM T_MENUS m
  JOIN T_USERMENU um
    ON um.MENU_ID = m.ID
  JOIN T_USERS u
    ON u.USERID = um.USER_ID
 WHERE LOWER(TRIM(u.USERNAME)) = 'admin'
   AND LOWER(TRIM(m.MENUURL)) IN
       (
           LOWER('MessManagement/LiveFireMessage.aspx'),
           LOWER('MessManagement/LiveFireMessageAccepted.aspx')
       )
 ORDER BY m.MENUURL;

PROMPT ========================================================================
PROMPT LIVE FIRE MESSAGE DEPLOYMENT COMPLETED SUCCESSFULLY
PROMPT Kiem tra file log: 20260810_LiveFireMessage_deploy_all.log
PROMPT ========================================================================

SPOOL OFF
EXIT SUCCESS
