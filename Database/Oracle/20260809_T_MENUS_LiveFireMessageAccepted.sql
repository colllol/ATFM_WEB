-- Menu duyet dien van ban dan that. R_PUB cho phep duyet/tu choi/export.
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
        SELECT NVL(PARRENTID, 0) INTO v_parent_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) = LOWER('MessManagement/LiveFireMessage.aspx')
           AND ROWNUM = 1;
    EXCEPTION WHEN NO_DATA_FOUND THEN
        v_parent_id := 0;
    END;

    BEGIN
        SELECT ID INTO v_menu_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) = LOWER('MessManagement/LiveFireMessageAccepted.aspx')
           AND ROWNUM = 1;

        UPDATE T_MENUS
           SET MENUNAME = 'Approve Live Fire Message',
               MENUDESC = 'Approve, reject and export live-fire messages',
               MENUORDER = 31,
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
            NULL, 'Approve Live Fire Message',
            'Approve, reject and export live-fire messages',
            31, v_parent_id, SYSDATE, SYSDATE,
            'MessManagement/LiveFireMessageAccepted.aspx', NULL,
            v_admin_id, v_admin_id, 1, 0, 0
        )
        RETURNING ID INTO v_menu_id;
    END;

    UPDATE T_USERMENU
       SET R_EDIT = 0, R_DEL = 0, R_ADD = 0, R_PUB = 1, DATEMODIFY = SYSDATE
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
            NULL, v_admin_id, v_menu_id, 0, 0, 0, 1,
            SYSDATE, SYSDATE, 0
        );
    END IF;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Approve Live Fire Message MENU_ID=' || v_menu_id);
END;
/
