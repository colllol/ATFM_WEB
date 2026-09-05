-- Rollback menu tra cuu lich su export AMHS.
-- Khong xoa bang T_SEND_AMHS va khong xoa package.

DECLARE
    V_MENU_ID T_MENUS.ID%TYPE;
BEGIN
    BEGIN
        SELECT ID
          INTO V_MENU_ID
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) =
               LOWER('PlanMessage/SendAMHSHistory.aspx')
           AND ROWNUM = 1;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            V_MENU_ID := NULL;
    END;

    IF V_MENU_ID IS NOT NULL THEN
        DELETE FROM T_USERMENU WHERE MENU_ID = V_MENU_ID;
        DELETE FROM T_MENUS WHERE ID = V_MENU_ID;
    END IF;

    COMMIT;
END;
/
