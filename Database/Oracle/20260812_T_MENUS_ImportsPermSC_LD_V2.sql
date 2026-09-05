-- Tao menu cho chuc nang Import Permission SC V2 va cap quyen admin.
-- Chay lai an toan: MENUURL duoc dung lam khoa nghiep vu.
SET SERVEROUTPUT ON;

DECLARE
    v_menu_id   T_MENUS.ID%TYPE;
    v_parent_id T_MENUS.PARRENTID%TYPE;
    v_admin_id  T_USERS.USERID%TYPE;
BEGIN
    SELECT USERID INTO v_admin_id
      FROM T_USERS
     WHERE LOWER(TRIM(USERNAME)) = 'admin'
       AND ROWNUM = 1;

    -- Dat menu V2 canh chuc nang import cu, khong thay doi menu cu.
    BEGIN
        SELECT PARRENTID INTO v_parent_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) = LOWER('Tool/ImportsPermSC_LD.aspx')
           AND ROWNUM = 1;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_parent_id := 0;
    END;

    BEGIN
        SELECT ID INTO v_menu_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) = LOWER('Tool/ImportsPermSC_LD_V2.aspx')
           AND ROWNUM = 1;

        UPDATE T_MENUS
           SET MENUNAME = 'Cancel Permission SC V2',
               MENUDESC = 'Huy chuyen SC co xem truoc, kiem tra loi va chon tung dong',
               PARRENTID = v_parent_id,
               ISDISPLAY = 1,
               DATEMODIFY = SYSDATE,
               USERMODIFY = v_admin_id
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
                NULL, 'Cancel Permission SC V2',
                'Huy chuyen SC co xem truoc, kiem tra loi va chon tung dong',
                99, v_parent_id, SYSDATE, SYSDATE,
                'Tool/ImportsPermSC_LD_V2.aspx', NULL,
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
    DBMS_OUTPUT.PUT_LINE('MENU_ID=' || v_menu_id || ', PARENT_ID=' || v_parent_id);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

SELECT m.ID, m.MENUNAME, m.MENUURL, m.PARRENTID, u.USERNAME,
       um.R_EDIT, um.R_DEL, um.R_ADD, um.R_PUB
  FROM T_MENUS m
  JOIN T_USERMENU um ON um.MENU_ID = m.ID
  JOIN T_USERS u ON u.USERID = um.USER_ID
 WHERE LOWER(TRIM(m.MENUURL)) = LOWER('Tool/ImportsPermSC_LD_V2.aspx')
   AND LOWER(TRIM(u.USERNAME)) = 'admin';
