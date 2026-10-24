-- ============================================================================
-- Store package cho trang ReportNew/AnomalyWarning.aspx (canh bao delay).
-- API:
--   packageName=DELAY_ALERT_PKG&storeName=GET_TODAY_FLIGHTS
--
-- Tra ve cac chuyen bay hom nay co du ETD + ATD tu T_DAY_FLIGHTS_GOINGON;
-- viec tinh muc delay (15/30/60 phut) van do code-behind xu ly nhu cu.
-- Chi doc du lieu. Script chay lai nhieu lan duoc.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM USER_TABLES WHERE TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON';
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20991, 'Thieu bang T_DAY_FLIGHTS_GOINGON');
    END IF;
    DBMS_OUTPUT.PUT_LINE('DELAY_ALERT_PKG precheck: OK');
END;
/

CREATE OR REPLACE PACKAGE DELAY_ALERT_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_TODAY_FLIGHTS
    (
        P_OUT_CURSOR OUT T_CURSOR
    );
END DELAY_ALERT_PKG;
/

CREATE OR REPLACE PACKAGE BODY DELAY_ALERT_PKG AS

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

    PROCEDURE GET_TODAY_FLIGHTS
    (
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
    BEGIN
        OPEN P_OUT_CURSOR FOR
            SELECT TRUNC(SYSDATE) REPORT_DAY,
                   f.FLIGHTDATE,
                   f.FLIGHTNBR,
                   f.OPER_ID,
                   f.REGISTRATION,
                   f.PERMTYPE,
                   f.FROM_AIRP,
                   f.TO_AIRP,
                   TRIM(f.ETD) ETD,
                   TRIM(f.ATD) ATD
              FROM T_DAY_FLIGHTS_GOINGON f
             WHERE f.FLIGHTDATE >= TRUNC(SYSDATE)
               AND f.FLIGHTDATE < TRUNC(SYSDATE) + 1
               AND f.ETD IS NOT NULL
               AND f.ATD IS NOT NULL
             ORDER BY f.FLIGHTDATE, f.FLIGHTNBR;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('DELAY_ALERT_PKG.GET_TODAY_FLIGHTS');
            RAISE;
    END GET_TODAY_FLIGHTS;
END DELAY_ALERT_PKG;
/

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'DELAY_ALERT_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF V_COUNT <> 2 THEN
        RAISE_APPLICATION_ERROR(-20992, 'DELAY_ALERT_PKG chua VALID day du');
    END IF;

    DBMS_OUTPUT.PUT_LINE('DELAY_ALERT_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'DELAY_ALERT_PKG'
 ORDER BY OBJECT_TYPE;
