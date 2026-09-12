-- Tao menu tra cuu lich su export AMHS trong nhom Flights Plan.
-- Chay bang user ATFM sau khi package SEND_AMHS_HISTORY_PKG da VALID.
-- Script co the chay lai an toan; menu duoc nhan dien bang MENUURL.

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    V_MENU_ID   T_MENUS.ID%TYPE;
    V_PARENT_ID T_MENUS.ID%TYPE;
    V_ADMIN_ID  T_USERS.USERID%TYPE;
BEGIN
    SELECT USERID
      INTO V_ADMIN_ID
      FROM T_USERS
     WHERE LOWER(TRIM(USERNAME)) = 'admin'
       AND ROWNUM = 1;

    BEGIN
        SELECT ID
          INTO V_PARENT_ID
          FROM
          (
              SELECT ID,
                     CASE
                         WHEN UPPER(TRIM(MENUNAME)) = 'FLIGHTS PLAN' THEN 1
                         WHEN UPPER(TRIM(MENUNAME)) = 'FLIGHT PLAN' THEN 2
                         WHEN UPPER(TRIM(MENUNAME)) = 'FLIGHTS PERMISSION' THEN 3
                         ELSE 4
                     END AS PRIORITY
                FROM T_MENUS
               WHERE UPPER(TRIM(MENUNAME)) IN
                     ('FLIGHTS PLAN', 'FLIGHT PLAN', 'FLIGHTS PERMISSION')
                  OR ID = 42
               ORDER BY PRIORITY, ID
          )
         WHERE ROWNUM = 1;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(
                -20971,
                'Khong tim thay menu cha Flights Plan (hoac menu ID=42)'
            );
    END;

    BEGIN
        SELECT ID
          INTO V_MENU_ID
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) =
               LOWER('PlanMessage/SendAMHSHistory.aspx')
           AND ROWNUM = 1;

        UPDATE T_MENUS
           SET MENUNAME         = 'AMHS Send History',
               MENUDESC         = 'Tra cuu lich su export AMHS theo ngay',
               MENUORDER        = 31,
               PARRENTID        = V_PARENT_ID,
               DATEMODIFY       = SYSDATE,
               USERMODIFY       = V_ADMIN_ID,
               ISDISPLAY        = 1,
               ACTIVESYNC       = 0,
               ACTIVESYNCIMAGES = 0
         WHERE ID = V_MENU_ID;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            INSERT INTO T_MENUS
            (
                ID, MENUNAME, MENUDESC, MENUORDER, PARRENTID,
                DATECREATED, DATEMODIFY, MENUURL, MENUICON,
                USERCREATE, USERMODIFY, ISDISPLAY,
                ACTIVESYNC, ACTIVESYNCIMAGES
            )
            VALUES
            (
                NULL,
                'AMHS Send History',
                'Tra cuu lich su export AMHS theo ngay',
                31,
                V_PARENT_ID,
                SYSDATE,
                SYSDATE,
                'PlanMessage/SendAMHSHistory.aspx',
                NULL,
                V_ADMIN_ID,
                V_ADMIN_ID,
                1,
                0,
                0
            )
            RETURNING ID INTO V_MENU_ID;
    END;

    UPDATE T_USERMENU
       SET R_EDIT     = 1,
           R_DEL      = 1,
           R_ADD      = 1,
           R_PUB      = 1,
           DATEMODIFY = SYSDATE
     WHERE USER_ID = V_ADMIN_ID
       AND MENU_ID = V_MENU_ID
       AND NVL(GROUP_ID, 0) = 0;

    IF SQL%ROWCOUNT = 0 THEN
        INSERT INTO T_USERMENU
        (
            ID, USER_ID, MENU_ID,
            R_EDIT, R_DEL, R_ADD, R_PUB,
            DATECREATED, DATEMODIFY, GROUP_ID
        )
        VALUES
        (
            NULL, V_ADMIN_ID, V_MENU_ID,
            1, 1, 1, 1,
            SYSDATE, SYSDATE, 0
        );
    END IF;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE(
        'AMHS Send History MENU_ID=' || V_MENU_ID
        || ', PARENT_ID=' || V_PARENT_ID
        || ', ADMIN_USER_ID=' || V_ADMIN_ID
    );
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20972, 'Khong tim thay user admin');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM T_MENUS M
      JOIN T_USERMENU UM ON UM.MENU_ID = M.ID
      JOIN T_USERS U ON U.USERID = UM.USER_ID
     WHERE LOWER(TRIM(M.MENUURL)) =
           LOWER('PlanMessage/SendAMHSHistory.aspx')
       AND LOWER(TRIM(U.USERNAME)) = 'admin'
       AND NVL(M.ISDISPLAY, 0) = 1
       AND NVL(UM.GROUP_ID, 0) = 0
       AND NVL(UM.R_EDIT, 0) = 1
       AND NVL(UM.R_DEL, 0) = 1
       AND NVL(UM.R_ADD, 0) = 1
       AND NVL(UM.R_PUB, 0) = 1;

    IF V_COUNT = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20973,
            'Menu AMHS Send History hoac quyen admin chua dat'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('AMHS Send History menu verify: OK');
END;
/

SELECT M.ID AS MENU_ID,
       M.PARRENTID AS PARENT_ID,
       P.MENUNAME AS PARENT_NAME,
       M.MENUNAME,
       M.MENUURL,
       M.MENUORDER,
       M.ISDISPLAY,
       U.USERNAME,
       UM.R_EDIT,
       UM.R_DEL,
       UM.R_ADD,
       UM.R_PUB,
       UM.GROUP_ID
  FROM T_MENUS M
  LEFT JOIN T_MENUS P ON P.ID = M.PARRENTID
  JOIN T_USERMENU UM ON UM.MENU_ID = M.ID
  JOIN T_USERS U ON U.USERID = UM.USER_ID
 WHERE LOWER(TRIM(M.MENUURL)) =
       LOWER('PlanMessage/SendAMHSHistory.aspx')
 ORDER BY LOWER(U.USERNAME), NVL(UM.GROUP_ID, 0);
