-- ============================================================================
-- Trien khai database/menu cho chuc nang Search Permission Adv.
--
-- DBeaver: mo file va chon Execute SQL Script (Alt+X).
-- SQL*Plus/SQLcl:
--   sqlplus ATFM@//HOST:1521/SERVICE @20260810_SearchPermissionAdv_deploy.sql
--
-- Chuc nang truy van truc tiep tu code-behind, khong tao package/bang moi.
-- Script nay:
--   1. Kiem tra cac bang/cot ma chuc nang su dung.
--   2. Tao/cap nhat menu Permission/SearchPermissionAdv.aspx.
--   3. Cap quyen truc tiep cho user admin.
--   4. Xac nhan menu va quyen sau khi trien khai.
-- ============================================================================

-- 1. Precheck cau truc database.
DECLARE
    v_missing VARCHAR2(32767);
    v_count   PLS_INTEGER;

    PROCEDURE require_table(p_table IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TABLES
         WHERE TABLE_NAME = UPPER(p_table);

        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10) || '- TABLE ' || UPPER(p_table);
        END IF;
    END require_table;

    PROCEDURE require_column
    (
        p_table  IN VARCHAR2,
        p_column IN VARCHAR2
    ) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TAB_COLUMNS
         WHERE TABLE_NAME = UPPER(p_table)
           AND COLUMN_NAME = UPPER(p_column);

        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10)
                || '- COLUMN ' || UPPER(p_table) || '.' || UPPER(p_column);
        END IF;
    END require_column;

    PROCEDURE require_master(p_table IN VARCHAR2) IS
    BEGIN
        require_table(p_table);
        require_column(p_table, 'PERM_ID');
        require_column(p_table, 'PERMNBR_ID');
        require_column(p_table, 'PERMNBR');
        require_column(p_table, 'AUTHOR_ID');
        require_column(p_table, 'PERMTYPE');
        require_column(p_table, 'FLIGHTTYPE');
        require_column(p_table, 'VERSION');
        require_column(p_table, 'PERMDATE');
        require_column(p_table, 'OPER_ID');
    END require_master;

    PROCEDURE require_detail_common(p_table IN VARCHAR2) IS
    BEGIN
        require_table(p_table);
        require_column(p_table, 'ID');
        require_column(p_table, 'PERM_ID');
        require_column(p_table, 'FLIGHT_PK');
        require_column(p_table, 'FLIGHTNBR');
        require_column(p_table, 'REGISTRATION');
        require_column(p_table, 'FROM_AIRP');
        require_column(p_table, 'TO_AIRP');
        require_column(p_table, 'ETD');
        require_column(p_table, 'ETA');
        require_column(p_table, 'CRAFT_ID');
        require_column(p_table, 'PURPOSE_ID');
        require_column(p_table, 'MTOW');
        require_column(p_table, 'VIA');
        require_column(p_table, 'REMARK');
        require_column(p_table, 'STATUS');
        require_column(p_table, 'LASTUSER');
        require_column(p_table, 'LASTMODIFY');
    END require_detail_common;
BEGIN
    require_master('T_PERMMASTER_SC');
    require_master('T_PERMMASTER_NO');

    require_detail_common('T_PERMDETAIL_SC');
    require_column('T_PERMDETAIL_SC', 'DAY1');
    require_column('T_PERMDETAIL_SC', 'DAY2');
    require_column('T_PERMDETAIL_SC', 'DAY3');
    require_column('T_PERMDETAIL_SC', 'DAY4');
    require_column('T_PERMDETAIL_SC', 'DAY5');
    require_column('T_PERMDETAIL_SC', 'DAY6');
    require_column('T_PERMDETAIL_SC', 'DAY7');
    require_column('T_PERMDETAIL_SC', 'BEGINDATE');
    require_column('T_PERMDETAIL_SC', 'ENDDATE');

    require_detail_common('T_PERMDETAIL_NO');
    require_column('T_PERMDETAIL_NO', 'DAYSFLIGHT');
    require_column('T_PERMDETAIL_NO', 'MAX_DATE');

    require_table('M_FPAUTHOR');
    require_column('M_FPAUTHOR', 'AUTHOR_CODE');
    require_column('M_FPAUTHOR', 'AUTHOR_NAME');

    require_table('M_CRAFT_TYPE');
    require_column('M_CRAFT_TYPE', 'CRAFT_ID');
    require_column('M_CRAFT_TYPE', 'MA');

    require_table('T_USERS');
    require_table('T_MENUS');
    require_table('T_USERMENU');

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20811,
            'Search Permission Adv thieu cau truc:' || SUBSTR(v_missing, 1, 1800)
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('Search Permission Adv database precheck: OK');
END;
/

-- 2. Tao/cap nhat menu va cap quyen admin. Chay lai khong tao trung menu.
DECLARE
    v_menu_id   T_MENUS.ID%TYPE;
    v_admin_id  T_USERS.USERID%TYPE;
    v_parent_id T_MENUS.ID%TYPE;
BEGIN
    SELECT USERID
      INTO v_admin_id
      FROM T_USERS
     WHERE LOWER(TRIM(USERNAME)) = 'admin'
       AND ROWNUM = 1;

    BEGIN
        SELECT ID
          INTO v_parent_id
          FROM T_MENUS
         WHERE ID = 42;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(
                -20812,
                'Khong tim thay menu cha Flights Permission ID=42'
            );
    END;

    BEGIN
        SELECT ID
          INTO v_menu_id
          FROM T_MENUS
         WHERE LOWER(TRIM(MENUURL)) =
               LOWER('Permission/SearchPermissionAdv.aspx')
           AND ROWNUM = 1;

        UPDATE T_MENUS
           SET MENUNAME         = 'Search Permission Adv',
               MENUDESC         = 'Search SC and NO permissions by permission date',
               MENUORDER        = 24,
               PARRENTID        = v_parent_id,
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
                v_parent_id,
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
       SET R_EDIT     = 1,
           R_DEL      = 1,
           R_ADD      = 1,
           R_PUB      = 1,
           DATEMODIFY = SYSDATE
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
        'Search Permission Adv MENU_ID=' || v_menu_id
        || ', PARENT_ID=' || v_parent_id
        || ', ADMIN_USER_ID=' || v_admin_id
    );
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20813, 'Khong tim thay user admin');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

-- 3. Verify va bao loi neu menu/quyen khong dat.
DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM T_MENUS m
      JOIN T_USERMENU um
        ON um.MENU_ID = m.ID
      JOIN T_USERS u
        ON u.USERID = um.USER_ID
     WHERE LOWER(TRIM(m.MENUURL)) =
           LOWER('Permission/SearchPermissionAdv.aspx')
       AND m.PARRENTID = 42
       AND NVL(m.ISDISPLAY, 0) = 1
       AND LOWER(TRIM(u.USERNAME)) = 'admin'
       AND NVL(um.GROUP_ID, 0) = 0
       AND NVL(um.R_EDIT, 0) = 1
       AND NVL(um.R_DEL, 0) = 1
       AND NVL(um.R_ADD, 0) = 1
       AND NVL(um.R_PUB, 0) = 1;

    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20814,
            'Search Permission Adv menu hoac quyen admin chua dat'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('Search Permission Adv deployment verify: OK');
END;
/

-- 4. Ket qua de doi chieu tren DBeaver/SQL*Plus.
SELECT m.ID,
       m.PARRENTID,
       p.MENUNAME AS PARENT_NAME,
       m.MENUNAME,
       m.MENUURL,
       m.MENUORDER,
       m.ISDISPLAY,
       u.USERNAME,
       um.R_EDIT,
       um.R_DEL,
       um.R_ADD,
       um.R_PUB,
       um.GROUP_ID
  FROM T_MENUS m
  LEFT JOIN T_MENUS p
    ON p.ID = m.PARRENTID
  JOIN T_USERMENU um
    ON um.MENU_ID = m.ID
  JOIN T_USERS u
    ON u.USERID = um.USER_ID
 WHERE LOWER(TRIM(m.MENUURL)) =
       LOWER('Permission/SearchPermissionAdv.aspx')
 ORDER BY LOWER(u.USERNAME), NVL(um.GROUP_ID, 0);
