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
    p_ISACCEPTED IN NUMBER                                      DEFAULT 0,
    p_CAT_HA     IN NUMBER,
    p_KHUNGGIO1  IN VARCHAR2,
    p_KHUNGGIO2  IN VARCHAR2,
    p_PageSize   IN INT,
    p_PageIndex  IN INT,
    p_StartDate  IN VARCHAR2,
    p_FinishDate IN VARCHAR2,
    P_OUT_CURSOR OUT T_CURSOR
);

PROCEDURE ACCEPT_FIN_FLIGHTS_MILITARY
(
    p_StartDate  IN VARCHAR2,
    p_FinishDate IN VARCHAR2,
    p_User       IN VARCHAR2 DEFAULT NULL,
    p_ReturnCode OUT NUMBER
);

PROCEDURE EXPORT_QS_PLAN_MESSAGE
(
    p_StartDate  IN VARCHAR2,
    p_FinishDate IN VARCHAR2,
    p_User       IN VARCHAR2 DEFAULT NULL,
    p_ReturnCode OUT NUMBER
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
    p_ISACCEPTED IN NUMBER                                      DEFAULT 0,
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
                  NVL(p_ISACCEPTED, 0) = -1
                  OR t.ISACCEPTED = NVL(p_ISACCEPTED, 0)
              )
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

/* ================================================================
   3. Accepted các chuyến quân sự theo khoảng ngày
   ================================================================ */
PROCEDURE ACCEPT_FIN_FLIGHTS_MILITARY
(
    p_StartDate  IN VARCHAR2,
    p_FinishDate IN VARCHAR2,
    p_User       IN VARCHAR2 DEFAULT NULL,
    p_ReturnCode OUT NUMBER
)
IS
    v_start_date  DATE;
    v_finish_date DATE;
BEGIN
    v_start_date := TO_DATE(TRIM(p_StartDate), 'FXDD-MM-YYYY');
    v_finish_date := TO_DATE(TRIM(p_FinishDate), 'FXDD-MM-YYYY');

    IF v_finish_date < v_start_date THEN
        RAISE_APPLICATION_ERROR(
            -20001,
            'P_FINISHDATE must be greater than or equal to P_STARTDATE'
        );
    END IF;

    UPDATE T_FINISHFLIGHTS_MILITARY
       SET ISACCEPTED = 1
     WHERE FLIGHTDATE >= v_start_date
       AND FLIGHTDATE <  v_finish_date + 1
       AND ISACCEPTED = 0;

    p_ReturnCode := SQL%ROWCOUNT;

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
        NVL(p_User, ' '),
        ' ',
        SYSDATE,
        'ACCEPT FINISHFLIGHTS_MILITARY',
        0,
        p_StartDate || ' - ' || p_FinishDate
            || '; ROWS=' || TO_CHAR(p_ReturnCode),
        843
    );

    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;

        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'ACCEPT_FIN_FLIGHTS_MILITARY',
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

        p_ReturnCode := -1;
END ACCEPT_FIN_FLIGHTS_MILITARY;

/* ================================================================
   4. Xuất các chuyến đã Accepted thành QS MESSAGE
   ================================================================ */
PROCEDURE EXPORT_QS_PLAN_MESSAGE
(
    p_StartDate  IN VARCHAR2,
    p_FinishDate IN VARCHAR2,
    p_User       IN VARCHAR2 DEFAULT NULL,
    p_ReturnCode OUT NUMBER
)
IS
    TYPE t_text_parts IS TABLE OF VARCHAR2(32767)
        INDEX BY PLS_INTEGER;

    v_parts         t_text_parts;
    v_part_ids      t_text_parts;
    v_start_date    DATE;
    v_finish_date   DATE;
    v_line          VARCHAR2(32767);
    v_content       VARCHAR2(32767);
    v_part_count    PLS_INTEGER;
    v_row_number    PLS_INTEGER;
    v_total_flights PLS_INTEGER := 0;
    v_total_parts   PLS_INTEGER := 0;

    FUNCTION clean_text
    (
        p_value      IN VARCHAR2,
        p_max_length IN PLS_INTEGER
    )
    RETURN VARCHAR2
    IS
    BEGIN
        RETURN SUBSTR(
            REPLACE(
                REPLACE(
                    NVL(TRIM(p_value), '-'),
                    CHR(13),
                    ' '
                ),
                CHR(10),
                ' '
            ),
            1,
            p_max_length
        );
    END clean_text;
BEGIN
    v_start_date := TO_DATE(TRIM(p_StartDate), 'FXDD-MM-YYYY');
    v_finish_date := TO_DATE(TRIM(p_FinishDate), 'FXDD-MM-YYYY');

    IF v_finish_date < v_start_date THEN
        RAISE_APPLICATION_ERROR(
            -20001,
            'P_FINISHDATE must be greater than or equal to P_STARTDATE'
        );
    END IF;

    /*
      Chỉ thay các QS MESSAGE chưa gửi. Các loại điện văn khác và các
      QS MESSAGE đã gửi (STATUS <> 0) được giữ nguyên.
    */
    DELETE FROM T_PLAN_MESSAGE
     WHERE STATUS = 0
       AND MESS_TYPE = 'QS MESSAGE'
       AND FLIGHTDATE >= v_start_date
       AND FLIGHTDATE <  v_finish_date + 1;

    FOR d IN
    (
        SELECT TRUNC(FLIGHTDATE) AS FLIGHT_DATE
          FROM T_FINISHFLIGHTS_MILITARY
         WHERE ISACCEPTED = 1
           AND FLIGHTDATE >= v_start_date
           AND FLIGHTDATE <  v_finish_date + 1
         GROUP BY TRUNC(FLIGHTDATE)
         ORDER BY TRUNC(FLIGHTDATE)
    )
    LOOP
        v_parts.DELETE;
        v_part_ids.DELETE;
        v_part_count := 1;
        v_row_number := 0;
        v_parts(1) := NULL;
        v_part_ids(1) := NULL;

        FOR f IN
        (
            SELECT
                FLIGHT_ID,
                CALLSIGN,
                REGIS,
                PURPOSE,
                FROM_AIRP,
                TO_AIRP,
                ETD,
                ETA,
                VIA,
                FPLVIA,
                REMARK
              FROM T_FINISHFLIGHTS_MILITARY
             WHERE ISACCEPTED = 1
               AND FLIGHTDATE >= d.FLIGHT_DATE
               AND FLIGHTDATE <  d.FLIGHT_DATE + 1
             ORDER BY
                CALLSIGN,
                FROM_AIRP,
                TO_AIRP,
                ETD,
                FLIGHT_ID
        )
        LOOP
            v_row_number := v_row_number + 1;

            v_line :=
                   TO_CHAR(v_row_number)
                || ' '
                || clean_text(f.REGIS, 10)
                || ' '
                || clean_text(f.CALLSIGN, 12)
                || ' '
                || clean_text(f.FROM_AIRP, 4)
                || ' '
                || clean_text(f.TO_AIRP, 4)
                || ' '
                || clean_text(f.ETD, 6)
                || ' '
                || clean_text(f.ETA, 6)
                || ' '
                || clean_text(f.PURPOSE, 10)
                || ' '
                || clean_text(f.VIA, 120)
                || ' '
                || clean_text(f.FPLVIA, 120)
                || ' '
                || clean_text(f.REMARK, 900);

            v_line := RTRIM(SUBSTR(v_line, 1, 1400)) || CHR(10);

            /*
              Chừa khoảng cho header/footer để CONTENT luôn dưới 2000 byte.
            */
            IF v_parts(v_part_count) IS NOT NULL
               AND LENGTHB(v_parts(v_part_count))
                   + LENGTHB(v_line) > 1650
            THEN
                v_part_count := v_part_count + 1;
                v_parts(v_part_count) := NULL;
                v_part_ids(v_part_count) := NULL;
            END IF;

            v_parts(v_part_count) :=
                v_parts(v_part_count) || v_line;

            v_part_ids(v_part_count) :=
                CASE
                    WHEN v_part_ids(v_part_count) IS NULL
                    THEN TO_CHAR(f.FLIGHT_ID)
                    ELSE v_part_ids(v_part_count)
                         || ',' || TO_CHAR(f.FLIGHT_ID)
                END;
        END LOOP;

        IF v_row_number > 0 THEN
            FOR i IN 1 .. v_part_count
            LOOP
                v_content :=
                       'PART ' || TO_CHAR(i)
                    || ' OF ' || TO_CHAR(v_part_count)
                    || CHR(10)
                    || 'FPL ON:'
                    || TO_CHAR(
                           d.FLIGHT_DATE,
                           'DD-MON-YYYY',
                           'NLS_DATE_LANGUAGE=ENGLISH'
                       )
                    || ': QS MESSAGE'
                    || CHR(10)
                    || v_parts(i)
                    /*
                      v_parts đã kết thúc bằng CHR(10). Bổ sung thêm 7
                      ký tự xuống dòng để NNNN cách dòng dữ liệu cuối
                      cùng đúng 7 dòng trống.
                    */
                    || CHR(10)
                    || CHR(10)
                    || CHR(10)
                    || CHR(10)
                    || CHR(10)
                    || CHR(10)
                    || CHR(10)
                    || 'NNNN';

                IF LENGTHB(v_content) > 2000 THEN
                    RAISE_APPLICATION_ERROR(
                        -20002,
                        'QS MESSAGE content exceeds 2000 bytes'
                    );
                END IF;

                INSERT INTO T_PLAN_MESSAGE
                (
                    FLIGHTDATE,
                    PART_NO,
                    CONTENT,
                    MESS_TYPE,
                    STATUS,
                    LISTFLIGHTID
                )
                VALUES
                (
                    d.FLIGHT_DATE,
                    i,
                    v_content,
                    'QS MESSAGE',
                    0,
                    v_part_ids(i)
                );

                v_total_parts := v_total_parts + 1;
            END LOOP;

            v_total_flights := v_total_flights + v_row_number;
        END IF;
    END LOOP;

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
        NVL(p_User, ' '),
        ' ',
        SYSDATE,
        'EXPORT QS MESSAGE',
        0,
        p_StartDate || ' - ' || p_FinishDate
            || '; FLIGHTS=' || TO_CHAR(v_total_flights)
            || '; PARTS=' || TO_CHAR(v_total_parts),
        843
    );

    p_ReturnCode := v_total_flights;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;

        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'EXPORT_QS_PLAN_MESSAGE',
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

        p_ReturnCode := -1;
END EXPORT_QS_PLAN_MESSAGE;
