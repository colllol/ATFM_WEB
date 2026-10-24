-- ============================================================================
-- Store package cho bao cao ADS-B Performance (ReportNew/AdsBPerformanceReport.aspx).
-- Chuyen SQL inline trong code-behind ve package theo chuan API extension:
--   packageName=ADSB_PERFORMANCE_PKG&storeName=GET_LOGS
--   packageName=ADSB_PERFORMANCE_PKG&storeName=GET_OPERATORS
--
-- Nguon du lieu: T_TRACKS_LOG (log ADS-B) + T_DAY_FLIGHTS_GOINGON (lay OPER_ID).
-- Khong thay doi du lieu, chi doc.
-- Chay bang user ATFM tren dung PDB/service. Script co the chay lai nhieu lan.
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

    PROCEDURE require_column(p_table IN VARCHAR2, p_column IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TAB_COLUMNS
         WHERE TABLE_NAME = UPPER(p_table)
           AND COLUMN_NAME = UPPER(p_column);

        IF v_count = 0 THEN
            v_missing := v_missing || ', ' || UPPER(p_table) || '.' || UPPER(p_column);
        END IF;
    END require_column;
BEGIN
    require_table('T_TRACKS_LOG');
    require_table('T_DAY_FLIGHTS_GOINGON');

    require_column('T_TRACKS_LOG', 'TRLOG_ID');
    require_column('T_TRACKS_LOG', 'CALLSIGN');
    require_column('T_TRACKS_LOG', 'FROM_AIRP');
    require_column('T_TRACKS_LOG', 'TO_AIRP');
    require_column('T_TRACKS_LOG', 'ETD');
    require_column('T_TRACKS_LOG', 'ETA');
    require_column('T_TRACKS_LOG', 'STATUS');
    require_column('T_TRACKS_LOG', 'UPDATED_AT_UTC');
    require_column('T_TRACKS_LOG', 'TIME_IN');
    require_column('T_TRACKS_LOG', 'TIME_OUT');
    require_column('T_TRACKS_LOG', 'PERMTYPE');
    require_column('T_TRACKS_LOG', 'DATE');
    require_column('T_DAY_FLIGHTS_GOINGON', 'FLIGHTNBR');
    require_column('T_DAY_FLIGHTS_GOINGON', 'FLIGHTDATE');
    require_column('T_DAY_FLIGHTS_GOINGON', 'OPER_ID');
    require_column('T_DAY_FLIGHTS_GOINGON', 'FROM_AIRP');
    require_column('T_DAY_FLIGHTS_GOINGON', 'TO_AIRP');

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20971,
            'Thieu doi tuong phu thuoc:' || LTRIM(v_missing, ',')
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('ADSB_PERFORMANCE_PKG precheck: OK');
END;
/

CREATE OR REPLACE PACKAGE ADSB_PERFORMANCE_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    -- Danh sach log ADS-B theo khoang ngay (P_TO_DATE bao gom ca ngay ket thuc).
    -- P_PERM_TYPE / P_OPER: NULL hoac 'ALL' = khong loc.
    PROCEDURE GET_LOGS
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_PERM_TYPE  IN VARCHAR2 DEFAULT NULL,
        P_OPER       IN VARCHAR2 DEFAULT NULL,
        P_OUT_CURSOR OUT T_CURSOR
    );

    -- Danh sach hang khai thac (OPER_ID) co trong khoang ngay, phuc vu dropdown loc.
    PROCEDURE GET_OPERATORS
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_PERM_TYPE  IN VARCHAR2 DEFAULT NULL,
        P_OUT_CURSOR OUT T_CURSOR
    );
END ADSB_PERFORMANCE_PKG;
/

CREATE OR REPLACE PACKAGE BODY ADSB_PERFORMANCE_PKG AS

    FUNCTION PARSE_DATE
    (
        P_VALUE IN VARCHAR2,
        P_NAME  IN VARCHAR2
    ) RETURN DATE
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

        RAISE_APPLICATION_ERROR(
            -20972,
            P_NAME || ' phai dung dinh dang YYYY-MM-DD hoac DD-MM-YYYY'
        );
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -20972 THEN
                RAISE;
            END IF;

            RAISE_APPLICATION_ERROR(
                -20972,
                P_NAME || ' khong phai la ngay hop le'
            );
    END PARSE_DATE;

    -- NULL/rong/'ALL' => NULL (khong loc), con lai UPPER + TRIM.
    FUNCTION NORMALIZE_FILTER(P_VALUE IN VARCHAR2) RETURN VARCHAR2
    IS
        V_VALUE VARCHAR2(4000) := UPPER(TRIM(P_VALUE));
    BEGIN
        IF V_VALUE IS NULL OR V_VALUE = 'ALL' THEN
            RETURN NULL;
        END IF;
        RETURN V_VALUE;
    END NORMALIZE_FILTER;

    PROCEDURE VALIDATE_RANGE
    (
        P_FROM_DATE IN DATE,
        P_TO_DATE   IN DATE
    )
    IS
    BEGIN
        IF P_TO_DATE < P_FROM_DATE THEN
            RAISE_APPLICATION_ERROR(
                -20973,
                'P_TO_DATE phai lon hon hoac bang P_FROM_DATE'
            );
        END IF;

        IF P_TO_DATE - P_FROM_DATE > 3660 THEN
            RAISE_APPLICATION_ERROR(
                -20974,
                'Khoang loc toi da la 10 nam'
            );
        END IF;
    END VALIDATE_RANGE;

    PROCEDURE WRITE_ERROR(P_ACTION IN VARCHAR2) IS
    BEGIN
        PROCESS_PKG.ADD_ERROR_LOG
        (
            P_ACTION,
            SQLCODE,
            SUBSTR
            (
                SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                1,
                200
            )
        );
    EXCEPTION
        WHEN OTHERS THEN
            NULL;
    END WRITE_ERROR;

    PROCEDURE GET_LOGS
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_PERM_TYPE  IN VARCHAR2 DEFAULT NULL,
        P_OPER       IN VARCHAR2 DEFAULT NULL,
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
        V_FROM_DATE DATE;
        V_TO_DATE   DATE;
        V_PERM_TYPE VARCHAR2(100);
        V_OPER      VARCHAR2(100);
    BEGIN
        V_FROM_DATE := PARSE_DATE(P_FROM_DATE, 'P_FROM_DATE');
        V_TO_DATE := PARSE_DATE(P_TO_DATE, 'P_TO_DATE');
        VALIDATE_RANGE(V_FROM_DATE, V_TO_DATE);
        V_PERM_TYPE := NORMALIZE_FILTER(P_PERM_TYPE);
        V_OPER := NORMALIZE_FILTER(P_OPER);

        OPEN P_OUT_CURSOR FOR
            WITH LOG_DATA AS
            (
                SELECT L.TRLOG_ID, L.CALLSIGN, L.FROM_AIRP, L.TO_AIRP,
                       L.ETD, L.ETA, L.STATUS, L.UPDATED_AT_UTC,
                       L.TIME_IN, L.TIME_OUT,
                       UPPER(TRIM(L.PERMTYPE)) PERMTYPE,
                       TO_DATE(L."DATE", 'DD-MM-YYYY') LOG_DATE,
                       (SELECT MAX(UPPER(TRIM(D.OPER_ID)))
                          FROM T_DAY_FLIGHTS_GOINGON D
                         WHERE UPPER(TRIM(D.FLIGHTNBR)) = UPPER(TRIM(L.CALLSIGN))
                           AND TRUNC(D.FLIGHTDATE) = TO_DATE(L."DATE", 'DD-MM-YYYY')
                           AND TRIM(D.FROM_AIRP) IS NOT NULL
                           AND TRIM(D.TO_AIRP) IS NOT NULL) OPER_ID
                  FROM T_TRACKS_LOG L
                 WHERE REGEXP_LIKE(L."DATE", '^[0-9]{2}-[0-9]{2}-[0-9]{4}$')
            )
            SELECT TRLOG_ID, CALLSIGN, FROM_AIRP, TO_AIRP, ETD, ETA,
                   STATUS, UPDATED_AT_UTC, TIME_IN, TIME_OUT, PERMTYPE, LOG_DATE, OPER_ID
              FROM LOG_DATA
             WHERE LOG_DATE >= V_FROM_DATE
               AND LOG_DATE < V_TO_DATE + 1
               AND (V_PERM_TYPE IS NULL OR PERMTYPE = V_PERM_TYPE)
               AND (V_OPER IS NULL OR OPER_ID = V_OPER)
             ORDER BY LOG_DATE, CALLSIGN, TRLOG_ID;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('ADSB_PERFORMANCE_PKG.GET_LOGS');
            RAISE;
    END GET_LOGS;

    PROCEDURE GET_OPERATORS
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_PERM_TYPE  IN VARCHAR2 DEFAULT NULL,
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
        V_FROM_DATE DATE;
        V_TO_DATE   DATE;
        V_PERM_TYPE VARCHAR2(100);
    BEGIN
        V_FROM_DATE := PARSE_DATE(P_FROM_DATE, 'P_FROM_DATE');
        V_TO_DATE := PARSE_DATE(P_TO_DATE, 'P_TO_DATE');
        VALIDATE_RANGE(V_FROM_DATE, V_TO_DATE);
        V_PERM_TYPE := NORMALIZE_FILTER(P_PERM_TYPE);

        OPEN P_OUT_CURSOR FOR
            WITH LOG_DATA AS
            (
                SELECT UPPER(TRIM(L.PERMTYPE)) PERMTYPE,
                       TO_DATE(L."DATE", 'DD-MM-YYYY') LOG_DATE,
                       (SELECT MAX(UPPER(TRIM(D.OPER_ID)))
                          FROM T_DAY_FLIGHTS_GOINGON D
                         WHERE UPPER(TRIM(D.FLIGHTNBR)) = UPPER(TRIM(L.CALLSIGN))
                           AND TRUNC(D.FLIGHTDATE) = TO_DATE(L."DATE", 'DD-MM-YYYY')
                           AND TRIM(D.FROM_AIRP) IS NOT NULL
                           AND TRIM(D.TO_AIRP) IS NOT NULL) OPER_ID
                  FROM T_TRACKS_LOG L
                 WHERE REGEXP_LIKE(L."DATE", '^[0-9]{2}-[0-9]{2}-[0-9]{4}$')
            )
            SELECT DISTINCT OPER_ID
              FROM LOG_DATA
             WHERE LOG_DATE >= V_FROM_DATE
               AND LOG_DATE < V_TO_DATE + 1
               AND (V_PERM_TYPE IS NULL OR PERMTYPE = V_PERM_TYPE)
               AND OPER_ID IS NOT NULL
             ORDER BY OPER_ID;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('ADSB_PERFORMANCE_PKG.GET_OPERATORS');
            RAISE;
    END GET_OPERATORS;
END ADSB_PERFORMANCE_PKG;
/

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'ADSB_PERFORMANCE_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF V_COUNT <> 2 THEN
        RAISE_APPLICATION_ERROR(-20975, 'ADSB_PERFORMANCE_PKG chua VALID day du');
    END IF;

    DBMS_OUTPUT.PUT_LINE('ADSB_PERFORMANCE_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'ADSB_PERFORMANCE_PKG'
 ORDER BY OBJECT_TYPE;
