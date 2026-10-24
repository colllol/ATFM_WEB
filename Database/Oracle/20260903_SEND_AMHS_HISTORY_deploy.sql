-- ============================================================================
-- DEPLOY CHUC NANG AMHS SEND HISTORY
--
-- Chay bang schema ATFM tren Oracle server. Hai file duoc goi can nam cung
-- thu muc voi script nay:
--   20260903_SEND_AMHS_HISTORY_PKG.sql
--   20260903_T_MENUS_SendAMHSHistory.sql
--
-- DBeaver: mo file va chon Execute SQL Script (Alt+X).
-- Chuc nang web: PlanMessage/SendAMHSHistory.aspx
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_SEND_AMHS';

    IF V_COUNT = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20980,
            'Chua co T_SEND_AMHS. Hay chay script tao bang truoc.'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('Precheck T_SEND_AMHS: OK');
END;
/

@@20260903_SEND_AMHS_HISTORY_PKG.sql
@@20260903_T_MENUS_SendAMHSHistory.sql

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'SEND_AMHS_HISTORY_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF V_COUNT <> 2 THEN
        RAISE_APPLICATION_ERROR(-20981, 'Package SEND_AMHS_HISTORY_PKG chua VALID');
    END IF;

    SELECT COUNT(*)
      INTO V_COUNT
      FROM T_MENUS M
      JOIN T_USERMENU UM ON UM.MENU_ID = M.ID
      JOIN T_USERS U ON U.USERID = UM.USER_ID
     WHERE LOWER(TRIM(M.MENUURL)) =
           LOWER('PlanMessage/SendAMHSHistory.aspx')
       AND LOWER(TRIM(U.USERNAME)) = 'admin'
       AND NVL(M.ISDISPLAY, 0) = 1
       AND NVL(UM.R_PUB, 0) = 1;

    IF V_COUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20982, 'Menu hoac quyen admin chua VALID');
    END IF;

    DBMS_OUTPUT.PUT_LINE('DEPLOY SEND AMHS HISTORY: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'SEND_AMHS_HISTORY_PKG'
 ORDER BY OBJECT_TYPE;

SELECT M.ID AS MENU_ID,
       M.PARRENTID AS PARENT_ID,
       P.MENUNAME AS PARENT_NAME,
       M.MENUNAME,
       M.MENUURL,
       U.USERNAME,
       UM.R_EDIT,
       UM.R_DEL,
       UM.R_ADD,
       UM.R_PUB
  FROM T_MENUS M
  LEFT JOIN T_MENUS P ON P.ID = M.PARRENTID
  JOIN T_USERMENU UM ON UM.MENU_ID = M.ID
  JOIN T_USERS U ON U.USERID = UM.USER_ID
 WHERE LOWER(TRIM(M.MENUURL)) =
       LOWER('PlanMessage/SendAMHSHistory.aspx');
