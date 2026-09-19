-- ============================================================================
-- Them MAKE_FINISHED.make_finished_flights_news4day cho Oracle 12.1/19c.
--
-- Ten nguoi dung de xuat "make_finished_flights_news4Date" dai 31 ky tu,
-- vuot gioi han 30 ky tu cua Oracle 12.1. Ten 4day dai dung 30 ky tu.
--
-- P_DATE: ngay chuyen bay can xu ly, dinh dang DD-MM-YYYY.
-- DAY_WORKING.FLIGHTS_SUMMARIZE_NEW va REMOVE_FINISHED_FLIGHS_DEL_NEW
-- deu nhan truc tiep ngay chuyen bay can xu ly.
-- Script chi chen/thay procedure moi, khong thay noi dung cac procedure khac.
-- Chay bang DBeaver: Execute SQL Script (Alt+X).
-- Day la script thuan Oracle/JDBC; khong dat cac lenh SET cua SQL*Plus
-- trong file vi mot so cau hinh DBeaver se gui SET xuong Oracle va gay
-- ORA-00922: missing or invalid option.
-- ============================================================================

-- Moi lan xu ly thanh cong can tao mot notification ATFM/DPLKL rieng.
-- Doi unique index cu sang function-based unique index: chi cap ATFM/DPLKL
-- duoc phep lap; cac SOURCE_TYPE/SOURCE_KEY khac van giu quy tac unique.
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

DECLARE
    c_procedure_name CONSTANT VARCHAR2(30) :=
        'MAKE_FINISHED_FLIGHTS_NEWS4DAY';

    v_spec_ddl       CLOB;
    v_body_ddl       CLOB;
    v_original_spec  VARCHAR2(32767);
    v_original_body  VARCHAR2(32767);
    v_new_spec       VARCHAR2(32767);
    v_new_body       VARCHAR2(32767);
    v_declaration    VARCHAR2(1000);
    v_implementation VARCHAR2(12000);
    v_start_pos      PLS_INTEGER;
    v_end_pos        PLS_INTEGER;
    v_insert_pos     PLS_INTEGER;
    v_error_count    PLS_INTEGER;

    PROCEDURE execute_ddl(p_ddl IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_ddl;
    END execute_ddl;

    PROCEDURE restore_original_package IS
    BEGIN
        BEGIN
            execute_ddl(v_original_spec);
            execute_ddl(v_original_body);
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE(
                    'WARNING: Khong tu phuc hoi duoc MAKE_FINISHED: ' || SQLERRM
                );
        END;
    END restore_original_package;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    v_spec_ddl := DBMS_METADATA.GET_DDL(
        'PACKAGE_SPEC',
        'MAKE_FINISHED',
        USER
    );
    v_body_ddl := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY',
        'MAKE_FINISHED',
        USER
    );

    IF DBMS_LOB.GETLENGTH(v_spec_ddl) > 32767
       OR DBMS_LOB.GETLENGTH(v_body_ddl) > 32767 THEN
        RAISE_APPLICATION_ERROR(
            -20871,
            'MAKE_FINISHED source vuot 32767 ky tu; dung ban full source de deploy'
        );
    END IF;

    v_original_spec := DBMS_LOB.SUBSTR(v_spec_ddl, 32767, 1);
    v_original_body := DBMS_LOB.SUBSTR(v_body_ddl, 32767, 1);
    v_new_spec := v_original_spec;
    v_new_body := v_original_body;

    v_declaration :=
        '  procedure make_finished_flights_news4day(' ||
        'p_string varchar2, p_date varchar2, p_out out number);';

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

    -- Nhanh NEW nhan truc tiep ngay chuyen bay, khong anh huong ham cu.
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

    -- Dem dung tap du lieu ma GetExportMOVEFINISH se tra ve cho ngay da chon.
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

    -- Moi lan xu ly thanh cong tao mot notification rieng.
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
        1,
        p_string,
        '',
        SYS_EXTRACT_UTC(SYSTIMESTAMP),
        'make_finished_flights_news4day',
        0,
        'FLIGHTDATE=' || TO_CHAR(v_selected_date, 'DD-MM-YYYY'),
        0
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

    -- Them hoac cap nhat declaration trong package spec.
    v_start_pos := INSTR(
        UPPER(v_new_spec),
        'PROCEDURE ' || c_procedure_name
    );
    IF v_start_pos > 0 THEN
        v_end_pos := INSTR(v_new_spec, ';', v_start_pos);
        v_new_spec := SUBSTR(v_new_spec, 1, v_start_pos - 1)
            || v_declaration
            || SUBSTR(v_new_spec, v_end_pos + 1);
    ELSE
        v_insert_pos := INSTR(UPPER(v_new_spec), 'END;', -1);
        IF v_insert_pos = 0 THEN
            RAISE_APPLICATION_ERROR(-20872, 'Khong tim thay END package spec');
        END IF;

        v_new_spec := SUBSTR(v_new_spec, 1, v_insert_pos - 1)
            || v_declaration || CHR(10)
            || SUBSTR(v_new_spec, v_insert_pos);
    END IF;

    -- Them hoac cap nhat implementation truoc SP_COPPYBRAVO.
    v_start_pos := INSTR(
        UPPER(v_new_body),
        'PROCEDURE ' || c_procedure_name
    );
    v_insert_pos := INSTR(UPPER(v_new_body), 'PROCEDURE SP_COPPYBRAVO');

    IF v_insert_pos = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20873,
            'Khong tim thay marker PROCEDURE SP_COPPYBRAVO'
        );
    END IF;

    IF v_start_pos > 0 THEN
        IF v_insert_pos <= v_start_pos THEN
            RAISE_APPLICATION_ERROR(-20874, 'Sai thu tu procedure trong body');
        END IF;

        v_new_body := SUBSTR(v_new_body, 1, v_start_pos - 1)
            || v_implementation
            || SUBSTR(v_new_body, v_insert_pos);
    ELSE
        v_new_body := SUBSTR(v_new_body, 1, v_insert_pos - 1)
            || v_implementation
            || SUBSTR(v_new_body, v_insert_pos);
    END IF;

    BEGIN
        execute_ddl(v_new_spec);
        execute_ddl(v_new_body);

        SELECT COUNT(*)
          INTO v_error_count
          FROM USER_ERRORS
         WHERE NAME = 'MAKE_FINISHED';

        IF v_error_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20875,
                'MAKE_FINISHED con ' || v_error_count || ' loi bien dich'
            );
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            restore_original_package;
            RAISE;
    END;

    DBMS_OUTPUT.PUT_LINE(
        'Deployed MAKE_FINISHED.make_finished_flights_news4day successfully.'
    );
END;
/

-- Verify object va chu ky API.
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
        RAISE_APPLICATION_ERROR(-20876, 'MAKE_FINISHED khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'MAKE_FINISHED'
       AND OBJECT_NAME = 'MAKE_FINISHED_FLIGHTS_NEWS4DAY'
       AND ARGUMENT_NAME IN ('P_STRING', 'P_DATE', 'P_OUT');

    IF v_count <> 3 THEN
        RAISE_APPLICATION_ERROR(-20877, 'Sai chu ky procedure NEWS4DAY');
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: package/body VALID, 3 arguments found.');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'MAKE_FINISHED'
 ORDER BY OBJECT_TYPE;

SELECT POSITION, ARGUMENT_NAME, IN_OUT, DATA_TYPE
  FROM USER_ARGUMENTS
 WHERE PACKAGE_NAME = 'MAKE_FINISHED'
   AND OBJECT_NAME = 'MAKE_FINISHED_FLIGHTS_NEWS4DAY'
 ORDER BY SEQUENCE;
