CREATE OR REPLACE PACKAGE ATFM_MENU_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_ALL_MENU_4_USER (
        P_USER_ID    IN NUMBER,
        P_OUT_CURSOR OUT T_CURSOR
    );
END ATFM_MENU_PKG;
/

CREATE OR REPLACE PACKAGE BODY ATFM_MENU_PKG AS
    PROCEDURE GET_ALL_MENU_4_USER (
        P_USER_ID    IN NUMBER,
        P_OUT_CURSOR OUT T_CURSOR
    ) IS
    BEGIN
        OPEN P_OUT_CURSOR FOR
            WITH ALLOWED_MENU AS (
                SELECT DISTINCT M.ID
                FROM T_USERMENU UM
                JOIN T_MENUS M ON M.ID = UM.MENU_ID
                WHERE UM.USER_ID = P_USER_ID
                  AND M.ISDISPLAY = 1
            ),
            VISIBLE_MENU AS (
                SELECT ID FROM ALLOWED_MENU
                UNION
                SELECT M.PARRENTID
                FROM T_MENUS M
                JOIN ALLOWED_MENU A ON A.ID = M.ID
                WHERE M.PARRENTID > 0
            )
            SELECT M.ID,
                   M.MENUNAME,
                   M.MENUORDER,
                   M.PARRENTID,
                   M.MENUURL,
                   M.MENUICON
            FROM T_MENUS M
            WHERE M.ISDISPLAY = 1
              AND M.ID IN (SELECT ID FROM VISIBLE_MENU)
            ORDER BY CASE
                         WHEN M.PARRENTID = 0 THEN M.MENUORDER
                         ELSE (SELECT P.MENUORDER FROM T_MENUS P WHERE P.ID = M.PARRENTID)
                     END,
                     CASE WHEN M.PARRENTID = 0 THEN 0 ELSE 1 END,
                     M.MENUORDER,
                     M.ID;
    EXCEPTION
        WHEN OTHERS THEN
            PROCESS_PKG.ADD_ERROR_LOG(
                'ATFM_MENU_PKG.GET_ALL_MENU_4_USER',
                SQLCODE,
                SUBSTR(SQLERRM, 1, 200));
            RAISE;
    END GET_ALL_MENU_4_USER;
END ATFM_MENU_PKG;
/
