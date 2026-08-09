-- Quan ly dien van ban dan that: nhap -> gui duyet -> duyet/tu choi -> export.
-- STATUS: 0=Nhaps, 1=Da duyet, 2=Cho duyet, 3=Tu choi, 4=Da export.

SET SERVEROUTPUT ON;

DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_LIVE_FIRE_MESSAGE';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE q'~
            CREATE TABLE T_LIVE_FIRE_MESSAGE
            (
                ID                    NUMBER(18)       NOT NULL,
                MESSAGE_DATE          DATE             NOT NULL,
                MESSAGE_CODE          VARCHAR2(100),
                SUBJECT               VARCHAR2(300)    NOT NULL,
                INTRO_TEXT            VARCHAR2(2000),
                LOCATION_TEXT         VARCHAR2(1000)   NOT NULL,
                COORDINATES_JSON      CLOB,
                COORDINATES_TEXT      VARCHAR2(3000),
                FIRING_DIRECTION      VARCHAR2(300),
                TRAJECTORY_HEIGHT     VARCHAR2(100),
                TRAJECTORY_RANGE      VARCHAR2(100),
                SCHEDULE_JSON         CLOB,
                SCHEDULE_TEXT         VARCHAR2(3000),
                COMMANDER_RANK        VARCHAR2(100),
                COMMANDER_NAME        VARCHAR2(200),
                COMMANDER_PHONE       VARCHAR2(50),
                REPLACEMENT_RANK      VARCHAR2(100),
                REPLACEMENT_NAME      VARCHAR2(200),
                REPLACEMENT_PHONE     VARCHAR2(50),
                RESTRICTION_TEXT      VARCHAR2(2000),
                SIGNATORY_TITLE       VARCHAR2(300),
                SIGNATORY_NAME        VARCHAR2(200),
                DRAFT_CONTENT         CLOB             NOT NULL,
                APPROVED_CONTENT      CLOB,
                STATUS                NUMBER(1)         DEFAULT 0 NOT NULL,
                VERSION_NO            NUMBER(10)        DEFAULT 1 NOT NULL,
                REJECT_REASON         VARCHAR2(1000),
                CREATED_BY            VARCHAR2(100)     NOT NULL,
                CREATED_DATE          DATE              DEFAULT SYSDATE NOT NULL,
                MODIFIED_BY           VARCHAR2(100),
                MODIFIED_DATE         DATE,
                SUBMITTED_BY          VARCHAR2(100),
                SUBMITTED_DATE        DATE,
                APPROVED_BY           VARCHAR2(100),
                APPROVED_DATE         DATE,
                REJECTED_BY           VARCHAR2(100),
                REJECTED_DATE         DATE,
                EXPORTED_BY           VARCHAR2(100),
                EXPORTED_DATE         DATE,
                CONSTRAINT PK_T_LIVE_FIRE_MESSAGE PRIMARY KEY (ID),
                CONSTRAINT CK_T_LIVE_FIRE_MSG_STATUS
                    CHECK (STATUS IN (0, 1, 2, 3, 4))
            )
        ~';
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM USER_SEQUENCES
     WHERE SEQUENCE_NAME = 'SEQ_T_LIVE_FIRE_MESSAGE';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE SEQUENCE SEQ_T_LIVE_FIRE_MESSAGE '
            || 'START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE';
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_LIVE_FIRE_STATUS_DATE';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_LIVE_FIRE_STATUS_DATE '
            || 'ON T_LIVE_FIRE_MESSAGE (STATUS, MESSAGE_DATE DESC)';
    END IF;

    SELECT COUNT(*) INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_LIVE_FIRE_CREATED_BY';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_LIVE_FIRE_CREATED_BY '
            || 'ON T_LIVE_FIRE_MESSAGE (CREATED_BY, CREATED_DATE DESC)';
    END IF;
END;
/
CREATE OR REPLACE PACKAGE LIVE_FIRE_MESSAGE_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_LIVE_FIRE_MESSAGES
    (
        p_STATUS      IN NUMBER DEFAULT -1,
        p_FROMDATE    IN VARCHAR2,
        p_TODATE      IN VARCHAR2,
        p_KEYWORD     IN VARCHAR2 DEFAULT NULL,
        p_PAGESIZE    IN NUMBER DEFAULT 50,
        p_PAGEINDEX   IN NUMBER DEFAULT 0,
        P_OUT_CURSOR  OUT T_CURSOR
    );

    PROCEDURE GET_LIVE_FIRE_MESSAGE
    (
        p_ID          IN NUMBER,
        P_OUT_CURSOR  OUT T_CURSOR
    );

    PROCEDURE SAVE_LIVE_FIRE_MESSAGE
    (
        p_ID                  IN NUMBER DEFAULT 0,
        p_VERSION_NO          IN NUMBER DEFAULT 0,
        p_MESSAGE_DATE        IN VARCHAR2,
        p_MESSAGE_CODE        IN VARCHAR2 DEFAULT NULL,
        p_SUBJECT             IN VARCHAR2,
        p_INTRO_TEXT          IN VARCHAR2 DEFAULT NULL,
        p_LOCATION_TEXT       IN VARCHAR2,
        p_COORDINATES_JSON    IN VARCHAR2 DEFAULT NULL,
        p_COORDINATES_TEXT    IN VARCHAR2 DEFAULT NULL,
        p_FIRING_DIRECTION    IN VARCHAR2 DEFAULT NULL,
        p_TRAJECTORY_HEIGHT   IN VARCHAR2 DEFAULT NULL,
        p_TRAJECTORY_RANGE    IN VARCHAR2 DEFAULT NULL,
        p_SCHEDULE_JSON       IN VARCHAR2 DEFAULT NULL,
        p_SCHEDULE_TEXT       IN VARCHAR2 DEFAULT NULL,
        p_COMMANDER_RANK      IN VARCHAR2 DEFAULT NULL,
        p_COMMANDER_NAME      IN VARCHAR2 DEFAULT NULL,
        p_COMMANDER_PHONE     IN VARCHAR2 DEFAULT NULL,
        p_REPLACEMENT_RANK    IN VARCHAR2 DEFAULT NULL,
        p_REPLACEMENT_NAME    IN VARCHAR2 DEFAULT NULL,
        p_REPLACEMENT_PHONE   IN VARCHAR2 DEFAULT NULL,
        p_RESTRICTION_TEXT    IN VARCHAR2 DEFAULT NULL,
        p_SIGNATORY_TITLE     IN VARCHAR2 DEFAULT NULL,
        p_SIGNATORY_NAME      IN VARCHAR2 DEFAULT NULL,
        p_DRAFT_CONTENT       IN VARCHAR2,
        p_USER                IN VARCHAR2,
        p_ReturnCode          OUT NUMBER
    );

    PROCEDURE SUBMIT_LIVE_FIRE_MESSAGE
    (
        p_ID          IN NUMBER,
        p_VERSION_NO  IN NUMBER,
        p_USER        IN VARCHAR2,
        p_ReturnCode  OUT NUMBER
    );

    PROCEDURE APPROVE_LIVE_FIRE_MESSAGE
    (
        p_ID          IN NUMBER,
        p_VERSION_NO  IN NUMBER,
        p_USER        IN VARCHAR2,
        p_ReturnCode  OUT NUMBER
    );

    PROCEDURE REJECT_LIVE_FIRE_MESSAGE
    (
        p_ID          IN NUMBER,
        p_VERSION_NO  IN NUMBER,
        p_REASON      IN VARCHAR2,
        p_USER        IN VARCHAR2,
        p_ReturnCode  OUT NUMBER
    );

    PROCEDURE EXPORT_LIVE_FIRE_MESSAGE
    (
        p_ID          IN NUMBER,
        p_USER        IN VARCHAR2,
        p_ReturnCode  OUT NUMBER
    );
END LIVE_FIRE_MESSAGE_PKG;
/

CREATE OR REPLACE PACKAGE BODY LIVE_FIRE_MESSAGE_PKG AS
    c_menu_url CONSTANT VARCHAR2(200) := 'MessManagement/LiveFireMessage.aspx';

    FUNCTION PARSE_DATE(p_value IN VARCHAR2) RETURN DATE IS
    BEGIN
        RETURN TO_DATE(TRIM(p_value), 'FXDD-MM-YYYY');
    EXCEPTION
        WHEN OTHERS THEN
            RETURN TO_DATE(TRIM(p_value), 'FXYYYY-MM-DD');
    END PARSE_DATE;

    FUNCTION HAS_MENU_RIGHT
    (
        p_user  IN VARCHAR2,
        p_right IN VARCHAR2
    ) RETURN BOOLEAN IS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM T_USERS u
          JOIN T_USERMENU um ON um.USER_ID = u.USERID
          JOIN T_MENUS m ON m.ID = um.MENU_ID
         WHERE LOWER(TRIM(u.USERNAME)) = LOWER(TRIM(p_user))
           AND LOWER(TRIM(m.MENUURL)) = LOWER(c_menu_url)
           AND CASE UPPER(p_right)
                   WHEN 'ADD'  THEN NVL(um.R_ADD, 0)
                   WHEN 'EDIT' THEN NVL(um.R_EDIT, 0)
                   WHEN 'PUB'  THEN NVL(um.R_PUB, 0)
                   ELSE 0
               END = 1;
        RETURN v_count > 0;
    EXCEPTION
        WHEN OTHERS THEN
            RETURN FALSE;
    END HAS_MENU_RIGHT;

    PROCEDURE WRITE_HISTORY
    (
        p_user   IN VARCHAR2,
        p_action IN VARCHAR2,
        p_id     IN NUMBER,
        p_note   IN VARCHAR2
    ) IS
    BEGIN
        INSERT INTO T_ACTIONHISTORY
        (
            USERID, FULLNAME, HOSTIP, DATEMODIFY,
            ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
        )
        SELECT NVL(MAX(u.USERID), 0), NVL(p_user, ' '), ' ', SYSDATE,
               p_action, p_id, SUBSTR(p_note, 1, 1000), NVL(MAX(m.ID), 0)
          FROM T_USERS u
          LEFT JOIN T_MENUS m
            ON LOWER(TRIM(m.MENUURL)) = LOWER(c_menu_url)
         WHERE LOWER(TRIM(u.USERNAME)) = LOWER(TRIM(p_user));
    EXCEPTION
        WHEN OTHERS THEN
            NULL;
    END WRITE_HISTORY;

    PROCEDURE WRITE_ERROR(p_action IN VARCHAR2) IS
    BEGIN
        PROCESS_PKG.ADD_ERROR_LOG(
            p_action,
            SQLCODE,
            SUBSTR(SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE, 1, 200)
        );
    EXCEPTION
        WHEN OTHERS THEN NULL;
    END WRITE_ERROR;

    PROCEDURE GET_LIVE_FIRE_MESSAGES
    (
        p_STATUS      IN NUMBER DEFAULT -1,
        p_FROMDATE    IN VARCHAR2,
        p_TODATE      IN VARCHAR2,
        p_KEYWORD     IN VARCHAR2 DEFAULT NULL,
        p_PAGESIZE    IN NUMBER DEFAULT 50,
        p_PAGEINDEX   IN NUMBER DEFAULT 0,
        P_OUT_CURSOR  OUT T_CURSOR
    ) IS
        v_from_date DATE := PARSE_DATE(p_FROMDATE);
        v_to_date   DATE := PARSE_DATE(p_TODATE);
        v_first     NUMBER := GREATEST(NVL(p_PAGESIZE, 50), 1)
                              * GREATEST(NVL(p_PAGEINDEX, 0), 0) + 1;
        v_last      NUMBER := GREATEST(NVL(p_PAGESIZE, 50), 1)
                              * (GREATEST(NVL(p_PAGEINDEX, 0), 0) + 1);
        v_keyword   VARCHAR2(500) := UPPER(TRIM(p_KEYWORD));
    BEGIN
        IF v_to_date < v_from_date THEN
            RAISE_APPLICATION_ERROR(-20001, 'TODATE must be >= FROMDATE');
        END IF;

        OPEN P_OUT_CURSOR FOR
            SELECT q.*
              FROM (
                    SELECT COUNT(*) OVER () AS SUMRECORD,
                           m.ID,
                           TO_CHAR(m.MESSAGE_DATE, 'DD-MM-YYYY') AS MESSAGE_DATE,
                           m.MESSAGE_CODE,
                           m.SUBJECT,
                           m.LOCATION_TEXT,
                           m.STATUS,
                           m.VERSION_NO,
                           m.CREATED_BY,
                           TO_CHAR(m.CREATED_DATE, 'DD-MM-YYYY HH24:MI') AS CREATED_DATE,
                           m.SUBMITTED_BY,
                           TO_CHAR(m.SUBMITTED_DATE, 'DD-MM-YYYY HH24:MI') AS SUBMITTED_DATE,
                           m.APPROVED_BY,
                           TO_CHAR(m.APPROVED_DATE, 'DD-MM-YYYY HH24:MI') AS APPROVED_DATE,
                           m.REJECT_REASON,
                           ROW_NUMBER() OVER (ORDER BY m.MESSAGE_DATE DESC, m.ID DESC) AS RNUM
                      FROM T_LIVE_FIRE_MESSAGE m
                     WHERE m.MESSAGE_DATE >= v_from_date
                       AND m.MESSAGE_DATE < v_to_date + 1
                       AND (NVL(p_STATUS, -1) = -1 OR m.STATUS = p_STATUS)
                       AND (v_keyword IS NULL
                            OR UPPER(m.MESSAGE_CODE) LIKE '%' || v_keyword || '%'
                            OR UPPER(m.SUBJECT) LIKE '%' || v_keyword || '%'
                            OR UPPER(m.LOCATION_TEXT) LIKE '%' || v_keyword || '%'
                            OR UPPER(m.CREATED_BY) LIKE '%' || v_keyword || '%')
                   ) q
             WHERE q.RNUM BETWEEN v_first AND v_last
             ORDER BY q.RNUM;
    END GET_LIVE_FIRE_MESSAGES;

    PROCEDURE GET_LIVE_FIRE_MESSAGE
    (
        p_ID          IN NUMBER,
        P_OUT_CURSOR  OUT T_CURSOR
    ) IS
    BEGIN
        OPEN P_OUT_CURSOR FOR
            SELECT m.ID,
                   TO_CHAR(m.MESSAGE_DATE, 'DD-MM-YYYY') AS MESSAGE_DATE,
                   m.MESSAGE_CODE, m.SUBJECT, m.INTRO_TEXT, m.LOCATION_TEXT,
                   DBMS_LOB.SUBSTR(m.COORDINATES_JSON, 4000, 1) AS COORDINATES_JSON,
                   m.COORDINATES_TEXT, m.FIRING_DIRECTION,
                   m.TRAJECTORY_HEIGHT, m.TRAJECTORY_RANGE,
                   DBMS_LOB.SUBSTR(m.SCHEDULE_JSON, 4000, 1) AS SCHEDULE_JSON,
                   m.SCHEDULE_TEXT, m.COMMANDER_RANK, m.COMMANDER_NAME,
                   m.COMMANDER_PHONE, m.REPLACEMENT_RANK, m.REPLACEMENT_NAME,
                   m.REPLACEMENT_PHONE, m.RESTRICTION_TEXT,
                   m.SIGNATORY_TITLE, m.SIGNATORY_NAME,
                   DBMS_LOB.SUBSTR(m.DRAFT_CONTENT, 4000, 1) AS DRAFT_CONTENT,
                   DBMS_LOB.SUBSTR(m.APPROVED_CONTENT, 4000, 1) AS APPROVED_CONTENT,
                   m.STATUS, m.VERSION_NO, m.REJECT_REASON,
                   m.CREATED_BY,
                   TO_CHAR(m.CREATED_DATE, 'DD-MM-YYYY HH24:MI') AS CREATED_DATE,
                   m.SUBMITTED_BY,
                   TO_CHAR(m.SUBMITTED_DATE, 'DD-MM-YYYY HH24:MI') AS SUBMITTED_DATE,
                   m.APPROVED_BY,
                   TO_CHAR(m.APPROVED_DATE, 'DD-MM-YYYY HH24:MI') AS APPROVED_DATE,
                   m.EXPORTED_BY,
                   TO_CHAR(m.EXPORTED_DATE, 'DD-MM-YYYY HH24:MI') AS EXPORTED_DATE
              FROM T_LIVE_FIRE_MESSAGE m
             WHERE m.ID = p_ID;
    END GET_LIVE_FIRE_MESSAGE;

    PROCEDURE SAVE_LIVE_FIRE_MESSAGE
    (
        p_ID                  IN NUMBER DEFAULT 0,
        p_VERSION_NO          IN NUMBER DEFAULT 0,
        p_MESSAGE_DATE        IN VARCHAR2,
        p_MESSAGE_CODE        IN VARCHAR2 DEFAULT NULL,
        p_SUBJECT             IN VARCHAR2,
        p_INTRO_TEXT          IN VARCHAR2 DEFAULT NULL,
        p_LOCATION_TEXT       IN VARCHAR2,
        p_COORDINATES_JSON    IN VARCHAR2 DEFAULT NULL,
        p_COORDINATES_TEXT    IN VARCHAR2 DEFAULT NULL,
        p_FIRING_DIRECTION    IN VARCHAR2 DEFAULT NULL,
        p_TRAJECTORY_HEIGHT   IN VARCHAR2 DEFAULT NULL,
        p_TRAJECTORY_RANGE    IN VARCHAR2 DEFAULT NULL,
        p_SCHEDULE_JSON       IN VARCHAR2 DEFAULT NULL,
        p_SCHEDULE_TEXT       IN VARCHAR2 DEFAULT NULL,
        p_COMMANDER_RANK      IN VARCHAR2 DEFAULT NULL,
        p_COMMANDER_NAME      IN VARCHAR2 DEFAULT NULL,
        p_COMMANDER_PHONE     IN VARCHAR2 DEFAULT NULL,
        p_REPLACEMENT_RANK    IN VARCHAR2 DEFAULT NULL,
        p_REPLACEMENT_NAME    IN VARCHAR2 DEFAULT NULL,
        p_REPLACEMENT_PHONE   IN VARCHAR2 DEFAULT NULL,
        p_RESTRICTION_TEXT    IN VARCHAR2 DEFAULT NULL,
        p_SIGNATORY_TITLE     IN VARCHAR2 DEFAULT NULL,
        p_SIGNATORY_NAME      IN VARCHAR2 DEFAULT NULL,
        p_DRAFT_CONTENT       IN VARCHAR2,
        p_USER                IN VARCHAR2,
        p_ReturnCode          OUT NUMBER
    ) IS
        v_id NUMBER;
    BEGIN
        IF TRIM(p_SUBJECT) IS NULL OR TRIM(p_LOCATION_TEXT) IS NULL
           OR TRIM(p_DRAFT_CONTENT) IS NULL THEN
            p_ReturnCode := -4;
            RETURN;
        END IF;

        IF NVL(p_ID, 0) = 0 THEN
            IF NOT HAS_MENU_RIGHT(p_USER, 'ADD') THEN
                p_ReturnCode := -5;
                RETURN;
            END IF;

            v_id := SEQ_T_LIVE_FIRE_MESSAGE.NEXTVAL;
            INSERT INTO T_LIVE_FIRE_MESSAGE
            (
                ID, MESSAGE_DATE, MESSAGE_CODE, SUBJECT, INTRO_TEXT,
                LOCATION_TEXT, COORDINATES_JSON, COORDINATES_TEXT,
                FIRING_DIRECTION, TRAJECTORY_HEIGHT, TRAJECTORY_RANGE,
                SCHEDULE_JSON, SCHEDULE_TEXT,
                COMMANDER_RANK, COMMANDER_NAME, COMMANDER_PHONE,
                REPLACEMENT_RANK, REPLACEMENT_NAME, REPLACEMENT_PHONE,
                RESTRICTION_TEXT, SIGNATORY_TITLE, SIGNATORY_NAME,
                DRAFT_CONTENT, STATUS, VERSION_NO, CREATED_BY, CREATED_DATE
            )
            VALUES
            (
                v_id, PARSE_DATE(p_MESSAGE_DATE), TRIM(p_MESSAGE_CODE),
                TRIM(p_SUBJECT), p_INTRO_TEXT, TRIM(p_LOCATION_TEXT),
                p_COORDINATES_JSON, p_COORDINATES_TEXT,
                p_FIRING_DIRECTION, p_TRAJECTORY_HEIGHT, p_TRAJECTORY_RANGE,
                p_SCHEDULE_JSON, p_SCHEDULE_TEXT,
                p_COMMANDER_RANK, p_COMMANDER_NAME, p_COMMANDER_PHONE,
                p_REPLACEMENT_RANK, p_REPLACEMENT_NAME, p_REPLACEMENT_PHONE,
                p_RESTRICTION_TEXT, p_SIGNATORY_TITLE, p_SIGNATORY_NAME,
                p_DRAFT_CONTENT, 0, 1, TRIM(p_USER), SYSDATE
            );
            WRITE_HISTORY(p_USER, 'CREATE LIVE FIRE MESSAGE', v_id, p_SUBJECT);
        ELSE
            IF NOT HAS_MENU_RIGHT(p_USER, 'EDIT') THEN
                p_ReturnCode := -5;
                RETURN;
            END IF;

            v_id := p_ID;
            UPDATE T_LIVE_FIRE_MESSAGE
               SET MESSAGE_DATE = PARSE_DATE(p_MESSAGE_DATE),
                   MESSAGE_CODE = TRIM(p_MESSAGE_CODE),
                   SUBJECT = TRIM(p_SUBJECT), INTRO_TEXT = p_INTRO_TEXT,
                   LOCATION_TEXT = TRIM(p_LOCATION_TEXT),
                   COORDINATES_JSON = p_COORDINATES_JSON,
                   COORDINATES_TEXT = p_COORDINATES_TEXT,
                   FIRING_DIRECTION = p_FIRING_DIRECTION,
                   TRAJECTORY_HEIGHT = p_TRAJECTORY_HEIGHT,
                   TRAJECTORY_RANGE = p_TRAJECTORY_RANGE,
                   SCHEDULE_JSON = p_SCHEDULE_JSON,
                   SCHEDULE_TEXT = p_SCHEDULE_TEXT,
                   COMMANDER_RANK = p_COMMANDER_RANK,
                   COMMANDER_NAME = p_COMMANDER_NAME,
                   COMMANDER_PHONE = p_COMMANDER_PHONE,
                   REPLACEMENT_RANK = p_REPLACEMENT_RANK,
                   REPLACEMENT_NAME = p_REPLACEMENT_NAME,
                   REPLACEMENT_PHONE = p_REPLACEMENT_PHONE,
                   RESTRICTION_TEXT = p_RESTRICTION_TEXT,
                   SIGNATORY_TITLE = p_SIGNATORY_TITLE,
                   SIGNATORY_NAME = p_SIGNATORY_NAME,
                   DRAFT_CONTENT = p_DRAFT_CONTENT,
                   MODIFIED_BY = TRIM(p_USER), MODIFIED_DATE = SYSDATE,
                   VERSION_NO = VERSION_NO + 1
             WHERE ID = p_ID
               AND VERSION_NO = p_VERSION_NO
               AND STATUS IN (0, 3);

            IF SQL%ROWCOUNT = 0 THEN
                p_ReturnCode := -2;
                ROLLBACK;
                RETURN;
            END IF;
            WRITE_HISTORY(p_USER, 'UPDATE LIVE FIRE MESSAGE', v_id, p_SUBJECT);
        END IF;

        COMMIT;
        p_ReturnCode := v_id;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            WRITE_ERROR('SAVE_LIVE_FIRE_MESSAGE');
            p_ReturnCode := -1;
    END SAVE_LIVE_FIRE_MESSAGE;

    PROCEDURE SUBMIT_LIVE_FIRE_MESSAGE
    (
        p_ID IN NUMBER, p_VERSION_NO IN NUMBER, p_USER IN VARCHAR2,
        p_ReturnCode OUT NUMBER
    ) IS
    BEGIN
        IF NOT HAS_MENU_RIGHT(p_USER, 'EDIT') THEN
            p_ReturnCode := -5;
            RETURN;
        END IF;

        UPDATE T_LIVE_FIRE_MESSAGE
           SET STATUS = 2, SUBMITTED_BY = TRIM(p_USER), SUBMITTED_DATE = SYSDATE,
               REJECT_REASON = NULL, VERSION_NO = VERSION_NO + 1
         WHERE ID = p_ID AND VERSION_NO = p_VERSION_NO AND STATUS IN (0, 3);
        IF SQL%ROWCOUNT = 0 THEN
            p_ReturnCode := -2;
            ROLLBACK;
            RETURN;
        END IF;
        WRITE_HISTORY(p_USER, 'SUBMIT LIVE FIRE MESSAGE', p_ID, 'Sent for approval');
        COMMIT;
        p_ReturnCode := p_ID;
    EXCEPTION WHEN OTHERS THEN
        ROLLBACK; WRITE_ERROR('SUBMIT_LIVE_FIRE_MESSAGE'); p_ReturnCode := -1;
    END SUBMIT_LIVE_FIRE_MESSAGE;

    PROCEDURE APPROVE_LIVE_FIRE_MESSAGE
    (
        p_ID IN NUMBER, p_VERSION_NO IN NUMBER, p_USER IN VARCHAR2,
        p_ReturnCode OUT NUMBER
    ) IS
    BEGIN
        IF NOT HAS_MENU_RIGHT(p_USER, 'PUB') THEN
            p_ReturnCode := -5;
            RETURN;
        END IF;

        UPDATE T_LIVE_FIRE_MESSAGE
           SET STATUS = 1, APPROVED_CONTENT = DRAFT_CONTENT,
               APPROVED_BY = TRIM(p_USER), APPROVED_DATE = SYSDATE,
               REJECT_REASON = NULL, VERSION_NO = VERSION_NO + 1
         WHERE ID = p_ID AND VERSION_NO = p_VERSION_NO AND STATUS = 2;
        IF SQL%ROWCOUNT = 0 THEN
            p_ReturnCode := -2;
            ROLLBACK;
            RETURN;
        END IF;
        WRITE_HISTORY(p_USER, 'APPROVE LIVE FIRE MESSAGE', p_ID, 'Approved');
        COMMIT;
        p_ReturnCode := p_ID;
    EXCEPTION WHEN OTHERS THEN
        ROLLBACK; WRITE_ERROR('APPROVE_LIVE_FIRE_MESSAGE'); p_ReturnCode := -1;
    END APPROVE_LIVE_FIRE_MESSAGE;

    PROCEDURE REJECT_LIVE_FIRE_MESSAGE
    (
        p_ID IN NUMBER, p_VERSION_NO IN NUMBER, p_REASON IN VARCHAR2,
        p_USER IN VARCHAR2, p_ReturnCode OUT NUMBER
    ) IS
    BEGIN
        IF NOT HAS_MENU_RIGHT(p_USER, 'PUB') THEN
            p_ReturnCode := -5;
            RETURN;
        END IF;
        IF TRIM(p_REASON) IS NULL THEN
            p_ReturnCode := -4;
            RETURN;
        END IF;

        UPDATE T_LIVE_FIRE_MESSAGE
           SET STATUS = 3, REJECT_REASON = SUBSTR(TRIM(p_REASON), 1, 1000),
               REJECTED_BY = TRIM(p_USER), REJECTED_DATE = SYSDATE,
               VERSION_NO = VERSION_NO + 1
         WHERE ID = p_ID AND VERSION_NO = p_VERSION_NO AND STATUS = 2;
        IF SQL%ROWCOUNT = 0 THEN
            p_ReturnCode := -2;
            ROLLBACK;
            RETURN;
        END IF;
        WRITE_HISTORY(p_USER, 'REJECT LIVE FIRE MESSAGE', p_ID, p_REASON);
        COMMIT;
        p_ReturnCode := p_ID;
    EXCEPTION WHEN OTHERS THEN
        ROLLBACK; WRITE_ERROR('REJECT_LIVE_FIRE_MESSAGE'); p_ReturnCode := -1;
    END REJECT_LIVE_FIRE_MESSAGE;

    PROCEDURE EXPORT_LIVE_FIRE_MESSAGE
    (
        p_ID IN NUMBER, p_USER IN VARCHAR2, p_ReturnCode OUT NUMBER
    ) IS
        v_date    DATE;
        v_content CLOB;
    BEGIN
        IF NOT HAS_MENU_RIGHT(p_USER, 'PUB') THEN
            p_ReturnCode := -5;
            RETURN;
        END IF;

        SELECT MESSAGE_DATE, APPROVED_CONTENT
          INTO v_date, v_content
          FROM T_LIVE_FIRE_MESSAGE
         WHERE ID = p_ID AND STATUS = 1
         FOR UPDATE;

        IF DBMS_LOB.GETLENGTH(v_content) = 0 OR LENGTHB(DBMS_LOB.SUBSTR(v_content, 2001, 1)) > 2000 THEN
            p_ReturnCode := -3;
            ROLLBACK;
            RETURN;
        END IF;

        DELETE FROM T_PLAN_MESSAGE
         WHERE STATUS = 0
           AND MESS_TYPE = 'LIVE FIRE MESSAGE'
           AND LISTFLIGHTID = TO_CHAR(p_ID);

        INSERT INTO T_PLAN_MESSAGE
        (
            FLIGHTDATE, PART_NO, CONTENT, MESS_TYPE, STATUS, LISTFLIGHTID
        )
        VALUES
        (
            v_date, 1, DBMS_LOB.SUBSTR(v_content, 2000, 1),
            'LIVE FIRE MESSAGE', 0, TO_CHAR(p_ID)
        );

        UPDATE T_LIVE_FIRE_MESSAGE
           SET STATUS = 4, EXPORTED_BY = TRIM(p_USER), EXPORTED_DATE = SYSDATE,
               VERSION_NO = VERSION_NO + 1
         WHERE ID = p_ID;

        WRITE_HISTORY(p_USER, 'EXPORT LIVE FIRE MESSAGE', p_ID,
                      'Inserted into T_PLAN_MESSAGE');
        COMMIT;
        p_ReturnCode := p_ID;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            ROLLBACK; p_ReturnCode := -2;
        WHEN OTHERS THEN
            ROLLBACK; WRITE_ERROR('EXPORT_LIVE_FIRE_MESSAGE'); p_ReturnCode := -1;
    END EXPORT_LIVE_FIRE_MESSAGE;
END LIVE_FIRE_MESSAGE_PKG;
/

BEGIN
    DBMS_STATS.GATHER_TABLE_STATS(USER, 'T_LIVE_FIRE_MESSAGE', cascade => TRUE);
END;
/
