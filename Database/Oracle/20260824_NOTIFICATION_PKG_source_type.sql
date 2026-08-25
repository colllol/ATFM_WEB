-- Bo sung SOURCE_TYPE vao ket qua GET_STATE va GET_PAGE de giao dien hien thi dong Type.
-- Chi thay PACKAGE BODY, khong doi PACKAGE SPEC.

CREATE OR REPLACE PACKAGE BODY NOTIFICATION_PKG AS
    PROCEDURE VALIDATE_USER(P_USER_ID IN NUMBER) IS
    BEGIN
        IF P_USER_ID IS NULL OR P_USER_ID <= 0 THEN
            RAISE_APPLICATION_ERROR(-20001, 'USER_ID is required.');
        END IF;
    END VALIDATE_USER;

    PROCEDURE GET_STATE(
        P_USER_ID IN NUMBER,
        P_OUT_CURSOR OUT T_CURSOR) IS
    BEGIN
        VALIDATE_USER(P_USER_ID);

        OPEN P_OUT_CURSOR FOR
            SELECT RECENT.ID,
                   RECENT.TITLE,
                   RECENT.CONTENT,
                   RECENT.DATETIME,
                   RECENT.SOURCE_TYPE,
                   RECENT.STATUS,
                   RECENT.UNREAD_COUNT
            FROM (
                SELECT ID,
                       TITLE,
                       CONTENT,
                       DATETIME,
                       SOURCE_TYPE,
                       0 AS STATUS,
                       COUNT(*) OVER () AS UNREAD_COUNT
                FROM T_NOTIFICATION N
                WHERE (N.TARGET_TYPE = 0
                       OR EXISTS (
                           SELECT 1
                           FROM T_NOTIFICATION_TARGET T
                           WHERE T.NOTIFICATION_ID = N.ID
                             AND T.USER_ID = P_USER_ID))
                  AND NOT EXISTS (
                      SELECT 1
                      FROM T_NOTIFICATION_READ R
                      WHERE R.NOTIFICATION_ID = N.ID
                        AND R.USER_ID = P_USER_ID)
                ORDER BY N.DATETIME DESC, N.ID DESC
            ) RECENT
            WHERE ROWNUM <= 50;
    END GET_STATE;

    PROCEDURE GET_PAGE(
        P_USER_ID IN NUMBER,
        P_STATUS IN NUMBER,
        P_PAGE_INDEX IN NUMBER,
        P_PAGE_SIZE IN NUMBER,
        P_DATA_CURSOR OUT T_CURSOR,
        P_TOTAL_CURSOR OUT T_CURSOR) IS
        V_STATUS NUMBER := CASE WHEN P_STATUS IN (0, 1) THEN P_STATUS ELSE -1 END;
        V_PAGE_INDEX NUMBER := GREATEST(NVL(P_PAGE_INDEX, 1), 1);
        V_PAGE_SIZE NUMBER := LEAST(GREATEST(NVL(P_PAGE_SIZE, 100), 1), 100);
        V_FIRST_ROW NUMBER;
        V_LAST_ROW NUMBER;
    BEGIN
        VALIDATE_USER(P_USER_ID);
        V_FIRST_ROW := ((V_PAGE_INDEX - 1) * V_PAGE_SIZE) + 1;
        V_LAST_ROW := V_PAGE_INDEX * V_PAGE_SIZE;

        OPEN P_DATA_CURSOR FOR
            SELECT PAGE_DATA.ID,
                   PAGE_DATA.TITLE,
                   PAGE_DATA.CONTENT,
                   PAGE_DATA.DATETIME,
                   PAGE_DATA.SOURCE_TYPE,
                   PAGE_DATA.STATUS
            FROM (
                SELECT N.ID,
                       N.TITLE,
                       N.CONTENT,
                       N.DATETIME,
                       N.SOURCE_TYPE,
                       CASE
                           WHEN EXISTS (
                               SELECT 1
                               FROM T_NOTIFICATION_READ R
                               WHERE R.NOTIFICATION_ID = N.ID
                                 AND R.USER_ID = P_USER_ID)
                           THEN 1 ELSE 0
                       END AS STATUS,
                       ROW_NUMBER() OVER (ORDER BY N.DATETIME DESC, N.ID DESC) AS ROW_NUMBER_VALUE
                FROM T_NOTIFICATION N
                WHERE (N.TARGET_TYPE = 0
                       OR EXISTS (
                           SELECT 1
                           FROM T_NOTIFICATION_TARGET T
                           WHERE T.NOTIFICATION_ID = N.ID
                             AND T.USER_ID = P_USER_ID))
                  AND (V_STATUS = -1
                       OR V_STATUS = CASE
                           WHEN EXISTS (
                               SELECT 1
                               FROM T_NOTIFICATION_READ R
                               WHERE R.NOTIFICATION_ID = N.ID
                                 AND R.USER_ID = P_USER_ID)
                           THEN 1 ELSE 0
                       END)
            ) PAGE_DATA
            WHERE PAGE_DATA.ROW_NUMBER_VALUE BETWEEN V_FIRST_ROW AND V_LAST_ROW
            ORDER BY PAGE_DATA.ROW_NUMBER_VALUE;

        OPEN P_TOTAL_CURSOR FOR
            SELECT (SELECT COUNT(*)
                    FROM T_NOTIFICATION N
                    WHERE (N.TARGET_TYPE = 0
                           OR EXISTS (
                               SELECT 1
                               FROM T_NOTIFICATION_TARGET T
                               WHERE T.NOTIFICATION_ID = N.ID
                                 AND T.USER_ID = P_USER_ID))
                      AND (V_STATUS = -1
                           OR V_STATUS = CASE
                               WHEN EXISTS (
                                   SELECT 1
                                   FROM T_NOTIFICATION_READ R
                                   WHERE R.NOTIFICATION_ID = N.ID
                                     AND R.USER_ID = P_USER_ID)
                               THEN 1 ELSE 0
                           END)) AS TOTAL_COUNT,
                   (SELECT COUNT(*)
                    FROM T_NOTIFICATION N
                    WHERE (N.TARGET_TYPE = 0
                           OR EXISTS (
                               SELECT 1
                               FROM T_NOTIFICATION_TARGET T
                               WHERE T.NOTIFICATION_ID = N.ID
                                 AND T.USER_ID = P_USER_ID))
                      AND NOT EXISTS (
                          SELECT 1
                          FROM T_NOTIFICATION_READ R
                          WHERE R.NOTIFICATION_ID = N.ID
                            AND R.USER_ID = P_USER_ID)) AS UNREAD_COUNT
            FROM DUAL;
    END GET_PAGE;

    PROCEDURE MARK_READ(
        P_USER_ID IN NUMBER,
        P_ID IN NUMBER,
        P_UPDATED_COUNT OUT NUMBER) IS
    BEGIN
        VALIDATE_USER(P_USER_ID);

        INSERT INTO T_NOTIFICATION_READ (NOTIFICATION_ID, USER_ID)
        SELECT N.ID, P_USER_ID
        FROM T_NOTIFICATION N
        WHERE N.ID = P_ID
          AND (N.TARGET_TYPE = 0
               OR EXISTS (
                   SELECT 1
                   FROM T_NOTIFICATION_TARGET T
                   WHERE T.NOTIFICATION_ID = N.ID
                     AND T.USER_ID = P_USER_ID))
          AND NOT EXISTS (
              SELECT 1
              FROM T_NOTIFICATION_READ R
              WHERE R.NOTIFICATION_ID = N.ID
                AND R.USER_ID = P_USER_ID);
        P_UPDATED_COUNT := SQL%ROWCOUNT;
        COMMIT;
    END MARK_READ;

    PROCEDURE MARK_ALL_READ(
        P_USER_ID IN NUMBER,
        P_UPDATED_COUNT OUT NUMBER) IS
    BEGIN
        VALIDATE_USER(P_USER_ID);

        INSERT INTO T_NOTIFICATION_READ (NOTIFICATION_ID, USER_ID)
        SELECT N.ID, P_USER_ID
        FROM T_NOTIFICATION N
        WHERE (N.TARGET_TYPE = 0
               OR EXISTS (
                   SELECT 1
                   FROM T_NOTIFICATION_TARGET T
                   WHERE T.NOTIFICATION_ID = N.ID
                     AND T.USER_ID = P_USER_ID))
          AND NOT EXISTS (
              SELECT 1
              FROM T_NOTIFICATION_READ R
              WHERE R.NOTIFICATION_ID = N.ID
                AND R.USER_ID = P_USER_ID);
        P_UPDATED_COUNT := SQL%ROWCOUNT;
        COMMIT;
    END MARK_ALL_READ;

    PROCEDURE CREATE_NOTIFICATION(
        P_TITLE IN NVARCHAR2,
        P_CONTENT IN NVARCHAR2,
        P_USER_IDS IN VARCHAR2,
        P_ID OUT NUMBER) IS
        V_TARGET_TYPE NUMBER(1);
        V_TARGET_COUNT NUMBER;
    BEGIN
        IF TRIM(P_TITLE) IS NULL OR TRIM(P_CONTENT) IS NULL THEN
            RAISE_APPLICATION_ERROR(-20002, 'TITLE and CONTENT are required.');
        END IF;

        V_TARGET_TYPE := CASE WHEN TRIM(P_USER_IDS) IS NULL THEN 0 ELSE 1 END;

        INSERT INTO T_NOTIFICATION (TITLE, CONTENT, DATETIME, TARGET_TYPE)
        VALUES (TRIM(P_TITLE), TRIM(P_CONTENT), SYSTIMESTAMP, V_TARGET_TYPE)
        RETURNING ID INTO P_ID;

        IF V_TARGET_TYPE = 1 THEN
            INSERT INTO T_NOTIFICATION_TARGET (NOTIFICATION_ID, USER_ID)
            SELECT P_ID, USER_ID
            FROM (
                SELECT DISTINCT
                       TO_NUMBER(TRIM(REGEXP_SUBSTR(P_USER_IDS, '[^,]+', 1, LEVEL))) AS USER_ID
                FROM DUAL
                CONNECT BY REGEXP_SUBSTR(P_USER_IDS, '[^,]+', 1, LEVEL) IS NOT NULL
            )
            WHERE USER_ID > 0;

            V_TARGET_COUNT := SQL%ROWCOUNT;
            IF V_TARGET_COUNT = 0 THEN
                RAISE_APPLICATION_ERROR(-20003, 'At least one target USER_ID is required.');
            END IF;
        END IF;

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END CREATE_NOTIFICATION;
END NOTIFICATION_PKG;
/


