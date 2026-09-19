/*
  PERM_PKG.PAcceptedPermSC
  Date: 2026-07-31

  The leading P in PAcceptedPermSC intentionally matches the API URL:
    packageName=PERM_PKG&storeName=PAcceptedPermSC

  Add the declaration to PACKAGE SPEC PERM_PKG and the implementation to
  PACKAGE BODY PERM_PKG. Do not create a standalone procedure.
*/

/* PACKAGE SPEC declaration */
PROCEDURE PAcceptedPermSC
(
    P_ID          IN NUMBER,
    P_RETURN_CODE OUT NUMBER
);

/* PACKAGE BODY implementation */
PROCEDURE PAcceptedPermSC
(
    P_ID          IN NUMBER,
    P_RETURN_CODE OUT NUMBER
)
IS
    vDetailCount PLS_INTEGER := 0;
    vInsertCount PLS_INTEGER := 0;
BEGIN
    P_RETURN_CODE := -1;

    IF P_ID IS NULL OR P_ID <= 0 THEN
        P_RETURN_CODE := -2;
        RETURN;
    END IF;

    SELECT COUNT(*)
    INTO vDetailCount
    FROM T_PERMDETAIL_SC d
    WHERE d.PERM_ID = P_ID
      AND d.FLIGHT_PK IS NOT NULL
      AND d.BEGINDATE IS NOT NULL
      AND d.ENDDATE IS NOT NULL
      AND d.ENDDATE >= d.BEGINDATE;

    IF vDetailCount = 0 THEN
        P_RETURN_CODE := -2;
        RETURN;
    END IF;

    /*
      Delete once for the whole permission instead of scanning the
      6.9-million-row schedule table once per permission detail.
    */
    DELETE FROM T_SCHEDULE_DAYFLIGHTS s
    WHERE s.ISRENDER = 0
      AND s.PERM_ID = P_ID
      AND EXISTS
          (
              SELECT 1
              FROM T_PERMDETAIL_SC d
              WHERE d.PERM_ID = P_ID
                AND d.FLIGHT_PK = s.FLIGHT_PK
          );

    /*
      Render all active dates in one INSERT statement.

      This replaces the row-by-row chain:
        PAcceptedPermSC
          -> ConvertPermScToSchedule
          -> spRENDERSCHEDULEBY_Cus

      The old chain performs one COMMIT for every active flight date. The
      set-based implementation performs one INSERT and one final COMMIT.
      ISO weekday arithmetic makes Monday=1 ... Sunday=7 without depending
      on NLS_DATE_LANGUAGE.
    */
    INSERT INTO T_SCHEDULE_DAYFLIGHTS
    (
        FLIGHT_ID,
        FLIGHT_PK,
        PERM_ID,
        PERMNBR,
        PERMTYPE,
        FLIGHT_TYPE,
        OPER_ID,
        PURPOSE,
        CRAFT_ID,
        MTOW,
        VALIDHOURS,
        DATE_OLD,
        FLIGHTDATE,
        FLIGHTNBR,
        REGISTRATION,
        FROM_AIRP,
        TO_AIRP,
        ETD,
        ETA,
        VIA,
        REMARK,
        LASTUSER
    )
    WITH details AS
    (
        SELECT
            d.ID,
            d.FLIGHT_PK,
            d.PERM_ID,
            d.PURPOSE_ID,
            d.CRAFT_ID,
            d.MTOW,
            d.FLIGHTNBR,
            d.REGISTRATION,
            d.DAY1,
            d.DAY2,
            d.DAY3,
            d.DAY4,
            d.DAY5,
            d.DAY6,
            d.DAY7,
            d.FROM_AIRP,
            d.TO_AIRP,
            d.ETD,
            d.ETA,
            d.VIA,
            d.REMARK,
            d.LASTUSER,
            TRUNC(d.BEGINDATE) AS BEGINDATE,
            TRUNC(d.ENDDATE) - TRUNC(d.BEGINDATE) + 1 AS SPAN_DAYS
        FROM T_PERMDETAIL_SC d
        WHERE d.PERM_ID = P_ID
          AND d.FLIGHT_PK IS NOT NULL
          AND d.BEGINDATE IS NOT NULL
          AND d.ENDDATE IS NOT NULL
          AND d.ENDDATE >= d.BEGINDATE
    ),
    day_offsets AS
    (
        SELECT LEVEL - 1 AS DAY_OFFSET
        FROM DUAL
        CONNECT BY LEVEL <=
            (
                SELECT MAX(SPAN_DAYS)
                FROM details
            )
    ),
    render_rows AS
    (
        SELECT
            d.*,
            d.BEGINDATE + o.DAY_OFFSET AS FLIGHT_DATE,
            TRUNC(d.BEGINDATE + o.DAY_OFFSET)
              - TRUNC(d.BEGINDATE + o.DAY_OFFSET, 'IW') + 1 AS WEEKDAY_NO
        FROM details d
        JOIN day_offsets o
          ON o.DAY_OFFSET < d.SPAN_DAYS
    )
    SELECT
        r.FLIGHT_PK AS FLIGHT_ID,
        r.FLIGHT_PK,
        r.PERM_ID,
        m.PERMNBR_ID AS PERMNBR,
        m.PERMTYPE,
        m.FLIGHTTYPE AS FLIGHT_TYPE,
        m.OPER_ID,
        r.PURPOSE_ID AS PURPOSE,
        r.CRAFT_ID,
        r.MTOW,
        m.VALIDHOURS,
        r.FLIGHT_DATE AS DATE_OLD,
        r.FLIGHT_DATE AS FLIGHTDATE,
        r.FLIGHTNBR,
        r.REGISTRATION,
        r.FROM_AIRP,
        r.TO_AIRP,
        r.ETD,
        r.ETA,
        r.VIA,
        r.REMARK,
        r.LASTUSER
    FROM render_rows r
    JOIN T_PERMMASTER_SC m
      ON m.PERM_ID = r.PERM_ID
    WHERE CASE r.WEEKDAY_NO
              WHEN 1 THEN NVL(TRIM(r.DAY1), '0')
              WHEN 2 THEN NVL(TRIM(r.DAY2), '0')
              WHEN 3 THEN NVL(TRIM(r.DAY3), '0')
              WHEN 4 THEN NVL(TRIM(r.DAY4), '0')
              WHEN 5 THEN NVL(TRIM(r.DAY5), '0')
              WHEN 6 THEN NVL(TRIM(r.DAY6), '0')
              WHEN 7 THEN NVL(TRIM(r.DAY7), '0')
          END <> '0';

    vInsertCount := SQL%ROWCOUNT;

    IF vInsertCount = 0 THEN
        ROLLBACK;
        P_RETURN_CODE := -3;
        RETURN;
    END IF;

    COMMIT;
    P_RETURN_CODE := 1;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        PROCESS_PKG.ADD_ERROR_LOG
        (
            'PERM_PKG.PACCEPTEDPERMSC',
            SQLCODE,
            SUBSTR
            (
                SQLERRM || CHR(10) ||
                DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                1,
                200
            )
        );
        P_RETURN_CODE := -1;
END PAcceptedPermSC;

/* Verification after deployment:

SELECT object_type, status
FROM user_objects
WHERE object_name = 'PERM_PKG'
  AND object_type IN ('PACKAGE', 'PACKAGE BODY');

SELECT type, line, position, text
FROM user_errors
WHERE name = 'PERM_PKG'
ORDER BY sequence;

SELECT argument_name, data_type, in_out, position
FROM user_arguments
WHERE package_name = 'PERM_PKG'
  AND object_name = 'PACCEPTEDPERMSC'
ORDER BY sequence;
*/
