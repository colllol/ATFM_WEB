-- Khai bao menu va cap day du quyen cho admin. Cac user khac phan quyen tren UI:
-- R_ADD/R_EDIT = nhap va gui duyet; R_PUB = duyet, tu choi va export.
SET SERVEROUTPUT ON;

DECLARE
    v_menu_id   T_MENUS.ID%TYPE;
    v_admin_id  T_USERS.USERID%TYPE;
    v_parent_id T_MENUS.PARRENTID%TYPE := 0;
BEGIN
    SELECT USERID INTO v_admin_id
      FROM T_USERS
     WHERE LOWER(USERNAME) = 'admin' AND ROWNUM = 1;

    BEGIN
        SELECT NVL(PARRENTID, 0)
          INTO v_parent_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) = LOWER('MessManagement/MessManagement.aspx')
           AND ROWNUM = 1;
    EXCEPTION WHEN NO_DATA_FOUND THEN
        v_parent_id := 0;
    END;

    BEGIN
        SELECT ID INTO v_menu_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) = LOWER('MessManagement/LiveFireMessage.aspx')
           AND ROWNUM = 1;

        UPDATE T_MENUS
           SET MENUNAME = 'Live Fire Message',
               MENUDESC = 'Create, approve and export live-fire messages',
               MENUORDER = 30,
               PARRENTID = v_parent_id,
               DATEMODIFY = SYSDATE,
               USERMODIFY = v_admin_id,
               ISDISPLAY = 1
         WHERE ID = v_menu_id;
    EXCEPTION WHEN NO_DATA_FOUND THEN
        INSERT INTO T_MENUS
        (
            ID, MENUNAME, MENUDESC, MENUORDER, PARRENTID,
            DATECREATED, DATEMODIFY, MENUURL, MENUICON,
            USERCREATE, USERMODIFY, ISDISPLAY, ACTIVESYNC, ACTIVESYNCIMAGES
        )
        VALUES
        (
            NULL, 'Live Fire Message',
            'Create, approve and export live-fire messages',
            30, v_parent_id, SYSDATE, SYSDATE,
            'MessManagement/LiveFireMessage.aspx', NULL,
            v_admin_id, v_admin_id, 1, 0, 0
        )
        RETURNING ID INTO v_menu_id;
    END;

    UPDATE T_USERMENU
       SET R_EDIT = 1, R_DEL = 0, R_ADD = 1, R_PUB = 1, DATEMODIFY = SYSDATE
     WHERE USER_ID = v_admin_id
       AND MENU_ID = v_menu_id
       AND NVL(GROUP_ID, 0) = 0;

    IF SQL%ROWCOUNT = 0 THEN
        INSERT INTO T_USERMENU
        (
            ID, USER_ID, MENU_ID, R_EDIT, R_DEL, R_ADD, R_PUB,
            DATECREATED, DATEMODIFY, GROUP_ID
        )
        VALUES
        (
            NULL, v_admin_id, v_menu_id, 1, 0, 1, 1,
            SYSDATE, SYSDATE, 0
        );
    END IF;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Live Fire Message MENU_ID=' || v_menu_id);
END;
/
