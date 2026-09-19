-- ============================================================================
-- Luong gui tat ca dien van AMHS rieng cho MessManagement.aspx.
--
-- Khong thay doi MESSAGE_PKG/luong gui AFTN cu. Package nay:
--   * doc cac ban tin chua gui (STATUS 0/1) theo ngay va 3 co HVN/LANDING/OVER;
--   * ghi tung ban tin vao OUTBOX_ORACLE va OUTBOX_ADDRESS_ORACLE;
--   * SUBJECT lay tu MESS_TYPE cua tung ban tin (P_HEADER la gia tri fallback);
--   * cap nhat ISMESSAGE va STATUS sau khi ghi outbox thanh cong;
--   * khong sleep giua cac ban tin, commit mot lan sau khi hoan tat.
--
-- Chay bang user ATFM tren dung PDB/service. DBeaver: Execute SQL Script (Alt+X).
-- ============================================================================

CREATE OR REPLACE PACKAGE MESSAGE_AMHS_PKG AS
    PROCEDURE SEND_ALL_AMHS
    (
        P_VN       IN VARCHAR2,
        P_LD       IN VARCHAR2,
        P_OF       IN VARCHAR2,
        P_ORIGIN   IN VARCHAR2,
        P_HEADER   IN VARCHAR2,
        P_DATE     IN DATE,
        P_TOADD    IN CLOB,
        P_OUT      OUT NUMBER
    );
END MESSAGE_AMHS_PKG;
/

CREATE OR REPLACE PACKAGE BODY MESSAGE_AMHS_PKG AS

    FUNCTION ENQUEUE_AMHS
    (
        P_HEADER IN VARCHAR2,
        P_ORIGIN IN VARCHAR2,
        P_TOADD  IN CLOB,
        P_CONTENT IN VARCHAR2
    ) RETURN NUMBER
    IS
        V_OUTBOX_ID OUTBOX_ORACLE.ID%TYPE;
        V_FROM_NAME VARCHAR2(100);
    BEGIN
        IF P_CONTENT IS NULL THEN
            RAISE_APPLICATION_ERROR(-20941, 'Noi dung dien van rong');
        END IF;

        IF P_TOADD IS NULL OR DBMS_LOB.GETLENGTH(P_TOADD) = 0 THEN
            RAISE_APPLICATION_ERROR(-20942, 'Dia chi AMHS rong');
        END IF;

        V_FROM_NAME := NVL(NULLIF(TRIM(P_ORIGIN), ''), 'VVVVZGZX');

        -- OUTBOX_ORACLE hien chua co sequence trong luong cu; giu quy tac
        -- cap ID hien tai de khong thay doi schema/luong phat AMHS.
        SELECT NVL(MAX(ID), 0) + 1
          INTO V_OUTBOX_ID
          FROM OUTBOX_ORACLE;

        INSERT INTO OUTBOX_ORACLE
        (
            ID, ATTACH, CONTENT, FROM_ADDRESS,
            PRIORITY, SUBJECT, TIME
        )
        VALUES
        (
            V_OUTBOX_ID,
            0,
            P_CONTENT,
            (
                SELECT NVL(A.ADDRESS, 'AXXCA')
                  FROM ADDRESS_ORACLE A
                 WHERE A.NAME = V_FROM_NAME
                   AND A.SCHEME = 'C'
                   AND ROWNUM < 2
            ),
            CASE WHEN UPPER(TRIM(P_HEADER)) = 'GG' THEN 1 ELSE 0 END,
            NVL(NULLIF(TRIM(P_HEADER), ''), 'object'),
            SYSDATE
        );

        INSERT INTO OUTBOX_ADDRESS_ORACLE
        (
            ADDRESS,
            OUTBOX_ORACLE_ID
        )
        SELECT A.ADDRESS, V_OUTBOX_ID
          FROM TABLE
          (
              SPLIT_STRING(TRIM(REPLACE(P_TOADD, ' ', ',')), ',')
          ) S
          INNER JOIN ADDRESS_ORACLE A
             ON A.NAME = S.COLUMN_VALUE
            AND A.SCHEME <> 'C';

        RETURN 1;
    EXCEPTION
        WHEN OTHERS THEN
            RETURN -1;
    END ENQUEUE_AMHS;

    PROCEDURE SEND_ALL_AMHS
    (
        P_VN       IN VARCHAR2,
        P_LD       IN VARCHAR2,
        P_OF       IN VARCHAR2,
        P_ORIGIN   IN VARCHAR2,
        P_HEADER   IN VARCHAR2,
        P_DATE     IN DATE,
        P_TOADD    IN CLOB,
        P_OUT      OUT NUMBER
    )
    IS
        V_DATE       DATE := TRUNC(P_DATE);
        V_RESULT     NUMBER;
        V_SENT_COUNT PLS_INTEGER := 0;
    BEGIN
        P_OUT := -1;

        IF P_DATE IS NULL THEN
            RAISE_APPLICATION_ERROR(-20943, 'P_DATE is required');
        END IF;

        IF NVL(TRIM(P_VN), '0') <> '1'
           AND NVL(TRIM(P_LD), '0') <> '1'
           AND NVL(TRIM(P_OF), '0') <> '1' THEN
            RAISE_APPLICATION_ERROR(-20944, 'Chua chon loai dien van');
        END IF;

        FOR R IN
        (
            SELECT ID, LISTFLIGHTID, CONTENT, MESS_TYPE
              FROM T_PLAN_MESSAGE
             WHERE FLIGHTDATE >= V_DATE
               AND FLIGHTDATE <  V_DATE + 1
               AND STATUS IN (0, 1)
               AND
               (
                   (NVL(TRIM(P_VN), '0') = '1' AND MESS_TYPE = 'HVN MESSAGE')
                   OR
                   (NVL(TRIM(P_LD), '0') = '1' AND MESS_TYPE = 'LANDING FLIGHTS')
                   OR
                   (NVL(TRIM(P_OF), '0') = '1' AND MESS_TYPE = 'OVER FLIGHTS')
               )
             ORDER BY PART_NO, ID
        )
        LOOP
            V_RESULT := ENQUEUE_AMHS
            (
                NVL(NULLIF(TRIM(R.MESS_TYPE), ''), NULLIF(TRIM(P_HEADER), '')),
                P_ORIGIN,
                P_TOADD,
                R.CONTENT
            );

            IF NVL(V_RESULT, -1) <> 1 THEN
                RAISE_APPLICATION_ERROR(
                    -20945,
                    'Khong ghi duoc OUTBOX cho ban tin ID=' || R.ID
                );
            END IF;

            UPDATE T_DAY_FLIGHTS
               SET ISMESSAGE = 1
             WHERE FLIGHT_ID IN
             (
                 SELECT COLUMN_VALUE
                   FROM TABLE(SPLIT_STRING(R.LISTFLIGHTID, ','))
             );

            UPDATE T_PLAN_MESSAGE
               SET STATUS = 1
             WHERE ID = R.ID;

            V_SENT_COUNT := V_SENT_COUNT + 1;
        END LOOP;

        COMMIT;
        P_OUT := 1;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            P_OUT := -1;

            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG
                (
                    'MESSAGE_AMHS_PKG.SEND_ALL_AMHS',
                    SQLCODE,
                    SUBSTR
                    (
                        SQLERRM || CHR(10) ||
                        DBMS_UTILITY.FORMAT_ERROR_BACKTRACE ||
                        '; sent=' || V_SENT_COUNT,
                        1,
                        200
                    )
                );
            EXCEPTION
                WHEN OTHERS THEN
                    NULL;
            END;
    END SEND_ALL_AMHS;

END MESSAGE_AMHS_PKG;
/

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'MESSAGE_AMHS_PKG'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF V_COUNT <> 2 THEN
        RAISE_APPLICATION_ERROR(-20946, 'MESSAGE_AMHS_PKG khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_ERRORS
     WHERE NAME = 'MESSAGE_AMHS_PKG';

    IF V_COUNT > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20947,
            'MESSAGE_AMHS_PKG con ' || V_COUNT || ' loi bien dich'
        );
    END IF;

    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'MESSAGE_AMHS_PKG'
       AND OBJECT_NAME = 'SEND_ALL_AMHS'
       AND ARGUMENT_NAME IN
           ('P_VN', 'P_LD', 'P_OF', 'P_ORIGIN', 'P_HEADER', 'P_DATE', 'P_TOADD', 'P_OUT');

    IF V_COUNT <> 8 THEN
        RAISE_APPLICATION_ERROR(-20948, 'Sai chu ky MESSAGE_AMHS_PKG.SEND_ALL_AMHS');
    END IF;

    DBMS_OUTPUT.PUT_LINE('MESSAGE_AMHS_PKG deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'MESSAGE_AMHS_PKG'
 ORDER BY OBJECT_TYPE;

SELECT POSITION, ARGUMENT_NAME, IN_OUT, DATA_TYPE
  FROM USER_ARGUMENTS
 WHERE PACKAGE_NAME = 'MESSAGE_AMHS_PKG'
   AND OBJECT_NAME = 'SEND_ALL_AMHS'
 ORDER BY SEQUENCE;
