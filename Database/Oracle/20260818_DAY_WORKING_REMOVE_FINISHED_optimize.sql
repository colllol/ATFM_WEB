-- ============================================================================
-- Toi uu DAY_WORKING.REMOVE_FINISHED_FLIGHS_DELETE tren Oracle 12.1/19c.
-- Schema: ATFM. Chay bang DBeaver Execute SQL Script (Alt+X).
--
-- Noi dung:
--   1. Luu package body goc vao T_PACKAGE_SOURCE_BACKUP.
--   2. Chuyen REMOVE_CANCELED_FLIGHTS va REMOVE_FINISHED_FLIGHS_DELETE
--      sang SQL tinh, khong COMMIT ben trong function.
--   3. Sua hai procedure MAKE_FINISHED kiem tra ma tra ve va chi COMMIT mot lan.
--   4. REMOVE_FINISHED_FLIGHS_DELETE xu ly dung ngay DATE_FLY, khong tu tru 1.
--   5. Moc don du lieu cu dung ngay UTC hien tai - 62, khong phu thuoc DATE_FLY.
--   6. Tao index (FLIGHTDATE, MOVEFINISH) neu chua co index tuong duong.
--
-- Rollback: 20260818_DAY_WORKING_REMOVE_FINISHED_optimize_rollback.sql
-- ============================================================================

DECLARE
    v_count PLS_INTEGER;
BEGIN
    IF UPPER(USER) <> 'ATFM' THEN
        RAISE_APPLICATION_ERROR(-20901, 'Script chi duoc chay tren schema ATFM');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
       AND OBJECT_TYPE = 'PACKAGE BODY'
       AND STATUS = 'VALID';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20902,
            'DAY_WORKING hoac MAKE_FINISHED package body khong VALID'
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
       AND COLUMN_NAME IN
           (
               'FLIGHTDATE', 'MOVEFINISH', 'ISMESSAGEBIND',
               'ATD', 'ATA', 'OPER_ID', 'REGISTRATION',
               'CRAFT_TYPE', 'FROM_AIRP', 'TO_AIRP', 'FLIGHT_ID'
           );

    IF v_count <> 11 THEN
        RAISE_APPLICATION_ERROR(
            -20903,
            'T_DAY_FLIGHTS_GOINGON thieu cot bat buoc'
        );
    END IF;
END;
/

-- Luu source package body goc mot lan de rollback chinh xac.
BEGIN
    EXECUTE IMMEDIATE q'~
        CREATE TABLE T_PACKAGE_SOURCE_BACKUP
        (
            DEPLOYMENT_ID VARCHAR2(80 CHAR) NOT NULL,
            OBJECT_NAME  VARCHAR2(30 CHAR) NOT NULL,
            OBJECT_TYPE  VARCHAR2(30 CHAR) NOT NULL,
            DDL_TEXT     CLOB NOT NULL,
            BACKUP_DATE  DATE DEFAULT SYSDATE NOT NULL,
            CONSTRAINT PK_T_PACKAGE_SOURCE_BACKUP
                PRIMARY KEY (DEPLOYMENT_ID, OBJECT_NAME, OBJECT_TYPE)
        )
    ~';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE <> -955 THEN
            RAISE;
        END IF;
END;
/

DECLARE
    c_deployment_id CONSTANT VARCHAR2(80) :=
        '20260818_DAY_WORKING_REMOVE_FINISHED_V1';

    PROCEDURE backup_body(p_object_name IN VARCHAR2) IS
        v_count PLS_INTEGER;
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM T_PACKAGE_SOURCE_BACKUP
         WHERE DEPLOYMENT_ID = c_deployment_id
           AND OBJECT_NAME = UPPER(p_object_name)
           AND OBJECT_TYPE = 'PACKAGE_BODY';

        IF v_count = 0 THEN
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
                UPPER(p_object_name),
                'PACKAGE_BODY',
                DBMS_METADATA.GET_DDL(
                    'PACKAGE_BODY',
                    UPPER(p_object_name),
                    USER
                ),
                SYSDATE
            );
        END IF;
    END backup_body;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    backup_body('DAY_WORKING');
    backup_body('MAKE_FINISHED');
    COMMIT;
END;
/

DECLARE
    c_day_marker CONSTANT VARCHAR2(40) := 'ATFM_OPT_20260818_FINISHED';

    v_day_original   CLOB;
    v_make_original  CLOB;
    v_day_body       CLOB;
    v_make_body      CLOB;
    v_function       CLOB;
    v_procedure      CLOB;
    v_error_count    PLS_INTEGER;
    v_count          PLS_INTEGER;
    v_index_created  BOOLEAN := FALSE;

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

    FUNCTION replace_segment
    (
        p_source       IN CLOB,
        p_start_marker IN VARCHAR2,
        p_end_marker   IN VARCHAR2,
        p_replacement  IN CLOB
    ) RETURN CLOB
    IS
        v_upper_source CLOB;
        v_result       CLOB;
        v_start_pos    PLS_INTEGER;
        v_end_pos      PLS_INTEGER;
        v_prefix_len   PLS_INTEGER;
        v_replace_len  PLS_INTEGER;
        v_suffix_len   PLS_INTEGER;
        v_dest_offset  PLS_INTEGER := 1;
    BEGIN
        v_upper_source := UPPER(p_source);
        v_start_pos := DBMS_LOB.INSTR(
            v_upper_source,
            UPPER(p_start_marker),
            1,
            1
        );
        v_end_pos := DBMS_LOB.INSTR(
            v_upper_source,
            UPPER(p_end_marker),
            v_start_pos + 1,
            1
        );

        IF v_start_pos = 0 OR v_end_pos = 0 OR v_end_pos <= v_start_pos THEN
            RAISE_APPLICATION_ERROR(
                -20904,
                'Khong tim thay segment ' || p_start_marker ||
                ' -> ' || p_end_marker
            );
        END IF;

        DBMS_LOB.CREATETEMPORARY(v_result, TRUE);
        v_prefix_len := v_start_pos - 1;
        v_replace_len := DBMS_LOB.GETLENGTH(p_replacement);
        v_suffix_len := DBMS_LOB.GETLENGTH(p_source) - v_end_pos + 1;

        IF v_prefix_len > 0 THEN
            DBMS_LOB.COPY(
                v_result, p_source, v_prefix_len, v_dest_offset, 1
            );
            v_dest_offset := v_dest_offset + v_prefix_len;
        END IF;

        IF v_replace_len > 0 THEN
            DBMS_LOB.COPY(
                v_result, p_replacement, v_replace_len, v_dest_offset, 1
            );
            v_dest_offset := v_dest_offset + v_replace_len;
        END IF;

        IF v_suffix_len > 0 THEN
            DBMS_LOB.COPY(
                v_result, p_source, v_suffix_len, v_dest_offset, v_end_pos
            );
        END IF;

        RETURN v_result;
    END replace_segment;

    PROCEDURE restore_original_bodies IS
    BEGIN
        IF v_day_original IS NOT NULL THEN
            execute_clob_ddl(v_day_original);
        END IF;
        IF v_make_original IS NOT NULL THEN
            execute_clob_ddl(v_make_original);
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(
                'WARNING: Khong tu phuc hoi duoc package body: ' || SQLERRM
            );
    END restore_original_bodies;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    v_day_original := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY', 'DAY_WORKING', USER
    );
    v_make_original := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY', 'MAKE_FINISHED', USER
    );
    v_day_body := v_day_original;
    v_make_body := v_make_original;

    -- Function huy chuyen transaction-safe de FLIGHTS_SUMMARIZE khong COMMIT som.
    v_function := q'~
FUNCTION REMOVE_CANCELED_FLIGHTS
(
    DATE_FLY DATE,
    ERR      OUT VARCHAR2
) RETURN NUMBER
IS
    v_target_date DATE := TRUNC(DATE_FLY) - 1;
BEGIN
    SAVEPOINT REMOVE_CANCELED_BEGINNING;

    INSERT INTO T_CANCELED_FLIGHTS
    (
        FLIGHT_ID, FLIGHT_PK, PERM_ID, PERMNBR, OPER_ID,
        PERMTYPE, FLIGHT_TYPE, PURPOSE, CRAFT_ID, MTOW,
        VALIDHOURS, DATE_OLD, FLIGHTDATE, FLIGHTNBR,
        REGISTRATION, FROM_AIRP, TO_AIRP, ETD, ETA,
        ATD, ATA, VIA, CRAFT_TYPE, REMARK, STT
    )
    SELECT DISTINCT
        g.FLIGHT_ID, g.FLIGHT_PK, g.PERM_ID, g.PERMNBR, g.OPER_ID,
        g.PERMTYPE, g.FLIGHT_TYPE, g.PURPOSE, g.CRAFT_ID, g.MTOW,
        g.VALIDHOURS, g.DATE_OLD, g.FLIGHTDATE, g.FLIGHTNBR,
        g.REGISTRATION, g.FROM_AIRP, g.TO_AIRP, g.ETD, g.ETA,
        g.ATD, g.ATA, g.VIA, g.CRAFT_TYPE, g.REMARK, 1
    FROM T_DAY_FLIGHTS_GOINGON g
    WHERE g.MOVEFINISH = 0
      AND (g.LETTER_TYPE = 'CNL' OR UPPER(g.REMARK) LIKE '%CNL%')
      AND g.FLIGHT_ID IS NOT NULL
      AND g.FLIGHTDATE >= v_target_date
      AND g.FLIGHTDATE <  v_target_date + 1
      AND NOT EXISTS
          (
              SELECT 1
              FROM T_CANCELED_FLIGHTS c
              WHERE c.FLIGHT_ID = g.FLIGHT_ID
          );

    UPDATE T_DAY_FLIGHTS_GOINGON g
       SET g.MOVEFINISH = 1
     WHERE g.MOVEFINISH = 0
       AND (g.LETTER_TYPE = 'CNL' OR UPPER(g.REMARK) LIKE '%CNL%')
       AND g.FLIGHT_ID IS NOT NULL
       AND g.FLIGHTDATE >= v_target_date
       AND g.FLIGHTDATE <  v_target_date + 1;

    INSERT INTO T_LOG
    VALUES
    (
        'CANCELED FLIGHTS2026',
        SYSDATE,
        'STATIC SQL; FLIGHTDATE=' ||
        TO_CHAR(v_target_date, 'DD-MM-YYYY')
    );

    -- Khong COMMIT: procedure MAKE_FINISHED dieu khien transaction.
    RETURN 0;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK TO REMOVE_CANCELED_BEGINNING;
        ERR := NVL(ERR, '') ||
               'INSERT CANCELED FLIGHTS TO T_CANCELED_FLIGHTS TABLE: ' ||
               SQLERRM || CHR(10);
        RETURN 30;
END REMOVE_CANCELED_FLIGHTS;

--------------------------
~';

    v_day_body := replace_segment(
        v_day_body,
        'FUNCTION REMOVE_CANCELED_FLIGHTS',
        'FUNCTION REMOVE_NOTCOMPLETED_FLIGHTS',
        v_function
    );

    -- Function chuyen chuyen bay hoan thanh: SQL tinh, ngay truc tiep, khong COMMIT.
    v_function := q'~
FUNCTION REMOVE_FINISHED_FLIGHS_DELETE
(
    DATE_FLY DATE,
    ERR      OUT VARCHAR2
) RETURN NUMBER
IS
    -- ATFM_OPT_20260818_FINISHED
    v_target_date      DATE := TRUNC(DATE_FLY);
    v_retention_cutoff DATE :=
        TRUNC(CAST(SYS_EXTRACT_UTC(SYSTIMESTAMP) AS DATE)) - 62;
    v_error1           NUMBER := 0;
    v_error2           NUMBER := 0;
BEGIN
    IF DATE_FLY IS NULL THEN
        ERR := NVL(ERR, '') || 'DATE_FLY IS REQUIRED' || CHR(10);
        RETURN 10;
    END IF;

    SAVEPOINT FINISHED_MAIN_BEGINNING;
    BEGIN
        INSERT INTO T_FINISHED_FLIGHTS
        (
            FLIGHT_ID, FLIGHT_PK, PERM_ID, PERMNBR, OPER_ID,
            PERMTYPE, FLIGHT_TYPE, PURPOSE, CRAFT_ID, MTOW,
            VALIDHOURS, DATE_OLD, FLIGHTDATE, FLIGHTNBR,
            REGISTRATION, FROM_AIRP, TO_AIRP, ETD, ETA,
            ATD, ATA, VIA, FPL_VIA, CRAFT_TYPE, REMARK,
            REAL_CRAFT_TYPE, ISMESSAGEBIND
        )
        SELECT
            g.FLIGHT_ID,
            g.FLIGHT_PK,
            g.PERM_ID,
            g.PERMNBR,
            g.OPER_ID,
            g.PERMTYPE,
            g.FLIGHT_TYPE,
            g.PURPOSE,
            g.CRAFT_ID,
            g.MTOW,
            g.VALIDHOURS,
            g.DATE_OLD,
            g.FLIGHTDATE,
            g.FLIGHTNBR,
            g.REGISTRATION,
            g.FROM_AIRP,
            g.TO_AIRP,
            DAY_WORKING.ADD_DAY_TO_TIME(g.FLIGHTDATE, g.ETD),
            DAY_WORKING.ADD_DAY_TO_TIME(g.FLIGHTDATE, g.ETA),
            g.ATD,
            g.ATA,
            DAY_WORKING.ROUTE_NAME(g.FROM_AIRP, g.TO_AIRP),
            g.ROUTE_TT,
            g.CRAFT_TYPE,
            g.REMARK,
            g.CRAFT_TYPE,
            CASE
                WHEN
                    (g.ATA IS NOT NULL AND g.ATD IS NOT NULL)
                    OR
                    (
                        g.OPER_ID IN
                        ('BAV','HVN','VJC','PIC','VFC','HAI','VAG')
                        AND g.FROM_AIRP LIKE 'VV%'
                        AND g.ATD IS NOT NULL
                        AND g.TO_AIRP NOT LIKE 'VV%'
                    )
                THEN 1
                ELSE g.ISMESSAGEBIND
            END
        FROM T_DAY_FLIGHTS_GOINGON g
        WHERE g.FLIGHTDATE >= v_target_date
          AND g.FLIGHTDATE <  v_target_date + 1
          AND g.ISMESSAGEBIND IN (0, 1, 2)
          AND g.MOVEFINISH = 0
          AND g.OPER_ID IS NOT NULL
          AND g.REGISTRATION IS NOT NULL
          AND g.CRAFT_TYPE IS NOT NULL
          AND
          (
              (g.ATA IS NOT NULL AND g.ATD IS NOT NULL)
              OR
              (
                  g.OPER_ID IN
                  ('BAV','HVN','VJC','PIC','VFC','HAI','VAG')
                  AND g.FROM_AIRP LIKE 'VV%'
                  AND g.ATD IS NOT NULL
                  AND g.TO_AIRP NOT LIKE 'VV%'
              )
              OR
              (
                  g.OPER_ID NOT IN
                  ('BAV','HVN','VJC','PIC','VFC','HAI','VAG')
                  AND
                  (
                      (g.FROM_AIRP LIKE 'VV%' AND g.ATD IS NOT NULL)
                      OR
                      (g.TO_AIRP LIKE 'VV%' AND g.ATA IS NOT NULL)
                  )
              )
          )
          AND NOT EXISTS
          (
              SELECT 1
              FROM T_FINISHED_FLIGHTS f
              WHERE f.FLIGHT_ID = g.FLIGHT_ID
          );

        UPDATE T_DAY_FLIGHTS_GOINGON g
           SET g.MOVEFINISH = 1
         WHERE g.FLIGHTDATE >= v_target_date
           AND g.FLIGHTDATE <  v_target_date + 1
           AND g.ISMESSAGEBIND IN (0, 1, 2)
           AND g.MOVEFINISH = 0
           AND g.OPER_ID IS NOT NULL
           AND g.REGISTRATION IS NOT NULL
           AND g.CRAFT_TYPE IS NOT NULL
           AND
           (
               (g.ATA IS NOT NULL AND g.ATD IS NOT NULL)
               OR
               (
                   g.OPER_ID IN
                   ('BAV','HVN','VJC','PIC','VFC','HAI','VAG')
                   AND g.FROM_AIRP LIKE 'VV%'
                   AND g.ATD IS NOT NULL
                   AND g.TO_AIRP NOT LIKE 'VV%'
               )
               OR
               (
                   g.OPER_ID NOT IN
                   ('BAV','HVN','VJC','PIC','VFC','HAI','VAG')
                   AND
                   (
                       (g.FROM_AIRP LIKE 'VV%' AND g.ATD IS NOT NULL)
                       OR
                       (g.TO_AIRP LIKE 'VV%' AND g.ATA IS NOT NULL)
                   )
               )
           );

        UPDATE T_FINISHED_FLIGHTS f
           SET f.PERMTYPE = 'LD'
         WHERE f.FLIGHTDATE >= v_target_date
           AND f.FLIGHTDATE <  v_target_date + 1
           AND UPPER(TRIM(f.PERMTYPE)) IN ('OTH', 'ALL', 'LD');

        -- Sua loi cu: lan thu hai phai chuan hoa OF/of thanh O/F.
        UPDATE T_FINISHED_FLIGHTS f
           SET f.PERMTYPE = 'O/F'
         WHERE f.FLIGHTDATE >= v_target_date
           AND f.FLIGHTDATE <  v_target_date + 1
           AND UPPER(TRIM(f.PERMTYPE)) = 'OF';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK TO FINISHED_MAIN_BEGINNING;
            ERR := NVL(ERR, '') ||
                   'INSERT FINISHED FLIGHTS TO T_FINISHED_FLIGHTS TABLE: ' ||
                   SQLERRM || CHR(10);
            v_error1 := 1;
    END;

    IF v_error1 = 0 THEN
        SAVEPOINT FINISHED_CLEANUP_BEGINNING;
        BEGIN
            -- Moc retention doc lap ngay duoc chon, tinh theo ngay UTC hien tai.
            DELETE FROM T_DAY_FLIGHTS_GOINGON g
             WHERE g.FLIGHTDATE <= v_retention_cutoff;
        EXCEPTION
            WHEN OTHERS THEN
                ROLLBACK TO FINISHED_CLEANUP_BEGINNING;
                ERR := NVL(ERR, '') ||
                       'REMOVE OLD T_DAY_FLIGHTS_GOINGON DATA: ' ||
                       SQLERRM || CHR(10);
                v_error2 := 2;
        END;
    END IF;

    -- Khong COMMIT: procedure MAKE_FINISHED commit/rollback toan bo.
    RETURN v_error1 * 10 + v_error2;
END REMOVE_FINISHED_FLIGHS_DELETE;

-----------------------
~';

    v_day_body := replace_segment(
        v_day_body,
        'FUNCTION REMOVE_FINISHED_FLIGHS_DELETE',
        'FUNCTION REMOVE_FINISHED_FLIGHS_N',
        v_function
    );

    -- Procedure cu: van xu ly ngay UTC hom truoc, nhung kiem tra ma loi.
    v_procedure := q'~
procedure make_finished_flights_news
(
    p_string IN VARCHAR2,
    p_out    OUT NUMBER
)
IS
    v_error_text   VARCHAR2(4000);
    v_summary_code NUMBER;
    v_finish_code  NUMBER;
    v_utc_today    DATE :=
        TRUNC(CAST(SYS_EXTRACT_UTC(SYSTIMESTAMP) AS DATE));
BEGIN
    p_out := -1;

    v_summary_code := DAY_WORKING.FLIGHTS_SUMMARIZE(
        v_utc_today,
        v_error_text
    );

    IF NVL(v_summary_code, -1) <> 0 THEN
        RAISE_APPLICATION_ERROR(
            -20011,
            'FLIGHTS_SUMMARIZE failed: code=' || v_summary_code ||
            '; ' || SUBSTR(v_error_text, 1, 1000)
        );
    END IF;

    -- Function toi uu nhan ngay can xu ly truc tiep.
    v_finish_code := DAY_WORKING.REMOVE_FINISHED_FLIGHS_DELETE(
        v_utc_today - 1,
        v_error_text
    );

    IF NVL(v_finish_code, -1) <> 0 THEN
        RAISE_APPLICATION_ERROR(
            -20012,
            'REMOVE_FINISHED failed: code=' || v_finish_code ||
            '; ' || SUBSTR(v_error_text, 1, 1000)
        );
    END IF;

    INSERT INTO T_ACTIONHISTORY
    (
        USERID, FULLNAME, HOSTIP, DATEMODIFY,
        ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
    )
    VALUES
    (
        1, p_string, '', SYS_EXTRACT_UTC(SYSTIMESTAMP),
        'make_finished_flights', 0,
        'FLIGHTDATE=' || TO_CHAR(v_utc_today - 1, 'DD-MM-YYYY'), 0
    );

    COMMIT;
    p_out := 1;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        p_out := -1;
        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'MAKE_FINISHED.make_finished_flights_news',
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
END make_finished_flights_news;


~';

    v_make_body := replace_segment(
        v_make_body,
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS(',
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS4DAY',
        v_procedure
    );

    -- Procedure theo ngay: summary van nhan D+1, finished nhan dung ngay D.
    v_procedure := q'~
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

    -- FLIGHTS_SUMMARIZE hien loc DATE_FLY - 1.
    v_summary_code := DAY_WORKING.FLIGHTS_SUMMARIZE(
        v_selected_date + 1,
        v_error_text
    );

    IF NVL(v_summary_code, -1) <> 0 THEN
        RAISE_APPLICATION_ERROR(
            -20011,
            'FLIGHTS_SUMMARIZE failed: code=' || v_summary_code ||
            '; ' || SUBSTR(v_error_text, 1, 1000)
        );
    END IF;

    -- Function toi uu nhan ngay can xu ly truc tiep.
    v_finish_code := DAY_WORKING.REMOVE_FINISHED_FLIGHS_DELETE(
        v_selected_date,
        v_error_text
    );

    IF NVL(v_finish_code, -1) <> 0 THEN
        RAISE_APPLICATION_ERROR(
            -20012,
            'REMOVE_FINISHED failed: code=' || v_finish_code ||
            '; ' || SUBSTR(v_error_text, 1, 1000)
        );
    END IF;

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

    v_make_body := replace_segment(
        v_make_body,
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS4DAY',
        'PROCEDURE SP_COPPYBRAVO',
        v_procedure
    );

    BEGIN
        execute_clob_ddl(v_day_body);
        execute_clob_ddl(v_make_body);

        SELECT COUNT(*)
          INTO v_error_count
          FROM USER_ERRORS
         WHERE NAME IN ('DAY_WORKING', 'MAKE_FINISHED');

        IF v_error_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20905,
                'Package con ' || v_error_count || ' loi bien dich'
            );
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            restore_original_bodies;
            RAISE;
    END;

    -- Tao index khi chua co index bat dau bang (FLIGHTDATE, MOVEFINISH).
    SELECT COUNT(*)
      INTO v_count
      FROM
      (
          SELECT ic.INDEX_NAME
            FROM USER_IND_COLUMNS ic
           WHERE ic.TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
           GROUP BY ic.INDEX_NAME
          HAVING MAX(
                     CASE
                         WHEN ic.COLUMN_POSITION = 1
                          AND ic.COLUMN_NAME = 'FLIGHTDATE'
                         THEN 1 ELSE 0
                     END
                 ) = 1
             AND MAX(
                     CASE
                         WHEN ic.COLUMN_POSITION = 2
                          AND ic.COLUMN_NAME = 'MOVEFINISH'
                         THEN 1 ELSE 0
                     END
                 ) = 1
      );

    IF v_count = 0 THEN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_INDEXES
         WHERE INDEX_NAME = 'IX_TDFG_FLIGHTDATE_MOVEFINISH';

        IF v_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20906,
                'Index name IX_TDFG_FLIGHTDATE_MOVEFINISH da ton tai'
            );
        END IF;

        EXECUTE IMMEDIATE q'~
            CREATE INDEX IX_TDFG_FLIGHTDATE_MOVEFINISH
            ON T_DAY_FLIGHTS_GOINGON (FLIGHTDATE, MOVEFINISH)
            ONLINE
        ~';
        v_index_created := TRUE;
        DBMS_STATS.GATHER_INDEX_STATS(
            USER,
            'IX_TDFG_FLIGHTDATE_MOVEFINISH'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'DEPLOY OK: DAY_WORKING/MAKE_FINISHED transaction-safe.'
    );
    IF v_index_created THEN
        DBMS_OUTPUT.PUT_LINE(
            'CREATED INDEX IX_TDFG_FLIGHTDATE_MOVEFINISH.'
        );
    ELSE
        DBMS_OUTPUT.PUT_LINE(
            'INDEX OK: da co index (FLIGHTDATE, MOVEFINISH).'
        );
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        IF v_index_created THEN
            BEGIN
                EXECUTE IMMEDIATE 'DROP INDEX IX_TDFG_FLIGHTDATE_MOVEFINISH';
            EXCEPTION
                WHEN OTHERS THEN
                    NULL;
            END;
        END IF;
        RAISE;
END;
/

-- Verify object, marker, chu ky va index.
DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 4 THEN
        RAISE_APPLICATION_ERROR(-20907, 'Package/spec/body khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_SOURCE
     WHERE NAME = 'DAY_WORKING'
       AND TYPE = 'PACKAGE BODY'
       AND TEXT LIKE '%ATFM_OPT_20260818_FINISHED%';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20908, 'Khong thay marker function toi uu');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'DAY_WORKING'
       AND OBJECT_NAME = 'REMOVE_FINISHED_FLIGHS_DELETE'
       AND ARGUMENT_NAME IN ('DATE_FLY', 'ERR');

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(-20909, 'Sai chu ky function finished');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM
      (
          SELECT ic.INDEX_NAME
            FROM USER_IND_COLUMNS ic
            JOIN USER_INDEXES ix
              ON ix.INDEX_NAME = ic.INDEX_NAME
           WHERE ic.TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
             AND ix.STATUS = 'VALID'
           GROUP BY ic.INDEX_NAME
          HAVING MAX(
                     CASE
                         WHEN ic.COLUMN_POSITION = 1
                          AND ic.COLUMN_NAME = 'FLIGHTDATE'
                         THEN 1 ELSE 0
                     END
                 ) = 1
             AND MAX(
                     CASE
                         WHEN ic.COLUMN_POSITION = 2
                          AND ic.COLUMN_NAME = 'MOVEFINISH'
                         THEN 1 ELSE 0
                     END
                 ) = 1
      );

    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20910, 'Thieu index FLIGHTDATE/MOVEFINISH');
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: package VALID, function marker/arguments/index hop le.'
    );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT INDEX_NAME, STATUS, LAST_ANALYZED
  FROM USER_INDEXES
 WHERE TABLE_NAME = 'T_DAY_FLIGHTS_GOINGON'
   AND INDEX_NAME = 'IX_TDFG_FLIGHTDATE_MOVEFINISH';
