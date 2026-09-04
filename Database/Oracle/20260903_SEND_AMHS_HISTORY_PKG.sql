-- ============================================================================
-- Tra cuu lich su export AMHS tu T_SEND_AMHS.
--
-- Chuc nang web: PlanMessage/SendAMHSHistory.aspx
-- Loc theo khoang ngay EXPORTED_AT (ngay gateway luu lich su va tao .tpl).
-- Khong thay doi MESSAGE_PKG, MESSAGE_AMHS_PKG hoac AMHS_OUTBOX_PKG.
-- Chay bang user ATFM tren dung PDB/service. DBeaver: Execute SQL Script (Alt+X).
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    v_missing VARCHAR2(32767);
    v_count   PLS_INTEGER;

    PROCEDURE require_column(p_column IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TAB_COLUMNS
         WHERE TABLE_NAME = 'T_SEND_AMHS'
           AND COLUMN_NAME = UPPER(p_column);

        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10) || '- T_SEND_AMHS.' || UPPER(p_column);
        END IF;
    END require_column;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_SEND_AMHS';

    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20961,
            'Chua co bang T_SEND_AMHS. Hay chay script tao bang truoc.'
        );
    END IF;

    require_column('ID');
    require_column('OUTBOX_ORACLE_ID');
    require_column('OUTBOX_ADDRESS_ORACLE_ID');
    require_column('ATTACH');
    require_column('CONTENT');
    require_column('FROM_ADDRESS');
    require_column('RECIPIENT_ADDRESS');
    require_column('PRIORITY');
    require_column('SUBJECT');
    require_column('OUTBOX_TIME');
    require_column('TPL_FILE_NAME');
    require_column('SEND_STATUS');
    require_column('EXPORTED_AT');

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20962,
            'T_SEND_AMHS thieu cot:' || SUBSTR(v_missing, 1, 1800)
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('T_SEND_AMHS precheck: OK');
END;
/

CREATE OR REPLACE PACKAGE SEND_AMHS_HISTORY_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_PAGE
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_CONTENT    IN VARCHAR2 DEFAULT NULL,
        P_PAGE_SIZE  IN NUMBER DEFAULT 20,
        P_PAGE_INDEX IN NUMBER DEFAULT 0,
        P_OUT_CURSOR OUT T_CURSOR
    );
END SEND_AMHS_HISTORY_PKG;
/

CREATE OR REPLACE PACKAGE BODY SEND_AMHS_HISTORY_PKG AS

    FUNCTION PARSE_DATE
    (
        P_VALUE IN VARCHAR2,
        P_NAME  IN VARCHAR2
    ) RETURN DATE
    IS
        V_VALUE VARCHAR2(40) := TRIM(P_VALUE);
    BEGIN
        IF REGEXP_LIKE(V_VALUE, '^[0-9]{2}-[0-9]{2}-[0-9]{4}$') THEN
            RETURN TO_DATE(V_VALUE, 'FXDD-MM-YYYY');
        ELSIF REGEXP_LIKE(V_VALUE, '^[0-9]{2}/[0-9]{2}/[0-9]{4}$') THEN
            RETURN TO_DATE(V_VALUE, 'FXDD/MM/YYYY');
        END IF;

        RAISE_APPLICATION_ERROR(
            -20963,
            P_NAME || ' phai dung dinh dang DD-MM-YYYY'
        );
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -20963 THEN
                RAISE;
            END IF;

            RAISE_APPLICATION_ERROR(
                -20963,
                P_NAME || ' khong phai la ngay hop le'
            );
    END PARSE_DATE;

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

    PROCEDURE GET_PAGE
    (
        P_FROM_DATE  IN VARCHAR2,
        P_TO_DATE    IN VARCHAR2,
        P_CONTENT    IN VARCHAR2 DEFAULT NULL,
        P_PAGE_SIZE  IN NUMBER DEFAULT 20,
        P_PAGE_INDEX IN NUMBER DEFAULT 0,
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
        V_FROM_DATE  DATE;
        V_TO_DATE    DATE;
        V_PAGE_SIZE  PLS_INTEGER;
        V_PAGE_INDEX PLS_INTEGER;
        V_FIRST_ROW  PLS_INTEGER;
        V_LAST_ROW   PLS_INTEGER;
        V_CONTENT    VARCHAR2(4000);
    BEGIN
        V_FROM_DATE := PARSE_DATE(P_FROM_DATE, 'P_FROM_DATE');
        V_TO_DATE := PARSE_DATE(P_TO_DATE, 'P_TO_DATE');
        V_CONTENT := UPPER(TRIM(P_CONTENT));

        IF V_TO_DATE < V_FROM_DATE THEN
            RAISE_APPLICATION_ERROR(
                -20964,
                'P_TO_DATE phai lon hon hoac bang P_FROM_DATE'
            );
        END IF;

        V_PAGE_SIZE := LEAST(
            GREATEST(NVL(TRUNC(P_PAGE_SIZE), 20), 1),
            500
        );
        V_PAGE_INDEX := GREATEST(NVL(TRUNC(P_PAGE_INDEX), 0), 0);
        V_FIRST_ROW := V_PAGE_SIZE * V_PAGE_INDEX + 1;
        V_LAST_ROW := V_PAGE_SIZE * (V_PAGE_INDEX + 1);

        OPEN P_OUT_CURSOR FOR
            WITH RESULT_DATA AS
            (
                SELECT
                    COUNT(*) OVER () AS SUMRECORD,
                    ROW_NUMBER() OVER
                    (
                        ORDER BY H.EXPORTED_AT DESC, H.ID DESC
                    ) AS RNUM,
                    H.ID,
                    H.OUTBOX_ORACLE_ID,
                    H.OUTBOX_ADDRESS_ORACLE_ID,
                    H.ATTACH,
                    H.CONTENT,
                    H.FROM_ADDRESS,
                    H.RECIPIENT_ADDRESS,
                    H.PRIORITY,
                    H.SUBJECT,
                    H.OUTBOX_TIME,
                    H.TPL_FILE_NAME,
                    H.SEND_STATUS,
                    H.EXPORTED_AT
                FROM T_SEND_AMHS H
                WHERE H.EXPORTED_AT >= CAST(V_FROM_DATE AS TIMESTAMP)
                  AND H.EXPORTED_AT <  CAST(V_TO_DATE + 1 AS TIMESTAMP)
                  AND
                  (
                      V_CONTENT IS NULL
                      OR INSTR(UPPER(H.CONTENT), V_CONTENT) > 0
                      OR INSTR(UPPER(H.SUBJECT), V_CONTENT) > 0
                      OR INSTR(UPPER(H.RECIPIENT_ADDRESS), V_CONTENT) > 0
                  )
            )
            SELECT
                SUMRECORD,
                RNUM,
                ID,
                OUTBOX_ORACLE_ID,
                OUTBOX_ADDRESS_ORACLE_ID,
                ATTACH,
                CONTENT,
                FROM_ADDRESS,
                RECIPIENT_ADDRESS,
                PRIORITY,
                SUBJECT,
                OUTBOX_TIME,
                TPL_FILE_NAME,
                SEND_STATUS,
                EXPORTED_AT
            FROM RESULT_DATA
            WHERE RNUM BETWEEN V_FIRST_ROW AND V_LAST_ROW
            ORDER BY RNUM;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('SEND_AMHS_HISTORY_PKG.GET_PAGE');
            RAISE;
    END GET_PAGE;
END SEND_AMHS_HISTORY_PKG;
/

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'SEND_AMHS_HISTORY_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF V_COUNT <> 2 THEN
        RAISE_APPLICATION_ERROR(-20965, 'SEND_AMHS_HISTORY_PKG khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_ERRORS
     WHERE NAME = 'SEND_AMHS_HISTORY_PKG';

    IF V_COUNT > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20966,
            'SEND_AMHS_HISTORY_PKG con ' || V_COUNT || ' loi bien dich'
        );
    END IF;

    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'SEND_AMHS_HISTORY_PKG'
       AND OBJECT_NAME = 'GET_PAGE'
       AND ARGUMENT_NAME IN
           ('P_FROM_DATE', 'P_TO_DATE', 'P_CONTENT', 'P_PAGE_SIZE', 'P_PAGE_INDEX', 'P_OUT_CURSOR');

    IF V_COUNT <> 6 THEN
        RAISE_APPLICATION_ERROR(-20967, 'Sai chu ky SEND_AMHS_HISTORY_PKG.GET_PAGE');
    END IF;

    DBMS_OUTPUT.PUT_LINE('SEND_AMHS_HISTORY_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'SEND_AMHS_HISTORY_PKG'
 ORDER BY OBJECT_TYPE;

SELECT POSITION, ARGUMENT_NAME, IN_OUT, DATA_TYPE
  FROM USER_ARGUMENTS
 WHERE PACKAGE_NAME = 'SEND_AMHS_HISTORY_PKG'
   AND OBJECT_NAME = 'GET_PAGE'
 ORDER BY SEQUENCE;

SELECT INDEX_NAME, STATUS, VISIBILITY
  FROM USER_INDEXES
 WHERE INDEX_NAME = 'T_SEND_AMHS_EXPORTED_AT_IX';
