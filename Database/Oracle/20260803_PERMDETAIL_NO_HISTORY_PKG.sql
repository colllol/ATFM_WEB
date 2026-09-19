-- API package tra cuu lich su thay doi chi tiet phep bay NO theo PERM_ID.

CREATE OR REPLACE PACKAGE PERMDETAIL_NO_HISTORY_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_BY_PERM_ID
    (
        P_PERM_ID    IN T_PERMDETAIL_NO_ACTION_HISTORY.PERM_ID%TYPE,
        P_OUT_CURSOR OUT T_CURSOR
    );
END PERMDETAIL_NO_HISTORY_PKG;
/

CREATE OR REPLACE PACKAGE BODY PERMDETAIL_NO_HISTORY_PKG AS
    PROCEDURE GET_BY_PERM_ID
    (
        P_PERM_ID    IN T_PERMDETAIL_NO_ACTION_HISTORY.PERM_ID%TYPE,
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
    BEGIN
        OPEN P_OUT_CURSOR FOR
            SELECT HISTORY_ID,
                   PERM_DETAIL_ID,
                   FLIGHT_PK,
                   PERM_ID,
                   ACTION_TYPE,
                   FLIGHTNBR,
                   ACTION_USER,
                   TO_CHAR(
                       ACTION_DATE,
                       'DD-MM-YYYY HH24:MI:SS'
                   ) AS ACTION_DATE,
                   DBMS_LOB.SUBSTR(
                       CHANGE_DETAIL,
                       4000,
                       1
                   ) AS CHANGE_DETAIL
              FROM T_PERMDETAIL_NO_ACTION_HISTORY h
             WHERE h.PERM_ID = P_PERM_ID
             ORDER BY h.HISTORY_ID DESC;
    END GET_BY_PERM_ID;
END PERMDETAIL_NO_HISTORY_PKG;
/

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERMDETAIL_NO_HISTORY_PKG'
 ORDER BY OBJECT_TYPE;
