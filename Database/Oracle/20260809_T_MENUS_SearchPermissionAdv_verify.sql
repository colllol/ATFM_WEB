-- Verify the menu hierarchy and the direct admin permission.

SELECT m.ID,
       m.PARRENTID,
       p.MENUNAME AS PARENT_NAME,
       m.MENUNAME,
       m.MENUURL,
       m.MENUORDER,
       m.ISDISPLAY
  FROM T_MENUS m
  LEFT JOIN T_MENUS p ON p.ID = m.PARRENTID
 WHERE LOWER(TRIM(m.MENUURL)) = LOWER('Permission/SearchPermissionAdv.aspx');

SELECT u.USERID,
       u.USERNAME,
       um.MENU_ID,
       um.R_EDIT,
       um.R_DEL,
       um.R_ADD,
       um.R_PUB,
       um.GROUP_ID
  FROM T_USERS u
  JOIN T_USERMENU um ON um.USER_ID = u.USERID
  JOIN T_MENUS m ON m.ID = um.MENU_ID
 WHERE LOWER(u.USERNAME) = 'admin'
   AND LOWER(TRIM(m.MENUURL)) = LOWER('Permission/SearchPermissionAdv.aspx')
 ORDER BY NVL(um.GROUP_ID, 0);
