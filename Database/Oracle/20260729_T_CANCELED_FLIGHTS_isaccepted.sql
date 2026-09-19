-- Phân tách danh sách chuyến bay hủy chưa Accepted/đã Accepted.
-- ISACCEPTED = 0: dữ liệu mới, chờ Accepted.
-- ISACCEPTED = 1: dữ liệu đã Accepted.

DECLARE
    v_column_exists NUMBER;
    v_column_created BOOLEAN := FALSE;
BEGIN
    SELECT COUNT(*)
      INTO v_column_exists
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_CANCELED_FLIGHTS'
       AND COLUMN_NAME = 'ISACCEPTED';

    IF v_column_exists = 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_CANCELED_FLIGHTS '
            || 'ADD (ISACCEPTED NUMBER(1) DEFAULT 0 NOT NULL)';
        v_column_created := TRUE;
    END IF;

    /*
      Chỉ chuyển dữ liệu lịch sử sang trạng thái đã Accepted trong lần đầu
      tạo cột. Các lần chạy lại không được thay đổi dữ liệu mới trạng thái 0.
    */
    IF v_column_created THEN
        EXECUTE IMMEDIATE
            'UPDATE T_CANCELED_FLIGHTS SET ISACCEPTED = 1';
        COMMIT;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

DECLARE
    v_constraint_exists NUMBER;
BEGIN
    SELECT COUNT(*)
      INTO v_constraint_exists
      FROM USER_CONSTRAINTS
     WHERE TABLE_NAME = 'T_CANCELED_FLIGHTS'
       AND CONSTRAINT_NAME = 'CK_TCAN_ISACCEPTED';

    IF v_constraint_exists = 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_CANCELED_FLIGHTS '
            || 'ADD CONSTRAINT CK_TCAN_ISACCEPTED '
            || 'CHECK (ISACCEPTED IN (0, 1))';
    END IF;
END;
/

DECLARE
    v_index_exists NUMBER;
BEGIN
    SELECT COUNT(*)
      INTO v_index_exists
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TCAN_STATUS_DATE';

    IF v_index_exists = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_TCAN_STATUS_DATE '
            || 'ON T_CANCELED_FLIGHTS '
            || '(ISACCEPTED, STT, FLIGHTDATE)';
    END IF;
END;
/

CREATE OR REPLACE PACKAGE CANCELED_STATUS_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_CANCELED_FLIGHTS
    (
        P_PERMNBR         IN VARCHAR2 DEFAULT NULL,
        P_OPER_ID         IN VARCHAR2 DEFAULT NULL,
        P_PERMTYPE        IN VARCHAR2 DEFAULT NULL,
        P_FLIGHT_TYPE     IN VARCHAR2 DEFAULT NULL,
        P_PURPOSE         IN VARCHAR2 DEFAULT NULL,
        P_CRAFT_ID        IN NUMBER DEFAULT NULL,
        P_CRAFT_TYPE      IN VARCHAR2 DEFAULT NULL,
        P_VALIDHOURS      IN NUMBER DEFAULT NULL,
        P_FLIGHTNBR       IN VARCHAR2 DEFAULT NULL,
        P_REGISTRATION    IN VARCHAR2 DEFAULT NULL,
        P_FROM_AIRP       IN VARCHAR2 DEFAULT NULL,
        P_TO_AIRP         IN VARCHAR2 DEFAULT NULL,
        P_ETD             IN VARCHAR2 DEFAULT NULL,
        P_ETA             IN VARCHAR2 DEFAULT NULL,
        P_ATD             IN VARCHAR2 DEFAULT NULL,
        P_ATA             IN VARCHAR2 DEFAULT NULL,
        P_VIA             IN VARCHAR2 DEFAULT NULL,
        P_FPL_VIA         IN VARCHAR2 DEFAULT NULL,
        P_REMARK          IN VARCHAR2 DEFAULT NULL,
        P_KHUNGGIO1       IN VARCHAR2 DEFAULT '0000',
        P_KHUNGGIO2       IN VARCHAR2 DEFAULT '2359',
        P_PAGESIZE        IN NUMBER DEFAULT 100,
        P_PAGEINDEX       IN NUMBER DEFAULT 0,
        P_STARTDATE       IN VARCHAR2,
        P_FINISHDATE      IN VARCHAR2,
        P_WHECONDITION    IN VARCHAR2 DEFAULT NULL,
        P_CAT_HA          IN NUMBER DEFAULT 0,
        P_ISACCEPTED      IN NUMBER DEFAULT 0,
        P_OUT_CURSOR      OUT T_CURSOR
    );

    PROCEDURE ACCEPT_CANCELED_FLIGHTS
    (
        P_FLIGHT_IDS IN VARCHAR2,
        P_USER       IN VARCHAR2 DEFAULT NULL,
        P_RETURNCODE OUT NUMBER
    );
END CANCELED_STATUS_PKG;
/

CREATE OR REPLACE PACKAGE BODY CANCELED_STATUS_PKG AS
    FUNCTION PARSE_DATE(P_VALUE IN VARCHAR2)
        RETURN DATE
    IS
    BEGIN
        IF REGEXP_LIKE(TRIM(P_VALUE), '^[0-9]{2}-[0-9]{2}-[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(P_VALUE), 'FXDD-MM-YYYY');
        ELSIF REGEXP_LIKE(TRIM(P_VALUE), '^[0-9]{2}/[0-9]{2}/[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(P_VALUE), 'FXDD/MM/YYYY');
        END IF;

        RAISE_APPLICATION_ERROR(-20101, 'Date must use DD-MM-YYYY');
    END PARSE_DATE;

    PROCEDURE WRITE_ERROR(P_ACTION IN VARCHAR2)
    IS
    BEGIN
        PROCESS_PKG.ADD_ERROR_LOG(
            P_ACTION,
            SQLCODE,
            SUBSTR(
                SQLERRM
                || CHR(10)
                || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                1,
                200
            )
        );
    EXCEPTION
        WHEN OTHERS THEN
            NULL;
    END WRITE_ERROR;

    PROCEDURE GET_CANCELED_FLIGHTS
    (
        P_PERMNBR         IN VARCHAR2 DEFAULT NULL,
        P_OPER_ID         IN VARCHAR2 DEFAULT NULL,
        P_PERMTYPE        IN VARCHAR2 DEFAULT NULL,
        P_FLIGHT_TYPE     IN VARCHAR2 DEFAULT NULL,
        P_PURPOSE         IN VARCHAR2 DEFAULT NULL,
        P_CRAFT_ID        IN NUMBER DEFAULT NULL,
        P_CRAFT_TYPE      IN VARCHAR2 DEFAULT NULL,
        P_VALIDHOURS      IN NUMBER DEFAULT NULL,
        P_FLIGHTNBR       IN VARCHAR2 DEFAULT NULL,
        P_REGISTRATION    IN VARCHAR2 DEFAULT NULL,
        P_FROM_AIRP       IN VARCHAR2 DEFAULT NULL,
        P_TO_AIRP         IN VARCHAR2 DEFAULT NULL,
        P_ETD             IN VARCHAR2 DEFAULT NULL,
        P_ETA             IN VARCHAR2 DEFAULT NULL,
        P_ATD             IN VARCHAR2 DEFAULT NULL,
        P_ATA             IN VARCHAR2 DEFAULT NULL,
        P_VIA             IN VARCHAR2 DEFAULT NULL,
        P_FPL_VIA         IN VARCHAR2 DEFAULT NULL,
        P_REMARK          IN VARCHAR2 DEFAULT NULL,
        P_KHUNGGIO1       IN VARCHAR2 DEFAULT '0000',
        P_KHUNGGIO2       IN VARCHAR2 DEFAULT '2359',
        P_PAGESIZE        IN NUMBER DEFAULT 100,
        P_PAGEINDEX       IN NUMBER DEFAULT 0,
        P_STARTDATE       IN VARCHAR2,
        P_FINISHDATE      IN VARCHAR2,
        P_WHECONDITION    IN VARCHAR2 DEFAULT NULL,
        P_CAT_HA          IN NUMBER DEFAULT 0,
        P_ISACCEPTED      IN NUMBER DEFAULT 0,
        P_OUT_CURSOR      OUT T_CURSOR
    )
    IS
        v_start_date  DATE;
        v_finish_date DATE;
        v_filter_date DATE;
        v_page_size   PLS_INTEGER;
        v_page_index  PLS_INTEGER;
        v_first_row   PLS_INTEGER;
        v_last_row    PLS_INTEGER;
    BEGIN
        v_start_date := PARSE_DATE(P_STARTDATE);
        v_finish_date := PARSE_DATE(P_FINISHDATE);

        IF v_finish_date < v_start_date THEN
            RAISE_APPLICATION_ERROR(
                -20102,
                'P_FINISHDATE must be greater than or equal to P_STARTDATE'
            );
        END IF;

        IF TRIM(P_WHECONDITION) IS NOT NULL THEN
            v_filter_date := PARSE_DATE(P_WHECONDITION);
        END IF;

        v_page_size := GREATEST(NVL(P_PAGESIZE, 100), 1);
        v_page_index := GREATEST(NVL(P_PAGEINDEX, 0), 0);
        v_first_row := (v_page_size * v_page_index) + 1;
        v_last_row := v_page_size * (v_page_index + 1);

        OPEN P_OUT_CURSOR FOR
            SELECT page_data.*
              FROM
              (
                  SELECT
                      COUNT(*) OVER () AS SUMRECORD,
                      ROW_NUMBER() OVER
                      (
                          ORDER BY t.FLIGHT_ID
                      ) AS RNUM,
                      CAST(NULL AS VARCHAR2(10)) AS REAL_CRAFT_TYPE,
                      t.*
                    FROM T_CANCELED_FLIGHTS t
                   WHERE t.ISACCEPTED = NVL(P_ISACCEPTED, 0)
                     AND t.STT = 1
                     AND t.FLIGHTDATE >= v_start_date
                     AND t.FLIGHTDATE < v_finish_date + 1
                     AND
                     (
                         v_filter_date IS NULL
                         OR
                         (
                             t.FLIGHTDATE >= v_filter_date
                             AND t.FLIGHTDATE < v_filter_date + 1
                         )
                     )
                     AND
                     (
                         TRIM(P_PERMTYPE) IS NULL
                         OR UPPER(t.PERMTYPE) = UPPER(TRIM(P_PERMTYPE))
                     )
                     AND
                     (
                         TRIM(P_PURPOSE) IS NULL
                         OR UPPER(t.PURPOSE) = UPPER(TRIM(P_PURPOSE))
                     )
                     AND
                     (
                         TRIM(P_PERMNBR) IS NULL
                         OR UPPER(t.PERMNBR) LIKE
                            '%' || UPPER(TRIM(P_PERMNBR)) || '%'
                     )
                     AND
                     (
                         TRIM(P_OPER_ID) IS NULL
                         OR UPPER(t.OPER_ID) LIKE
                            '%' || UPPER(TRIM(P_OPER_ID)) || '%'
                     )
                     AND
                     (
                         TRIM(P_FLIGHT_TYPE) IS NULL
                         OR UPPER(t.FLIGHT_TYPE) LIKE
                            '%' || UPPER(TRIM(P_FLIGHT_TYPE)) || '%'
                     )
                     AND
                     (
                         P_CRAFT_ID IS NULL
                         OR P_CRAFT_ID = 0
                         OR t.CRAFT_ID = P_CRAFT_ID
                     )
                     AND
                     (
                         TRIM(P_CRAFT_TYPE) IS NULL
                         OR UPPER(t.CRAFT_TYPE) =
                            UPPER(TRIM(P_CRAFT_TYPE))
                     )
                     AND
                     (
                         P_VALIDHOURS IS NULL
                         OR P_VALIDHOURS = 0
                         OR t.VALIDHOURS = P_VALIDHOURS
                     )
                     AND
                     (
                         TRIM(P_FLIGHTNBR) IS NULL
                         OR UPPER(t.FLIGHTNBR) LIKE
                            '%' || UPPER(TRIM(P_FLIGHTNBR)) || '%'
                     )
                     AND
                     (
                         TRIM(P_REGISTRATION) IS NULL
                         OR UPPER(t.REGISTRATION) LIKE
                            '%' || UPPER(TRIM(P_REGISTRATION)) || '%'
                     )
                     AND
                     (
                         TRIM(P_FROM_AIRP) IS NULL
                         OR UPPER(t.FROM_AIRP) LIKE
                            '%' || UPPER(TRIM(P_FROM_AIRP)) || '%'
                     )
                     AND
                     (
                         TRIM(P_TO_AIRP) IS NULL
                         OR UPPER(t.TO_AIRP) LIKE
                            '%' || UPPER(TRIM(P_TO_AIRP)) || '%'
                     )
                     AND
                     (
                         TRIM(P_ETD) IS NULL
                         OR UPPER(t.ETD) LIKE
                            '%' || UPPER(TRIM(P_ETD)) || '%'
                     )
                     AND
                     (
                         TRIM(P_ETA) IS NULL
                         OR UPPER(t.ETA) LIKE
                            '%' || UPPER(TRIM(P_ETA)) || '%'
                     )
                     AND
                     (
                         TRIM(P_ATD) IS NULL
                         OR UPPER(t.ATD) LIKE
                            '%' || UPPER(TRIM(P_ATD)) || '%'
                     )
                     AND
                     (
                         TRIM(P_ATA) IS NULL
                         OR UPPER(t.ATA) LIKE
                            '%' || UPPER(TRIM(P_ATA)) || '%'
                     )
                     AND
                     (
                         TRIM(P_VIA) IS NULL
                         OR UPPER(t.VIA) LIKE
                            '%' || UPPER(TRIM(P_VIA)) || '%'
                     )
                     AND
                     (
                         TRIM(P_FPL_VIA) IS NULL
                         OR UPPER(t.FPL_VIA) LIKE
                            '%' || UPPER(TRIM(P_FPL_VIA)) || '%'
                     )
                     AND
                     (
                         TRIM(P_REMARK) IS NULL
                         OR UPPER(t.REMARK) LIKE
                            '%' || UPPER(TRIM(P_REMARK)) || '%'
                     )
                     AND
                     (
                         NVL(P_CAT_HA, 0) = 0
                         OR
                         (
                             P_CAT_HA = 1
                             AND SUBSTR(TRIM(t.ETD), 1, 4)
                                 BETWEEN P_KHUNGGIO1 AND P_KHUNGGIO2
                         )
                         OR
                         (
                             P_CAT_HA = 2
                             AND SUBSTR(TRIM(t.ETA), 1, 4)
                                 BETWEEN P_KHUNGGIO1 AND P_KHUNGGIO2
                         )
                         OR
                         (
                             P_CAT_HA = 3
                             AND SUBSTR(TRIM(t.ATD), 1, 4)
                                 BETWEEN P_KHUNGGIO1 AND P_KHUNGGIO2
                         )
                         OR
                         (
                             P_CAT_HA = 4
                             AND SUBSTR(TRIM(t.ATA), 1, 4)
                                 BETWEEN P_KHUNGGIO1 AND P_KHUNGGIO2
                         )
                     )
              ) page_data
             WHERE page_data.RNUM BETWEEN v_first_row AND v_last_row
             ORDER BY page_data.RNUM;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('GET_CANCELED_FLIGHTS');
            RAISE;
    END GET_CANCELED_FLIGHTS;

    PROCEDURE ACCEPT_CANCELED_FLIGHTS
    (
        P_FLIGHT_IDS IN VARCHAR2,
        P_USER       IN VARCHAR2 DEFAULT NULL,
        P_RETURNCODE OUT NUMBER
    )
    IS
        v_flight_ids VARCHAR2(4000);
        v_id_count   PLS_INTEGER;
    BEGIN
        v_flight_ids := TRIM(BOTH ',' FROM TRIM(P_FLIGHT_IDS));

        IF v_flight_ids IS NULL
           OR NOT REGEXP_LIKE(v_flight_ids, '^[0-9]+(,[0-9]+)*$') THEN
            RAISE_APPLICATION_ERROR(
                -20103,
                'P_FLIGHT_IDS must be a comma-separated list of numeric IDs'
            );
        END IF;

        v_id_count := REGEXP_COUNT(v_flight_ids, ',') + 1;

        UPDATE T_CANCELED_FLIGHTS t
           SET t.ISACCEPTED = 1
         WHERE t.ISACCEPTED = 0
           AND t.FLIGHT_ID IN
               (
                   SELECT DISTINCT
                          TO_NUMBER(
                              REGEXP_SUBSTR(
                                  v_flight_ids,
                                  '[^,]+',
                                  1,
                                  LEVEL
                              )
                          )
                     FROM DUAL
                   CONNECT BY LEVEL <= v_id_count
               );

        P_RETURNCODE := SQL%ROWCOUNT;

        IF P_RETURNCODE > 0 THEN
            INSERT INTO T_ACTIONHISTORY
            (
                USERID,
                FULLNAME,
                HOSTIP,
                DATEMODIFY,
                ACTIONSCODE,
                NEWS_ID,
                NOTES,
                MENU_ID
            )
            VALUES
            (
                0,
                NVL(TRIM(P_USER), ' '),
                ' ',
                SYSDATE,
                'ACCEPT CANCELED FLIGHTS',
                0,
                'VISIBLE_IDS=' || TO_CHAR(v_id_count)
                    || '; ROWS=' || TO_CHAR(P_RETURNCODE),
                905
            );
        END IF;

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            P_RETURNCODE := -1;
            WRITE_ERROR('ACCEPT_CANCELED_FLIGHTS');
    END ACCEPT_CANCELED_FLIGHTS;
END CANCELED_STATUS_PKG;
/

/*
  ALTER TABLE làm Oracle invalid hóa package body cũ có phụ thuộc trực tiếp
  vào cấu trúc T_CANCELED_FLIGHTS. Biên dịch lại ngay trong lượt triển khai
  để không dồn chi phí/lỗi auto-compile sang request đầu tiên của người dùng.
*/
ALTER PACKAGE CANCEL_FLIGHTS_PKG COMPILE BODY;
ALTER PACKAGE FINISH_FLIGHTS_PKG COMPILE BODY;

BEGIN
    DBMS_STATS.GATHER_TABLE_STATS(
        ownname          => USER,
        tabname          => 'T_CANCELED_FLIGHTS',
        cascade          => TRUE,
        estimate_percent => DBMS_STATS.AUTO_SAMPLE_SIZE,
        method_opt       => 'FOR ALL COLUMNS SIZE AUTO'
    );
END;
/

SELECT COLUMN_NAME, DATA_TYPE, DATA_DEFAULT, NULLABLE
  FROM USER_TAB_COLUMNS
 WHERE TABLE_NAME = 'T_CANCELED_FLIGHTS'
   AND COLUMN_NAME = 'ISACCEPTED';

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'CANCELED_STATUS_PKG'
 ORDER BY OBJECT_TYPE;
