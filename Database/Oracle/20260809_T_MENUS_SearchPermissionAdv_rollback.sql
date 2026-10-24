-- Remove the Search Permission Adv menu and its assigned user/group rights.

SET SERVEROUTPUT ON;

DECLARE
    v_menu_id T_MENUS.ID%TYPE;
BEGIN
    SELECT ID
      INTO v_menu_id
      FROM T_MENUS
     WHERE LOWER(TRIM(MENUURL)) = LOWER('Permission/SearchPermissionAdv.aspx')
       AND ROWNUM = 1;

    DELETE FROM T_USERMENU WHERE MENU_ID = v_menu_id;
    DELETE FROM T_GROUPMENU WHERE MENU_ID = v_menu_id;
    DELETE FROM T_MENUS WHERE ID = v_menu_id;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Removed Search Permission Adv MENU_ID=' || v_menu_id);
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Search Permission Adv does not exist; nothing to roll back.');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/
