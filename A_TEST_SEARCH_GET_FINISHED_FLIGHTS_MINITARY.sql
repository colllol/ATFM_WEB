/*
  Sửa lỗi biên dịch khi bổ sung GET_FINISHED_FLIGHTS_MINITARY vào
  package A_TEST_SEARCH.

  Lưu ý:
  - Giữ nguyên tên MINITARY để không làm thay đổi hợp đồng API hiện tại.
  - p_PURPOSE chỉ được khai báo một lần.
  - Package specification chỉ chứa declaration.
  - Package body chỉ chứa một implementation, không đặt thêm một forward
    declaration trùng ngay trước implementation.
*/

/* ================================================================
   1. Declaration đặt trong PACKAGE A_TEST_SEARCH AS
   ================================================================ */
PROCEDURE GET_FINISHED_FLIGHTS_MINITARY
(
    p_CALLSIGN   IN T_FINISHFLIGHTS_MILITARY.CALLSIGN%TYPE   DEFAULT NULL,
    p_P_TYPE     IN T_FINISHFLIGHTS_MILITARY.P_TYPE%TYPE     DEFAULT NULL,
    p_FROM_AIRP  IN T_FINISHFLIGHTS_MILITARY.FROM_AIRP%TYPE  DEFAULT NULL,
    p_TO_AIRP    IN T_FINISHFLIGHTS_MILITARY.TO_AIRP%TYPE    DEFAULT NULL,
    p_ETD        IN T_FINISHFLIGHTS_MILITARY.ETD%TYPE        DEFAULT NULL,
    p_ETA        IN T_FINISHFLIGHTS_MILITARY.ETA%TYPE        DEFAULT NULL,
    p_ATD        IN T_FINISHFLIGHTS_MILITARY.ATD%TYPE        DEFAULT NULL,
    p_ATA        IN T_FINISHFLIGHTS_MILITARY.ATA%TYPE        DEFAULT NULL,
    p_VIA        IN T_FINISHFLIGHTS_MILITARY.VIA%TYPE        DEFAULT NULL,
    p_REMARK     IN T_FINISHFLIGHTS_MILITARY.REMARK%TYPE     DEFAULT NULL,
    p_OPER       IN T_FINISHFLIGHTS_MILITARY.OPER%TYPE       DEFAULT NULL,
    p_REGIS      IN T_FINISHFLIGHTS_MILITARY.REGIS%TYPE      DEFAULT NULL,
    p_PURPOSE    IN T_FINISHFLIGHTS_MILITARY.PURPOSE%TYPE    DEFAULT NULL,
    p_RCRAFT     IN T_FINISHFLIGHTS_MILITARY.RCRAFT%TYPE     DEFAULT NULL,
    p_FCRAFT     IN T_FINISHFLIGHTS_MILITARY.FCRAFT%TYPE     DEFAULT NULL,
    p_FPLVIA     IN T_FINISHFLIGHTS_MILITARY.FPLVIA%TYPE     DEFAULT NULL,
    p_CAT_HA     IN NUMBER,
    p_KHUNGGIO1  IN VARCHAR2,
    p_KHUNGGIO2  IN VARCHAR2,
    p_PageSize   IN INT,
    p_PageIndex  IN INT,
    p_StartDate  IN VARCHAR2,
    p_FinishDate IN VARCHAR2,
    P_OUT_CURSOR OUT T_CURSOR
);

/* ================================================================
   2. Implementation đặt trong PACKAGE BODY A_TEST_SEARCH
   ================================================================ */
PROCEDURE GET_FINISHED_FLIGHTS_MINITARY
(
    p_CALLSIGN   IN T_FINISHFLIGHTS_MILITARY.CALLSIGN%TYPE   DEFAULT NULL,
    p_P_TYPE     IN T_FINISHFLIGHTS_MILITARY.P_TYPE%TYPE     DEFAULT NULL,
    p_FROM_AIRP  IN T_FINISHFLIGHTS_MILITARY.FROM_AIRP%TYPE  DEFAULT NULL,
    p_TO_AIRP    IN T_FINISHFLIGHTS_MILITARY.TO_AIRP%TYPE    DEFAULT NULL,
    p_ETD        IN T_FINISHFLIGHTS_MILITARY.ETD%TYPE        DEFAULT NULL,
    p_ETA        IN T_FINISHFLIGHTS_MILITARY.ETA%TYPE        DEFAULT NULL,
    p_ATD        IN T_FINISHFLIGHTS_MILITARY.ATD%TYPE        DEFAULT NULL,
    p_ATA        IN T_FINISHFLIGHTS_MILITARY.ATA%TYPE        DEFAULT NULL,
    p_VIA        IN T_FINISHFLIGHTS_MILITARY.VIA%TYPE        DEFAULT NULL,
    p_REMARK     IN T_FINISHFLIGHTS_MILITARY.REMARK%TYPE     DEFAULT NULL,
    p_OPER       IN T_FINISHFLIGHTS_MILITARY.OPER%TYPE       DEFAULT NULL,
    p_REGIS      IN T_FINISHFLIGHTS_MILITARY.REGIS%TYPE      DEFAULT NULL,
    p_PURPOSE    IN T_FINISHFLIGHTS_MILITARY.PURPOSE%TYPE    DEFAULT NULL,
    p_RCRAFT     IN T_FINISHFLIGHTS_MILITARY.RCRAFT%TYPE     DEFAULT NULL,
    p_FCRAFT     IN T_FINISHFLIGHTS_MILITARY.FCRAFT%TYPE     DEFAULT NULL,
    p_FPLVIA     IN T_FINISHFLIGHTS_MILITARY.FPLVIA%TYPE     DEFAULT NULL,
    p_CAT_HA     IN NUMBER,
    p_KHUNGGIO1  IN VARCHAR2,
    p_KHUNGGIO2  IN VARCHAR2,
    p_PageSize   IN INT,
    p_PageIndex  IN INT,
    p_StartDate  IN VARCHAR2,
    p_FinishDate IN VARCHAR2,
    P_OUT_CURSOR OUT T_CURSOR
)
IS
    v_first_index PLS_INTEGER;
    v_last_index  PLS_INTEGER;
    v_page_size   PLS_INTEGER;
    v_page_index  PLS_INTEGER;
    v_start_date  DATE;
    v_finish_date DATE;
BEGIN
    v_page_size := GREATEST(NVL(p_PageSize, 100), 1);
    v_page_index := GREATEST(NVL(p_PageIndex, 0), 0);
    v_first_index := (v_page_size * v_page_index) + 1;
    v_last_index := v_page_size * (v_page_index + 1);

    v_start_date := TO_DATE(TRIM(p_StartDate), 'FXDD-MM-YYYY');
    v_finish_date := TO_DATE(TRIM(p_FinishDate), 'FXDD-MM-YYYY');

    IF v_finish_date < v_start_date THEN
        RAISE_APPLICATION_ERROR(
            -20001,
            'P_FINISHDATE must be greater than or equal to P_STARTDATE'
        );
    END IF;

    OPEN P_OUT_CURSOR FOR
        SELECT page_data.*
        FROM
        (
            SELECT
                COUNT(*) OVER () AS SUMRECORD,
                t.*,
                ROW_NUMBER() OVER
                (
                    ORDER BY
                        t.CALLSIGN,
                        t.FROM_AIRP,
                        t.TO_AIRP,
                        t.FLIGHT_ID
                ) AS RNUM
            FROM T_FINISHFLIGHTS_MILITARY t
            WHERE t.FLIGHTDATE >= v_start_date
              AND t.FLIGHTDATE <  v_finish_date + 1
              AND
              (
                  p_CALLSIGN IS NULL
                  OR UPPER(t.CALLSIGN) LIKE
                     '%' || UPPER(TRIM(p_CALLSIGN)) || '%'
              )
              AND
              (
                  p_P_TYPE IS NULL
                  OR UPPER(t.P_TYPE) = UPPER(TRIM(p_P_TYPE))
              )
              AND
              (
                  p_FROM_AIRP IS NULL
                  OR UPPER(t.FROM_AIRP) LIKE
                     '%' || UPPER(TRIM(p_FROM_AIRP)) || '%'
              )
              AND
              (
                  p_TO_AIRP IS NULL
                  OR UPPER(t.TO_AIRP) LIKE
                     '%' || UPPER(TRIM(p_TO_AIRP)) || '%'
              )
              AND
              (
                  p_ETD IS NULL
                  OR UPPER(t.ETD) LIKE
                     '%' || UPPER(TRIM(p_ETD)) || '%'
              )
              AND
              (
                  p_ETA IS NULL
                  OR UPPER(t.ETA) LIKE
                     '%' || UPPER(TRIM(p_ETA)) || '%'
              )
              AND
              (
                  p_ATD IS NULL
                  OR UPPER(t.ATD) LIKE
                     '%' || UPPER(TRIM(p_ATD)) || '%'
              )
              AND
              (
                  p_ATA IS NULL
                  OR UPPER(t.ATA) LIKE
                     '%' || UPPER(TRIM(p_ATA)) || '%'
              )
              AND
              (
                  p_VIA IS NULL
                  OR UPPER(t.VIA) LIKE
                     '%' || UPPER(TRIM(p_VIA)) || '%'
              )
              AND
              (
                  p_REMARK IS NULL
                  OR UPPER(t.REMARK) LIKE
                     '%' || UPPER(TRIM(p_REMARK)) || '%'
              )
              AND
              (
                  p_OPER IS NULL
                  OR UPPER(t.OPER) LIKE
                     '%' || UPPER(TRIM(p_OPER)) || '%'
              )
              AND
              (
                  p_REGIS IS NULL
                  OR UPPER(t.REGIS) LIKE
                     '%' || UPPER(TRIM(p_REGIS)) || '%'
              )
              AND
              (
                  p_PURPOSE IS NULL
                  OR UPPER(t.PURPOSE) = UPPER(TRIM(p_PURPOSE))
              )
              AND
              (
                  p_RCRAFT IS NULL
                  OR UPPER(t.RCRAFT) = UPPER(TRIM(p_RCRAFT))
              )
              AND
              (
                  p_FCRAFT IS NULL
                  OR UPPER(t.FCRAFT) = UPPER(TRIM(p_FCRAFT))
              )
              AND
              (
                  p_FPLVIA IS NULL
                  OR UPPER(t.FPLVIA) LIKE
                     '%' || UPPER(TRIM(p_FPLVIA)) || '%'
              )
              AND
              (
                  NVL(p_CAT_HA, 0) = 0
                  OR
                  (
                      p_CAT_HA = 1
                      AND SUBSTR(TRIM(t.ETD), 1, 4)
                          BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2
                  )
                  OR
                  (
                      p_CAT_HA = 2
                      AND SUBSTR(TRIM(t.ETA), 1, 4)
                          BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2
                  )
                  OR
                  (
                      p_CAT_HA = 3
                      AND SUBSTR(TRIM(t.ATD), 1, 4)
                          BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2
                  )
                  OR
                  (
                      p_CAT_HA = 4
                      AND SUBSTR(TRIM(t.ATA), 1, 4)
                          BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2
                  )
              )
        ) page_data
        WHERE page_data.RNUM BETWEEN v_first_index AND v_last_index
        ORDER BY page_data.RNUM;

EXCEPTION
    WHEN OTHERS THEN
        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'GET_FINISHED_FLIGHTS_MINITARY',
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
        END;

        RAISE;
END GET_FINISHED_FLIGHTS_MINITARY;
