-- Add Search Permission Adv below Flights Permission and grant it to admin.
-- Safe to run repeatedly: the menu is identified by MENUURL and the permission
-- is identified by USER_ID + MENU_ID + GROUP_ID.

SET SERVEROUTPUT ON;

DECLARE
    v_menu_id   T_MENUS.ID%TYPE;
    v_admin_id  T_USERS.USERID%TYPE;
BEGIN
    SELECT USERID
      INTO v_admin_id
      FROM T_USERS
     WHERE LOWER(USERNAME) = 'admin'
       AND ROWNUM = 1;

    BEGIN
        SELECT ID
          INTO v_menu_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) = LOWER('Permission/SearchPermissionAdv.aspx')
           AND ROWNUM = 1;

        UPDATE T_MENUS
           SET MENUNAME         = 'Search Permission Adv',
               MENUDESC         = 'Search SC and NO permissions by permission date',
               MENUORDER        = 24,
               PARRENTID        = 42,
               DATEMODIFY       = SYSDATE,
               USERMODIFY       = v_admin_id,
               ISDISPLAY        = 1,
               ACTIVESYNC       = 0,
               ACTIVESYNCIMAGES = 0
         WHERE ID = v_menu_id;
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
                'Search Permission Adv',
                'Search SC and NO permissions by permission date',
                24,
                42,
                SYSDATE,
                SYSDATE,
                'Permission/SearchPermissionAdv.aspx',
                NULL,
                v_admin_id,
                v_admin_id,
                1,
                0,
                0
            )
            RETURNING ID INTO v_menu_id;
    END;

    UPDATE T_USERMENU
       SET R_EDIT      = 1,
           R_DEL       = 1,
           R_ADD       = 1,
           R_PUB       = 1,
           DATEMODIFY  = SYSDATE
     WHERE USER_ID = v_admin_id
       AND MENU_ID = v_menu_id
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
            NULL, v_admin_id, v_menu_id,
            1, 1, 1, 1,
            SYSDATE, SYSDATE, 0
        );
    END IF;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE(
        'Search Permission Adv MENU_ID=' || v_menu_id ||
        ' granted to admin USER_ID=' || v_admin_id
    );
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20001, 'User admin was not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/
