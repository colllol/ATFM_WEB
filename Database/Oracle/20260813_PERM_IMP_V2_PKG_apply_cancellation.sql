-- Package rieng cho Cancel Permission SC V2.
-- Toan bo staging nam trong T_PERMSC_CANCEL_V2; khong doc/ghi T_PERMSC_IMP.

DECLARE
    v_table_count    PLS_INTEGER;
    v_sequence_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_table_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_PERMSC_CANCEL_V2';

    SELECT COUNT(*) INTO v_sequence_count
      FROM USER_SEQUENCES
     WHERE SEQUENCE_NAME = 'SEQ_T_PERMSC_CANCEL_V2';

    IF v_table_count <> 1 OR v_sequence_count <> 1 THEN
        RAISE_APPLICATION_ERROR(
            -20820,
            'Chua co T_PERMSC_CANCEL_V2/SEQ_T_PERMSC_CANCEL_V2. '
            || 'Chay 20260813_T_PERMSC_CANCEL_V2.sql truoc.'
        );
    END IF;
END;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TSDF_CANCEL_MATCH_V2';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_TSDF_CANCEL_MATCH_V2 ON T_SCHEDULE_DAYFLIGHTS '
            || '(FLIGHTNBR, FROM_AIRP, TO_AIRP, ETD, FLIGHTDATE, ISRENDER, ETA) ONLINE';
    END IF;
END;
/

CREATE OR REPLACE PACKAGE PERM_IMP_V2_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE INSERT_CANCELLATION
    (
        P_CALLSIGN        IN VARCHAR2,
        P_FROMDATE        IN VARCHAR2,
        P_TODATE          IN VARCHAR2,
        P_DAILY           IN VARCHAR2,
        P_CRAFT           IN VARCHAR2,
        P_FROM_AIRP       IN VARCHAR2,
        P_TO_AIRP         IN VARCHAR2,
        P_ETD             IN VARCHAR2,
        P_ETA             IN VARCHAR2,
        P_VIA             IN VARCHAR2,
        P_REMARK          IN VARCHAR2,
        P_PERMTYPE        IN VARCHAR2,
        P_FLIGHTTYPE      IN VARCHAR2,
        P_PERMNBR         IN VARCHAR2,
        P_SEASON          IN VARCHAR2,
        P_AUTHOR          IN VARCHAR2,
        P_PERMDATE        IN VARCHAR2,
        P_PURPOSE         IN VARCHAR2,
        P_VERSION         IN VARCHAR2,
        P_REGISTRATION    IN VARCHAR2,
        P_CREATED_BY      IN VARCHAR2,
        P_IMPORT_BATCH_ID IN VARCHAR2,
        P_OUT             OUT NUMBER
    );

    PROCEDURE REFRESH_VALIDATION
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    );

    PROCEDURE SEARCH_PENDING
    (
        P_CREATED_BY  IN VARCHAR2,
        P_PERMNBR     IN VARCHAR2,
        P_CALLSIGN    IN VARCHAR2,
        P_FROMDATE    IN VARCHAR2,
        P_TODATE      IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT T_CURSOR
    );

    PROCEDURE GET_CANCELLATION_MATCHES
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT T_CURSOR
    );

    PROCEDURE DELETE_CANCELLATIONS
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    );

    PROCEDURE APPLY_CANCELLATIONS
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    );
END PERM_IMP_V2_PKG;
/

CREATE OR REPLACE PACKAGE BODY PERM_IMP_V2_PKG AS
    FUNCTION clean_user(P_CREATED_BY IN VARCHAR2) RETURN VARCHAR2 IS
    BEGIN
        IF TRIM(P_CREATED_BY) IS NULL THEN
            RAISE_APPLICATION_ERROR(-20821, 'Khong xac dinh duoc nguoi thao tac');
        END IF;
        RETURN UPPER(TRIM(P_CREATED_BY));
    END clean_user;

    PROCEDURE validate_ids(P_STAGING_IDS IN VARCHAR2) IS
    BEGIN
        IF P_STAGING_IDS IS NULL OR
           NOT REGEXP_LIKE(TRIM(P_STAGING_IDS), '^[0-9]+(,[0-9]+)*$') THEN
            RAISE_APPLICATION_ERROR(-20822, 'Danh sach staging ID khong hop le');
        END IF;
    END validate_ids;

    FUNCTION resolve_oper(P_CALLSIGN IN VARCHAR2) RETURN VARCHAR2 IS
        v_oper VARCHAR2(10);
        v_call VARCHAR2(30) := REGEXP_REPLACE(UPPER(TRIM(P_CALLSIGN)), '[^A-Z0-9]', '');
    BEGIN
        BEGIN
            SELECT OPER_ICAO
              INTO v_oper
              FROM
              (
                  SELECT UPPER(TRIM(OPER_ICAO)) AS OPER_ICAO,
                         CASE
                             WHEN UPPER(TRIM(OPER_ICAO)) = SUBSTR(v_call, 1, 3) THEN 1
                             ELSE 2
                         END AS PRIORITY_NO
                    FROM M_OPER
                   WHERE UPPER(TRIM(OPER_ICAO)) = SUBSTR(v_call, 1, 3)
                      OR UPPER(TRIM(OPER_IATA)) = SUBSTR(v_call, 1, 2)
                   ORDER BY PRIORITY_NO
              )
             WHERE ROWNUM = 1;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                v_oper := SUBSTR(v_call, 1, 3);
        END;
        RETURN v_oper;
    END resolve_oper;

    FUNCTION clean_time
    (
        P_VALUE IN VARCHAR2,
        P_FIELD IN VARCHAR2
    ) RETURN VARCHAR2 IS
        v_value    VARCHAR2(20);
        v_base     VARCHAR2(4);
        v_next_day BOOLEAN;
    BEGIN
        v_value := UPPER(REPLACE(REPLACE(TRIM(P_VALUE), ':', ''), ' ', ''));
        v_value := REGEXP_REPLACE(v_value, '\+1$', '+');
        v_next_day := REGEXP_LIKE(v_value, '\+$');
        v_base := REGEXP_REPLACE(v_value, '\+$', '');

        IF NOT REGEXP_LIKE(v_base, '^[0-9]{3,4}$') THEN
            RAISE_APPLICATION_ERROR(
                -20825,
                P_FIELD || ' khong hop le; yeu cau HH24MI, co the kem + hoac +1'
            );
        END IF;

        v_base := LPAD(v_base, 4, '0');
        IF TO_NUMBER(SUBSTR(v_base, 1, 2)) > 23
           OR TO_NUMBER(SUBSTR(v_base, 3, 2)) > 59 THEN
            RAISE_APPLICATION_ERROR(
                -20825,
                P_FIELD || ' khong hop le; yeu cau HH24MI, co the kem + hoac +1'
            );
        END IF;

        RETURN v_base || CASE WHEN v_next_day THEN '+' ELSE NULL END;
    END clean_time;

    PROCEDURE INSERT_CANCELLATION
    (
        P_CALLSIGN        IN VARCHAR2,
        P_FROMDATE        IN VARCHAR2,
        P_TODATE          IN VARCHAR2,
        P_DAILY           IN VARCHAR2,
        P_CRAFT           IN VARCHAR2,
        P_FROM_AIRP       IN VARCHAR2,
        P_TO_AIRP         IN VARCHAR2,
        P_ETD             IN VARCHAR2,
        P_ETA             IN VARCHAR2,
        P_VIA             IN VARCHAR2,
        P_REMARK          IN VARCHAR2,
        P_PERMTYPE        IN VARCHAR2,
        P_FLIGHTTYPE      IN VARCHAR2,
        P_PERMNBR         IN VARCHAR2,
        P_SEASON          IN VARCHAR2,
        P_AUTHOR          IN VARCHAR2,
        P_PERMDATE        IN VARCHAR2,
        P_PURPOSE         IN VARCHAR2,
        P_VERSION         IN VARCHAR2,
        P_REGISTRATION    IN VARCHAR2,
        P_CREATED_BY      IN VARCHAR2,
        P_IMPORT_BATCH_ID IN VARCHAR2,
        P_OUT             OUT NUMBER
    ) IS
        v_id        NUMBER;
        v_from_date DATE;
        v_to_date   DATE;
        v_perm_date DATE;
        v_user      VARCHAR2(100);
        v_oper      VARCHAR2(10);
        v_callsign  VARCHAR2(30);
        v_from_airp VARCHAR2(4);
        v_to_airp   VARCHAR2(4);
        v_etd       VARCHAR2(5);
        v_eta       VARCHAR2(5);
        v_error_code NUMBER;
        v_error_msg  VARCHAR2(1000);
    BEGIN
        P_OUT := -1;
        v_from_date := TO_DATE(TRIM(P_FROMDATE), 'FXDD-MM-YYYY');
        v_to_date := TO_DATE(TRIM(P_TODATE), 'FXDD-MM-YYYY');
        v_perm_date := CASE WHEN TRIM(P_PERMDATE) IS NULL THEN NULL
                            ELSE TO_DATE(TRIM(P_PERMDATE), 'FXDD-MM-YYYY') END;
        v_user := clean_user(P_CREATED_BY);
        v_callsign := UPPER(TRIM(PERM_IMP_PKG.GetCallSign_ICAO(P_CALLSIGN)));
        v_from_airp := UPPER(TRIM(PERM_IMP_PKG.Getaero(P_FROM_AIRP)));
        v_to_airp := UPPER(TRIM(PERM_IMP_PKG.Getaero(P_TO_AIRP)));
        v_etd := clean_time(P_ETD, 'ETD');
        v_eta := clean_time(P_ETA, 'ETA');
        v_oper := resolve_oper(v_callsign);

        IF v_to_date < v_from_date THEN
            RAISE_APPLICATION_ERROR(-20823, 'Den ngay nho hon Tu ngay');
        END IF;
        IF LENGTH(TRIM(P_PERMNBR)) > 8 THEN
            RAISE_APPLICATION_ERROR(-20824, 'PERMNBR vuot qua 8 ky tu');
        END IF;

        v_id := SEQ_T_PERMSC_CANCEL_V2.NEXTVAL;
        INSERT INTO T_PERMSC_CANCEL_V2
        (
            ID, IMPORT_BATCH_ID, CALLSIGN, FROMDATE, TODATE, DAILY,
            CRAFT, FROM_AIRP, TO_AIRP, ETD, ETA, VIA, PERMTYPE,
            FLIGHTTYPE, REMARK, PERMDATE, AUTHOR, OPER, SEASON,
            PERMNBR, VERSION, PURPOSE, REGISTRATION,
            CREATED_BY, CREATED_AT, PROCESS_STATUS
        )
        VALUES
        (
            v_id, TRIM(P_IMPORT_BATCH_ID),
            v_callsign,
            v_from_date, v_to_date, TRIM(P_DAILY), UPPER(TRIM(P_CRAFT)),
            v_from_airp, v_to_airp,
            v_etd, v_eta, UPPER(TRIM(P_VIA)),
            NVL(UPPER(TRIM(P_PERMTYPE)), 'LD'), UPPER(TRIM(P_FLIGHTTYPE)),
            TRIM(P_REMARK), v_perm_date, UPPER(TRIM(P_AUTHOR)),
            v_oper, UPPER(TRIM(P_SEASON)),
            UPPER(TRIM(P_PERMNBR)), NVL(UPPER(TRIM(P_VERSION)), 'A'),
            UPPER(TRIM(P_PURPOSE)), UPPER(TRIM(P_REGISTRATION)),
            v_user, SYSDATE, 'PENDING'
        );
        COMMIT;
        P_OUT := v_id;
    EXCEPTION
        WHEN OTHERS THEN
            v_error_code := SQLCODE;
            v_error_msg := SUBSTR(
                SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                1,
                1000
            );
            ROLLBACK;
            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG(
                    'PERM_IMP_V2_INSERT',
                    v_error_code,
                    SUBSTR(v_error_msg, 1, 200)
                );
            EXCEPTION WHEN OTHERS THEN NULL;
            END;
            P_OUT := v_error_code;
    END INSERT_CANCELLATION;

    PROCEDURE REFRESH_VALIDATION
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    ) IS
        v_user VARCHAR2(100);
    BEGIN
        validate_ids(P_STAGING_IDS);
        v_user := clean_user(P_CREATED_BY);
        UPDATE T_PERMSC_CANCEL_V2 i
           SET (PROCESS_STATUS, ERROR_MESSAGE) =
               (
                   SELECT CASE WHEN COUNT(*) > 0 THEN 'PENDING' ELSE 'ERROR' END,
                          CASE WHEN COUNT(*) > 0 THEN NULL
                               ELSE 'Khong tim thay chuyen bay/phep SC tuong ung de huy' END
                     FROM T_PERMDETAIL_SC d,
                          T_PERMMASTER_SC m
                    WHERE m.PERM_ID = d.PERM_ID
                      AND UPPER(TRIM(d.FLIGHTNBR)) = UPPER(TRIM(i.CALLSIGN))
                      AND UPPER(TRIM(d.FROM_AIRP)) = UPPER(TRIM(i.FROM_AIRP))
                      AND UPPER(TRIM(d.TO_AIRP)) = UPPER(TRIM(i.TO_AIRP))
                      AND TRIM(d.ETD) = TRIM(i.ETD)
                      AND (TRIM(d.ETA) IS NULL OR TRIM(i.ETA) IS NULL OR TRIM(d.ETA) = TRIM(i.ETA))
                      AND d.STATUS IN (0, 1)
                      AND d.BEGINDATE <= i.TODATE
                      AND d.ENDDATE >= i.FROMDATE
                      AND (INSTR(i.DAILY, '1') = 0 OR d.DAY1 = '1')
                      AND (INSTR(i.DAILY, '2') = 0 OR d.DAY2 = '2')
                      AND (INSTR(i.DAILY, '3') = 0 OR d.DAY3 = '3')
                      AND (INSTR(i.DAILY, '4') = 0 OR d.DAY4 = '4')
                      AND (INSTR(i.DAILY, '5') = 0 OR d.DAY5 = '5')
                      AND (INSTR(i.DAILY, '6') = 0 OR d.DAY6 = '6')
                      AND (INSTR(i.DAILY, '7') = 0 OR d.DAY7 = '7')
               )
         WHERE i.CREATED_BY = v_user
           AND i.PROCESS_STATUS IN ('PENDING', 'ERROR')
           AND INSTR(
               ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
               ',' || TO_CHAR(i.ID) || ','
           ) > 0;
        COMMIT;
        P_OUT := 1;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            P_OUT := -1;
    END REFRESH_VALIDATION;

    PROCEDURE SEARCH_PENDING
    (
        P_CREATED_BY  IN VARCHAR2,
        P_PERMNBR     IN VARCHAR2,
        P_CALLSIGN    IN VARCHAR2,
        P_FROMDATE    IN VARCHAR2,
        P_TODATE      IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT T_CURSOR
    ) IS
        v_from_date DATE;
        v_to_date   DATE;
        v_user      VARCHAR2(100);
    BEGIN
        IF P_STAGING_IDS IS NOT NULL THEN validate_ids(P_STAGING_IDS); END IF;
        v_user := clean_user(P_CREATED_BY);
        v_from_date := CASE WHEN TRIM(P_FROMDATE) IS NULL THEN NULL
                            ELSE TO_DATE(TRIM(P_FROMDATE), 'FXDD-MM-YYYY') END;
        v_to_date := CASE WHEN TRIM(P_TODATE) IS NULL THEN NULL
                          ELSE TO_DATE(TRIM(P_TODATE), 'FXDD-MM-YYYY') END;

        OPEN P_OUT FOR
            SELECT i.ID AS STAGING_ID, i.IMPORT_BATCH_ID, i.CALLSIGN,
                   TO_CHAR(i.FROMDATE, 'DD-MM-YYYY') AS FROMDATE,
                   TO_CHAR(i.TODATE, 'DD-MM-YYYY') AS TODATE,
                   i.DAILY, i.CRAFT, i.FROM_AIRP, i.TO_AIRP,
                   i.ETD, i.ETA, i.VIA, i.REMARK, i.PERMTYPE,
                   i.FLIGHTTYPE, i.PERMNBR, i.AUTHOR, i.OPER,
                   i.PROCESS_STATUS, i.ERROR_MESSAGE,
                   TO_CHAR(i.CREATED_AT, 'DD-MM-YYYY HH24:MI:SS') AS CREATED_AT
              FROM T_PERMSC_CANCEL_V2 i
             WHERE i.CREATED_BY = v_user
               AND i.PROCESS_STATUS IN ('PENDING', 'ERROR')
               AND (TRIM(P_PERMNBR) IS NULL OR UPPER(i.PERMNBR) LIKE '%' || UPPER(TRIM(P_PERMNBR)) || '%')
               AND (TRIM(P_CALLSIGN) IS NULL OR UPPER(i.CALLSIGN) LIKE '%' || UPPER(TRIM(P_CALLSIGN)) || '%')
               AND (v_from_date IS NULL OR i.FROMDATE >= v_from_date)
               AND (v_to_date IS NULL OR i.TODATE < v_to_date + 1)
               AND (
                   P_STAGING_IDS IS NULL OR INSTR(
                       ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                       ',' || TO_CHAR(i.ID) || ','
                   ) > 0
               )
             ORDER BY i.CREATED_AT DESC, i.ID DESC;
    END SEARCH_PENDING;

    PROCEDURE GET_CANCELLATION_MATCHES
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT T_CURSOR
    ) IS
        v_user VARCHAR2(100);
    BEGIN
        validate_ids(P_STAGING_IDS);
        v_user := clean_user(P_CREATED_BY);
        OPEN P_OUT FOR
            SELECT DISTINCT
                   i.ID AS STAGING_ID, d.FLIGHTNBR AS CALLSIGN, m.PERMNBR_ID,
                   d.FROM_AIRP, d.TO_AIRP, d.ETD, d.ETA, m.OPER_ID AS OPER,
                   m.SEASON, m.PERMTYPE, d.REMARK, d.PURPOSE_ID AS PURPOSE,
                   d.FLIGHT_PK AS IDPERMDETAIL, i.PERMNBR AS THAMCHIEU,
                   d.BEGINDATE AS FROMDATE, d.ENDDATE AS TODATE,
                   i.FROMDATE AS HUY_FROMDATE, i.TODATE AS HUY_TODATE,
                   REPLACE(d.DAY1 || d.DAY2 || d.DAY3 || d.DAY4 ||
                           d.DAY5 || d.DAY6 || d.DAY7, '0', '.') AS DAILY_PHEP,
                   i.DAILY, 'HUY' AS LOAIPHEP
              FROM T_PERMSC_CANCEL_V2 i
              JOIN T_PERMDETAIL_SC d
                ON UPPER(TRIM(d.FLIGHTNBR)) = UPPER(TRIM(i.CALLSIGN))
               AND UPPER(TRIM(d.FROM_AIRP)) = UPPER(TRIM(i.FROM_AIRP))
               AND UPPER(TRIM(d.TO_AIRP)) = UPPER(TRIM(i.TO_AIRP))
               AND TRIM(d.ETD) = TRIM(i.ETD)
               AND (TRIM(d.ETA) IS NULL OR TRIM(i.ETA) IS NULL OR TRIM(d.ETA) = TRIM(i.ETA))
               AND d.STATUS IN (0, 1)
               AND d.BEGINDATE <= i.TODATE
               AND d.ENDDATE >= i.FROMDATE
               AND (INSTR(i.DAILY, '1') = 0 OR d.DAY1 = '1')
               AND (INSTR(i.DAILY, '2') = 0 OR d.DAY2 = '2')
               AND (INSTR(i.DAILY, '3') = 0 OR d.DAY3 = '3')
               AND (INSTR(i.DAILY, '4') = 0 OR d.DAY4 = '4')
               AND (INSTR(i.DAILY, '5') = 0 OR d.DAY5 = '5')
               AND (INSTR(i.DAILY, '6') = 0 OR d.DAY6 = '6')
               AND (INSTR(i.DAILY, '7') = 0 OR d.DAY7 = '7')
              JOIN T_PERMMASTER_SC m ON m.PERM_ID = d.PERM_ID
             WHERE i.CREATED_BY = v_user
               AND i.PROCESS_STATUS = 'PENDING'
               AND INSTR(
                   ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                   ',' || TO_CHAR(i.ID) || ','
               ) > 0
             ORDER BY STAGING_ID, FROMDATE, PERMNBR_ID;
    END GET_CANCELLATION_MATCHES;

    PROCEDURE DELETE_CANCELLATIONS
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    ) IS
        v_rows PLS_INTEGER;
        v_user VARCHAR2(100);
    BEGIN
        validate_ids(P_STAGING_IDS);
        v_user := clean_user(P_CREATED_BY);
        UPDATE T_PERMSC_CANCEL_V2 i
           SET PROCESS_STATUS = 'CANCELLED',
               PROCESSED_AT = SYSDATE,
               ERROR_MESSAGE = NULL
         WHERE i.CREATED_BY = v_user
           AND i.PROCESS_STATUS IN ('PENDING', 'ERROR')
           AND INSTR(
               ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
               ',' || TO_CHAR(i.ID) || ','
           ) > 0;
        v_rows := SQL%ROWCOUNT;
        COMMIT;
        P_OUT := CASE WHEN v_rows > 0 THEN 1 ELSE 0 END;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            P_OUT := -1;
    END DELETE_CANCELLATIONS;

    PROCEDURE APPLY_CANCELLATIONS
    (
        P_CREATED_BY  IN VARCHAR2,
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    ) IS
        v_selected_count PLS_INTEGER;
        v_matched_count  PLS_INTEGER;
        v_updated_count  PLS_INTEGER;
        v_error          VARCHAR2(2000);
        v_user           VARCHAR2(100) := clean_user(P_CREATED_BY);
    BEGIN
        P_OUT := -1;
        validate_ids(P_STAGING_IDS);

        SELECT COUNT(*) INTO v_selected_count
          FROM T_PERMSC_CANCEL_V2 i
         WHERE i.CREATED_BY = v_user
           AND i.PROCESS_STATUS = 'PENDING'
           AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                     ',' || TO_CHAR(i.ID) || ',') > 0;

        SELECT COUNT(DISTINCT i.ID) INTO v_matched_count
          FROM T_PERMSC_CANCEL_V2 i
          JOIN T_PERMDETAIL_SC d
            ON UPPER(TRIM(d.FLIGHTNBR)) = UPPER(TRIM(i.CALLSIGN))
           AND UPPER(TRIM(d.FROM_AIRP)) = UPPER(TRIM(i.FROM_AIRP))
           AND UPPER(TRIM(d.TO_AIRP)) = UPPER(TRIM(i.TO_AIRP))
           AND TRIM(d.ETD) = TRIM(i.ETD)
           AND (TRIM(d.ETA) IS NULL OR TRIM(i.ETA) IS NULL OR TRIM(d.ETA) = TRIM(i.ETA))
           AND d.STATUS IN (0, 1)
           AND d.BEGINDATE <= i.TODATE
           AND d.ENDDATE >= i.FROMDATE
           AND (INSTR(i.DAILY, '1') = 0 OR d.DAY1 = '1')
           AND (INSTR(i.DAILY, '2') = 0 OR d.DAY2 = '2')
           AND (INSTR(i.DAILY, '3') = 0 OR d.DAY3 = '3')
           AND (INSTR(i.DAILY, '4') = 0 OR d.DAY4 = '4')
           AND (INSTR(i.DAILY, '5') = 0 OR d.DAY5 = '5')
           AND (INSTR(i.DAILY, '6') = 0 OR d.DAY6 = '6')
           AND (INSTR(i.DAILY, '7') = 0 OR d.DAY7 = '7')
         WHERE i.CREATED_BY = v_user
           AND i.PROCESS_STATUS = 'PENDING'
           AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                     ',' || TO_CHAR(i.ID) || ',') > 0;

        IF v_selected_count = 0 THEN
            RAISE_APPLICATION_ERROR(-20831, 'Khong con du lieu huy chuyen de xu ly');
        END IF;
        IF v_matched_count <> v_selected_count THEN
            RAISE_APPLICATION_ERROR(-20832, 'Co dong khong con tim thay phep SC tuong ung');
        END IF;

        SAVEPOINT APPLY_CANCEL_V2;
        UPDATE T_PERMSC_CANCEL_V2 i
           SET PROCESS_STATUS = 'PROCESSING', ERROR_MESSAGE = NULL
         WHERE i.CREATED_BY = v_user
           AND i.PROCESS_STATUS = 'PENDING'
           AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                     ',' || TO_CHAR(i.ID) || ',') > 0;

        FOR h IN
        (
            SELECT * FROM
            (
                SELECT i.*,
                       FNCREATEPERMSC_NBR_ID(
                           i.AUTHOR, i.PERMTYPE, i.PERMNBR, i.SEASON,
                           NVL(i.PERMDATE, TRUNC(SYSDATE))
                       ) AS GENERATED_PERMNBR_ID,
                       ROW_NUMBER() OVER
                       (
                           PARTITION BY FNCREATEPERMSC_NBR_ID(
                               i.AUTHOR, i.PERMTYPE, i.PERMNBR, i.SEASON,
                               NVL(i.PERMDATE, TRUNC(SYSDATE))
                           )
                           ORDER BY i.ID
                       ) AS RN
                  FROM T_PERMSC_CANCEL_V2 i
                 WHERE i.CREATED_BY = v_user
                   AND i.PROCESS_STATUS = 'PROCESSING'
                   AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                             ',' || TO_CHAR(i.ID) || ',') > 0
            ) WHERE RN = 1
        ) LOOP
            INSERT INTO T_PERMMASTER_SC
            (
                PERMNBR_ID, AUTHOR_ID, PERMTYPE, FLIGHTTYPE, PERMNBR,
                VERSION, PERMDATE, OPER_ID, REFERENCE, VALIDHOURS,
                LASTUSER, BILLINGADDRESS, PERMCONTENT, SEASON
            )
            SELECT h.GENERATED_PERMNBR_ID, h.AUTHOR, h.PERMTYPE,
                   h.FLIGHTTYPE, h.PERMNBR, NVL(TRIM(h.VERSION), 'A'),
                   NVL(h.PERMDATE, TRUNC(SYSDATE)), h.OPER, h.REFERENCE,
                   CASE WHEN h.PERMTYPE = 'LD' THEN 24 ELSE 72 END,
                   v_user, h.BILLINGADDRESS, h.PERMCONTENT, h.SEASON
              FROM DUAL
             WHERE NOT EXISTS
                   (
                       SELECT 1 FROM T_PERMMASTER_SC m
                        WHERE TRIM(m.PERMNBR_ID) = TRIM(h.GENERATED_PERMNBR_ID)
                   );
        END LOOP;

        INSERT INTO T_PERMDETAIL_SC
        (
            PERM_ID, PURPOSE_ID, CRAFT_ID, FLIGHTNBR, REGISTRATION,
            DAY1, DAY2, DAY3, DAY4, DAY5, DAY6, DAY7,
            FROM_AIRP, TO_AIRP, ETD, ETA, VIA, STATUS, LASTUSER,
            BEGINDATE, ENDDATE, REMARK
        )
        SELECT m.PERM_ID, i.PURPOSE,
               (SELECT MIN(c.CRAFT_ID) FROM M_CRAFT_TYPE c
                 WHERE UPPER(TRIM(c.MA)) = UPPER(TRIM(i.CRAFT))),
               i.CALLSIGN, i.REGISTRATION,
               CASE WHEN INSTR(i.DAILY, '1') > 0 THEN '1' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '2') > 0 THEN '2' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '3') > 0 THEN '3' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '4') > 0 THEN '4' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '5') > 0 THEN '5' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '6') > 0 THEN '6' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '7') > 0 THEN '7' ELSE '0' END,
               i.FROM_AIRP, i.TO_AIRP, TRIM(i.ETD), TRIM(i.ETA), i.VIA,
               '2', v_user, i.FROMDATE, i.TODATE,
               'PHEP HUY ' || i.REMARK || ' '
          FROM T_PERMSC_CANCEL_V2 i
          JOIN T_PERMMASTER_SC m
            ON TRIM(m.PERMNBR_ID) = TRIM(FNCREATEPERMSC_NBR_ID(
                i.AUTHOR, i.PERMTYPE, i.PERMNBR, i.SEASON,
                NVL(i.PERMDATE, TRUNC(SYSDATE))))
         WHERE i.CREATED_BY = v_user
           AND i.PROCESS_STATUS = 'PROCESSING'
           AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                     ',' || TO_CHAR(i.ID) || ',') > 0
           AND NOT EXISTS
               (
                   SELECT 1 FROM T_PERMDETAIL_SC d
                    WHERE d.PERM_ID = m.PERM_ID
                      AND TRIM(d.FLIGHTNBR) = TRIM(i.CALLSIGN)
                      AND TRIM(d.FROM_AIRP) = TRIM(i.FROM_AIRP)
                      AND TRIM(d.TO_AIRP) = TRIM(i.TO_AIRP)
                      AND TRIM(d.ETD) = TRIM(i.ETD)
                      AND NVL(TRIM(d.ETA), '#') = NVL(TRIM(i.ETA), '#')
                      AND d.BEGINDATE = i.FROMDATE
                      AND d.ENDDATE = i.TODATE
                      AND d.STATUS = '2'
               );

        MERGE INTO T_SCHEDULE_DAYFLIGHTS s
        USING
        (
            SELECT schedule_id, MIN(permnbr) AS permnbr
              FROM
              (
                  SELECT s2.ID AS schedule_id, i.PERMNBR AS permnbr
                    FROM T_PERMSC_CANCEL_V2 i
                    JOIN T_SCHEDULE_DAYFLIGHTS s2
                      ON s2.FLIGHTNBR = i.CALLSIGN
                     AND s2.FROM_AIRP = i.FROM_AIRP
                     AND s2.TO_AIRP = i.TO_AIRP
                     AND s2.ETD = TRIM(i.ETD)
                     AND (TRIM(s2.ETA) IS NULL OR TRIM(i.ETA) IS NULL OR TRIM(s2.ETA) = TRIM(i.ETA))
                     AND s2.FLIGHTDATE >= i.FROMDATE
                     AND s2.FLIGHTDATE < i.TODATE + 1
                     AND s2.ISRENDER IN (0, 1)
                     AND INSTR(i.DAILY, TO_CHAR(TRUNC(s2.FLIGHTDATE) - TRUNC(s2.FLIGHTDATE, 'IW') + 1)) > 0
                   WHERE i.CREATED_BY = v_user
                     AND i.PROCESS_STATUS = 'PROCESSING'
                     AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                               ',' || TO_CHAR(i.ID) || ',') > 0
              )
             GROUP BY schedule_id
        ) x ON (s.ID = x.schedule_id)
        WHEN MATCHED THEN UPDATE SET
            s.STATUS = '1', s.ISRENDER = 1,
            s.REMARK = 'HUY CHUYEN THEO PHEP ' || x.permnbr;
        v_updated_count := SQL%ROWCOUNT;

        UPDATE T_PERMSC_CANCEL_V2 i
           SET PROCESS_STATUS = 'DONE', PROCESSED_AT = SYSDATE,
               ERROR_MESSAGE = NULL
         WHERE i.CREATED_BY = v_user
           AND i.PROCESS_STATUS = 'PROCESSING'
           AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                     ',' || TO_CHAR(i.ID) || ',') > 0;

        COMMIT;
        P_OUT := 1;
        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'PERM_IMP_V2_APPLY_OK', v_selected_count,
                'User=' || v_user || '; schedule rows=' || v_updated_count
            );
        EXCEPTION WHEN OTHERS THEN NULL;
        END;
    EXCEPTION
        WHEN OTHERS THEN
            v_error := SUBSTR(SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE, 1, 2000);
            BEGIN ROLLBACK TO APPLY_CANCEL_V2; EXCEPTION WHEN OTHERS THEN ROLLBACK; END;
            BEGIN
                UPDATE T_PERMSC_CANCEL_V2 i
                   SET PROCESS_STATUS = 'ERROR', ERROR_MESSAGE = SUBSTR(v_error, 1, 1000)
                 WHERE i.CREATED_BY = v_user
                   AND i.PROCESS_STATUS IN ('PENDING', 'PROCESSING')
                   AND INSTR(',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                             ',' || TO_CHAR(i.ID) || ',') > 0;
                COMMIT;
            EXCEPTION WHEN OTHERS THEN ROLLBACK;
            END;
            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG('PERM_IMP_V2_APPLY_ERROR', SQLCODE, SUBSTR(v_error, 1, 200));
            EXCEPTION WHEN OTHERS THEN NULL;
            END;
            P_OUT := -1;
    END APPLY_CANCELLATIONS;
END PERM_IMP_V2_PKG;
/

SHOW ERRORS PACKAGE PERM_IMP_V2_PKG;
SHOW ERRORS PACKAGE BODY PERM_IMP_V2_PKG;

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERM_IMP_V2_PKG'
 ORDER BY OBJECT_TYPE;
