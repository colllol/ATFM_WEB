-- ============================================================================
-- Database deployment for:
--   1. Permission/Edit_PermNo.aspx
--   2. Permission/Edit_PermNO4Mail.aspx
--
-- Compatible with Oracle 12.1+ and Oracle 19c.
-- Run as schema ATFM with DBeaver Execute SQL Script (Alt+X), SQLcl or SQL*Plus.
--
-- Changes:
--   - Add (PERM_ID, ID) index when no equivalent index exists.
--   - Replace PERM_PKG.PermDetail_No_GetBySearch implementation without
--     changing its public signature.
--   - Remove duplicate count/scalar lookup work and fix page 2+ pagination.
--   - Create/replace PERM_NO_APPROVAL_PKG for the Accepted action.
--   - Back up all replaced DDL into T_ATFM_DEPLOY_DDL_BACKUP.
--
-- This script does not accept a permission and does not modify business data.
-- ============================================================================

-- 1. Precheck required objects.
DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;

    PROCEDURE require_table(p_name IN VARCHAR2) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TABLES
         WHERE TABLE_NAME = UPPER(p_name);

        IF v_count = 0 THEN
            v_missing := v_missing || ', TABLE ' || UPPER(p_name);
        END IF;
    END require_table;

    PROCEDURE require_object
    (
        p_name IN VARCHAR2,
        p_type IN VARCHAR2
    ) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_OBJECTS
         WHERE OBJECT_NAME = UPPER(p_name)
           AND OBJECT_TYPE = UPPER(p_type)
           AND STATUS = 'VALID';

        IF v_count = 0 THEN
            v_missing := v_missing || ', ' || UPPER(p_type) || ' ' || UPPER(p_name);
        END IF;
    END require_object;
BEGIN
    require_table('T_PERMDETAIL_NO');
    require_table('T_PERMMASTER_NO');
    require_table('T_SCHEDULE_DAYFLIGHTS');
    require_table('M_FLY_PURPOSE');
    require_table('M_CRAFT_TYPE');
    require_table('M_AERO');
    require_object('PERM_PKG', 'PACKAGE');
    require_object('PERM_PKG', 'PACKAGE BODY');
    require_object('PROCESS_PKG', 'PACKAGE');
    require_object('PROCESS_PKG', 'PACKAGE BODY');

    SELECT COUNT(*)
      INTO v_count
      FROM USER_ARGUMENTS
     WHERE PACKAGE_NAME = 'PERM_PKG'
       AND OBJECT_NAME = 'PERMDETAIL_NO_GETBYSEARCH'
       AND ARGUMENT_NAME IN
           (
               'P_PERM_ID', 'P_CRAFT_ID', 'P_DAYSFLIGHT', 'P_FLIGHTNBR',
               'P_REGISTRATION', 'P_FROM_AIRP', 'P_TO_AIRP', 'P_ETD',
               'P_ETA', 'P_VIA', 'P_PURPOSE_ID', 'P_REMARK',
               'P_REMARK_SEND', 'P_PAGESIZE', 'P_PAGEINDEX', 'P_RSTART',
               'P_RFINISH', 'P_RETURN_CODE'
           );

    IF v_count <> 18 THEN
        v_missing := v_missing
            || ', SIGNATURE PERM_PKG.PermDetail_No_GetBySearch';
    END IF;

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20851,
            'Missing required objects:' || LTRIM(v_missing, ',')
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('Precheck OK. Schema=' || USER);
END;
/

-- 2. Persistent DDL backup table used by the rollback script.
BEGIN
    EXECUTE IMMEDIATE q'~
        CREATE TABLE T_ATFM_DEPLOY_DDL_BACKUP
        (
            RUN_ID           VARCHAR2(40)  NOT NULL,
            DEPLOYMENT_NAME  VARCHAR2(60)  NOT NULL,
            OBJECT_NAME      VARCHAR2(128) NOT NULL,
            OBJECT_TYPE      VARCHAR2(30)  NOT NULL,
            EXISTED_BEFORE   NUMBER(1)     NOT NULL,
            DDL_TEXT         CLOB,
            BACKUP_AT        DATE          DEFAULT SYSDATE NOT NULL,
            BACKUP_USER      VARCHAR2(128) DEFAULT USER NOT NULL,
            CONSTRAINT PK_ATFM_DEPLOY_DDL_BK
                PRIMARY KEY (RUN_ID, OBJECT_NAME, OBJECT_TYPE)
        )
    ~';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE <> -955 THEN
            RAISE;
        END IF;
END;
/

-- 3. Back up the current package body, approval package and named index.
DECLARE
    v_run_id VARCHAR2(40) :=
        'EPN_' || TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF3');

    PROCEDURE backup_object
    (
        p_object_name  IN VARCHAR2,
        p_object_type  IN VARCHAR2,
        p_metadata_type IN VARCHAR2
    ) IS
        v_count PLS_INTEGER;
        v_ddl   CLOB;
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_OBJECTS
         WHERE OBJECT_NAME = UPPER(p_object_name)
           AND OBJECT_TYPE = UPPER(p_object_type);

        IF v_count > 0 THEN
            v_ddl := DBMS_METADATA.GET_DDL(
                UPPER(p_metadata_type),
                UPPER(p_object_name),
                USER
            );
        END IF;

        INSERT INTO T_ATFM_DEPLOY_DDL_BACKUP
        (
            RUN_ID,
            DEPLOYMENT_NAME,
            OBJECT_NAME,
            OBJECT_TYPE,
            EXISTED_BEFORE,
            DDL_TEXT,
            BACKUP_AT,
            BACKUP_USER
        )
        VALUES
        (
            v_run_id,
            'EDIT_PERM_NO_AND_MAIL',
            UPPER(p_object_name),
            UPPER(p_object_type),
            CASE WHEN v_count > 0 THEN 1 ELSE 0 END,
            v_ddl,
            SYSDATE,
            USER
        );
    END backup_object;
BEGIN
    backup_object('PERM_PKG', 'PACKAGE BODY', 'PACKAGE_BODY');
    backup_object('PERM_NO_APPROVAL_PKG', 'PACKAGE', 'PACKAGE');
    backup_object('PERM_NO_APPROVAL_PKG', 'PACKAGE BODY', 'PACKAGE_BODY');
    backup_object('IX_PERMDETAIL_NO_PERM_ID_ID', 'INDEX', 'INDEX');
    COMMIT;

    DBMS_OUTPUT.PUT_LINE('DDL backup RUN_ID=' || v_run_id);
END;
/

-- 4. Add the access index only when no equivalent leading columns exist.
DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM
      (
          SELECT INDEX_NAME
            FROM USER_IND_COLUMNS
           WHERE TABLE_NAME = 'T_PERMDETAIL_NO'
           GROUP BY INDEX_NAME
          HAVING MAX(
                     CASE
                         WHEN COLUMN_POSITION = 1 AND COLUMN_NAME = 'PERM_ID'
                         THEN 1 ELSE 0
                     END
                 ) = 1
             AND MAX(
                     CASE
                         WHEN COLUMN_POSITION = 2 AND COLUMN_NAME = 'ID'
                         THEN 1 ELSE 0
                     END
                 ) = 1
      );

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_PERMDETAIL_NO_PERM_ID_ID '
            || 'ON T_PERMDETAIL_NO (PERM_ID, ID)';
        DBMS_OUTPUT.PUT_LINE('Created IX_PERMDETAIL_NO_PERM_ID_ID');
    ELSE
        DBMS_OUTPUT.PUT_LINE('Equivalent (PERM_ID, ID) index already exists');
    END IF;
END;
/

-- 5. Replace only PermDetail_No_GetBySearch inside PACKAGE BODY PERM_PKG.
--    The old package body is restored immediately if compilation fails.
DECLARE
    v_start_line   PLS_INTEGER;
    v_end_line     PLS_INTEGER;
    v_new_body     CLOB;
    v_backup_body  CLOB;
    v_error_count  PLS_INTEGER;
    v_status       USER_OBJECTS.STATUS%TYPE;

    v_replacement VARCHAR2(32767) := q'~
    PROCEDURE PermDetail_No_GetBySearch
    (
        p_PERM_ID      IN VARCHAR2,
        p_CRAFT_ID     IN VARCHAR2,
        p_DAYSFLIGHT   IN VARCHAR2,
        p_FLIGHTNBR    IN VARCHAR2,
        p_REGISTRATION IN VARCHAR2,
        p_FROM_AIRP    IN VARCHAR2,
        p_TO_AIRP      IN VARCHAR2,
        p_ETD          IN VARCHAR2,
        p_ETA          IN VARCHAR2,
        p_VIA          IN VARCHAR2,
        p_PURPOSE_ID   IN VARCHAR2,
        p_REMARK       IN VARCHAR2,
        p_REMARK_SEND  IN VARCHAR2,
        p_PageSize     IN INT,
        p_PageIndex    IN INT,
        p_rStart       IN NUMBER,
        p_rFinish      IN NUMBER,
        P_RETURN_CODE  OUT T_CURSOR
    )
    IS
        v_perm_id       NUMBER;
        v_craft_id      NUMBER;
        v_offset        PLS_INTEGER;
        v_fetch         PLS_INTEGER;
        v_page_size     PLS_INTEGER;
        v_page_index    PLS_INTEGER;
        v_daysflight    VARCHAR2(4000);
        v_flightnbr     VARCHAR2(4000);
        v_registration VARCHAR2(4000);
        v_from_airp     VARCHAR2(4000);
        v_to_airp       VARCHAR2(4000);
        v_etd           VARCHAR2(4000);
        v_eta           VARCHAR2(4000);
        v_via           VARCHAR2(4000);
        v_purpose_id    VARCHAR2(4000);
        v_remark        VARCHAR2(4000);
        v_remark_send   VARCHAR2(4000);
    BEGIN
        IF TRIM(p_PERM_ID) IS NULL THEN
            RAISE_APPLICATION_ERROR(-20021, 'P_PERM_ID is required');
        END IF;

        BEGIN
            v_perm_id := TO_NUMBER(TRIM(p_PERM_ID));
        EXCEPTION
            WHEN VALUE_ERROR THEN
                RAISE_APPLICATION_ERROR(-20022, 'P_PERM_ID must be numeric');
        END;

        IF TRIM(p_CRAFT_ID) IS NOT NULL AND TRIM(p_CRAFT_ID) <> '0' THEN
            BEGIN
                v_craft_id := TO_NUMBER(TRIM(p_CRAFT_ID));
            EXCEPTION
                WHEN VALUE_ERROR THEN
                    RAISE_APPLICATION_ERROR(-20023, 'P_CRAFT_ID must be numeric');
            END;
        END IF;

        v_daysflight    := NULLIF(UPPER(TRIM(p_DAYSFLIGHT)), '0');
        v_flightnbr     := NULLIF(UPPER(TRIM(p_FLIGHTNBR)), '0');
        v_registration := NULLIF(UPPER(TRIM(p_REGISTRATION)), '0');
        v_from_airp     := NULLIF(UPPER(TRIM(p_FROM_AIRP)), '0');
        v_to_airp       := NULLIF(UPPER(TRIM(p_TO_AIRP)), '0');
        v_etd           := NULLIF(UPPER(TRIM(p_ETD)), '0');
        v_eta           := NULLIF(UPPER(TRIM(p_ETA)), '0');
        v_via           := NULLIF(UPPER(TRIM(p_VIA)), '0');
        v_purpose_id    := NULLIF(UPPER(TRIM(p_PURPOSE_ID)), '0');
        v_remark        := NULLIF(UPPER(TRIM(p_REMARK)), '0');
        v_remark_send   := NULLIF(UPPER(TRIM(p_REMARK_SEND)), '0');

        IF NVL(p_rFinish, 0) > NVL(p_rStart, 0) THEN
            v_offset := GREATEST(NVL(TRUNC(p_rStart), 0), 0);
            v_fetch  := GREATEST(TRUNC(p_rFinish) - v_offset, 1);
        ELSE
            v_page_size  := GREATEST(NVL(p_PageSize, 100), 1);
            v_page_index := GREATEST(NVL(p_PageIndex, 0), 0);
            v_offset     := v_page_size * v_page_index;
            v_fetch      := v_page_size;
        END IF;

        OPEN P_RETURN_CODE FOR
            WITH
            purpose_lookup AS
            (
                SELECT PURPOSE_CODE, MAX(PURPOSE_NAME) AS PURPOSE_NAME
                  FROM M_FLY_PURPOSE
                 GROUP BY PURPOSE_CODE
            ),
            craft_lookup AS
            (
                SELECT CRAFT_ID,
                       MAX(MA) AS MA,
                       MAX(TAITRONG) AS TAITRONG
                  FROM M_CRAFT_TYPE
                 GROUP BY CRAFT_ID
            ),
            aero_lookup AS
            (
                SELECT AE_CODE
                  FROM M_AERO
                 GROUP BY AE_CODE
            ),
            perm_lookup AS
            (
                SELECT PERM_ID, MAX(PERMNBR_ID) AS PERMNBR_ID
                  FROM T_PERMMASTER_NO
                 GROUP BY PERM_ID
            ),
            filtered AS
            (
                SELECT d.*,
                       fp.PURPOSE_NAME,
                       ct.MA AS CRAFT_NAME,
                       ct.TAITRONG,
                       af.AE_CODE AS FROM_NAME,
                       atp.AE_CODE AS TO_NAME,
                       pm.PERMNBR_ID
                  FROM T_PERMDETAIL_NO d
                  LEFT JOIN purpose_lookup fp
                    ON fp.PURPOSE_CODE = d.PURPOSE_ID
                  LEFT JOIN craft_lookup ct
                    ON ct.CRAFT_ID = d.CRAFT_ID
                  LEFT JOIN aero_lookup af
                    ON af.AE_CODE = d.FROM_AIRP
                  LEFT JOIN aero_lookup atp
                    ON atp.AE_CODE = d.TO_AIRP
                  LEFT JOIN perm_lookup pm
                    ON pm.PERM_ID = d.PERM_ID
                 WHERE d.PERM_ID = v_perm_id
                   AND (v_craft_id IS NULL OR d.CRAFT_ID = v_craft_id)
                   AND (v_daysflight IS NULL OR UPPER(d.DAYSFLIGHT) LIKE '%' || v_daysflight || '%')
                   AND (v_flightnbr IS NULL OR UPPER(d.FLIGHTNBR) LIKE '%' || v_flightnbr || '%')
                   AND (v_registration IS NULL OR UPPER(d.REGISTRATION) LIKE '%' || v_registration || '%')
                   AND (v_from_airp IS NULL OR UPPER(d.FROM_AIRP) LIKE '%' || v_from_airp || '%')
                   AND (v_to_airp IS NULL OR UPPER(d.TO_AIRP) LIKE '%' || v_to_airp || '%')
                   AND (v_etd IS NULL OR UPPER(d.ETD) LIKE '%' || v_etd || '%')
                   AND (v_eta IS NULL OR UPPER(d.ETA) LIKE '%' || v_eta || '%')
                   AND (v_via IS NULL OR UPPER(d.VIA) LIKE '%' || v_via || '%')
                   AND (v_purpose_id IS NULL OR UPPER(d.PURPOSE_ID) LIKE '%' || v_purpose_id || '%')
                   AND (v_remark IS NULL OR UPPER(d.REMARK) LIKE '%' || v_remark || '%')
                   AND (v_remark_send IS NULL OR UPPER(d.REMARK_SEND) LIKE '%' || v_remark_send || '%')
            ),
            numbered AS
            (
                SELECT filtered.*,
                       COUNT(*) OVER () AS RECORD_SUM,
                       ROW_NUMBER() OVER (ORDER BY ID) AS RNUM
                  FROM filtered
            )
            SELECT numbered.*
              FROM numbered
             WHERE RNUM > v_offset
               AND RNUM <= v_offset + v_fetch
             ORDER BY RNUM;
    END PermDetail_No_GetBySearch;
~';

    PROCEDURE append_text
    (
        p_target IN OUT NOCOPY CLOB,
        p_text   IN VARCHAR2
    ) IS
    BEGIN
        IF p_text IS NOT NULL THEN
            DBMS_LOB.WRITEAPPEND(p_target, LENGTH(p_text), p_text);
        END IF;
    END append_text;

    PROCEDURE execute_ddl(p_ddl IN CLOB) IS
        v_ddl_cursor INTEGER;
    BEGIN
        v_ddl_cursor := DBMS_SQL.OPEN_CURSOR;
        BEGIN
            DBMS_SQL.PARSE(v_ddl_cursor, p_ddl, DBMS_SQL.NATIVE);
            DBMS_SQL.CLOSE_CURSOR(v_ddl_cursor);
        EXCEPTION
            WHEN OTHERS THEN
                IF DBMS_SQL.IS_OPEN(v_ddl_cursor) THEN
                    DBMS_SQL.CLOSE_CURSOR(v_ddl_cursor);
                END IF;
                RAISE;
        END;
    END execute_ddl;
BEGIN
    SELECT DDL_TEXT
      INTO v_backup_body
      FROM
      (
          SELECT DDL_TEXT
            FROM T_ATFM_DEPLOY_DDL_BACKUP
           WHERE DEPLOYMENT_NAME = 'EDIT_PERM_NO_AND_MAIL'
             AND OBJECT_NAME = 'PERM_PKG'
             AND OBJECT_TYPE = 'PACKAGE BODY'
             AND EXISTED_BEFORE = 1
           ORDER BY BACKUP_AT DESC, RUN_ID DESC
      )
     WHERE ROWNUM = 1;

    SELECT MIN(LINE)
      INTO v_start_line
      FROM USER_SOURCE
     WHERE NAME = 'PERM_PKG'
       AND TYPE = 'PACKAGE BODY'
       AND REGEXP_LIKE(
               TEXT,
               '^[[:space:]]*PROCEDURE[[:space:]]+PERMDETAIL_NO_GETBYSEARCH([[:space:]]|\()' ,
               'i'
           );

    IF v_start_line IS NULL THEN
        RAISE_APPLICATION_ERROR(
            -20852,
            'PERM_PKG.PermDetail_No_GetBySearch implementation was not found'
        );
    END IF;

    SELECT MIN(LINE)
      INTO v_end_line
      FROM USER_SOURCE
     WHERE NAME = 'PERM_PKG'
       AND TYPE = 'PACKAGE BODY'
       AND LINE > v_start_line
       AND REGEXP_LIKE(
               TEXT,
               '^[[:space:]]*END[[:space:]]+PERMDETAIL_NO_GETBYSEARCH[[:space:]]*;',
               'i'
           );

    IF v_end_line IS NULL THEN
        RAISE_APPLICATION_ERROR(
            -20853,
            'End of PERM_PKG.PermDetail_No_GetBySearch was not found'
        );
    END IF;

    DBMS_LOB.CREATETEMPORARY(v_new_body, TRUE, DBMS_LOB.CALL);
    append_text(v_new_body, 'CREATE OR REPLACE ');

    FOR src IN
    (
        SELECT LINE, TEXT
          FROM USER_SOURCE
         WHERE NAME = 'PERM_PKG'
           AND TYPE = 'PACKAGE BODY'
         ORDER BY LINE
    )
    LOOP
        IF src.LINE < v_start_line OR src.LINE > v_end_line THEN
            append_text(v_new_body, src.TEXT);
        ELSIF src.LINE = v_start_line THEN
            append_text(v_new_body, v_replacement);
        END IF;
    END LOOP;

    BEGIN
        execute_ddl(v_new_body);

        SELECT STATUS
          INTO v_status
          FROM USER_OBJECTS
         WHERE OBJECT_NAME = 'PERM_PKG'
           AND OBJECT_TYPE = 'PACKAGE BODY';

        SELECT COUNT(*)
          INTO v_error_count
          FROM USER_ERRORS
         WHERE NAME = 'PERM_PKG'
           AND TYPE = 'PACKAGE BODY';

        IF v_status <> 'VALID' OR v_error_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20854,
                'PERM_PKG body is invalid after patch; errors=' || v_error_count
            );
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            execute_ddl(v_backup_body);
            RAISE;
    END;

    DBMS_LOB.FREETEMPORARY(v_new_body);
    DBMS_OUTPUT.PUT_LINE('PERM_PKG.PermDetail_No_GetBySearch deployed');
EXCEPTION
    WHEN OTHERS THEN
        IF DBMS_LOB.ISTEMPORARY(v_new_body) = 1 THEN
            DBMS_LOB.FREETEMPORARY(v_new_body);
        END IF;
        RAISE;
END;
/

-- 6. Package used by the Accepted button on Edit_PermNO4Mail.aspx.
CREATE OR REPLACE PACKAGE PERM_NO_APPROVAL_PKG AS
    FUNCTION PARSE_FLIGHT_DATE(p_value IN VARCHAR2)
        RETURN DATE;

    PROCEDURE ACCEPT_PERM_NO
    (
        P_ID          IN  NUMBER,
        P_RETURN_CODE OUT NUMBER
    );
END PERM_NO_APPROVAL_PKG;
/

CREATE OR REPLACE PACKAGE BODY PERM_NO_APPROVAL_PKG AS
    FUNCTION PARSE_FLIGHT_DATE(p_value IN VARCHAR2)
        RETURN DATE
    IS
        v_value VARCHAR2(100) := UPPER(TRIM(p_value));
        v_date  DATE;
    BEGIN
        IF v_value IS NULL THEN
            RETURN NULL;
        END IF;

        BEGIN
            v_date := TO_DATE(
                v_value,
                'FXDD-MON-YYYY',
                'NLS_DATE_LANGUAGE=ENGLISH'
            );
            RETURN v_date;
        EXCEPTION
            WHEN OTHERS THEN NULL;
        END;

        BEGIN
            v_date := TO_DATE(v_value, 'FXDD-MM-YYYY');
            RETURN v_date;
        EXCEPTION
            WHEN OTHERS THEN NULL;
        END;

        BEGIN
            v_date := TO_DATE(v_value, 'FXDD/MM/YYYY');
            RETURN v_date;
        EXCEPTION
            WHEN OTHERS THEN NULL;
        END;

        RETURN NULL;
    END PARSE_FLIGHT_DATE;

    PROCEDURE ACCEPT_PERM_NO
    (
        P_ID          IN  NUMBER,
        P_RETURN_CODE OUT NUMBER
    )
    IS
        v_detail_count PLS_INTEGER;
        v_insert_count PLS_INTEGER;
    BEGIN
        P_RETURN_CODE := -1;

        IF P_ID IS NULL OR P_ID <= 0 THEN
            P_RETURN_CODE := -2;
            RETURN;
        END IF;

        SELECT COUNT(*)
          INTO v_detail_count
          FROM T_PERMDETAIL_NO d
         WHERE d.PERM_ID = P_ID
           AND d.FLIGHT_PK IS NOT NULL
           AND PERM_NO_APPROVAL_PKG.PARSE_FLIGHT_DATE(d.DAYSFLIGHT) IS NOT NULL;

        IF v_detail_count = 0 THEN
            P_RETURN_CODE := -2;
            RETURN;
        END IF;

        DELETE FROM T_SCHEDULE_DAYFLIGHTS s
         WHERE s.ISRENDER = 0
           AND s.PERM_ID = P_ID
           AND EXISTS
               (
                   SELECT 1
                     FROM T_PERMDETAIL_NO d
                    WHERE d.PERM_ID = P_ID
                      AND d.FLIGHT_PK = s.FLIGHT_PK
               );

        INSERT INTO T_SCHEDULE_DAYFLIGHTS
        (
            FLIGHT_ID,
            FLIGHT_PK,
            PERM_ID,
            PERMNBR,
            PERMTYPE,
            FLIGHT_TYPE,
            OPER_ID,
            PURPOSE,
            CRAFT_ID,
            MTOW,
            VALIDHOURS,
            DATE_OLD,
            FLIGHTDATE,
            FLIGHTNBR,
            REGISTRATION,
            FROM_AIRP,
            TO_AIRP,
            ETD,
            ETA,
            VIA,
            REMARK,
            LASTUSER
        )
        SELECT
            d.FLIGHT_PK,
            d.FLIGHT_PK,
            d.PERM_ID,
            m.PERMNBR_ID,
            m.PERMTYPE,
            m.FLIGHTTYPE,
            m.OPER_ID,
            d.PURPOSE_ID,
            d.CRAFT_ID,
            d.MTOW,
            m.VALIDHOURS,
            PERM_NO_APPROVAL_PKG.PARSE_FLIGHT_DATE(d.DAYSFLIGHT),
            PERM_NO_APPROVAL_PKG.PARSE_FLIGHT_DATE(d.DAYSFLIGHT),
            d.FLIGHTNBR,
            d.REGISTRATION,
            d.FROM_AIRP,
            d.TO_AIRP,
            d.ETD,
            d.ETA,
            d.VIA,
            d.REMARK,
            d.LASTUSER
        FROM T_PERMDETAIL_NO d
        JOIN T_PERMMASTER_NO m
          ON m.PERM_ID = d.PERM_ID
        WHERE d.PERM_ID = P_ID
          AND d.FLIGHT_PK IS NOT NULL
          AND PERM_NO_APPROVAL_PKG.PARSE_FLIGHT_DATE(d.DAYSFLIGHT) IS NOT NULL;

        v_insert_count := SQL%ROWCOUNT;

        IF v_insert_count = 0 THEN
            ROLLBACK;
            P_RETURN_CODE := -3;
            RETURN;
        END IF;

        COMMIT;
        P_RETURN_CODE := 1;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;

            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG(
                    'PERM_NO_APPROVAL_PKG.ACCEPT_PERM_NO',
                    SQLCODE,
                    SUBSTR(
                        SQLERRM || CHR(10)
                        || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                        1,
                        200
                    )
                );
            EXCEPTION
                WHEN OTHERS THEN NULL;
            END;

            P_RETURN_CODE := -1;
    END ACCEPT_PERM_NO;
END PERM_NO_APPROVAL_PKG;
/

-- 7. Compile and verify all database objects required by both pages.
BEGIN
    EXECUTE IMMEDIATE 'ALTER PACKAGE PERM_PKG COMPILE BODY';
    EXECUTE IMMEDIATE 'ALTER PACKAGE PERM_NO_APPROVAL_PKG COMPILE';
    EXECUTE IMMEDIATE 'ALTER PACKAGE PERM_NO_APPROVAL_PKG COMPILE BODY';
END;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE
           (OBJECT_NAME = 'PERM_PKG' AND OBJECT_TYPE = 'PACKAGE BODY' AND STATUS = 'VALID')
        OR (OBJECT_NAME = 'PERM_NO_APPROVAL_PKG' AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY') AND STATUS = 'VALID');

    IF v_count <> 3 THEN
        RAISE_APPLICATION_ERROR(
            -20855,
            'Required package objects are not all VALID; valid count=' || v_count
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_ERRORS
     WHERE NAME IN ('PERM_PKG', 'PERM_NO_APPROVAL_PKG');

    IF v_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20856,
            'Package compilation errors remain; count=' || v_count
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('Edit PermNo + Edit PermNO4Mail deployment verify: OK');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('PERM_PKG', 'PERM_NO_APPROVAL_PKG')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT INDEX_NAME, COLUMN_POSITION, COLUMN_NAME
  FROM USER_IND_COLUMNS
 WHERE TABLE_NAME = 'T_PERMDETAIL_NO'
   AND INDEX_NAME IN
       (
           SELECT INDEX_NAME
             FROM USER_IND_COLUMNS
            WHERE TABLE_NAME = 'T_PERMDETAIL_NO'
            GROUP BY INDEX_NAME
           HAVING MAX(CASE WHEN COLUMN_POSITION = 1 AND COLUMN_NAME = 'PERM_ID' THEN 1 ELSE 0 END) = 1
              AND MAX(CASE WHEN COLUMN_POSITION = 2 AND COLUMN_NAME = 'ID' THEN 1 ELSE 0 END) = 1
       )
 ORDER BY INDEX_NAME, COLUMN_POSITION;

SELECT RUN_ID,
       DEPLOYMENT_NAME,
       OBJECT_NAME,
       OBJECT_TYPE,
       EXISTED_BEFORE,
       BACKUP_AT,
       BACKUP_USER
  FROM T_ATFM_DEPLOY_DDL_BACKUP
 WHERE DEPLOYMENT_NAME = 'EDIT_PERM_NO_AND_MAIL'
   AND RUN_ID =
       (
           SELECT MAX(RUN_ID)
             FROM T_ATFM_DEPLOY_DDL_BACKUP
            WHERE DEPLOYMENT_NAME = 'EDIT_PERM_NO_AND_MAIL'
       )
 ORDER BY OBJECT_NAME, OBJECT_TYPE;
