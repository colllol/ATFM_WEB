-- ============================================================================
-- Toi uu ExportBravo (CalendarFlight/CanlendarFlights.aspx).
--
-- Thay doi chinh:
--   1. Khong goi BRAVO_MSG_COPY/BRAVO_BILLINGADDRESS cho tung dong.
--   2. Lay noi dung phep truc tiep bang PERM_ID + FLIGHT_TYPE.
--   3. Loc DATE theo khoang nua mo, khong ep kieu ngam DATE/VARCHAR2.
--   4. Them danh sach cot INSERT de tranh dao ATD/ATA.
--   5. Them index FLIGHTDATE cho lenh DELETE neu chua co index tuong duong.
--
-- Co the chay lai script nhieu lan.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;

    PROCEDURE require_table(p_table_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TABLES
         WHERE TABLE_NAME = UPPER(p_table_name);

        IF v_count = 0 THEN
            v_missing := v_missing || ', TABLE ' || UPPER(p_table_name);
        END IF;
    END require_table;
BEGIN
    require_table('T_DAY_FLIGHTS');
    require_table('T_DAY_FLIGHTS_BRAVO');
    require_table('T_FLIGHTCANCEL');
    require_table('T_PERMMASTER_SC');
    require_table('T_PERMMASTER_NO');

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20831,
            'Thieu doi tuong phu thuoc:' || LTRIM(v_missing, ',')
        );
    END IF;
END;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_IND_COLUMNS
     WHERE TABLE_NAME = 'T_DAY_FLIGHTS_BRAVO'
       AND COLUMN_POSITION = 1
       AND COLUMN_NAME = 'FLIGHTDATE';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_TDF_BRAVO_FLIGHTDATE '
            || 'ON T_DAY_FLIGHTS_BRAVO (FLIGHTDATE)';
        DBMS_OUTPUT.PUT_LINE('Created IX_TDF_BRAVO_FLIGHTDATE');
    ELSE
        DBMS_OUTPUT.PUT_LINE(
            'Skipped IX_TDF_BRAVO_FLIGHTDATE: equivalent leading index exists'
        );
    END IF;
END;
/

CREATE OR REPLACE PACKAGE BRAVO_EXPORT_PKG AS
    PROCEDURE COPY_BRAVO_FLIGHT
    (
        P_DATE        IN  VARCHAR2,
        P_RETURN_CODE OUT NUMBER
    );
END BRAVO_EXPORT_PKG;
/

CREATE OR REPLACE PACKAGE BODY BRAVO_EXPORT_PKG AS
    PROCEDURE COPY_BRAVO_FLIGHT
    (
        P_DATE        IN  VARCHAR2,
        P_RETURN_CODE OUT NUMBER
    )
    IS
        v_flight_date DATE;
        v_cancel_date VARCHAR2(11);
    BEGIN
        P_RETURN_CODE := -99;

        BEGIN
            v_flight_date := TO_DATE(TRIM(P_DATE), 'FXDD-MM-YYYY');
        EXCEPTION
            WHEN OTHERS THEN
                RAISE_APPLICATION_ERROR(
                    -20832,
                    'P_DATE khong hop le, dinh dang yeu cau DD-MM-YYYY'
                );
        END;

        v_cancel_date := TO_CHAR(
            v_flight_date,
            'DD-Mon-RR',
            'NLS_DATE_LANGUAGE=ENGLISH'
        );

        DELETE FROM T_DAY_FLIGHTS_BRAVO b
         WHERE b.FLIGHTDATE >= v_flight_date
           AND b.FLIGHTDATE <  v_flight_date + 1;

        INSERT INTO T_DAY_FLIGHTS_BRAVO
        (
            PURPOSE,
            PERMTYPE,
            FLIGHTDATE,
            FLIGHT_TYPE,
            VIA,
            VALIDHOURS,
            PERMNBR,
            PERM_ID,
            FLIGHTNBR,
            REGISTRATION,
            FROM_AIRP,
            TO_AIRP,
            ETD,
            ETA,
            ATD,
            ATA,
            REMARK,
            CRAFT_TYPE,
            OPER_ID,
            "msg_copy",
            "billaddress"
        )
        SELECT
            d.PURPOSE,
            d.PERMTYPE,
            d.FLIGHTDATE,
            d.FLIGHT_TYPE,
            d.VIA,
            d.VALIDHOURS,
            d.PERMNBR,
            d.PERM_ID,
            d.FLIGHTNBR,
            d.REGISTRATION,
            d.FROM_AIRP,
            d.TO_AIRP,
            d.ETD,
            d.ETA,
            d.ATD,
            d.ATA,
            d.REMARK,
            d.CRAFT_TYPE,
            d.OPER_ID,
            CASE UPPER(TRIM(d.FLIGHT_TYPE))
                WHEN 'SC' THEN DBMS_LOB.SUBSTR(sc.PERMCONTENT1, 3000, 1)
                WHEN 'NO' THEN DBMS_LOB.SUBSTR(no.PERMCONTENT1, 3000, 1)
                ELSE NULL
            END AS MSG_COPY,
            CASE UPPER(TRIM(d.FLIGHT_TYPE))
                WHEN 'SC' THEN sc.BILLINGADDRESS
                WHEN 'NO' THEN no.BILLINGADDRESS
                ELSE NULL
            END AS BILLADDRESS
        FROM T_DAY_FLIGHTS d
        LEFT JOIN T_PERMMASTER_SC sc
          ON UPPER(TRIM(d.FLIGHT_TYPE)) = 'SC'
         AND sc.PERM_ID = d.PERM_ID
        LEFT JOIN T_PERMMASTER_NO no
          ON UPPER(TRIM(d.FLIGHT_TYPE)) = 'NO'
         AND no.PERM_ID = d.PERM_ID
        WHERE d.FLIGHTDATE >= v_flight_date
          AND d.FLIGHTDATE <  v_flight_date + 1
          AND d.PERMNBR <> 'NoPerm'
          AND d.ISSCHEDULE IN (0, 1, 2)
          AND d.ISACCESS = 1
          AND NOT EXISTS
              (
                  SELECT 1
                    FROM T_FLIGHTCANCEL fc
                   WHERE fc.FLIGHTDATE = v_cancel_date
                     AND fc.CALLSIGN = d.FLIGHTNBR
                     AND fc.FROM_AIRP = d.FROM_AIRP
                     AND fc.TO_AIRP = d.TO_AIRP
              );

        COMMIT;
        P_RETURN_CODE := 1;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;

            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG(
                    'BRAVO_EXPORT_PKG.COPY_BRAVO_FLIGHT',
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

            P_RETURN_CODE := -99;
    END COPY_BRAVO_FLIGHT;
END BRAVO_EXPORT_PKG;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'BRAVO_EXPORT_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20833,
            'BRAVO_EXPORT_PKG chua VALID day du'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('BRAVO_EXPORT_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'BRAVO_EXPORT_PKG'
 ORDER BY OBJECT_TYPE;
