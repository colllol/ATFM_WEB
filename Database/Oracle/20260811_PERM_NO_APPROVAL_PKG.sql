-- ============================================================================
-- Duyet phep NO tu Permission/Edit_PermNO4Mail.aspx.
-- API:
--   packageName=PERM_NO_APPROVAL_PKG&storeName=ACCEPT_PERM_NO
--
-- Script co the chay lai nhieu lan.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;

    PROCEDURE require_table(p_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TABLES
         WHERE TABLE_NAME = UPPER(p_name);

        IF v_count = 0 THEN
            v_missing := v_missing || ', TABLE ' || UPPER(p_name);
        END IF;
    END require_table;
BEGIN
    require_table('T_PERMMASTER_NO');
    require_table('T_PERMDETAIL_NO');
    require_table('T_SCHEDULE_DAYFLIGHTS');

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20841,
            'Thieu doi tuong phu thuoc:' || LTRIM(v_missing, ',')
        );
    END IF;
END;
/

CREATE OR REPLACE PACKAGE PERM_NO_APPROVAL_PKG AS
    PROCEDURE ACCEPT_PERM_NO
    (
        P_ID          IN  NUMBER,
        P_RETURN_CODE OUT NUMBER
    );
END PERM_NO_APPROVAL_PKG;
/

CREATE OR REPLACE PACKAGE BODY PERM_NO_APPROVAL_PKG AS
    PROCEDURE ACCEPT_PERM_NO
    (
        P_ID          IN  NUMBER,
        P_RETURN_CODE OUT NUMBER
    )
    IS
        v_detail_count PLS_INTEGER;
        v_insert_count PLS_INTEGER;
    BEGIN
        P_RETURN_CODE := -1;

        IF P_ID IS NULL OR P_ID <= 0 THEN
            P_RETURN_CODE := -2;
            RETURN;
        END IF;

        SELECT COUNT(*)
          INTO v_detail_count
          FROM T_PERMDETAIL_NO d
         WHERE d.PERM_ID = P_ID
           AND d.FLIGHT_PK IS NOT NULL
           AND d.DAYSFLIGHT IS NOT NULL
           AND LENGTH(TRIM(d.DAYSFLIGHT)) = 11;

        IF v_detail_count = 0 THEN
            P_RETURN_CODE := -2;
            RETURN;
        END IF;

        DELETE FROM T_SCHEDULE_DAYFLIGHTS s
         WHERE s.ISRENDER = 0
           AND s.PERM_ID = P_ID
           AND EXISTS
               (
                   SELECT 1
                     FROM T_PERMDETAIL_NO d
                    WHERE d.PERM_ID = P_ID
                      AND d.FLIGHT_PK = s.FLIGHT_PK
               );

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
        SELECT
            d.FLIGHT_PK,
            d.FLIGHT_PK,
            d.PERM_ID,
            m.PERMNBR_ID,
            m.PERMTYPE,
            m.FLIGHTTYPE,
            m.OPER_ID,
            d.PURPOSE_ID,
            d.CRAFT_ID,
            d.MTOW,
            m.VALIDHOURS,
            TO_DATE(
                UPPER(TRIM(d.DAYSFLIGHT)),
                'DD-MON-YYYY',
                'NLS_DATE_LANGUAGE=ENGLISH'
            ),
            TO_DATE(
                UPPER(TRIM(d.DAYSFLIGHT)),
                'DD-MON-YYYY',
                'NLS_DATE_LANGUAGE=ENGLISH'
            ),
            d.FLIGHTNBR,
            d.REGISTRATION,
            d.FROM_AIRP,
            d.TO_AIRP,
            d.ETD,
            d.ETA,
            d.VIA,
            d.REMARK,
            d.LASTUSER
        FROM T_PERMDETAIL_NO d
        JOIN T_PERMMASTER_NO m
          ON m.PERM_ID = d.PERM_ID
        WHERE d.PERM_ID = P_ID
          AND d.FLIGHT_PK IS NOT NULL
          AND d.DAYSFLIGHT IS NOT NULL
          AND LENGTH(TRIM(d.DAYSFLIGHT)) = 11;

        v_insert_count := SQL%ROWCOUNT;

        IF v_insert_count = 0 THEN
            ROLLBACK;
            P_RETURN_CODE := -3;
            RETURN;
        END IF;

        COMMIT;
        P_RETURN_CODE := 1;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;

            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG(
                    'PERM_NO_APPROVAL_PKG.ACCEPT_PERM_NO',
                    SQLCODE,
                    SUBSTR(
                        SQLERRM || CHR(10)
                        || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                        1,
                        200
                    )
                );
            EXCEPTION
                WHEN OTHERS THEN
                    NULL;
            END;

            P_RETURN_CODE := -1;
    END ACCEPT_PERM_NO;
END PERM_NO_APPROVAL_PKG;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'PERM_NO_APPROVAL_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20842,
            'PERM_NO_APPROVAL_PKG chua VALID day du'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('PERM_NO_APPROVAL_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERM_NO_APPROVAL_PKG'
 ORDER BY OBJECT_TYPE;

