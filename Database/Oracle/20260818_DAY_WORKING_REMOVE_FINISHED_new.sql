-- ============================================================================
-- Tach nhanh xu ly moi khoi luong cu cua DAY_WORKING/MAKE_FINISHED.
-- Oracle 12.1 gioi han identifier 30 ky tu, vi vay ten yeu cau
-- REMOVE_FINISHED_FLIGHS_DELETE_NEW (33 ky tu) duoc rut gon thanh:
--   REMOVE_FINISHED_FLIGHS_DEL_NEW (30 ky tu)
--
-- Ket qua sau deploy:
--   1. Khoi phuc nguyen ban cac ham/procedure cu tu backup V1:
--      - DAY_WORKING.REMOVE_FINISHED_FLIGHS_DELETE
--      - DAY_WORKING.REMOVE_CANCELED_FLIGHTS
--      - DAY_WORKING.FLIGHTS_SUMMARIZE
--      - MAKE_FINISHED.make_finished_flights_news
--   2. Tao nhanh moi transaction-safe:
--      - DAY_WORKING.REMOVE_FINISHED_FLIGHS_DEL_NEW
--      - DAY_WORKING.REMOVE_CANCELED_FLIGHTS_NEW
--      - DAY_WORKING.FLIGHTS_SUMMARIZE_NEW
--   3. Chi MAKE_FINISHED.make_finished_flights_news4day goi nhanh moi.
--   4. Khong xoa index IX_TDFG_FLIGHTDATE_MOVEFINISH da tao truoc do.
--
-- Chay bang DBeaver: Execute SQL Script (Alt+X), schema ATFM.
-- Rollback: 20260818_DAY_WORKING_REMOVE_FINISHED_new_rollback.sql
-- ============================================================================

DECLARE
    c_legacy_backup CONSTANT VARCHAR2(80) :=
        '20260818_DAY_WORKING_REMOVE_FINISHED_V1';
    v_count PLS_INTEGER;
BEGIN
    IF UPPER(USER) <> 'ATFM' THEN
        RAISE_APPLICATION_ERROR(-20931, 'Script chi duoc chay tren schema ATFM');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS = 'VALID';

    IF v_count <> 4 THEN
        RAISE_APPLICATION_ERROR(
            -20932,
            'DAY_WORKING hoac MAKE_FINISHED dang co object khong VALID'
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_PACKAGE_SOURCE_BACKUP';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20933, 'Khong co T_PACKAGE_SOURCE_BACKUP');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_legacy_backup
       AND OBJECT_TYPE = 'PACKAGE_BODY'
       AND OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED');

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20934,
            'Thieu source goc cua DAY_WORKING/MAKE_FINISHED trong backup V1'
        );
    END IF;
END;
/

-- Luu trang thai ngay truoc khi tach nhanh de rollback chinh xac.
DECLARE
    c_deployment_id CONSTANT VARCHAR2(80) :=
        '20260818_DAY_WORKING_REMOVE_FINISHED_NEW_V2';

    PROCEDURE backup_object
    (
        p_object_name IN VARCHAR2,
        p_object_type IN VARCHAR2,
        p_metadata_type IN VARCHAR2
    ) IS
        v_count PLS_INTEGER;
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM T_PACKAGE_SOURCE_BACKUP
         WHERE DEPLOYMENT_ID = c_deployment_id
           AND OBJECT_NAME = UPPER(p_object_name)
           AND OBJECT_TYPE = UPPER(p_object_type);

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
                UPPER(p_object_type),
                DBMS_METADATA.GET_DDL(
                    UPPER(p_metadata_type),
                    UPPER(p_object_name),
                    USER
                ),
                SYSDATE
            );
        END IF;
    END backup_object;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    backup_object('DAY_WORKING', 'PACKAGE_SPEC', 'PACKAGE_SPEC');
    backup_object('DAY_WORKING', 'PACKAGE_BODY', 'PACKAGE_BODY');
    backup_object('MAKE_FINISHED', 'PACKAGE_BODY', 'PACKAGE_BODY');
    COMMIT;
END;
/

DECLARE
    c_legacy_backup CONSTANT VARCHAR2(80) :=
        '20260818_DAY_WORKING_REMOVE_FINISHED_V1';

    v_day_spec_current CLOB;
    v_day_body_current CLOB;
    v_make_body_current CLOB;
    v_day_body_legacy  CLOB;
    v_make_body_legacy CLOB;

    v_day_spec_new CLOB;
    v_day_body_new CLOB;
    v_make_body_new CLOB;

    v_old_cancel    CLOB;
    v_old_finished  CLOB;
    v_old_summary   CLOB;
    v_old_make_news CLOB;

    v_new_cancel    CLOB;
    v_new_finished  CLOB;
    v_new_summary   CLOB;
    v_new_make_day  CLOB;
    v_declarations  CLOB;
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

    FUNCTION extract_segment
    (
        p_source       IN CLOB,
        p_start_marker IN VARCHAR2,
        p_end_marker   IN VARCHAR2
    ) RETURN CLOB
    IS
        v_upper_source CLOB;
        v_result       CLOB;
        v_start_pos    PLS_INTEGER;
        v_end_pos      PLS_INTEGER;
        v_length       PLS_INTEGER;
    BEGIN
        v_upper_source := UPPER(p_source);
        v_start_pos := DBMS_LOB.INSTR(
            v_upper_source, UPPER(p_start_marker), 1, 1
        );
        v_end_pos := DBMS_LOB.INSTR(
            v_upper_source,
            UPPER(p_end_marker),
            v_start_pos + LENGTH(p_start_marker),
            1
        );

        IF v_start_pos = 0 OR v_end_pos = 0 OR v_end_pos <= v_start_pos THEN
            RAISE_APPLICATION_ERROR(
                -20935,
                'Khong tim thay segment ' || p_start_marker ||
                ' -> ' || p_end_marker
            );
        END IF;

        v_length := v_end_pos - v_start_pos;
        DBMS_LOB.CREATETEMPORARY(v_result, TRUE);
        DBMS_LOB.COPY(v_result, p_source, v_length, 1, v_start_pos);
        RETURN v_result;
    END extract_segment;

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
            v_upper_source, UPPER(p_start_marker), 1, 1
        );
        v_end_pos := DBMS_LOB.INSTR(
            v_upper_source,
            UPPER(p_end_marker),
            v_start_pos + LENGTH(p_start_marker),
            1
        );

        IF v_start_pos = 0 OR v_end_pos = 0 OR v_end_pos <= v_start_pos THEN
            RAISE_APPLICATION_ERROR(
                -20936,
                'Khong tim thay segment ' || p_start_marker ||
                ' -> ' || p_end_marker
            );
        END IF;

        DBMS_LOB.CREATETEMPORARY(v_result, TRUE);
        v_prefix_len := v_start_pos - 1;
        v_replace_len := DBMS_LOB.GETLENGTH(p_replacement);
        v_suffix_len := DBMS_LOB.GETLENGTH(p_source) - v_end_pos + 1;

        IF v_prefix_len > 0 THEN
            DBMS_LOB.COPY(v_result, p_source, v_prefix_len, 1, 1);
            v_dest_offset := v_prefix_len + 1;
        END IF;

        IF v_replace_len > 0 THEN
            DBMS_LOB.COPY(
                v_result,
                p_replacement,
                v_replace_len,
                v_dest_offset,
                1
            );
            v_dest_offset := v_dest_offset + v_replace_len;
        END IF;

        IF v_suffix_len > 0 THEN
            DBMS_LOB.COPY(
                v_result,
                p_source,
                v_suffix_len,
                v_dest_offset,
                v_end_pos
            );
        END IF;

        RETURN v_result;
    END replace_segment;

    FUNCTION insert_before
    (
        p_source    IN CLOB,
        p_marker    IN VARCHAR2,
        p_insertion IN CLOB
    ) RETURN CLOB
    IS
        v_upper_source CLOB;
        v_result       CLOB;
        v_marker_pos   PLS_INTEGER;
        v_prefix_len   PLS_INTEGER;
        v_insert_len   PLS_INTEGER;
        v_suffix_len   PLS_INTEGER;
        v_dest_offset  PLS_INTEGER := 1;
    BEGIN
        v_upper_source := UPPER(p_source);
        v_marker_pos := DBMS_LOB.INSTR(
            v_upper_source, UPPER(p_marker), 1, 1
        );

        IF v_marker_pos = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20937,
                'Khong tim thay marker ' || p_marker
            );
        END IF;

        DBMS_LOB.CREATETEMPORARY(v_result, TRUE);
        v_prefix_len := v_marker_pos - 1;
        v_insert_len := DBMS_LOB.GETLENGTH(p_insertion);
        v_suffix_len := DBMS_LOB.GETLENGTH(p_source) - v_marker_pos + 1;

        IF v_prefix_len > 0 THEN
            DBMS_LOB.COPY(v_result, p_source, v_prefix_len, 1, 1);
            v_dest_offset := v_prefix_len + 1;
        END IF;

        DBMS_LOB.COPY(
            v_result, p_insertion, v_insert_len, v_dest_offset, 1
        );
        v_dest_offset := v_dest_offset + v_insert_len;

        DBMS_LOB.COPY(
            v_result,
            p_source,
            v_suffix_len,
            v_dest_offset,
            v_marker_pos
        );
        RETURN v_result;
    END insert_before;

    PROCEDURE restore_current IS
    BEGIN
        IF v_day_spec_current IS NOT NULL THEN
            execute_clob_ddl(v_day_spec_current);
        END IF;
        IF v_day_body_current IS NOT NULL THEN
            execute_clob_ddl(v_day_body_current);
        END IF;
        IF v_make_body_current IS NOT NULL THEN
            execute_clob_ddl(v_make_body_current);
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(
                'WARNING: Khong tu phuc hoi duoc source truoc deploy: ' ||
                SQLERRM
            );
    END restore_current;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    v_day_spec_current := DBMS_METADATA.GET_DDL(
        'PACKAGE_SPEC', 'DAY_WORKING', USER
    );
    v_day_body_current := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY', 'DAY_WORKING', USER
    );
    v_make_body_current := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY', 'MAKE_FINISHED', USER
    );

    SELECT DDL_TEXT
      INTO v_day_body_legacy
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_legacy_backup
       AND OBJECT_NAME = 'DAY_WORKING'
       AND OBJECT_TYPE = 'PACKAGE_BODY';

    SELECT DDL_TEXT
      INTO v_make_body_legacy
      FROM T_PACKAGE_SOURCE_BACKUP
     WHERE DEPLOYMENT_ID = c_legacy_backup
       AND OBJECT_NAME = 'MAKE_FINISHED'
       AND OBJECT_TYPE = 'PACKAGE_BODY';

    v_old_cancel := extract_segment(
        v_day_body_legacy,
        'FUNCTION REMOVE_CANCELED_FLIGHTS',
        'FUNCTION REMOVE_NOTCOMPLETED_FLIGHTS'
    );
    v_old_finished := extract_segment(
        v_day_body_legacy,
        'FUNCTION REMOVE_FINISHED_FLIGHS_DELETE',
        'FUNCTION REMOVE_FINISHED_FLIGHS_N'
    );
    v_old_summary := extract_segment(
        v_day_body_legacy,
        'FUNCTION FLIGHTS_SUMMARIZE',
        'FUNCTION INCREMENT_FLIGHDATE'
    );
    v_old_make_news := extract_segment(
        v_make_body_legacy,
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS',
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS4DAY'
    );

    v_declarations := q'~
    -- Nhanh moi theo ngay, khong thay doi cac ham cu.
    FUNCTION REMOVE_FINISHED_FLIGHS_DEL_NEW
    (
        DATE_FLY DATE,
        ERR OUT VARCHAR2
    ) RETURN NUMBER;

    FUNCTION REMOVE_CANCELED_FLIGHTS_NEW
    (
        DATE_FLY DATE,
        ERR OUT VARCHAR2
    ) RETURN NUMBER;

    FUNCTION FLIGHTS_SUMMARIZE_NEW
    (
        DATE_FLY DATE,
        ERR OUT VARCHAR2
    ) RETURN NUMBER;

~';

    v_day_spec_new := v_day_spec_current;
    IF DBMS_LOB.INSTR(
           UPPER(v_day_spec_new),
           'REMOVE_FINISHED_FLIGHS_DEL_NEW',
           1,
           1
       ) = 0 THEN
        v_day_spec_new := insert_before(
            v_day_spec_new,
            'END DAY_WORKING;',
            v_declarations
        );
    END IF;

    v_new_cancel := q'~
FUNCTION REMOVE_CANCELED_FLIGHTS_NEW
(
    DATE_FLY DATE,
    ERR      OUT VARCHAR2
) RETURN NUMBER
IS
    v_target_date DATE;
BEGIN
    IF DATE_FLY IS NULL THEN
        ERR := NVL(ERR, '') || 'DATE_FLY IS REQUIRED' || CHR(10);
        RETURN 30;
    END IF;

    v_target_date := TRUNC(DATE_FLY);
    SAVEPOINT REMOVE_CANCELED_NEW_BEGIN;

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
        'CANCELED FLIGHTS NEW',
        SYSDATE,
        'FLIGHTDATE=' || TO_CHAR(v_target_date, 'DD-MM-YYYY')
    );

    RETURN 0;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK TO REMOVE_CANCELED_NEW_BEGIN;
        ERR := NVL(ERR, '') ||
               'INSERT CANCELED FLIGHTS NEW: ' || SQLERRM || CHR(10);
        RETURN 30;
END REMOVE_CANCELED_FLIGHTS_NEW;

--------------------------
~';

    v_new_finished := q'~
FUNCTION REMOVE_FINISHED_FLIGHS_DEL_NEW
(
    DATE_FLY DATE,
    ERR      OUT VARCHAR2
) RETURN NUMBER
IS
    v_target_date      DATE;
    v_retention_cutoff DATE :=
        TRUNC(CAST(SYS_EXTRACT_UTC(SYSTIMESTAMP) AS DATE)) - 62;
    v_error1           NUMBER := 0;
    v_error2           NUMBER := 0;
BEGIN
    IF DATE_FLY IS NULL THEN
        ERR := NVL(ERR, '') || 'DATE_FLY IS REQUIRED' || CHR(10);
        RETURN 10;
    END IF;

    v_target_date := TRUNC(DATE_FLY);
    SAVEPOINT FINISHED_NEW_MAIN_BEGIN;

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

        UPDATE T_FINISHED_FLIGHTS f
           SET f.PERMTYPE = 'O/F'
         WHERE f.FLIGHTDATE >= v_target_date
           AND f.FLIGHTDATE <  v_target_date + 1
           AND UPPER(TRIM(f.PERMTYPE)) = 'OF';
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK TO FINISHED_NEW_MAIN_BEGIN;
            ERR := NVL(ERR, '') ||
                   'INSERT FINISHED FLIGHTS NEW: ' || SQLERRM || CHR(10);
            v_error1 := 1;
    END;

    IF v_error1 = 0 THEN
        SAVEPOINT FINISHED_NEW_CLEANUP_BEGIN;
        BEGIN
            DELETE FROM T_DAY_FLIGHTS_GOINGON g
             WHERE g.FLIGHTDATE <= v_retention_cutoff;
        EXCEPTION
            WHEN OTHERS THEN
                ROLLBACK TO FINISHED_NEW_CLEANUP_BEGIN;
                ERR := NVL(ERR, '') ||
                       'REMOVE OLD GOINGON DATA NEW: ' || SQLERRM || CHR(10);
                v_error2 := 2;
        END;
    END IF;

    RETURN v_error1 * 10 + v_error2;
END REMOVE_FINISHED_FLIGHS_DEL_NEW;

-----------------------
~';

    v_new_summary := q'~
FUNCTION FLIGHTS_SUMMARIZE_NEW
(
    DATE_FLY DATE,
    ERR      OUT VARCHAR2
) RETURN NUMBER
IS
    v_cancel_code NUMBER := 0;
    v_result      NUMBER := 0;
BEGIN
    v_cancel_code := REMOVE_CANCELED_FLIGHTS_NEW(DATE_FLY, ERR);
    v_result := v_cancel_code * 100;

    IF v_result <> 0 THEN
        ERR := 'ERRORS OCCUR WHEN:' || CHR(10) || ERR;
    END IF;

    RETURN v_result;
END FLIGHTS_SUMMARIZE_NEW;

--------------------------
~';

    v_new_make_day := q'~
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

    -- Khoi phuc ham cu va chen ham NEW ngay sau ham cu.
    v_day_body_new := replace_segment(
        v_day_body_current,
        'FUNCTION REMOVE_CANCELED_FLIGHTS',
        'FUNCTION REMOVE_NOTCOMPLETED_FLIGHTS',
        v_old_cancel || v_new_cancel
    );

    v_day_body_new := replace_segment(
        v_day_body_new,
        'FUNCTION REMOVE_FINISHED_FLIGHS_DELETE',
        'FUNCTION REMOVE_FINISHED_FLIGHS_N',
        v_old_finished || v_new_finished
    );

    v_day_body_new := replace_segment(
        v_day_body_new,
        'FUNCTION FLIGHTS_SUMMARIZE',
        'FUNCTION INCREMENT_FLIGHDATE',
        v_old_summary || v_new_summary
    );

    -- Tra procedure cu ve source backup, chi procedure 4day goi nhanh NEW.
    v_make_body_new := replace_segment(
        v_make_body_current,
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS',
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS4DAY',
        v_old_make_news
    );

    v_make_body_new := replace_segment(
        v_make_body_new,
        'PROCEDURE MAKE_FINISHED_FLIGHTS_NEWS4DAY',
        'PROCEDURE SP_COPPYBRAVO',
        v_new_make_day
    );

    BEGIN
        execute_clob_ddl(v_day_spec_new);
        execute_clob_ddl(v_day_body_new);
        execute_clob_ddl(v_make_body_new);

        SELECT COUNT(*)
          INTO v_error_count
          FROM USER_ERRORS
         WHERE NAME IN ('DAY_WORKING', 'MAKE_FINISHED');

        IF v_error_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20938,
                'Package con ' || v_error_count || ' loi bien dich'
            );
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            restore_current;
            RAISE;
    END;

    DBMS_OUTPUT.PUT_LINE(
        'DEPLOY OK: legacy restored; NEW functions added; 4day uses NEW branch.'
    );
END;
/

-- Verify package, chu ky va huong goi.
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
        RAISE_APPLICATION_ERROR(-20939, 'Co package/package body khong VALID');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_PROCEDURES
     WHERE OBJECT_NAME = 'DAY_WORKING'
       AND PROCEDURE_NAME IN
           (
               'REMOVE_FINISHED_FLIGHS_DEL_NEW',
               'REMOVE_CANCELED_FLIGHTS_NEW',
               'FLIGHTS_SUMMARIZE_NEW'
           );

    IF v_count <> 3 THEN
        RAISE_APPLICATION_ERROR(-20940, 'Thieu function NEW trong DAY_WORKING');
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_SOURCE
     WHERE NAME = 'MAKE_FINISHED'
       AND TYPE = 'PACKAGE BODY'
       AND
       (
           UPPER(TEXT) LIKE '%FLIGHTS_SUMMARIZE_NEW%'
           OR UPPER(TEXT) LIKE '%REMOVE_FINISHED_FLIGHS_DEL_NEW%'
       );

    IF v_count < 2 THEN
        RAISE_APPLICATION_ERROR(-20941, 'NEWS4DAY chua goi du nhanh NEW');
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: 4 objects VALID, 3 NEW functions, NEWS4DAY calls NEW.'
    );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('DAY_WORKING', 'MAKE_FINISHED')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT PROCEDURE_NAME
  FROM USER_PROCEDURES
 WHERE OBJECT_NAME = 'DAY_WORKING'
   AND PROCEDURE_NAME LIKE '%NEW%'
 ORDER BY PROCEDURE_NAME;
