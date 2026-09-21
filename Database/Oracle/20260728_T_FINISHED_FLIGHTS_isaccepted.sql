-- Target: ATFM@PDBORCL (Oracle 12c)
-- ISACCEPTED = 0: danh sách mới/chưa chấp nhận.
-- ISACCEPTED = 1: danh sách đã chấp nhận.

DECLARE
    v_count        PLS_INTEGER;
    v_column_added BOOLEAN := FALSE;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_FINISHED_FLIGHTS'
       AND COLUMN_NAME = 'ISACCEPTED';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_FINISHED_FLIGHTS '
            || 'ADD (ISACCEPTED NUMBER(1) DEFAULT 0 NOT NULL)';
        v_column_added := TRUE;
    END IF;

    /*
      Chỉ chuyển dữ liệu lịch sử sang trạng thái đã chấp nhận trong lần
      đầu thêm cột. Chạy lại script sẽ không làm thay đổi các dòng mới
      đang có ISACCEPTED = 0.
    */
    IF v_column_added THEN
        EXECUTE IMMEDIATE
            'UPDATE T_FINISHED_FLIGHTS SET ISACCEPTED = 1';
        COMMIT;
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_CONSTRAINTS
     WHERE TABLE_NAME = 'T_FINISHED_FLIGHTS'
       AND CONSTRAINT_NAME = 'CK_TFIN_ISACCEPTED';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_FINISHED_FLIGHTS '
            || 'ADD CONSTRAINT CK_TFIN_ISACCEPTED '
            || 'CHECK (ISACCEPTED IN (0, 1))';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TFIN_ACCEPTED_DATE';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_TFIN_ACCEPTED_DATE '
            || 'ON T_FINISHED_FLIGHTS (ISACCEPTED, FLIGHTDATE)';
    END IF;
END;
/

CREATE OR REPLACE PACKAGE FINISHED_STATUS_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_FINISHED_FLIGHTS
    (
        PERMNBR         IN VARCHAR2 DEFAULT NULL,
        REGISTRATION    IN VARCHAR2 DEFAULT NULL,
        FROM_AIRP       IN VARCHAR2 DEFAULT NULL,
        TO_AIRP         IN VARCHAR2 DEFAULT NULL,
        PURPOSE         IN VARCHAR2 DEFAULT NULL,
        VALIDHOURS      IN NUMBER DEFAULT NULL,
        OPER_ID         IN VARCHAR2 DEFAULT NULL,
        PERMTYPE        IN VARCHAR2 DEFAULT NULL,
        FLIGHT_TYPE     IN VARCHAR2 DEFAULT NULL,
        CRAFT_TYPE      IN VARCHAR2 DEFAULT NULL,
        CRAFT_ID        IN NUMBER DEFAULT NULL,
        REAL_CRAFT_TYPE IN VARCHAR2 DEFAULT NULL,
        FLIGHTNBR       IN VARCHAR2 DEFAULT NULL,
        VIA             IN VARCHAR2 DEFAULT NULL,
        FPL_VIA         IN VARCHAR2 DEFAULT NULL,
        REMARK          IN VARCHAR2 DEFAULT NULL,
        ETA             IN VARCHAR2 DEFAULT NULL,
        ETD             IN VARCHAR2 DEFAULT NULL,
        ATA             IN VARCHAR2 DEFAULT NULL,
        ATD             IN VARCHAR2 DEFAULT NULL,
        STARTDATE       IN VARCHAR2,
        FINISHDATE      IN VARCHAR2,
        KHUNGGIO1       IN VARCHAR2 DEFAULT '0000',
        KHUNGGIO2       IN VARCHAR2 DEFAULT '2359',
        WHECONDITION    IN VARCHAR2 DEFAULT NULL,
        CAT_HA          IN NUMBER DEFAULT 0,
        TYPEOPER        IN NUMBER DEFAULT 0,
        TYPEFLIGHT      IN NUMBER DEFAULT 0,
        FIR             IN VARCHAR2 DEFAULT '0',
        SANBAYDI        IN VARCHAR2 DEFAULT NULL,
        SANBAYDEN       IN VARCHAR2 DEFAULT NULL,
        P_ISACCEPTED    IN NUMBER DEFAULT 0,
        PAGESIZE        IN NUMBER DEFAULT 100,
        PAGEINDEX       IN NUMBER DEFAULT 0,
        OUT_CURSOR      OUT T_CURSOR
    );

    PROCEDURE ACCEPT_FINISHED_FLIGHTS
    (
        P_FLIGHT_IDS IN VARCHAR2,
        P_USER       IN VARCHAR2 DEFAULT NULL,
        P_RETURNCODE OUT NUMBER
    );
END FINISHED_STATUS_PKG;
/

CREATE OR REPLACE PACKAGE BODY FINISHED_STATUS_PKG AS
    FUNCTION PARSE_DATE(p_value IN VARCHAR2)
        RETURN DATE
    IS
    BEGIN
        IF REGEXP_LIKE(TRIM(p_value), '^[0-9]{2}-[0-9]{2}-[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(p_value), 'FXDD-MM-YYYY');
        ELSIF REGEXP_LIKE(TRIM(p_value), '^[0-9]{2}/[0-9]{2}/[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(p_value), 'FXDD/MM/YYYY');
        END IF;

        RAISE_APPLICATION_ERROR(-20001, 'Date must use DD-MM-YYYY');
    END PARSE_DATE;

    PROCEDURE GET_FINISHED_FLIGHTS
    (
        PERMNBR         IN VARCHAR2 DEFAULT NULL,
        REGISTRATION    IN VARCHAR2 DEFAULT NULL,
        FROM_AIRP       IN VARCHAR2 DEFAULT NULL,
        TO_AIRP         IN VARCHAR2 DEFAULT NULL,
        PURPOSE         IN VARCHAR2 DEFAULT NULL,
        VALIDHOURS      IN NUMBER DEFAULT NULL,
        OPER_ID         IN VARCHAR2 DEFAULT NULL,
        PERMTYPE        IN VARCHAR2 DEFAULT NULL,
        FLIGHT_TYPE     IN VARCHAR2 DEFAULT NULL,
        CRAFT_TYPE      IN VARCHAR2 DEFAULT NULL,
        CRAFT_ID        IN NUMBER DEFAULT NULL,
        REAL_CRAFT_TYPE IN VARCHAR2 DEFAULT NULL,
        FLIGHTNBR       IN VARCHAR2 DEFAULT NULL,
        VIA             IN VARCHAR2 DEFAULT NULL,
        FPL_VIA         IN VARCHAR2 DEFAULT NULL,
        REMARK          IN VARCHAR2 DEFAULT NULL,
        ETA             IN VARCHAR2 DEFAULT NULL,
        ETD             IN VARCHAR2 DEFAULT NULL,
        ATA             IN VARCHAR2 DEFAULT NULL,
        ATD             IN VARCHAR2 DEFAULT NULL,
        STARTDATE       IN VARCHAR2,
        FINISHDATE      IN VARCHAR2,
        KHUNGGIO1       IN VARCHAR2 DEFAULT '0000',
        KHUNGGIO2       IN VARCHAR2 DEFAULT '2359',
        WHECONDITION    IN VARCHAR2 DEFAULT NULL,
        CAT_HA          IN NUMBER DEFAULT 0,
        TYPEOPER        IN NUMBER DEFAULT 0,
        TYPEFLIGHT      IN NUMBER DEFAULT 0,
        FIR             IN VARCHAR2 DEFAULT '0',
        SANBAYDI        IN VARCHAR2 DEFAULT NULL,
        SANBAYDEN       IN VARCHAR2 DEFAULT NULL,
        P_ISACCEPTED    IN NUMBER DEFAULT 0,
        PAGESIZE        IN NUMBER DEFAULT 100,
        PAGEINDEX       IN NUMBER DEFAULT 0,
        OUT_CURSOR      OUT T_CURSOR
    )
    IS
        v_start_date  DATE;
        v_finish_date DATE;
        v_filter_date DATE;
        v_permnbr         VARCHAR2(4000);
        v_registration    VARCHAR2(4000);
        v_from_airp       VARCHAR2(4000);
        v_to_airp         VARCHAR2(4000);
        v_purpose         VARCHAR2(4000);
        v_oper_id         VARCHAR2(4000);
        v_permtype        VARCHAR2(4000);
        v_flight_type     VARCHAR2(4000);
        v_craft_type      VARCHAR2(4000);
        v_real_craft_type VARCHAR2(4000);
        v_flight_nbr      VARCHAR2(4000);
        v_via             VARCHAR2(4000);
        v_fpl_via         VARCHAR2(4000);
        v_remark          VARCHAR2(4000);
        v_eta             VARCHAR2(4000);
        v_etd             VARCHAR2(4000);
        v_ata             VARCHAR2(4000);
        v_atd             VARCHAR2(4000);
        v_khunggio1       VARCHAR2(4000);
        v_khunggio2       VARCHAR2(4000);
        v_fir             VARCHAR2(4000);
        v_sanbaydi        VARCHAR2(4000);
        v_sanbayden       VARCHAR2(4000);
        v_validhours      NUMBER;
        v_craft_id        NUMBER;
        v_cat_ha          NUMBER;
        v_typeoper        NUMBER;
        v_typeflight      NUMBER;
        v_isaccepted      NUMBER;
        v_pagesize_input  NUMBER;
        v_pageindex_input NUMBER;
        v_first_row   PLS_INTEGER;
        v_last_row    PLS_INTEGER;
        v_page_size   PLS_INTEGER;
        v_page_index  PLS_INTEGER;
    BEGIN
        v_start_date := PARSE_DATE(STARTDATE);
        v_finish_date := PARSE_DATE(FINISHDATE);

        IF v_finish_date < v_start_date THEN
            RAISE_APPLICATION_ERROR(
                -20002,
                'FINISHDATE must be greater than or equal to STARTDATE'
            );
        END IF;

        IF TRIM(WHECONDITION) IS NOT NULL THEN
            v_filter_date := PARSE_DATE(WHECONDITION);
        END IF;

        -- Tat ca tham so loc dung bien v_ de tranh trung ten cot trong SQL.
        v_permnbr := NULLIF(UPPER(TRIM(PERMNBR)), '');
        v_registration := NULLIF(UPPER(TRIM(REGISTRATION)), '');
        v_from_airp := NULLIF(UPPER(TRIM(FROM_AIRP)), '');
        v_to_airp := NULLIF(UPPER(TRIM(TO_AIRP)), '');
        v_purpose := NULLIF(UPPER(TRIM(PURPOSE)), '');
        v_oper_id := NULLIF(UPPER(TRIM(OPER_ID)), '');
        v_permtype := NULLIF(UPPER(TRIM(PERMTYPE)), '');
        v_flight_type := NULLIF(UPPER(TRIM(FLIGHT_TYPE)), '');
        v_craft_type := NULLIF(UPPER(TRIM(CRAFT_TYPE)), '');
        v_real_craft_type := NULLIF(UPPER(TRIM(REAL_CRAFT_TYPE)), '');
        v_flight_nbr := NULLIF(UPPER(TRIM(FLIGHTNBR)), '');
        v_via := NULLIF(UPPER(TRIM(VIA)), '');
        v_fpl_via := NULLIF(UPPER(TRIM(FPL_VIA)), '');
        v_remark := NULLIF(UPPER(TRIM(REMARK)), '');
        v_eta := NULLIF(UPPER(TRIM(ETA)), '');
        v_etd := NULLIF(UPPER(TRIM(ETD)), '');
        v_ata := NULLIF(UPPER(TRIM(ATA)), '');
        v_atd := NULLIF(UPPER(TRIM(ATD)), '');
        v_khunggio1 := NVL(TRIM(KHUNGGIO1), '0000');
        v_khunggio2 := NVL(TRIM(KHUNGGIO2), '2359');
        v_fir := NVL(TRIM(FIR), '0');
        v_sanbaydi := NULLIF(UPPER(TRIM(SANBAYDI)), '');
        v_sanbayden := NULLIF(UPPER(TRIM(SANBAYDEN)), '');
        v_validhours := VALIDHOURS;
        v_craft_id := CRAFT_ID;
        v_cat_ha := NVL(CAT_HA, 0);
        v_typeoper := NVL(TYPEOPER, 0);
        v_typeflight := NVL(TYPEFLIGHT, 0);
        v_isaccepted := NVL(P_ISACCEPTED, 0);
        v_pagesize_input := PAGESIZE;
        v_pageindex_input := PAGEINDEX;

        v_page_size := GREATEST(NVL(v_pagesize_input, 100), 1);
        v_page_index := GREATEST(NVL(v_pageindex_input, 0), 0);
        v_first_row := (v_page_size * v_page_index) + 1;
        v_last_row := v_page_size * (v_page_index + 1);

        OPEN OUT_CURSOR FOR
            SELECT page_data.*
              FROM
              (
                  SELECT
                      COUNT(*) OVER () AS SUMRECORD,
                      ROW_NUMBER() OVER
                      (
                          ORDER BY
                              t.FLIGHTNBR,
                              t.FROM_AIRP,
                              t.TO_AIRP,
                              t.FLIGHT_ID
                      ) AS RNUM,
                      t.*
                    FROM T_FINISHED_FLIGHTS t
                   WHERE t.ISACCEPTED = v_isaccepted
                     AND t.FLIGHTDATE >= v_start_date
                     AND t.FLIGHTDATE < v_finish_date + 1
                     AND
                     (
                         v_filter_date IS NULL
                         OR
                         (
                             t.FLIGHTDATE >= v_filter_date
                             AND t.FLIGHTDATE < v_filter_date + 1
                         )
                     )
                     AND
                     (
                         v_permtype IS NULL
                         OR UPPER(t.PERMTYPE) = v_permtype
                     )
                     AND
                     (
                         v_purpose IS NULL
                         OR UPPER(t.PURPOSE) = v_purpose
                     )
                     AND
                     (
                         v_permnbr IS NULL
                         OR UPPER(t.PERMNBR) LIKE
                            '%' || v_permnbr || '%'
                     )
                     AND
                     (
                         v_registration IS NULL
                         OR UPPER(t.REGISTRATION) LIKE
                            '%' || v_registration || '%'
                     )
                     AND
                     (
                         v_remark IS NULL
                         OR UPPER(t.REMARK) LIKE
                            '%' || v_remark || '%'
                     )
                     AND
                     (
                         v_craft_id IS NULL
                         OR v_craft_id = 0
                         OR t.CRAFT_ID = v_craft_id
                     )
                     AND
                     (
                         v_craft_type IS NULL
                         OR UPPER(t.CRAFT_TYPE) = v_craft_type
                     )
                     AND
                     (
                         v_real_craft_type IS NULL
                         OR UPPER(t.REAL_CRAFT_TYPE) LIKE
                            '%' || v_real_craft_type || '%'
                     )
                     AND
                     (
                         v_flight_type IS NULL
                         OR UPPER(t.FLIGHT_TYPE) LIKE
                            '%' || v_flight_type || '%'
                     )
                     AND
                     (
                         v_from_airp IS NULL
                         OR UPPER(t.FROM_AIRP) LIKE
                            '%' || v_from_airp || '%'
                     )
                     AND
                     (
                         v_to_airp IS NULL
                         OR UPPER(t.TO_AIRP) LIKE
                            '%' || v_to_airp || '%'
                     )
                     AND
                     (
                         v_via IS NULL
                         OR UPPER(t.VIA) LIKE
                            '%' || v_via || '%'
                     )
                     AND
                     (
                         v_fpl_via IS NULL
                         OR UPPER(t.FPL_VIA) LIKE
                            '%' || v_fpl_via || '%'
                     )
                     AND
                     (
                         v_oper_id IS NULL
                         OR UPPER(t.OPER_ID) LIKE
                            '%' || v_oper_id || '%'
                     )
                     AND
                     (
                         v_validhours IS NULL
                         OR v_validhours = 0
                         OR t.VALIDHOURS = v_validhours
                     )
                     AND
                     (
                         v_flight_nbr IS NULL
                         OR UPPER(TRIM(t.FLIGHTNBR)) LIKE
                            '%' || v_flight_nbr || '%'
                     )
                     AND
                     (
                         v_etd IS NULL
                         OR UPPER(t.ETD) LIKE
                            '%' || v_etd || '%'
                     )
                     AND
                     (
                         v_eta IS NULL
                         OR UPPER(t.ETA) LIKE
                            '%' || v_eta || '%'
                     )
                     AND
                     (
                         v_atd IS NULL
                         OR UPPER(t.ATD) LIKE
                            '%' || v_atd || '%'
                     )
                     AND
                     (
                         v_ata IS NULL
                         OR UPPER(t.ATA) LIKE
                            '%' || v_ata || '%'
                     )
                     AND
                     (
                         v_typeoper = 0
                         OR EXISTS
                         (
                             SELECT 1
                               FROM M_OPER op
                              WHERE op.OPER_ICAO = t.OPER_ID
                                AND
                                (
                                    (v_typeoper = 1 AND op.IS_DOMESTIC = 1)
                                    OR
                                    (v_typeoper = 2 AND op.IS_DOMESTIC = 0)
                                )
                         )
                     )
                     AND
                     (
                         v_typeflight = 0
                         OR
                         (
                             v_typeflight = 1
                             AND t.FROM_AIRP LIKE 'VV%'
                             AND t.TO_AIRP LIKE 'VV%'
                         )
                         OR
                         (
                             v_typeflight = 2
                             AND
                             (
                                 t.FROM_AIRP NOT LIKE 'VV%'
                                 OR t.TO_AIRP NOT LIKE 'VV%'
                             )
                         )
                     )
                     AND
                     (
                         v_fir = '0'
                         OR
                         (
                             v_fir = '1'
                             AND
                             (
                                 (
                                     t.FROM_AIRP IN
                                     (
                                         'VVNB', 'VVDH', 'VVTX', 'VVCB',
                                         'VVVD', 'VVVH', 'VVDB'
                                     )
                                     AND t.ATD IS NOT NULL
                                 )
                                 OR
                                 (
                                     t.TO_AIRP IN
                                     (
                                         'VVNB', 'VVDH', 'VVTX', 'VVCB',
                                         'VVVD', 'VVVH', 'VVDB'
                                     )
                                     AND t.ATA IS NOT NULL
                                 )
                             )
                         )
                         OR
                         (
                             v_fir = '2'
                             AND
                             (
                                 (
                                     t.FROM_AIRP IN
                                     (
                                         'VVTS', 'VVCR', 'VVDN', 'VVPB',
                                         'VVCL', 'VVPC', 'VVRG', 'VVDL',
                                         'VVTH', 'VVCS', 'VVBM', 'VVCT',
                                         'VVCM', 'VVPQ', 'VVPK'
                                     )
                                     AND t.ATD IS NOT NULL
                                 )
                                 OR
                                 (
                                     t.TO_AIRP IN
                                     (
                                         'VVTS', 'VVCR', 'VVDN', 'VVPB',
                                         'VVCL', 'VVPC', 'VVRG', 'VVDL',
                                         'VVTH', 'VVCS', 'VVBM', 'VVCT',
                                         'VVCM', 'VVPQ', 'VVPK'
                                     )
                                     AND t.ATA IS NOT NULL
                                 )
                             )
                         )
                     )
                     AND
                     (
                         (
                             v_sanbaydi IS NULL
                             AND v_sanbayden IS NULL
                         )
                         OR
                         (
                             v_sanbaydi IS NOT NULL
                             AND UPPER(t.FROM_AIRP) LIKE
                                 '%' || v_sanbaydi || '%'
                         )
                         OR
                         (
                             v_sanbayden IS NOT NULL
                             AND UPPER(t.TO_AIRP) LIKE
                                 '%' || v_sanbayden || '%'
                         )
                     )
                     AND
                     (
                         v_cat_ha = 0
                         OR
                         (
                             v_cat_ha = 1
                             AND SUBSTR(TRIM(t.ETD), 1, 4)
                                 BETWEEN v_khunggio1 AND v_khunggio2
                         )
                         OR
                         (
                             v_cat_ha = 2
                             AND SUBSTR(TRIM(t.ETA), 1, 4)
                                 BETWEEN v_khunggio1 AND v_khunggio2
                         )
                         OR
                         (
                             v_cat_ha = 3
                             AND SUBSTR(TRIM(t.ATD), 1, 4)
                                 BETWEEN v_khunggio1 AND v_khunggio2
                         )
                         OR
                         (
                             v_cat_ha = 4
                             AND SUBSTR(TRIM(t.ATA), 1, 4)
                                 BETWEEN v_khunggio1 AND v_khunggio2
                         )
                     )
              ) page_data
             WHERE page_data.RNUM BETWEEN v_first_row AND v_last_row
             ORDER BY page_data.RNUM;
    END GET_FINISHED_FLIGHTS;

    PROCEDURE ACCEPT_FINISHED_FLIGHTS
    (
        P_FLIGHT_IDS IN VARCHAR2,
        P_USER       IN VARCHAR2 DEFAULT NULL,
        P_RETURNCODE OUT NUMBER
    )
    IS
        v_flight_ids VARCHAR2(4000);
        v_id_count   PLS_INTEGER;
    BEGIN
        v_flight_ids := TRIM(BOTH ',' FROM TRIM(P_FLIGHT_IDS));

        IF v_flight_ids IS NULL
           OR NOT REGEXP_LIKE(v_flight_ids, '^[0-9]+(,[0-9]+)*$') THEN
            RAISE_APPLICATION_ERROR(
                -20003,
                'P_FLIGHT_IDS must be a comma-separated list of numeric IDs'
            );
        END IF;

        v_id_count := REGEXP_COUNT(v_flight_ids, ',') + 1;

        UPDATE T_FINISHED_FLIGHTS t
           SET t.ISACCEPTED = 1
         WHERE t.ISACCEPTED = 0
           AND t.FLIGHT_ID IN
               (
                   SELECT DISTINCT
                          TO_NUMBER(
                              REGEXP_SUBSTR(
                                  v_flight_ids,
                                  '[^,]+',
                                  1,
                                  LEVEL
                              )
                          )
                     FROM DUAL
                   CONNECT BY LEVEL <= v_id_count
               );

        P_RETURNCODE := SQL%ROWCOUNT;

        IF P_RETURNCODE > 0 THEN
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
                0,
                NVL(TRIM(P_USER), ' '),
                ' ',
                SYSDATE,
                'ACCEPT FINISHED FLIGHTS',
                0,
                'VISIBLE_IDS=' || TO_CHAR(v_id_count)
                    || '; ROWS=' || TO_CHAR(P_RETURNCODE),
                905
            );
        END IF;

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            P_RETURNCODE := -1;

            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG(
                    'ACCEPT_FINISHED_FLIGHTS',
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
            END;
    END ACCEPT_FINISHED_FLIGHTS;
END FINISHED_STATUS_PKG;
/

BEGIN
    DBMS_STATS.GATHER_TABLE_STATS(
        ownname          => USER,
        tabname          => 'T_FINISHED_FLIGHTS',
        cascade          => TRUE,
        estimate_percent => DBMS_STATS.AUTO_SAMPLE_SIZE
    );
END;
/

SELECT COLUMN_NAME, DATA_TYPE, DATA_DEFAULT, NULLABLE
  FROM USER_TAB_COLUMNS
 WHERE TABLE_NAME = 'T_FINISHED_FLIGHTS'
   AND COLUMN_NAME = 'ISACCEPTED';

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
       (
           'FINISHED_STATUS_PKG',
           'IX_TFIN_ACCEPTED_DATE'
       )
 ORDER BY OBJECT_TYPE, OBJECT_NAME;
