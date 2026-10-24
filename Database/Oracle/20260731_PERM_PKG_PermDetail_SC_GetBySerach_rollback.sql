/*
  Rollback implementation for PERM_PKG.PermDetail_SC_GetBySerach.
  Replace the optimized implementation in PACKAGE BODY with this block,
  then compile PERM_PKG BODY.

  The PACKAGE SPEC declaration does not need to change because the optimized
  version deliberately preserves the legacy public signature.
*/

PROCEDURE PermDetail_SC_GetBySerach
(
    p_PERM_ID      IN VARCHAR2,
    p_PURPOSE_ID   IN VARCHAR2,
    p_CRAFT_ID     IN VARCHAR2,
    p_MTOW         IN VARCHAR2,
    p_FLIGHTNBR    IN VARCHAR2,
    p_REGISTRATION IN VARCHAR2,
    p_DAY1         IN VARCHAR2,
    p_DAY2         IN VARCHAR2,
    p_DAY3         IN VARCHAR2,
    p_DAY4         IN VARCHAR2,
    p_DAY5         IN VARCHAR2,
    p_DAY6         IN VARCHAR2,
    p_DAY7         IN VARCHAR2,
    p_FROM_AIRP    IN VARCHAR2,
    p_TO_AIRP      IN VARCHAR2,
    p_ETD          IN VARCHAR2,
    p_ETA          IN VARCHAR2,
    p_VIA          IN VARCHAR2,
    p_STATUS       IN VARCHAR2,
    p_LASTUSER     IN VARCHAR2,
    p_BEGINDATE    IN DATE,
    p_ENDDATE      IN DATE,
    p_REMARK       IN VARCHAR2,
    p_PageSize     IN INT,
    p_PageIndex    IN INT,
    p_rStart       IN NUMBER,
    p_rFinish      IN NUMBER,
    P_RETURN_CODE  OUT T_CURSOR
)
IS
    P_FIRST_INDEX INT;
    P_LAST_INDEX  INT;
BEGIN
    P_LAST_INDEX := p_PageSize * (p_PageIndex + 1);
    P_FIRST_INDEX := P_LAST_INDEX - p_PageSize + 1;

    OPEN P_RETURN_CODE FOR
        SELECT *
        FROM
        (
            SELECT
                ID,
                FLIGHT_PK,
                PERM_ID,
                PURPOSE_ID,
                CRAFT_ID,
                MTOW,
                FLIGHTNBR,
                REGISTRATION,
                DAY1,
                DAY2,
                DAY3,
                DAY4,
                DAY5,
                DAY6,
                DAY7,
                FROM_AIRP,
                TO_AIRP,
                ETA,
                ETD,
                VIA,
                LASTMODIFY,
                LASTUSER,
                BEGINDATE,
                ENDDATE,
                REMARK,
                REMARK_SEND,
                (
                    SELECT DISTINCT PURPOSE_NAME
                    FROM M_FLY_PURPOSE b
                    WHERE b.PURPOSE_CODE = T_PERMDETAIL_SC.PURPOSE_ID
                ) AS PURPOSE_NAME1,
                (
                    SELECT DISTINCT MA
                    FROM M_CRAFT_TYPE c
                    WHERE c.CRAFT_ID = T_PERMDETAIL_SC.CRAFT_ID
                ) AS CRAFT_NAME,
                (
                    SELECT DISTINCT TAITRONG
                    FROM M_CRAFT_TYPE c
                    WHERE c.CRAFT_ID = T_PERMDETAIL_SC.CRAFT_ID
                ) AS TAITRONG,
                (
                    SELECT DISTINCT AE_CODE
                    FROM M_AERO d
                    WHERE d.AE_CODE = T_PERMDETAIL_SC.FROM_AIRP
                ) AS FROM_NAME,
                (
                    SELECT DISTINCT AE_CODE
                    FROM M_AERO d
                    WHERE d.AE_CODE = T_PERMDETAIL_SC.TO_AIRP
                ) AS TO_NAME,
                (
                    SELECT DISTINCT PERMNBR_ID
                    FROM T_PERMMASTER_SC e
                    WHERE e.PERM_ID = T_PERMDETAIL_SC.PERM_ID
                ) AS PERMNBR_ID,
                0 AS PURPOSE_NAME,
                COUNT(*) OVER () AS RECORD_SUM,
                ROWNUM AS RNUM,
                CASE
                    WHEN ENDDATE > SYSDATE THEN 0
                    ELSE 1
                END AS HETHAN,
                STATUS
            FROM T_PERMDETAIL_SC
            WHERE PERM_ID = p_PERM_ID
              AND (
                    p_PURPOSE_ID IS NULL
                    OR p_PURPOSE_ID = '0'
                    OR UPPER(PURPOSE_ID) LIKE '%' || UPPER(p_PURPOSE_ID) || '%'
                  )
              AND (
                    p_CRAFT_ID IS NULL
                    OR p_CRAFT_ID = '0'
                    OR UPPER(CRAFT_ID) LIKE '%' || UPPER(p_CRAFT_ID) || '%'
                  )
              AND (
                    p_MTOW IS NULL
                    OR p_MTOW = '0'
                    OR UPPER(MTOW) LIKE '%' || UPPER(p_MTOW) || '%'
                  )
              AND (
                    p_FLIGHTNBR IS NULL
                    OR p_FLIGHTNBR = '0'
                    OR UPPER(FLIGHTNBR) LIKE '%' || UPPER(p_FLIGHTNBR) || '%'
                  )
              AND (
                    p_REGISTRATION IS NULL
                    OR p_REGISTRATION = '0'
                    OR UPPER(REGISTRATION) LIKE '%' || UPPER(p_REGISTRATION) || '%'
                  )
              AND (
                    p_FROM_AIRP IS NULL
                    OR p_FROM_AIRP = '0'
                    OR UPPER(FROM_AIRP) LIKE '%' || UPPER(p_FROM_AIRP) || '%'
                  )
              AND (
                    p_TO_AIRP IS NULL
                    OR p_TO_AIRP = '0'
                    OR UPPER(TO_AIRP) LIKE '%' || UPPER(p_TO_AIRP) || '%'
                  )
              AND (
                    p_ETD IS NULL
                    OR p_ETD = '0'
                    OR UPPER(ETD) LIKE '%' || UPPER(p_ETD) || '%'
                  )
              AND (
                    p_ETA IS NULL
                    OR p_ETA = '0'
                    OR UPPER(ETA) LIKE '%' || UPPER(p_ETA) || '%'
                  )
              AND (
                    p_VIA IS NULL
                    OR p_VIA = '0'
                    OR UPPER(VIA) LIKE '%' || UPPER(p_VIA) || '%'
                  )
              AND (
                    p_REMARK IS NULL
                    OR p_REMARK = '0'
                    OR UPPER(REMARK) LIKE '%' || UPPER(p_REMARK) || '%'
                  )
              AND (
                    p_BEGINDATE IS NULL
                    OR BEGINDATE = p_BEGINDATE
                  )
              AND (
                    p_ENDDATE IS NULL
                    OR ENDDATE = p_ENDDATE
                  )
            ORDER BY ID ASC
        )
        WHERE RNUM BETWEEN p_rStart AND p_rFinish
        ORDER BY RNUM;
END PermDetail_SC_GetBySerach;

-- ALTER PACKAGE ATFM.PERM_PKG COMPILE BODY;

-- SELECT type, line, position, text
-- FROM user_errors
-- WHERE name = 'PERM_PKG'
-- ORDER BY sequence;
