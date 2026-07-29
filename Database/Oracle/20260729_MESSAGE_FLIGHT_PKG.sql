-- Danh sách chuyến bay đã được đưa vào từng điện văn T_PLAN_MESSAGE.
-- LISTFLIGHTID hiện có độ dài tối đa 375 ký tự; mỗi phần điện văn được giới
-- hạn nội dung nên DBMS_LOB.SUBSTR(..., 4000, 1) không làm mất FLIGHT_ID.

CREATE OR REPLACE PACKAGE MESSAGE_FLIGHT_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_FLIGHTS_ON_MESSAGE
    (
        P_DATE         IN VARCHAR2,
        P_MESS_TYPE    IN VARCHAR2,
        P_THONG_BAO_MODE IN NUMBER DEFAULT 0,
        P_PART_NO      IN NUMBER DEFAULT 0,
        P_FROM_AIRP    IN VARCHAR2 DEFAULT NULL,
        P_TO_AIRP      IN VARCHAR2 DEFAULT NULL,
        P_OPER_ID      IN VARCHAR2 DEFAULT NULL,
        P_PAGESIZE     IN NUMBER DEFAULT 100,
        P_PAGEINDEX    IN NUMBER DEFAULT 0,
        P_OUT_CURSOR   OUT T_CURSOR
    );
END MESSAGE_FLIGHT_PKG;
/

CREATE OR REPLACE PACKAGE BODY MESSAGE_FLIGHT_PKG AS
    FUNCTION PARSE_DATE(P_VALUE IN VARCHAR2)
        RETURN DATE
    IS
    BEGIN
        IF REGEXP_LIKE(TRIM(P_VALUE), '^[0-9]{2}-[0-9]{2}-[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(P_VALUE), 'FXDD-MM-YYYY');
        ELSIF REGEXP_LIKE(TRIM(P_VALUE), '^[0-9]{2}/[0-9]{2}/[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(P_VALUE), 'FXDD/MM/YYYY');
        END IF;

        RAISE_APPLICATION_ERROR(
            -20201,
            'P_DATE must use DD-MM-YYYY'
        );
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

    PROCEDURE GET_FLIGHTS_ON_MESSAGE
    (
        P_DATE         IN VARCHAR2,
        P_MESS_TYPE    IN VARCHAR2,
        P_THONG_BAO_MODE IN NUMBER DEFAULT 0,
        P_PART_NO      IN NUMBER DEFAULT 0,
        P_FROM_AIRP    IN VARCHAR2 DEFAULT NULL,
        P_TO_AIRP      IN VARCHAR2 DEFAULT NULL,
        P_OPER_ID      IN VARCHAR2 DEFAULT NULL,
        P_PAGESIZE     IN NUMBER DEFAULT 100,
        P_PAGEINDEX    IN NUMBER DEFAULT 0,
        P_OUT_CURSOR   OUT T_CURSOR
    )
    IS
        v_date         DATE;
        v_mess_type    T_PLAN_MESSAGE.MESS_TYPE%TYPE;
        v_page_size    PLS_INTEGER;
        v_page_index   PLS_INTEGER;
        v_first_row    PLS_INTEGER;
        v_last_row     PLS_INTEGER;
    BEGIN
        v_date := PARSE_DATE(P_DATE);
        v_mess_type := UPPER(TRIM(P_MESS_TYPE));

        IF v_mess_type IS NULL THEN
            RAISE_APPLICATION_ERROR(
                -20202,
                'P_MESS_TYPE is required'
            );
        END IF;

        v_page_size := LEAST(
            GREATEST(NVL(P_PAGESIZE, 100), 1),
            100000
        );
        v_page_index := GREATEST(NVL(P_PAGEINDEX, 0), 0);
        v_first_row := (v_page_size * v_page_index) + 1;
        v_last_row := v_page_size * (v_page_index + 1);

        OPEN P_OUT_CURSOR FOR
            WITH MESSAGE_TOKEN AS
            (
                SELECT
                    PM.ID AS MESSAGE_ID,
                    PM.PART_NO,
                    TRIM(TOKEN.COLUMN_VALUE) AS FLIGHT_ID_TEXT
                FROM T_PLAN_MESSAGE PM,
                     TABLE(
                         SPLIT_STRING(
                             DBMS_LOB.SUBSTR(PM.LISTFLIGHTID, 4000, 1),
                             ','
                         )
                     ) TOKEN
                WHERE TRUNC(PM.FLIGHTDATE) = v_date
                  AND UPPER(PM.MESS_TYPE) = v_mess_type
                  AND
                  (
                      NVL(P_THONG_BAO_MODE, 0) = 2
                      OR
                      (
                          NVL(P_THONG_BAO_MODE, 0) = 1
                          AND INSTR(UPPER(PM.CONTENT), 'THONG BAO') > 0
                      )
                      OR
                      (
                          NVL(P_THONG_BAO_MODE, 0) = 0
                          AND
                          (
                              PM.CONTENT IS NULL
                              OR INSTR(UPPER(PM.CONTENT), 'THONG BAO') = 0
                          )
                      )
                  )
                  AND
                  (
                      NVL(P_PART_NO, 0) = 0
                      OR PM.PART_NO = P_PART_NO
                  )
            ),
            MESSAGE_ID_RAW AS
            (
                SELECT
                    MESSAGE_ID,
                    PART_NO,
                    CASE
                        WHEN REGEXP_LIKE(FLIGHT_ID_TEXT, '^[0-9]+$')
                        THEN TO_NUMBER(FLIGHT_ID_TEXT)
                    END AS FLIGHT_ID
                FROM MESSAGE_TOKEN
            ),
            MESSAGE_ID AS
            (
                SELECT
                    FLIGHT_ID,
                    MIN(MESSAGE_ID) AS MESSAGE_ID,
                    MIN(PART_NO) AS PART_NO
                FROM MESSAGE_ID_RAW
                WHERE FLIGHT_ID IS NOT NULL
                GROUP BY FLIGHT_ID
            ),
            RESULT_DATA AS
            (
                SELECT
                    COUNT(*) OVER () AS SUMRECORD,
                    ROW_NUMBER() OVER
                    (
                        ORDER BY
                            MI.PART_NO,
                            DF.FLIGHTNBR,
                            DF.FROM_AIRP,
                            DF.TO_AIRP,
                            DF.FLIGHT_ID
                    ) AS RNUM,
                    MI.MESSAGE_ID,
                    MI.PART_NO,
                    DF.FLIGHT_ID,
                    DF.OPER_ID,
                    DF.FLIGHTNBR,
                    DF.REGISTRATION,
                    NVL(DF.CRAFT_TYPE, CT.MA) AS CRAFT_NAME,
                    DF.PURPOSE,
                    DF.PERMTYPE,
                    DF.FROM_AIRP,
                    DF.TO_AIRP,
                    DF.FLIGHTDATE,
                    DF.ETD,
                    DF.ETA,
                    DF.VIA,
                    DF.REMARK
                FROM MESSAGE_ID MI
                INNER JOIN T_DAY_FLIGHTS DF
                        ON DF.FLIGHT_ID = MI.FLIGHT_ID
                LEFT JOIN M_CRAFT_TYPE CT
                       ON CT.CRAFT_ID = DF.CRAFT_ID
                WHERE
                    (
                        TRIM(P_FROM_AIRP) IS NULL
                        OR UPPER(DF.FROM_AIRP) =
                           UPPER(TRIM(P_FROM_AIRP))
                    )
                  AND
                    (
                        TRIM(P_TO_AIRP) IS NULL
                        OR UPPER(DF.TO_AIRP) =
                           UPPER(TRIM(P_TO_AIRP))
                    )
                  AND
                    (
                        TRIM(P_OPER_ID) IS NULL
                        OR UPPER(DF.OPER_ID) =
                           UPPER(TRIM(P_OPER_ID))
                    )
            )
            SELECT *
            FROM RESULT_DATA
            WHERE RNUM BETWEEN v_first_row AND v_last_row
            ORDER BY RNUM;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('GET_FLIGHTS_ON_MESSAGE');
            RAISE;
    END GET_FLIGHTS_ON_MESSAGE;
END MESSAGE_FLIGHT_PKG;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
FROM USER_OBJECTS
WHERE OBJECT_NAME = 'MESSAGE_FLIGHT_PKG'
ORDER BY OBJECT_TYPE;

SELECT TYPE, LINE, POSITION, TEXT
FROM USER_ERRORS
WHERE NAME = 'MESSAGE_FLIGHT_PKG'
ORDER BY SEQUENCE;
