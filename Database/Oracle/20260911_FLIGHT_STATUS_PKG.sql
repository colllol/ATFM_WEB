-- ============================================================================
-- Store package cho nhom bao cao trang thai chuyen bay (ReportNew):
--   - FlightStatusRate.aspx      (GET_STATUS + GET_CANCELLED + GET_OPERATORS)
--   - FlightOperationOverview.aspx (GET_STATUS)
--   - AirportTakeoffLanding.aspx   (GET_STATUS voi P_CURRENT_DAY=0)
--   - FlightTrendAnalysis.aspx     (GET_STATUS + GET_OPERATORS)
-- API:
--   packageName=FLIGHT_STATUS_PKG&storeName=GET_STATUS
--   packageName=FLIGHT_STATUS_PKG&storeName=GET_CANCELLED
--   packageName=FLIGHT_STATUS_PKG&storeName=GET_OPERATORS
--
-- Port nguyen ban logic BuildSql/BuildHistoricalFplCtes trong FlightStatusRate.aspx.cs:
--   P_CURRENT_DAY=1: doc T_DAY_FLIGHTS_GOINGON (ngay hien tai, co trang thai WAIT).
--   P_CURRENT_DAY=0: doc T_FINISHED_FLIGHTS, tim lai FPL theo REGISTRATION+FLIGHTDATE
--                    tu T_DAY_FLIGHTS_GOINGON (+ bang luu tru) de lay EOBT.
-- Chi doc du lieu. Script chay lai nhieu lan duoc.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;

    PROCEDURE require_table(p_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*) INTO v_count FROM USER_TABLES WHERE TABLE_NAME = UPPER(p_name);
        IF v_count = 0 THEN
            v_missing := v_missing || ', TABLE ' || UPPER(p_name);
        END IF;
    END require_table;
BEGIN
    require_table('T_FINISHED_FLIGHTS');
    require_table('T_DAY_FLIGHTS_GOINGON');
    require_table('T_DAY_FLIGHTS_GOINGON032024');
    require_table('T_T_DAY_FLIGHTS_GOINGON');
    require_table('T_DAY_FLIGHTS_CANCEL');
    require_table('T_KHH');

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(-20981, 'Thieu doi tuong phu thuoc:' || LTRIM(v_missing, ','));
    END IF;

    DBMS_OUTPUT.PUT_LINE('FLIGHT_STATUS_PKG precheck: OK');
END;
/

CREATE OR REPLACE PACKAGE FLIGHT_STATUS_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    -- Danh sach chuyen bay kem trang thai phan loai (FLIGHT_STATE):
    --   FINISHED / CANCEL / WAIT / DELAY_15_29 / DELAY_30_59 / DELAY_60_PLUS.
    -- P_TO_DATE bao gom ca ngay ket thuc. P_OPER/P_AIRPORT: NULL hoac 'ALL' = khong loc.
    -- P_CURRENT_DAY: 1 = T_DAY_FLIGHTS_GOINGON (hom nay), 0 = T_FINISHED_FLIGHTS (qua khu).
    PROCEDURE GET_STATUS
    (
        P_FROM_DATE   IN VARCHAR2,
        P_TO_DATE     IN VARCHAR2,
        P_OPER        IN VARCHAR2 DEFAULT NULL,
        P_AIRPORT     IN VARCHAR2 DEFAULT NULL,
        P_CURRENT_DAY IN NUMBER DEFAULT 0,
        P_OUT_CURSOR  OUT T_CURSOR
    );

    -- Danh sach chuyen huy tu T_DAY_FLIGHTS_CANCEL cho ky qua khu.
    -- P_FILTER_PERMTYPE: 1 = chi lay LD/O-F lien quan VN (mac dinh), 0 = lay tat ca.
    PROCEDURE GET_CANCELLED
    (
        P_FROM_DATE       IN VARCHAR2,
        P_TO_DATE         IN VARCHAR2,
        P_FILTER_PERMTYPE IN NUMBER DEFAULT 1,
        P_OUT_CURSOR      OUT T_CURSOR
    );

    -- Danh sach hang khai thac cho dropdown loc (union 3 bang bay + T_KHH).
    PROCEDURE GET_OPERATORS
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_OUT_CURSOR OUT T_CURSOR
    );
END FLIGHT_STATUS_PKG;
/

CREATE OR REPLACE PACKAGE BODY FLIGHT_STATUS_PKG AS

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
        RAISE_APPLICATION_ERROR(-20982, P_NAME || ' phai dung dinh dang YYYY-MM-DD hoac DD-MM-YYYY');
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -20982 THEN RAISE; END IF;
            RAISE_APPLICATION_ERROR(-20982, P_NAME || ' khong phai la ngay hop le');
    END PARSE_DATE;

    FUNCTION NORMALIZE_FILTER(P_VALUE IN VARCHAR2) RETURN VARCHAR2
    IS
        V_VALUE VARCHAR2(4000) := UPPER(TRIM(P_VALUE));
    BEGIN
        IF V_VALUE IS NULL OR V_VALUE = 'ALL' THEN RETURN NULL; END IF;
        RETURN V_VALUE;
    END NORMALIZE_FILTER;

    PROCEDURE VALIDATE_RANGE(P_FROM_DATE IN DATE, P_TO_DATE IN DATE) IS
    BEGIN
        IF P_TO_DATE < P_FROM_DATE THEN
            RAISE_APPLICATION_ERROR(-20983, 'P_TO_DATE phai lon hon hoac bang P_FROM_DATE');
        END IF;
    END VALIDATE_RANGE;

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

    PROCEDURE GET_STATUS
    (
        P_FROM_DATE   IN VARCHAR2,
        P_TO_DATE     IN VARCHAR2,
        P_OPER        IN VARCHAR2 DEFAULT NULL,
        P_AIRPORT     IN VARCHAR2 DEFAULT NULL,
        P_CURRENT_DAY IN NUMBER DEFAULT 0,
        P_OUT_CURSOR  OUT T_CURSOR
    )
    IS
        V_FROM    DATE;
        V_TO_EX   DATE;
        V_OPER    VARCHAR2(100);
        V_AIRPORT VARCHAR2(100);
    BEGIN
        V_FROM := PARSE_DATE(P_FROM_DATE, 'P_FROM_DATE');
        V_TO_EX := PARSE_DATE(P_TO_DATE, 'P_TO_DATE');
        VALIDATE_RANGE(V_FROM, V_TO_EX);
        V_TO_EX := V_TO_EX + 1;
        V_OPER := NORMALIZE_FILTER(P_OPER);
        V_AIRPORT := NORMALIZE_FILTER(P_AIRPORT);

        IF NVL(P_CURRENT_DAY, 0) = 1 THEN
            OPEN P_OUT_CURSOR FOR
                WITH source_rows AS (
                    SELECT f.*,
                           CASE
                             WHEN REGEXP_LIKE(TRIM(f.EOBTDATE), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$')
                               THEN TRIM(f.EOBTDATE)
                             WHEN REGEXP_LIKE(TRIM(f.EOBT), '^((0[1-9]|[12][0-9]|3[01]))?([01][0-9]|2[0-3])[0-5][0-9]$')
                               THEN TRIM(f.EOBT)
                             ELSE NULL
                           END planned_raw,
                           NULLIF(TRIM(f.ATD), '') actual_departure_raw,
                           CASE WHEN (NULLIF(TRIM(f.FROM_AIRP), '') IS NULL OR UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%')
                                     AND (NULLIF(TRIM(f.TO_AIRP), '') IS NULL OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                                THEN 1 ELSE 0 END domestic,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL THEN 1 ELSE 0 END has_from,
                           CASE WHEN NULLIF(TRIM(f.TO_AIRP), '') IS NOT NULL THEN 1 ELSE 0 END has_to,
                           CASE WHEN NULLIF(TRIM(f.ATD), '') IS NOT NULL THEN 1 ELSE 0 END has_atd,
                           CASE WHEN NULLIF(TRIM(f.ATA), '') IS NOT NULL THEN 1 ELSE 0 END has_ata,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' THEN 1 ELSE 0 END departing_vietnam,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_from,
                           CASE WHEN NULLIF(TRIM(f.TO_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.TO_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_to
                      FROM T_DAY_FLIGHTS_GOINGON f
                     WHERE (
                             UPPER(TRIM(f.PERMTYPE))='LD'
                             OR (
                                  UPPER(TRIM(f.PERMTYPE))='O/F'
                                  AND (UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                             )
                           )
                       AND f.OPER_ID IS NOT NULL
                       AND f.FLIGHTDATE >= V_FROM AND f.FLIGHTDATE < V_TO_EX
                       AND (V_OPER IS NULL OR UPPER(TRIM(f.OPER_ID)) = V_OPER)
                       AND (V_AIRPORT IS NULL OR UPPER(TRIM(f.FROM_AIRP)) = V_AIRPORT OR UPPER(TRIM(f.TO_AIRP)) = V_AIRPORT)
                ), parsed_rows AS (
                    SELECT s.*,
                           CASE
                             WHEN REGEXP_LIKE(TRIM(planned_raw), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               CASE
                                 WHEN SUBSTR(TRIM(planned_raw),1,2)=TO_CHAR(FLIGHTDATE,'DD') THEN TRUNC(FLIGHTDATE)
                                 WHEN SUBSTR(TRIM(planned_raw),1,2)=TO_CHAR(FLIGHTDATE+1,'DD') THEN TRUNC(FLIGHTDATE)+1
                                 WHEN SUBSTR(TRIM(planned_raw),1,2)=TO_CHAR(FLIGHTDATE-1,'DD') THEN TRUNC(FLIGHTDATE)-1
                               END
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),3,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),5,2))/1440
                             WHEN REGEXP_LIKE(TRIM(planned_raw), '^([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               TRUNC(FLIGHTDATE)
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),1,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),3,2))/1440
                           END planned_timestamp,
                           CASE
                             WHEN REGEXP_LIKE(TRIM(actual_departure_raw), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               CASE
                                 WHEN SUBSTR(TRIM(actual_departure_raw),1,2)=TO_CHAR(FLIGHTDATE,'DD') THEN TRUNC(FLIGHTDATE)
                                 WHEN SUBSTR(TRIM(actual_departure_raw),1,2)=TO_CHAR(FLIGHTDATE+1,'DD') THEN TRUNC(FLIGHTDATE)+1
                                 WHEN SUBSTR(TRIM(actual_departure_raw),1,2)=TO_CHAR(FLIGHTDATE-1,'DD') THEN TRUNC(FLIGHTDATE)-1
                               END
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),3,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),5,2))/1440
                             WHEN REGEXP_LIKE(TRIM(actual_departure_raw), '^([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               TRUNC(FLIGHTDATE)
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),1,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),3,2))/1440
                           END actual_departure_timestamp
                      FROM source_rows s
                ), metric_rows AS (
                    SELECT p.*,
                           CASE
                             WHEN planned_timestamp IS NOT NULL
                              AND actual_departure_timestamp IS NOT NULL
                              AND actual_departure_timestamp >= planned_timestamp-(6/24)
                              AND actual_departure_timestamp <= planned_timestamp+1
                             THEN ROUND((actual_departure_timestamp-planned_timestamp)*1440)
                           END delay_minutes
                      FROM parsed_rows p
                ), classified_rows AS (
                    SELECT m.*,
                           CASE
                             WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=60 THEN 'DELAY_60_PLUS'
                             WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=30 THEN 'DELAY_30_59'
                             WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=15 THEN 'DELAY_15_29'
                             WHEN (domestic=1 AND has_from=1 AND has_to=1 AND has_atd=1 AND has_ata=1)
                               OR (domestic=0 AND (
                                    (foreign_from=1 AND has_to=1 AND has_ata=1)
                                    OR (foreign_to=1 AND has_from=1 AND has_atd=1)
                                  )) THEN 'FINISHED'
                             WHEN domestic=0 AND (
                                    (foreign_to=1 AND (has_from=0 OR has_atd=0))
                                    OR (foreign_from=1 AND (has_to=0 OR has_ata=0))
                                  ) THEN 'CANCEL'
                             WHEN domestic=1 AND departing_vietnam=1
                                  AND has_to=1 AND planned_timestamp IS NOT NULL AND has_atd=0 THEN 'WAIT'
                             WHEN domestic=1 AND (has_from=0 OR has_to=0 OR has_atd=0 OR has_ata=0) THEN 'CANCEL'
                             ELSE 'CANCEL'
                           END FLIGHT_STATE
                      FROM metric_rows m
                )
                SELECT FLIGHTDATE, FLIGHTNBR, OPER_ID, REGISTRATION, PERMTYPE, FROM_AIRP, TO_AIRP,
                       NULLIF(TRIM(ATD), '') ATDDAY,
                       NULLIF(TRIM(ATA), '') ATADAY,
                       planned_raw EOBTDAY,
                       FLIGHT_STATE
                  FROM classified_rows
                 WHERE UPPER(TRIM(PERMTYPE))='LD'
                    OR (
                         UPPER(TRIM(PERMTYPE))='O/F'
                         AND (
                              FLIGHT_STATE='CANCEL'
                              OR FLIGHT_STATE='FINISHED'
                              OR FLIGHT_STATE LIKE 'DELAY_%'
                         )
                    )
                 ORDER BY FLIGHTDATE, FLIGHTNBR;
        ELSE
            OPEN P_OUT_CURSOR FOR
                WITH historical_fpl_source AS (
                    SELECT 1 source_priority, FLIGHT_ID, FLIGHTDATE, FLIGHTNBR, REGISTRATION,
                           FROM_AIRP, TO_AIRP, EOBTDATE, EOBT
                      FROM T_DAY_FLIGHTS_GOINGON
                     WHERE FLIGHTDATE >= TRUNC(V_FROM) AND FLIGHTDATE < TRUNC(V_TO_EX)
                    UNION ALL
                    SELECT 2 source_priority, FLIGHT_ID, FLIGHTDATE, FLIGHTNBR, REGISTRATION,
                           FROM_AIRP, TO_AIRP, EOBTDATE, EOBT
                      FROM T_DAY_FLIGHTS_GOINGON032024
                     WHERE FLIGHTDATE >= TRUNC(V_FROM) AND FLIGHTDATE < TRUNC(V_TO_EX)
                    UNION ALL
                    SELECT 3 source_priority, FLIGHT_ID, FLIGHTDATE, FLIGHTNBR, REGISTRATION,
                           FROM_AIRP, TO_AIRP, EOBTDATE, EOBT
                      FROM T_T_DAY_FLIGHTS_GOINGON
                     WHERE FLIGHTDATE >= TRUNC(V_FROM) AND FLIGHTDATE < TRUNC(V_TO_EX)
                ), historical_fpl_candidates AS (
                    SELECT ROWIDTOCHAR(f_match.ROWID) finished_rowid,
                           d.EOBTDATE,
                           d.EOBT,
                           d.source_priority,
                           d.FLIGHT_ID source_flight_id,
                           CASE
                             WHEN UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f_match.FLIGHTNBR))
                              AND NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f_match.FROM_AIRP)),'#')
                              AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f_match.TO_AIRP)),'#') THEN 0
                             WHEN UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f_match.FLIGHTNBR)) THEN 1
                             WHEN NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f_match.FROM_AIRP)),'#')
                              AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f_match.TO_AIRP)),'#') THEN 2
                             ELSE 3
                           END match_rank
                      FROM T_FINISHED_FLIGHTS f_match
                      JOIN historical_fpl_source d
                        ON d.FLIGHTDATE >= TRUNC(f_match.FLIGHTDATE)
                       AND d.FLIGHTDATE < TRUNC(f_match.FLIGHTDATE)+1
                       AND UPPER(TRIM(d.REGISTRATION))=UPPER(TRIM(f_match.REGISTRATION))
                     WHERE (
                             UPPER(TRIM(f_match.PERMTYPE))='LD'
                             OR (
                                  UPPER(TRIM(f_match.PERMTYPE))='O/F'
                                  AND (UPPER(TRIM(f_match.FROM_AIRP)) LIKE 'VV%' OR UPPER(TRIM(f_match.TO_AIRP)) LIKE 'VV%')
                             )
                           )
                       AND f_match.OPER_ID IS NOT NULL
                       AND f_match.REGISTRATION IS NOT NULL
                       AND f_match.FLIGHTDATE >= V_FROM AND f_match.FLIGHTDATE < V_TO_EX
                       AND (V_OPER IS NULL OR UPPER(TRIM(f_match.OPER_ID)) = V_OPER)
                       AND (V_AIRPORT IS NULL OR UPPER(TRIM(f_match.FROM_AIRP)) = V_AIRPORT OR UPPER(TRIM(f_match.TO_AIRP)) = V_AIRPORT)
                       AND COALESCE(TRIM(d.EOBTDATE), TRIM(d.EOBT)) IS NOT NULL
                       AND (
                            d.FLIGHT_ID = f_match.FLIGHT_ID
                            OR UPPER(TRIM(d.FLIGHTNBR)) = UPPER(TRIM(f_match.FLIGHTNBR))
                            OR (
                                 NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f_match.FROM_AIRP)),'#')
                                 AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f_match.TO_AIRP)),'#')
                               )
                           )
                ), historical_fpl AS (
                    SELECT c.*,
                           ROW_NUMBER() OVER (
                             PARTITION BY c.finished_rowid
                             ORDER BY c.match_rank,
                                      CASE WHEN REGEXP_LIKE(TRIM(c.EOBTDATE), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN 0 ELSE 1 END,
                                      c.source_priority,
                                      c.source_flight_id
                           ) match_order
                      FROM historical_fpl_candidates c
                ), source_rows AS (
                    SELECT f.*,
                           CASE
                             WHEN REGEXP_LIKE(TRIM(day_fpl.EOBTDATE), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$')
                               THEN TRIM(day_fpl.EOBTDATE)
                             WHEN REGEXP_LIKE(TRIM(day_fpl.EOBT), '^((0[1-9]|[12][0-9]|3[01]))?([01][0-9]|2[0-3])[0-5][0-9]$')
                               THEN TRIM(day_fpl.EOBT)
                             ELSE NULL
                           END planned_raw,
                           NULLIF(TRIM(f.ATD), '') actual_departure_raw,
                           CASE WHEN (NULLIF(TRIM(f.FROM_AIRP), '') IS NULL OR UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%')
                                     AND (NULLIF(TRIM(f.TO_AIRP), '') IS NULL OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                                THEN 1 ELSE 0 END domestic,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL THEN 1 ELSE 0 END has_from,
                           CASE WHEN NULLIF(TRIM(f.TO_AIRP), '') IS NOT NULL THEN 1 ELSE 0 END has_to,
                           CASE WHEN NULLIF(TRIM(f.ATD), '') IS NOT NULL THEN 1 ELSE 0 END has_atd,
                           CASE WHEN NULLIF(TRIM(f.ATA), '') IS NOT NULL THEN 1 ELSE 0 END has_ata,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' THEN 1 ELSE 0 END departing_vietnam,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_from,
                           CASE WHEN NULLIF(TRIM(f.TO_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.TO_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_to
                      FROM T_FINISHED_FLIGHTS f
                      LEFT JOIN historical_fpl day_fpl
                        ON day_fpl.finished_rowid = ROWIDTOCHAR(f.ROWID)
                       AND day_fpl.match_order = 1
                     WHERE (
                             UPPER(TRIM(f.PERMTYPE))='LD'
                             OR (
                                  UPPER(TRIM(f.PERMTYPE))='O/F'
                                  AND (UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                             )
                           )
                       AND f.OPER_ID IS NOT NULL
                       AND f.FLIGHTDATE >= V_FROM AND f.FLIGHTDATE < V_TO_EX
                       AND (V_OPER IS NULL OR UPPER(TRIM(f.OPER_ID)) = V_OPER)
                       AND (V_AIRPORT IS NULL OR UPPER(TRIM(f.FROM_AIRP)) = V_AIRPORT OR UPPER(TRIM(f.TO_AIRP)) = V_AIRPORT)
                ), parsed_rows AS (
                    SELECT s.*,
                           CASE
                             WHEN REGEXP_LIKE(TRIM(planned_raw), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               CASE
                                 WHEN SUBSTR(TRIM(planned_raw),1,2)=TO_CHAR(FLIGHTDATE,'DD') THEN TRUNC(FLIGHTDATE)
                                 WHEN SUBSTR(TRIM(planned_raw),1,2)=TO_CHAR(FLIGHTDATE+1,'DD') THEN TRUNC(FLIGHTDATE)+1
                                 WHEN SUBSTR(TRIM(planned_raw),1,2)=TO_CHAR(FLIGHTDATE-1,'DD') THEN TRUNC(FLIGHTDATE)-1
                               END
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),3,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),5,2))/1440
                             WHEN REGEXP_LIKE(TRIM(planned_raw), '^([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               TRUNC(FLIGHTDATE)
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),1,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(planned_raw),3,2))/1440
                           END planned_timestamp,
                           CASE
                             WHEN REGEXP_LIKE(TRIM(actual_departure_raw), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               CASE
                                 WHEN SUBSTR(TRIM(actual_departure_raw),1,2)=TO_CHAR(FLIGHTDATE,'DD') THEN TRUNC(FLIGHTDATE)
                                 WHEN SUBSTR(TRIM(actual_departure_raw),1,2)=TO_CHAR(FLIGHTDATE+1,'DD') THEN TRUNC(FLIGHTDATE)+1
                                 WHEN SUBSTR(TRIM(actual_departure_raw),1,2)=TO_CHAR(FLIGHTDATE-1,'DD') THEN TRUNC(FLIGHTDATE)-1
                               END
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),3,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),5,2))/1440
                             WHEN REGEXP_LIKE(TRIM(actual_departure_raw), '^([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                               TRUNC(FLIGHTDATE)
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),1,2))/24
                               + TO_NUMBER(SUBSTR(TRIM(actual_departure_raw),3,2))/1440
                           END actual_departure_timestamp
                      FROM source_rows s
                ), metric_rows AS (
                    SELECT p.*,
                           CASE
                             WHEN planned_timestamp IS NOT NULL
                              AND actual_departure_timestamp IS NOT NULL
                              AND actual_departure_timestamp >= planned_timestamp-(6/24)
                              AND actual_departure_timestamp <= planned_timestamp+1
                             THEN ROUND((actual_departure_timestamp-planned_timestamp)*1440)
                           END delay_minutes
                      FROM parsed_rows p
                ), classified_rows AS (
                    SELECT m.*,
                           CASE
                             WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=60 THEN 'DELAY_60_PLUS'
                             WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=30 THEN 'DELAY_30_59'
                             WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=15 THEN 'DELAY_15_29'
                             WHEN (domestic=1 AND has_from=1 AND has_to=1 AND has_atd=1 AND has_ata=1)
                               OR (domestic=0 AND (
                                    (foreign_from=1 AND has_to=1 AND has_ata=1)
                                    OR (foreign_to=1 AND has_from=1 AND has_atd=1)
                                  )) THEN 'FINISHED'
                             WHEN domestic=0 AND (
                                    (foreign_to=1 AND (has_from=0 OR has_atd=0))
                                    OR (foreign_from=1 AND (has_to=0 OR has_ata=0))
                                  ) THEN 'CANCEL'
                             WHEN domestic=1 AND (has_from=0 OR has_to=0 OR has_atd=0 OR has_ata=0) THEN 'CANCEL'
                             ELSE 'CANCEL'
                           END FLIGHT_STATE
                      FROM metric_rows m
                )
                SELECT FLIGHTDATE, FLIGHTNBR, OPER_ID, REGISTRATION, PERMTYPE, FROM_AIRP, TO_AIRP,
                       NULLIF(TRIM(ATD), '') ATDDAY,
                       NULLIF(TRIM(ATA), '') ATADAY,
                       planned_raw EOBTDAY,
                       FLIGHT_STATE
                  FROM classified_rows
                 WHERE UPPER(TRIM(PERMTYPE))='LD'
                    OR (
                         UPPER(TRIM(PERMTYPE))='O/F'
                         AND (
                              FLIGHT_STATE='CANCEL'
                              OR FLIGHT_STATE='FINISHED'
                              OR FLIGHT_STATE LIKE 'DELAY_%'
                         )
                    )
                 ORDER BY FLIGHTDATE, FLIGHTNBR;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('FLIGHT_STATUS_PKG.GET_STATUS');
            RAISE;
    END GET_STATUS;

    PROCEDURE GET_CANCELLED
    (
        P_FROM_DATE       IN VARCHAR2,
        P_TO_DATE         IN VARCHAR2,
        P_FILTER_PERMTYPE IN NUMBER DEFAULT 1,
        P_OUT_CURSOR      OUT T_CURSOR
    )
    IS
        V_FROM   DATE;
        V_TO_EX  DATE;
        V_FILTER PLS_INTEGER;
    BEGIN
        V_FROM := PARSE_DATE(P_FROM_DATE, 'P_FROM_DATE');
        V_TO_EX := PARSE_DATE(P_TO_DATE, 'P_TO_DATE');
        VALIDATE_RANGE(V_FROM, V_TO_EX);
        V_TO_EX := V_TO_EX + 1;
        V_FILTER := NVL(P_FILTER_PERMTYPE, 1);

        OPEN P_OUT_CURSOR FOR
            SELECT FLIGHTDATE, FLIGHTNBR, OPER_ID, REGISTRATION, PERMTYPE, FROM_AIRP, TO_AIRP,
                   NULLIF(TRIM(ATD),'') ATDDAY, NULLIF(TRIM(ATA),'') ATADAY,
                   NULLIF(TRIM(ETD),'') EOBTDAY
              FROM T_DAY_FLIGHTS_CANCEL
             WHERE (
                    V_FILTER = 0
                    OR UPPER(TRIM(PERMTYPE))='LD'
                    OR (
                         UPPER(TRIM(PERMTYPE))='O/F'
                         AND (UPPER(TRIM(FROM_AIRP)) LIKE 'VV%' OR UPPER(TRIM(TO_AIRP)) LIKE 'VV%')
                    )
               )
               AND FLIGHTDATE >= V_FROM AND FLIGHTDATE < V_TO_EX;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('FLIGHT_STATUS_PKG.GET_CANCELLED');
            RAISE;
    END GET_CANCELLED;

    PROCEDURE GET_OPERATORS
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
        V_FROM  DATE;
        V_TO_EX DATE;
    BEGIN
        V_FROM := PARSE_DATE(P_FROM_DATE, 'P_FROM_DATE');
        V_TO_EX := PARSE_DATE(P_TO_DATE, 'P_TO_DATE');
        VALIDATE_RANGE(V_FROM, V_TO_EX);
        V_TO_EX := V_TO_EX + 1;

        OPEN P_OUT_CURSOR FOR
            SELECT OPER_ID FROM (
                SELECT UPPER(TRIM(OPER_ID)) OPER_ID
                  FROM T_DAY_FLIGHTS_GOINGON f
                 WHERE (
                        UPPER(TRIM(f.PERMTYPE))='LD'
                        OR (
                             UPPER(TRIM(f.PERMTYPE))='O/F'
                             AND (UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                        )
                   )
                   AND f.OPER_ID IS NOT NULL
                   AND FLIGHTDATE >= V_FROM AND FLIGHTDATE < V_TO_EX
                UNION
                SELECT UPPER(TRIM(OPER_ID)) OPER_ID
                  FROM T_FINISHED_FLIGHTS f
                 WHERE (
                        UPPER(TRIM(f.PERMTYPE))='LD'
                        OR (
                             UPPER(TRIM(f.PERMTYPE))='O/F'
                             AND (UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                        )
                   )
                   AND f.OPER_ID IS NOT NULL
                   AND FLIGHTDATE >= V_FROM AND FLIGHTDATE < V_TO_EX
                UNION
                SELECT UPPER(TRIM(OPER_ID)) OPER_ID
                  FROM T_DAY_FLIGHTS_CANCEL f
                 WHERE (
                        UPPER(TRIM(f.PERMTYPE))='LD'
                        OR (
                             UPPER(TRIM(f.PERMTYPE))='O/F'
                             AND (UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                        )
                   )
                   AND f.OPER_ID IS NOT NULL
                   AND FLIGHTDATE >= V_FROM AND FLIGHTDATE < V_TO_EX
                UNION
                SELECT CASE WHEN UPPER(TRIM("OPER"))='VNA' THEN 'HVN'
                            ELSE UPPER(TRIM("OPER")) END OPER_ID
                  FROM T_KHH
                 WHERE "OPER" IS NOT NULL
            ) ORDER BY OPER_ID;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('FLIGHT_STATUS_PKG.GET_OPERATORS');
            RAISE;
    END GET_OPERATORS;
END FLIGHT_STATUS_PKG;
/

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'FLIGHT_STATUS_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF V_COUNT <> 2 THEN
        RAISE_APPLICATION_ERROR(-20984, 'FLIGHT_STATUS_PKG chua VALID day du');
    END IF;

    DBMS_OUTPUT.PUT_LINE('FLIGHT_STATUS_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'FLIGHT_STATUS_PKG'
 ORDER BY OBJECT_TYPE;
