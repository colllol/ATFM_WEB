-- Danh sach dien van da dua vao hang doi AMHS.
-- Loc theo khoang ngay cua OUTBOX_ORACLE.TIME, tim kiem noi dung va phan trang.

DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM USER_INDEXES
    WHERE INDEX_NAME = 'IX_OUTBOX_ORACLE_TIME';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_OUTBOX_ORACLE_TIME ON OUTBOX_ORACLE (TIME, ID)';
    END IF;
END;
/

DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM USER_INDEXES
    WHERE INDEX_NAME = 'IX_OUTBOX_ADDR_OUTBOX_ID';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_OUTBOX_ADDR_OUTBOX_ID '
            || 'ON OUTBOX_ADDRESS_ORACLE (OUTBOX_ORACLE_ID)';
    END IF;
END;
/

CREATE OR REPLACE PACKAGE AMHS_OUTBOX_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_OUTBOX_MESSAGES
    (
        P_FROM_DATE   IN VARCHAR2,
        P_TO_DATE     IN VARCHAR2,
        P_CONTENT     IN VARCHAR2 DEFAULT NULL,
        P_PAGESIZE    IN NUMBER DEFAULT 100,
        P_PAGEINDEX   IN NUMBER DEFAULT 0,
        P_OUT_CURSOR  OUT T_CURSOR
    );

    PROCEDURE GET_OUTBOX_ADDRESSES
    (
        P_OUTBOX_ID   IN NUMBER,
        P_OUT_CURSOR  OUT T_CURSOR
    );
END AMHS_OUTBOX_PKG;
/

CREATE OR REPLACE PACKAGE BODY AMHS_OUTBOX_PKG AS
    FUNCTION PARSE_DATE
    (
        P_VALUE IN VARCHAR2,
        P_NAME  IN VARCHAR2
    ) RETURN DATE
    IS
        v_value VARCHAR2(20) := TRIM(P_VALUE);
    BEGIN
        IF REGEXP_LIKE(v_value, '^[0-9]{2}-[0-9]{2}-[0-9]{4}$') THEN
            RETURN TO_DATE(v_value, 'FXDD-MM-YYYY');
        ELSIF REGEXP_LIKE(v_value, '^[0-9]{2}/[0-9]{2}/[0-9]{4}$') THEN
            RETURN TO_DATE(v_value, 'FXDD/MM/YYYY');
        END IF;

        RAISE_APPLICATION_ERROR(
            -20401,
            P_NAME || ' must use DD-MM-YYYY'
        );
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -20401 THEN
                RAISE;
            END IF;

            RAISE_APPLICATION_ERROR(
                -20401,
                P_NAME || ' is not a valid date'
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

    PROCEDURE GET_OUTBOX_MESSAGES
    (
        P_FROM_DATE   IN VARCHAR2,
        P_TO_DATE     IN VARCHAR2,
        P_CONTENT     IN VARCHAR2 DEFAULT NULL,
        P_PAGESIZE    IN NUMBER DEFAULT 100,
        P_PAGEINDEX   IN NUMBER DEFAULT 0,
        P_OUT_CURSOR  OUT T_CURSOR
    )
    IS
        v_from_date  DATE;
        v_to_date    DATE;
        v_page_size  PLS_INTEGER;
        v_page_index PLS_INTEGER;
        v_first_row  PLS_INTEGER;
        v_last_row   PLS_INTEGER;
        v_content    OUTBOX_ORACLE.CONTENT%TYPE;
    BEGIN
        v_from_date := PARSE_DATE(P_FROM_DATE, 'P_FROM_DATE');
        v_to_date := PARSE_DATE(P_TO_DATE, 'P_TO_DATE');

        IF v_to_date < v_from_date THEN
            RAISE_APPLICATION_ERROR(
                -20402,
                'P_TO_DATE must be greater than or equal to P_FROM_DATE'
            );
        END IF;

        v_content := UPPER(TRIM(P_CONTENT));
        v_page_size := LEAST(
            GREATEST(NVL(TRUNC(P_PAGESIZE), 100), 1),
            100000
        );
        v_page_index := GREATEST(NVL(TRUNC(P_PAGEINDEX), 0), 0);
        v_first_row := (v_page_size * v_page_index) + 1;
        v_last_row := v_page_size * (v_page_index + 1);

        OPEN P_OUT_CURSOR FOR
            WITH RESULT_DATA AS
            (
                SELECT
                    COUNT(*) OVER () AS SUMRECORD,
                    ROW_NUMBER() OVER
                    (
                        ORDER BY O.TIME DESC, O.ID DESC
                    ) AS RNUM,
                    O.ID,
                    O.ATTACH,
                    O.CONTENT,
                    O.FROM_ADDRESS,
                    O.PRIORITY,
                    O.SUBJECT,
                    O.TIME AS SENT_TIME,
                    (
                        SELECT COUNT(*)
                        FROM OUTBOX_ADDRESS_ORACLE A
                        WHERE A.OUTBOX_ORACLE_ID = O.ID
                    ) AS ADDRESS_COUNT
                FROM OUTBOX_ORACLE O
                WHERE O.TIME >= CAST(v_from_date AS TIMESTAMP)
                  AND O.TIME < CAST(v_to_date + 1 AS TIMESTAMP)
                  AND
                  (
                      v_content IS NULL
                      OR INSTR(UPPER(O.CONTENT), v_content) > 0
                  )
            )
            SELECT
                SUMRECORD,
                RNUM,
                ID,
                ATTACH,
                CONTENT,
                FROM_ADDRESS,
                PRIORITY,
                SUBJECT,
                SENT_TIME,
                ADDRESS_COUNT
            FROM RESULT_DATA
            WHERE RNUM BETWEEN v_first_row AND v_last_row
            ORDER BY RNUM;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('AMHS_OUTBOX_PKG.GET_OUTBOX_MESSAGES');
            RAISE;
    END GET_OUTBOX_MESSAGES;

    PROCEDURE GET_OUTBOX_ADDRESSES
    (
        P_OUTBOX_ID   IN NUMBER,
        P_OUT_CURSOR  OUT T_CURSOR
    )
    IS
        v_outbox_id OUTBOX_ORACLE.ID%TYPE;
    BEGIN
        v_outbox_id := TRUNC(P_OUTBOX_ID);

        IF v_outbox_id IS NULL OR v_outbox_id <= 0 THEN
            RAISE_APPLICATION_ERROR(
                -20403,
                'P_OUTBOX_ID must be greater than zero'
            );
        END IF;

        OPEN P_OUT_CURSOR FOR
            SELECT
                ROW_NUMBER() OVER (ORDER BY A.ID) AS RNUM,
                A.ID,
                A.OUTBOX_ORACLE_ID,
                A.ADDRESS
            FROM OUTBOX_ADDRESS_ORACLE A
            WHERE A.OUTBOX_ORACLE_ID = v_outbox_id
            ORDER BY A.ID;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('AMHS_OUTBOX_PKG.GET_OUTBOX_ADDRESSES');
            RAISE;
    END GET_OUTBOX_ADDRESSES;
END AMHS_OUTBOX_PKG;
/
