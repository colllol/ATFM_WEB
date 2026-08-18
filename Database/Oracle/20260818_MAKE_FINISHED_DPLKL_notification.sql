-- ============================================================================
-- Them thong bao DPLKL sau khi MAKE_FINISHED.make_finished_flights_news4day
-- xu ly thanh cong.
--
-- Noi dung:
--   Nhan du lieu Di/Den luc HH:mm - X chuyen. Nguoi chuyen <P_STRING>
-- SOURCE_TYPE = ATFM; SOURCE_KEY = DPLKL.
--
-- Moi lan xu ly thanh cong se INSERT mot dong T_NOTIFICATION rieng.
-- Unique index duoc doi thanh function-based unique index: chi cho phep trung
-- rieng cap ATFM/DPLKL; cac nguon/khoa khac van duoc chong trung.
-- X la so dong dung dieu kien cua A_TEST_SEARCH.GetExportMOVEFINISH:
-- MOVEFINISH=1 va FLIGHTDATE thuoc ngay P_DATE.
--
-- Chay bang DBeaver: Execute SQL Script (Alt+X), schema ATFM.
-- Rollback: 20260818_MAKE_FINISHED_DPLKL_notification_rollback.sql
-- ============================================================================

DECLARE
    v_count PLS_INTEGER;
BEGIN
    IF UPPER(USER) <> 'ATFM' THEN
        RAISE_APPLICATION_ERROR(-20961, 'Script chi duoc chay tren schema ATFM');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'MAKE_FINISHED'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(-20962, 'MAKE_FINISHED khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_NOTIFICATION'
       AND COLUMN_NAME IN
           (
               'ID', 'TITLE', 'CONTENT', 'DATETIME', 'TARGET_TYPE',
               'SOURCE_TYPE', 'SOURCE_KEY'
           );

    IF v_count <> 7 THEN
        RAISE_APPLICATION_ERROR(-20963, 'T_NOTIFICATION thieu cot bat buoc');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME IN
           (
               'T_NOTIFICATION_READ',
               'T_NOTIFICATION_TARGET',
               'T_DAY_FLIGHTS_GOINGON',
               'T_PACKAGE_SOURCE_BACKUP'
           );

    IF v_count <> 4 THEN
        RAISE_APPLICATION_ERROR(-20964, 'Thieu bang phu thuoc cho notification');
    END IF;
END;
/

-- Cho phep moi lan export tao mot dong ATFM/DPLKL, van giu unique cho moi
-- SOURCE_TYPE/SOURCE_KEY khac (bao gom EMAIL_API).
DECLARE
    v_function_index_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_function_index_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'UX_T_NOTIFICATION_SOURCE'
       AND UNIQUENESS = 'UNIQUE'
       AND INDEX_TYPE = 'FUNCTION-BASED NORMAL';

    IF v_function_index_count = 0 THEN
        BEGIN
            EXECUTE IMMEDIATE 'DROP INDEX UX_T_NOTIFICATION_SOURCE';
        EXCEPTION
            WHEN OTHERS THEN
                IF SQLCODE <> -1418 THEN
                    RAISE;
                END IF;
        END;

        EXECUTE IMMEDIATE q'~
            CREATE UNIQUE INDEX UX_T_NOTIFICATION_SOURCE
            ON T_NOTIFICATION
            (
                CASE
                    WHEN SOURCE_TYPE = 'ATFM' AND SOURCE_KEY = 'DPLKL'
                    THEN NULL
                    ELSE SOURCE_TYPE
                END,
                CASE
                    WHEN SOURCE_TYPE = 'ATFM' AND SOURCE_KEY = 'DPLKL'
                    THEN NULL
                    ELSE SOURCE_KEY
                END
            )
        ~';
    END IF;
END;
/

-- Luu package body ngay truoc khi them notification de rollback chinh xac.
DECLARE
    c_deployment_id CONSTANT VARCHAR2(80) :=
        '20260818_MAKE_FINISHED_DPLKL_NOTIFY_V1';
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_deployment_id
       AND OBJECT_NAME = 'MAKE_FINISHED'
       AND OBJECT_TYPE = 'PACKAGE_BODY';

    IF v_count = 0 THEN
        DBMS_METADATA.SET_TRANSFORM_PARAM(
            DBMS_METADATA.SESSION_TRANSFORM,
            'SQLTERMINATOR',
            FALSE
        );

        INSERT INTO T_PACKAGE_SOURCE_BACKUP
        (
            DEPLOYMENT_ID,
            OBJECT_NAME,
            OBJECT_TYPE,
            DDL_TEXT,
            BACKUP_DATE
        )
        VALUES
        (
            c_deployment_id,
            'MAKE_FINISHED',
            'PACKAGE_BODY',
            DBMS_METADATA.GET_DDL('PACKAGE_BODY', 'MAKE_FINISHED', USER),
            SYSDATE
        );
        COMMIT;
    END IF;
END;
/

DECLARE
    v_original_body CLOB;
    v_new_body      CLOB;
    v_implementation CLOB;
    v_upper_body    CLOB;
    v_start_pos     PLS_INTEGER;
    v_end_pos       PLS_INTEGER;
    v_prefix_len    PLS_INTEGER;
    v_replace_len   PLS_INTEGER;
    v_suffix_len    PLS_INTEGER;
    v_dest_offset   PLS_INTEGER := 1;
    v_error_count   PLS_INTEGER;

    PROCEDURE execute_clob_ddl(p_ddl IN CLOB) IS
        v_cursor INTEGER;
    BEGIN
        v_cursor := DBMS_SQL.OPEN_CURSOR;
        BEGIN
            DBMS_SQL.PARSE(v_cursor, p_ddl, DBMS_SQL.NATIVE);
            DBMS_SQL.CLOSE_CURSOR(v_cursor);
        EXCEPTION
            WHEN OTHERS THEN
                IF DBMS_SQL.IS_OPEN(v_cursor) THEN
                    DBMS_SQL.CLOSE_CURSOR(v_cursor);
                END IF;
                RAISE;
        END;
    END execute_clob_ddl;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    v_original_body := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY', 'MAKE_FINISHED', USER
    );
    v_upper_body := UPPER(v_original_body);

    v_start_pos := DBMS_LOB.INSTR(
        v_upper_body,
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS4DAY',
        1,
        1
    );
    v_end_pos := DBMS_LOB.INSTR(
        v_upper_body,
        'PROCEDURE SP_COPPYBRAVO',
        v_start_pos + 1,
        1
    );

    IF v_start_pos = 0 OR v_end_pos = 0 OR v_end_pos <= v_start_pos THEN
        RAISE_APPLICATION_ERROR(
            -20965,
            'Khong tim thay make_finished_flights_news4day -> sp_CoppyBravo'
        );
    END IF;

    v_implementation := q'~
procedure make_finished_flights_news4day
(
    p_string IN VARCHAR2,
    p_date   IN VARCHAR2,
    p_out    OUT NUMBER
)
IS
    v_error_text    VARCHAR2(4000);
    v_summary_code  NUMBER;
    v_finish_code   NUMBER;
    v_selected_date DATE;
    v_flight_count  NUMBER;
    v_notification_title   T_NOTIFICATION.TITLE%TYPE;
    v_notification_content T_NOTIFICATION.CONTENT%TYPE;
BEGIN
    p_out := -1;

    IF TRIM(p_date) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'P_DATE is required');
    END IF;

    BEGIN
        v_selected_date := TO_DATE(TRIM(p_date), 'FXDD-MM-YYYY');
    EXCEPTION
        WHEN OTHERS THEN
            RAISE_APPLICATION_ERROR(
                -20002,
                'P_DATE must use DD-MM-YYYY format'
            );
    END;

    v_selected_date := TRUNC(v_selected_date);

    v_summary_code := DAY_WORKING.FLIGHTS_SUMMARIZE_NEW(
        v_selected_date,
        v_error_text
    );

    IF NVL(v_summary_code, -1) <> 0 THEN
        RAISE_APPLICATION_ERROR(
            -20011,
            'FLIGHTS_SUMMARIZE_NEW failed: code=' || v_summary_code ||
            '; ' || SUBSTR(v_error_text, 1, 1000)
        );
    END IF;

    v_finish_code := DAY_WORKING.REMOVE_FINISHED_FLIGHS_DEL_NEW(
        v_selected_date,
        v_error_text
    );

    IF NVL(v_finish_code, -1) <> 0 THEN
        RAISE_APPLICATION_ERROR(
            -20012,
            'REMOVE_FINISHED_NEW failed: code=' || v_finish_code ||
            '; ' || SUBSTR(v_error_text, 1, 1000)
        );
    END IF;

    SELECT COUNT(*)
      INTO v_flight_count
      FROM T_DAY_FLIGHTS_GOINGON g
     WHERE g.MOVEFINISH = 1
       AND g.FLIGHTDATE >= v_selected_date
       AND g.FLIGHTDATE <  v_selected_date + 1;

    v_notification_title :=
        UNISTR('Nh\1EADn d\1EEF li\1EC7u \0110i/\0110\1EBFn');
    v_notification_content :=
        v_notification_title
        || UNISTR(' l\00FAc ')
        || TO_CHAR(
               SYSTIMESTAMP AT TIME ZONE 'Asia/Ho_Chi_Minh',
               'HH24:MI'
           )
        || UNISTR(' \2013 ')
        || TO_CHAR(v_flight_count)
        || UNISTR(' chuy\1EBFn. Ng\01B0\1EDDi chuy\1EC3n ')
        || NVL(TRIM(p_string), 'UNKNOWN');

    INSERT INTO T_NOTIFICATION
    (
        TITLE, CONTENT, DATETIME, TARGET_TYPE,
        SOURCE_TYPE, SOURCE_KEY
    )
    VALUES
    (
        v_notification_title,
        v_notification_content,
        SYSTIMESTAMP,
        0,
        'ATFM',
        'DPLKL'
    );

    INSERT INTO T_ACTIONHISTORY
    (
        USERID, FULLNAME, HOSTIP, DATEMODIFY,
        ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
    )
    VALUES
    (
        1, p_string, '', SYS_EXTRACT_UTC(SYSTIMESTAMP),
        'make_finished_flights_news4day', 0,
        'FLIGHTDATE=' || TO_CHAR(v_selected_date, 'DD-MM-YYYY'), 0
    );

    COMMIT;
    p_out := 1;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_out := -1;
        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'MAKE_FINISHED.make_finished_flights_news4day',
                SQLCODE,
                SUBSTR(
                    SQLERRM || CHR(10) ||
                    DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                    1,
                    200
                )
            );
        EXCEPTION
            WHEN OTHERS THEN
                NULL;
        END;
END make_finished_flights_news4day;


~';

    DBMS_LOB.CREATETEMPORARY(v_new_body, TRUE);
    v_prefix_len := v_start_pos - 1;
    v_replace_len := DBMS_LOB.GETLENGTH(v_implementation);
    v_suffix_len := DBMS_LOB.GETLENGTH(v_original_body) - v_end_pos + 1;

    IF v_prefix_len > 0 THEN
        DBMS_LOB.COPY(v_new_body, v_original_body, v_prefix_len, 1, 1);
        v_dest_offset := v_prefix_len + 1;
    END IF;

    DBMS_LOB.COPY(
        v_new_body,
        v_implementation,
        v_replace_len,
        v_dest_offset,
        1
    );
    v_dest_offset := v_dest_offset + v_replace_len;

    DBMS_LOB.COPY(
        v_new_body,
        v_original_body,
        v_suffix_len,
        v_dest_offset,
        v_end_pos
    );

    BEGIN
        execute_clob_ddl(v_new_body);

        SELECT COUNT(*)
          INTO v_error_count
          FROM USER_ERRORS
         WHERE NAME = 'MAKE_FINISHED';

        IF v_error_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20966,
                'MAKE_FINISHED con ' || v_error_count || ' loi bien dich'
            );
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            execute_clob_ddl(v_original_body);
            RAISE;
    END;

    DBMS_OUTPUT.PUT_LINE(
        'DEPLOY OK: MAKE_FINISHED.news4day creates ATFM/DPLKL notification.'
    );
END;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'MAKE_FINISHED'
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(-20967, 'MAKE_FINISHED khong VALID sau deploy');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_SOURCE
     WHERE NAME = 'MAKE_FINISHED'
       AND TYPE = 'PACKAGE BODY'
       AND
       (
           UPPER(TEXT) LIKE '%INSERT INTO T_NOTIFICATION%'
           OR UPPER(TEXT) LIKE '%''DPLKL''%'
       );

    IF v_count < 2 THEN
        RAISE_APPLICATION_ERROR(-20968, 'Chua tim thay logic DPLKL notification');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'UX_T_NOTIFICATION_SOURCE'
       AND TABLE_NAME = 'T_NOTIFICATION'
       AND UNIQUENESS = 'UNIQUE'
       AND INDEX_TYPE = 'FUNCTION-BASED NORMAL'
       AND STATUS = 'VALID';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20969, 'Unique index chua cho phep ATFM/DPLKL lap');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_IND_COLUMNS
     WHERE INDEX_NAME = 'UX_T_NOTIFICATION_SOURCE'
       AND TABLE_NAME = 'T_NOTIFICATION';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(-20970, 'Unique index DPLKL sai so cot');
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: MAKE_FINISHED VALID; every success inserts one DPLKL row.'
    );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'MAKE_FINISHED'
 ORDER BY OBJECT_TYPE;
