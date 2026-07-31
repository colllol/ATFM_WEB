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
    vFlightPK T_PERMDETAIL_SC.FLIGHT_PK%TYPE;
    nKq       NUMBER := -1;
    nCount    PLS_INTEGER := 0;
BEGIN
    P_RETURN_CODE := -1;

    IF P_ID IS NULL OR P_ID <= 0 THEN
        P_RETURN_CODE := -2;
        RETURN;
    END IF;

    FOR flight_rec IN
    (
        SELECT ID, FLIGHT_PK
        FROM T_PERMDETAIL_SC
        WHERE PERM_ID = P_ID
          AND FLIGHT_PK IS NOT NULL
        ORDER BY ID
    )
    LOOP
        vFlightPK := flight_rec.FLIGHT_PK;

        DELETE FROM T_SCHEDULE_DAYFLIGHTS d
        WHERE d.ISRENDER = 0
          AND d.FLIGHT_PK = vFlightPK
          AND d.PERM_ID = P_ID;

        nKq := ConvertPermScToSchedule(vFlightPK);

        IF NVL(nKq, -1) = -1 THEN
            RAISE_APPLICATION_ERROR
            (
                -20021,
                'ConvertPermScToSchedule failed for FLIGHT_PK=' ||
                TO_CHAR(vFlightPK)
            );
        END IF;

        nCount := nCount + 1;
    END LOOP;

    IF nCount = 0 THEN
        ROLLBACK;
        P_RETURN_CODE := -2;
        RETURN;
    END IF;

    COMMIT;
    P_RETURN_CODE := nKq;
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
