-- ============================================================================
-- Store package cho trang SLOTS/FlightTrackingMap.aspx.
-- API:
--   packageName=TRACKING_MAP_PKG&storeName=GET_FLIGHT_META
--
-- Thay the truy van inline lookup metadata (OPER/PERMTYPE/route/ETD/ETA)
-- theo danh sach callsign dang chuoi phan tach dau phay.
-- Chi doc du lieu. Script chay lai nhieu lan duoc.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM USER_TABLES WHERE TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON';
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20996, 'Thieu bang T_DAY_FLIGHTS_GOINGON');
    END IF;
    DBMS_OUTPUT.PUT_LINE('TRACKING_MAP_PKG precheck: OK');
END;
/

CREATE OR REPLACE PACKAGE TRACKING_MAP_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    -- P_FLIGHT_DATE: ngay bay (YYYY-MM-DD hoac DD-MM-YYYY).
    -- P_CALLSIGNS: danh sach callsign phan tach dau phay (toi da 32767 ky tu / lan goi);
    --              NULL = tra ve toan bo chuyen bay cua ngay.
    PROCEDURE GET_FLIGHT_META
    (
        P_FLIGHT_DATE IN VARCHAR2,
        P_CALLSIGNS   IN CLOB DEFAULT NULL,
        P_OUT_CURSOR  OUT T_CURSOR
    );
END TRACKING_MAP_PKG;
/

CREATE OR REPLACE PACKAGE BODY TRACKING_MAP_PKG AS

    FUNCTION PARSE_DATE(P_VALUE IN VARCHAR2, P_NAME IN VARCHAR2) RETURN DATE
    IS
        V_VALUE VARCHAR2(40) := TRIM(P_VALUE);
    BEGIN
        IF REGEXP_LIKE(V_VALUE, '^[0-9]{4}-[0-9]{2}-[0-9]{2}$') THEN
            RETURN TO_DATE(V_VALUE, 'FXYYYY-MM-DD');
        ELSIF REGEXP_LIKE(V_VALUE, '^[0-9]{2}-[0-9]{2}-[0-9]{4}$') THEN
            RETURN TO_DATE(V_VALUE, 'FXDD-MM-YYYY');
        ELSIF REGEXP_LIKE(V_VALUE, '^[0-9]{2}/[0-9]{2}/[0-9]{4}$') THEN
            RETURN TO_DATE(V_VALUE, 'FXDD/MM/YYYY');
        END IF;
        RAISE_APPLICATION_ERROR(-20997, P_NAME || ' phai dung dinh dang YYYY-MM-DD hoac DD-MM-YYYY');
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -20997 THEN RAISE; END IF;
            RAISE_APPLICATION_ERROR(-20997, P_NAME || ' khong phai la ngay hop le');
    END PARSE_DATE;

    PROCEDURE WRITE_ERROR(P_ACTION IN VARCHAR2) IS
    BEGIN
        PROCESS_PKG.ADD_ERROR_LOG
        (
            P_ACTION,
            SQLCODE,
            SUBSTR(SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE, 1, 200)
        );
    EXCEPTION
        WHEN OTHERS THEN NULL;
    END WRITE_ERROR;

    PROCEDURE GET_FLIGHT_META
    (
        P_FLIGHT_DATE IN VARCHAR2,
        P_CALLSIGNS   IN CLOB DEFAULT NULL,
        P_OUT_CURSOR  OUT T_CURSOR
    )
    IS
        V_DATE      DATE;
        V_CALLSIGNS VARCHAR2(32767);
    BEGIN
        V_DATE := PARSE_DATE(P_FLIGHT_DATE, 'P_FLIGHT_DATE');
        IF P_CALLSIGNS IS NOT NULL THEN
            V_CALLSIGNS := ',' || UPPER(REPLACE(DBMS_LOB.SUBSTR(P_CALLSIGNS, 32765, 1), ' ', '')) || ',';
        END IF;

        OPEN P_OUT_CURSOR FOR
            SELECT UPPER(TRIM(FLIGHTNBR)) FLIGHTNBR,
                   UPPER(TRIM(OPER_ID)) OPER_ID,
                   UPPER(TRIM(PERMTYPE)) PERMTYPE,
                   TRIM(FROM_AIRP) FROM_AIRP,
                   TRIM(TO_AIRP) TO_AIRP,
                   TRIM(ETD) ETD,
                   TRIM(ETA) ETA
              FROM T_DAY_FLIGHTS_GOINGON
             WHERE TRUNC(FLIGHTDATE) = V_DATE
               AND FLIGHTNBR IS NOT NULL
               AND (
                    V_CALLSIGNS IS NULL
                    OR INSTR(V_CALLSIGNS, ',' || UPPER(TRIM(FLIGHTNBR)) || ',') > 0
               );
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('TRACKING_MAP_PKG.GET_FLIGHT_META');
            RAISE;
    END GET_FLIGHT_META;
END TRACKING_MAP_PKG;
/

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'TRACKING_MAP_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF V_COUNT <> 2 THEN
        RAISE_APPLICATION_ERROR(-20998, 'TRACKING_MAP_PKG chua VALID day du');
    END IF;

    DBMS_OUTPUT.PUT_LINE('TRACKING_MAP_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'TRACKING_MAP_PKG'
 ORDER BY OBJECT_TYPE;
