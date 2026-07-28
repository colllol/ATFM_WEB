-- ============================================================================
-- Tạo phân hệ chuyến bay vùng trời.
-- Trạng thái ISACCEPTED:
--   0 = Chưa gửi duyệt
--   1 = Đã duyệt (dành cho bước duyệt tiếp theo)
--   2 = Chờ duyệt
-- ============================================================================

DECLARE
    v_count NUMBER;
    v_start NUMBER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_FINISHFLIGHTS_AIRSPACE';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE TABLE T_FINISHFLIGHTS_AIRSPACE '
            || 'AS SELECT * FROM T_FINISHFLIGHTS_MILITARY WHERE 1 = 0';
    END IF;

    EXECUTE IMMEDIATE
        'ALTER TABLE T_FINISHFLIGHTS_AIRSPACE '
        || 'MODIFY (ISACCEPTED DEFAULT 0)';

    SELECT COUNT(*)
      INTO v_count
      FROM USER_CONSTRAINTS
     WHERE TABLE_NAME = 'T_FINISHFLIGHTS_AIRSPACE'
       AND CONSTRAINT_TYPE = 'P';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_FINISHFLIGHTS_AIRSPACE '
            || 'ADD CONSTRAINT PK_T_FINFL_AIRSPACE PRIMARY KEY (FLIGHT_ID)';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_CONSTRAINTS
     WHERE TABLE_NAME = 'T_FINISHFLIGHTS_AIRSPACE'
       AND CONSTRAINT_NAME = 'CK_T_FINFL_AIRSPACE_ACC';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_FINISHFLIGHTS_AIRSPACE '
            || 'ADD CONSTRAINT CK_T_FINFL_AIRSPACE_ACC '
            || 'CHECK (ISACCEPTED IN (0, 1, 2))';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_T_FINFL_AIRSPACE_ACC';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_T_FINFL_AIRSPACE_ACC '
            || 'ON T_FINISHFLIGHTS_AIRSPACE (ISACCEPTED, FLIGHTDATE)';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_T_FINFL_AIRSPACE_DATE';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_T_FINFL_AIRSPACE_DATE '
            || 'ON T_FINISHFLIGHTS_AIRSPACE (FLIGHTDATE, CALLSIGN)';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_SEQUENCES
     WHERE SEQUENCE_NAME = 'SEQ_FINISHFLIGHTS_AIRSPACE';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'SELECT NVL(MAX(FLIGHT_ID), 0) + 1 '
            || 'FROM T_FINISHFLIGHTS_AIRSPACE'
            INTO v_start;

        EXECUTE IMMEDIATE
            'CREATE SEQUENCE SEQ_FINISHFLIGHTS_AIRSPACE '
            || 'START WITH ' || TO_CHAR(v_start)
            || ' INCREMENT BY 1 NOCACHE NOCYCLE';
    END IF;
END;
/

CREATE OR REPLACE PACKAGE AIRSPACE_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_FINISHED_AIRSPACE
    (
        p_CALLSIGN   IN T_FINISHFLIGHTS_AIRSPACE.CALLSIGN%TYPE DEFAULT NULL,
        p_P_TYPE     IN T_FINISHFLIGHTS_AIRSPACE.P_TYPE%TYPE DEFAULT NULL,
        p_FROM_AIRP  IN T_FINISHFLIGHTS_AIRSPACE.FROM_AIRP%TYPE DEFAULT NULL,
        p_TO_AIRP    IN T_FINISHFLIGHTS_AIRSPACE.TO_AIRP%TYPE DEFAULT NULL,
        p_ETD        IN T_FINISHFLIGHTS_AIRSPACE.ETD%TYPE DEFAULT NULL,
        p_ETA        IN T_FINISHFLIGHTS_AIRSPACE.ETA%TYPE DEFAULT NULL,
        p_ATD        IN T_FINISHFLIGHTS_AIRSPACE.ATD%TYPE DEFAULT NULL,
        p_ATA        IN T_FINISHFLIGHTS_AIRSPACE.ATA%TYPE DEFAULT NULL,
        p_VIA        IN T_FINISHFLIGHTS_AIRSPACE.VIA%TYPE DEFAULT NULL,
        p_REMARK     IN T_FINISHFLIGHTS_AIRSPACE.REMARK%TYPE DEFAULT NULL,
        p_OPER       IN T_FINISHFLIGHTS_AIRSPACE.OPER%TYPE DEFAULT NULL,
        p_REGIS      IN T_FINISHFLIGHTS_AIRSPACE.REGIS%TYPE DEFAULT NULL,
        p_PURPOSE    IN T_FINISHFLIGHTS_AIRSPACE.PURPOSE%TYPE DEFAULT NULL,
        p_RCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.RCRAFT%TYPE DEFAULT NULL,
        p_FCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.FCRAFT%TYPE DEFAULT NULL,
        p_FPLVIA     IN T_FINISHFLIGHTS_AIRSPACE.FPLVIA%TYPE DEFAULT NULL,
        p_ISACCEPTED IN NUMBER DEFAULT 0,
        p_CAT_HA     IN NUMBER DEFAULT 0,
        p_KHUNGGIO1  IN VARCHAR2 DEFAULT '0000',
        p_KHUNGGIO2  IN VARCHAR2 DEFAULT '2359',
        p_PageSize   IN INT DEFAULT 100,
        p_PageIndex  IN INT DEFAULT 0,
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        P_OUT_CURSOR OUT T_CURSOR
    );

    PROCEDURE INSERT_FINISHED_AIRSPACE
    (
        p_FLIGHTDATE IN VARCHAR2,
        p_OPER       IN T_FINISHFLIGHTS_AIRSPACE.OPER%TYPE DEFAULT NULL,
        p_CALLSIGN   IN T_FINISHFLIGHTS_AIRSPACE.CALLSIGN%TYPE,
        p_REGIS      IN T_FINISHFLIGHTS_AIRSPACE.REGIS%TYPE DEFAULT NULL,
        p_RCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.RCRAFT%TYPE DEFAULT NULL,
        p_FCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.FCRAFT%TYPE DEFAULT NULL,
        p_PURPOSE    IN T_FINISHFLIGHTS_AIRSPACE.PURPOSE%TYPE DEFAULT NULL,
        p_PTYPE      IN T_FINISHFLIGHTS_AIRSPACE.P_TYPE%TYPE DEFAULT NULL,
        p_FROM_AIRP  IN T_FINISHFLIGHTS_AIRSPACE.FROM_AIRP%TYPE DEFAULT NULL,
        p_TO_AIRP    IN T_FINISHFLIGHTS_AIRSPACE.TO_AIRP%TYPE DEFAULT NULL,
        p_ATD        IN T_FINISHFLIGHTS_AIRSPACE.ATD%TYPE DEFAULT NULL,
        p_ATA        IN T_FINISHFLIGHTS_AIRSPACE.ATA%TYPE DEFAULT NULL,
        p_VIA        IN T_FINISHFLIGHTS_AIRSPACE.VIA%TYPE DEFAULT NULL,
        p_FPLVIA     IN T_FINISHFLIGHTS_AIRSPACE.FPLVIA%TYPE DEFAULT NULL,
        p_ETD        IN T_FINISHFLIGHTS_AIRSPACE.ETD%TYPE DEFAULT NULL,
        p_ETA        IN T_FINISHFLIGHTS_AIRSPACE.ETA%TYPE DEFAULT NULL,
        p_USERCREATE IN T_FINISHFLIGHTS_AIRSPACE.USERCREATE%TYPE DEFAULT NULL,
        p_REMARK     IN T_FINISHFLIGHTS_AIRSPACE.REMARK%TYPE DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    );

    PROCEDURE UPDATE_FINISHED_AIRSPACE
    (
        p_FLIGHT_ID  IN T_FINISHFLIGHTS_AIRSPACE.FLIGHT_ID%TYPE,
        p_FLIGHTDATE IN VARCHAR2,
        p_OPER       IN T_FINISHFLIGHTS_AIRSPACE.OPER%TYPE DEFAULT NULL,
        p_CALLSIGN   IN T_FINISHFLIGHTS_AIRSPACE.CALLSIGN%TYPE,
        p_REGIS      IN T_FINISHFLIGHTS_AIRSPACE.REGIS%TYPE DEFAULT NULL,
        p_RCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.RCRAFT%TYPE DEFAULT NULL,
        p_FCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.FCRAFT%TYPE DEFAULT NULL,
        p_PURPOSE    IN T_FINISHFLIGHTS_AIRSPACE.PURPOSE%TYPE DEFAULT NULL,
        p_PTYPE      IN T_FINISHFLIGHTS_AIRSPACE.P_TYPE%TYPE DEFAULT NULL,
        p_FROM_AIRP  IN T_FINISHFLIGHTS_AIRSPACE.FROM_AIRP%TYPE DEFAULT NULL,
        p_TO_AIRP    IN T_FINISHFLIGHTS_AIRSPACE.TO_AIRP%TYPE DEFAULT NULL,
        p_ATD        IN T_FINISHFLIGHTS_AIRSPACE.ATD%TYPE DEFAULT NULL,
        p_ATA        IN T_FINISHFLIGHTS_AIRSPACE.ATA%TYPE DEFAULT NULL,
        p_VIA        IN T_FINISHFLIGHTS_AIRSPACE.VIA%TYPE DEFAULT NULL,
        p_FPLVIA     IN T_FINISHFLIGHTS_AIRSPACE.FPLVIA%TYPE DEFAULT NULL,
        p_ETD        IN T_FINISHFLIGHTS_AIRSPACE.ETD%TYPE DEFAULT NULL,
        p_ETA        IN T_FINISHFLIGHTS_AIRSPACE.ETA%TYPE DEFAULT NULL,
        p_USERCREATE IN T_FINISHFLIGHTS_AIRSPACE.USERCREATE%TYPE DEFAULT NULL,
        p_REMARK     IN T_FINISHFLIGHTS_AIRSPACE.REMARK%TYPE DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    );

    PROCEDURE DELETE_FINISHED_AIRSPACE
    (
        p_FLIGHT_ID  IN T_FINISHFLIGHTS_AIRSPACE.FLIGHT_ID%TYPE,
        p_USER       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    );

    PROCEDURE SUBMIT_FINISHED_AIRSPACE
    (
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        p_User       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    );

    PROCEDURE APPROVE_FINISHED_AIRSPACE
    (
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        p_User       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    );

    PROCEDURE EXPORT_AIRSPACE_MESSAGE
    (
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        p_User       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    );
END AIRSPACE_PKG;
/

CREATE OR REPLACE PACKAGE BODY AIRSPACE_PKG AS
    FUNCTION PARSE_DATE(p_value IN VARCHAR2) RETURN DATE IS
    BEGIN
        RETURN TO_DATE(TRIM(p_value), 'FXDD-MM-YYYY');
    EXCEPTION
        WHEN OTHERS THEN
            RETURN TO_DATE(TRIM(p_value), 'FXYYYY/MM/DD');
    END PARSE_DATE;

    PROCEDURE WRITE_ERROR(p_action IN VARCHAR2) IS
    BEGIN
        PROCESS_PKG.ADD_ERROR_LOG(
            p_action,
            SQLCODE,
            SUBSTR(
                SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                1,
                200
            )
        );
    EXCEPTION
        WHEN OTHERS THEN
            NULL;
    END WRITE_ERROR;

    PROCEDURE GET_FINISHED_AIRSPACE
    (
        p_CALLSIGN   IN T_FINISHFLIGHTS_AIRSPACE.CALLSIGN%TYPE DEFAULT NULL,
        p_P_TYPE     IN T_FINISHFLIGHTS_AIRSPACE.P_TYPE%TYPE DEFAULT NULL,
        p_FROM_AIRP  IN T_FINISHFLIGHTS_AIRSPACE.FROM_AIRP%TYPE DEFAULT NULL,
        p_TO_AIRP    IN T_FINISHFLIGHTS_AIRSPACE.TO_AIRP%TYPE DEFAULT NULL,
        p_ETD        IN T_FINISHFLIGHTS_AIRSPACE.ETD%TYPE DEFAULT NULL,
        p_ETA        IN T_FINISHFLIGHTS_AIRSPACE.ETA%TYPE DEFAULT NULL,
        p_ATD        IN T_FINISHFLIGHTS_AIRSPACE.ATD%TYPE DEFAULT NULL,
        p_ATA        IN T_FINISHFLIGHTS_AIRSPACE.ATA%TYPE DEFAULT NULL,
        p_VIA        IN T_FINISHFLIGHTS_AIRSPACE.VIA%TYPE DEFAULT NULL,
        p_REMARK     IN T_FINISHFLIGHTS_AIRSPACE.REMARK%TYPE DEFAULT NULL,
        p_OPER       IN T_FINISHFLIGHTS_AIRSPACE.OPER%TYPE DEFAULT NULL,
        p_REGIS      IN T_FINISHFLIGHTS_AIRSPACE.REGIS%TYPE DEFAULT NULL,
        p_PURPOSE    IN T_FINISHFLIGHTS_AIRSPACE.PURPOSE%TYPE DEFAULT NULL,
        p_RCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.RCRAFT%TYPE DEFAULT NULL,
        p_FCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.FCRAFT%TYPE DEFAULT NULL,
        p_FPLVIA     IN T_FINISHFLIGHTS_AIRSPACE.FPLVIA%TYPE DEFAULT NULL,
        p_ISACCEPTED IN NUMBER DEFAULT 0,
        p_CAT_HA     IN NUMBER DEFAULT 0,
        p_KHUNGGIO1  IN VARCHAR2 DEFAULT '0000',
        p_KHUNGGIO2  IN VARCHAR2 DEFAULT '2359',
        p_PageSize   IN INT DEFAULT 100,
        p_PageIndex  IN INT DEFAULT 0,
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
        v_first_index PLS_INTEGER;
        v_last_index  PLS_INTEGER;
        v_page_size   PLS_INTEGER;
        v_page_index  PLS_INTEGER;
        v_start_date  DATE;
        v_finish_date DATE;
    BEGIN
        v_page_size := GREATEST(NVL(p_PageSize, 100), 1);
        v_page_index := GREATEST(NVL(p_PageIndex, 0), 0);
        v_first_index := (v_page_size * v_page_index) + 1;
        v_last_index := v_page_size * (v_page_index + 1);
        v_start_date := PARSE_DATE(p_StartDate);
        v_finish_date := PARSE_DATE(p_FinishDate);

        IF v_finish_date < v_start_date THEN
            RAISE_APPLICATION_ERROR(-20001, 'P_FINISHDATE must be >= P_STARTDATE');
        END IF;

        OPEN P_OUT_CURSOR FOR
            SELECT page_data.*
              FROM
              (
                  SELECT COUNT(*) OVER () AS SUMRECORD,
                         t.*,
                         ROW_NUMBER() OVER
                         (
                             ORDER BY t.FLIGHTDATE DESC,
                                      t.CALLSIGN,
                                      t.FROM_AIRP,
                                      t.TO_AIRP,
                                      t.FLIGHT_ID
                         ) AS RNUM
                    FROM T_FINISHFLIGHTS_AIRSPACE t
                   WHERE t.FLIGHTDATE >= v_start_date
                     AND t.FLIGHTDATE < v_finish_date + 1
                     AND (NVL(p_ISACCEPTED, 0) = -1
                          OR t.ISACCEPTED = NVL(p_ISACCEPTED, 0))
                     AND (p_CALLSIGN IS NULL OR UPPER(t.CALLSIGN)
                          LIKE '%' || UPPER(TRIM(p_CALLSIGN)) || '%')
                     AND (p_P_TYPE IS NULL OR UPPER(t.P_TYPE)
                          = UPPER(TRIM(p_P_TYPE)))
                     AND (p_FROM_AIRP IS NULL OR UPPER(t.FROM_AIRP)
                          LIKE '%' || UPPER(TRIM(p_FROM_AIRP)) || '%')
                     AND (p_TO_AIRP IS NULL OR UPPER(t.TO_AIRP)
                          LIKE '%' || UPPER(TRIM(p_TO_AIRP)) || '%')
                     AND (p_ETD IS NULL OR UPPER(t.ETD)
                          LIKE '%' || UPPER(TRIM(p_ETD)) || '%')
                     AND (p_ETA IS NULL OR UPPER(t.ETA)
                          LIKE '%' || UPPER(TRIM(p_ETA)) || '%')
                     AND (p_ATD IS NULL OR UPPER(t.ATD)
                          LIKE '%' || UPPER(TRIM(p_ATD)) || '%')
                     AND (p_ATA IS NULL OR UPPER(t.ATA)
                          LIKE '%' || UPPER(TRIM(p_ATA)) || '%')
                     AND (p_VIA IS NULL OR UPPER(t.VIA)
                          LIKE '%' || UPPER(TRIM(p_VIA)) || '%')
                     AND (p_REMARK IS NULL OR UPPER(t.REMARK)
                          LIKE '%' || UPPER(TRIM(p_REMARK)) || '%')
                     AND (p_OPER IS NULL OR UPPER(t.OPER)
                          LIKE '%' || UPPER(TRIM(p_OPER)) || '%')
                     AND (p_REGIS IS NULL OR UPPER(t.REGIS)
                          LIKE '%' || UPPER(TRIM(p_REGIS)) || '%')
                     AND (p_PURPOSE IS NULL OR UPPER(t.PURPOSE)
                          = UPPER(TRIM(p_PURPOSE)))
                     AND (p_RCRAFT IS NULL OR UPPER(t.RCRAFT)
                          = UPPER(TRIM(p_RCRAFT)))
                     AND (p_FCRAFT IS NULL OR UPPER(t.FCRAFT)
                          = UPPER(TRIM(p_FCRAFT)))
                     AND (p_FPLVIA IS NULL OR UPPER(t.FPLVIA)
                          LIKE '%' || UPPER(TRIM(p_FPLVIA)) || '%')
                     AND
                     (
                         NVL(p_CAT_HA, 0) = 0
                         OR (p_CAT_HA = 1 AND SUBSTR(TRIM(t.ETD), 1, 4)
                             BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2)
                         OR (p_CAT_HA = 2 AND SUBSTR(TRIM(t.ETA), 1, 4)
                             BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2)
                         OR (p_CAT_HA = 3 AND SUBSTR(TRIM(t.ATD), 1, 4)
                             BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2)
                         OR (p_CAT_HA = 4 AND SUBSTR(TRIM(t.ATA), 1, 4)
                             BETWEEN p_KHUNGGIO1 AND p_KHUNGGIO2)
                     )
              ) page_data
             WHERE page_data.RNUM BETWEEN v_first_index AND v_last_index
             ORDER BY page_data.RNUM;
    EXCEPTION
        WHEN OTHERS THEN
            WRITE_ERROR('GET_FINISHED_AIRSPACE');
            RAISE;
    END GET_FINISHED_AIRSPACE;

    PROCEDURE INSERT_FINISHED_AIRSPACE
    (
        p_FLIGHTDATE IN VARCHAR2,
        p_OPER       IN T_FINISHFLIGHTS_AIRSPACE.OPER%TYPE DEFAULT NULL,
        p_CALLSIGN   IN T_FINISHFLIGHTS_AIRSPACE.CALLSIGN%TYPE,
        p_REGIS      IN T_FINISHFLIGHTS_AIRSPACE.REGIS%TYPE DEFAULT NULL,
        p_RCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.RCRAFT%TYPE DEFAULT NULL,
        p_FCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.FCRAFT%TYPE DEFAULT NULL,
        p_PURPOSE    IN T_FINISHFLIGHTS_AIRSPACE.PURPOSE%TYPE DEFAULT NULL,
        p_PTYPE      IN T_FINISHFLIGHTS_AIRSPACE.P_TYPE%TYPE DEFAULT NULL,
        p_FROM_AIRP  IN T_FINISHFLIGHTS_AIRSPACE.FROM_AIRP%TYPE DEFAULT NULL,
        p_TO_AIRP    IN T_FINISHFLIGHTS_AIRSPACE.TO_AIRP%TYPE DEFAULT NULL,
        p_ATD        IN T_FINISHFLIGHTS_AIRSPACE.ATD%TYPE DEFAULT NULL,
        p_ATA        IN T_FINISHFLIGHTS_AIRSPACE.ATA%TYPE DEFAULT NULL,
        p_VIA        IN T_FINISHFLIGHTS_AIRSPACE.VIA%TYPE DEFAULT NULL,
        p_FPLVIA     IN T_FINISHFLIGHTS_AIRSPACE.FPLVIA%TYPE DEFAULT NULL,
        p_ETD        IN T_FINISHFLIGHTS_AIRSPACE.ETD%TYPE DEFAULT NULL,
        p_ETA        IN T_FINISHFLIGHTS_AIRSPACE.ETA%TYPE DEFAULT NULL,
        p_USERCREATE IN T_FINISHFLIGHTS_AIRSPACE.USERCREATE%TYPE DEFAULT NULL,
        p_REMARK     IN T_FINISHFLIGHTS_AIRSPACE.REMARK%TYPE DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    )
    IS
        v_flight_date DATE;
    BEGIN
        v_flight_date := PARSE_DATE(p_FLIGHTDATE);

        INSERT INTO T_FINISHFLIGHTS_AIRSPACE
        (
            FLIGHT_ID, FLIGHTDATE, OPER, CALLSIGN, REGIS, RCRAFT, FCRAFT,
            PURPOSE, P_TYPE, FROM_AIRP, TO_AIRP, ATD, ATA, VIA, FPLVIA,
            ETD, ETA, USERCREATE, DATECREATE, REMARK, ISACCEPTED
        )
        VALUES
        (
            SEQ_FINISHFLIGHTS_AIRSPACE.NEXTVAL, v_flight_date,
            TRIM(p_OPER), UPPER(TRIM(p_CALLSIGN)), UPPER(TRIM(p_REGIS)),
            UPPER(TRIM(p_RCRAFT)), UPPER(TRIM(p_FCRAFT)),
            UPPER(TRIM(p_PURPOSE)), UPPER(TRIM(p_PTYPE)),
            UPPER(TRIM(p_FROM_AIRP)), UPPER(TRIM(p_TO_AIRP)),
            TRIM(p_ATD), TRIM(p_ATA), UPPER(TRIM(p_VIA)),
            UPPER(TRIM(p_FPLVIA)), TRIM(p_ETD), TRIM(p_ETA),
            NVL(TRIM(p_USERCREATE), ' '),
            TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS'),
            TRIM(p_REMARK), 0
        );

        p_ReturnCode := 1;
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            WRITE_ERROR('INSERT_FINISHED_AIRSPACE');
            p_ReturnCode := -1;
    END INSERT_FINISHED_AIRSPACE;

    PROCEDURE UPDATE_FINISHED_AIRSPACE
    (
        p_FLIGHT_ID  IN T_FINISHFLIGHTS_AIRSPACE.FLIGHT_ID%TYPE,
        p_FLIGHTDATE IN VARCHAR2,
        p_OPER       IN T_FINISHFLIGHTS_AIRSPACE.OPER%TYPE DEFAULT NULL,
        p_CALLSIGN   IN T_FINISHFLIGHTS_AIRSPACE.CALLSIGN%TYPE,
        p_REGIS      IN T_FINISHFLIGHTS_AIRSPACE.REGIS%TYPE DEFAULT NULL,
        p_RCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.RCRAFT%TYPE DEFAULT NULL,
        p_FCRAFT     IN T_FINISHFLIGHTS_AIRSPACE.FCRAFT%TYPE DEFAULT NULL,
        p_PURPOSE    IN T_FINISHFLIGHTS_AIRSPACE.PURPOSE%TYPE DEFAULT NULL,
        p_PTYPE      IN T_FINISHFLIGHTS_AIRSPACE.P_TYPE%TYPE DEFAULT NULL,
        p_FROM_AIRP  IN T_FINISHFLIGHTS_AIRSPACE.FROM_AIRP%TYPE DEFAULT NULL,
        p_TO_AIRP    IN T_FINISHFLIGHTS_AIRSPACE.TO_AIRP%TYPE DEFAULT NULL,
        p_ATD        IN T_FINISHFLIGHTS_AIRSPACE.ATD%TYPE DEFAULT NULL,
        p_ATA        IN T_FINISHFLIGHTS_AIRSPACE.ATA%TYPE DEFAULT NULL,
        p_VIA        IN T_FINISHFLIGHTS_AIRSPACE.VIA%TYPE DEFAULT NULL,
        p_FPLVIA     IN T_FINISHFLIGHTS_AIRSPACE.FPLVIA%TYPE DEFAULT NULL,
        p_ETD        IN T_FINISHFLIGHTS_AIRSPACE.ETD%TYPE DEFAULT NULL,
        p_ETA        IN T_FINISHFLIGHTS_AIRSPACE.ETA%TYPE DEFAULT NULL,
        p_USERCREATE IN T_FINISHFLIGHTS_AIRSPACE.USERCREATE%TYPE DEFAULT NULL,
        p_REMARK     IN T_FINISHFLIGHTS_AIRSPACE.REMARK%TYPE DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    )
    IS
        v_flight_date DATE;
    BEGIN
        v_flight_date := PARSE_DATE(p_FLIGHTDATE);

        UPDATE T_FINISHFLIGHTS_AIRSPACE
           SET FLIGHTDATE = v_flight_date,
               OPER = TRIM(p_OPER),
               CALLSIGN = UPPER(TRIM(p_CALLSIGN)),
               REGIS = UPPER(TRIM(p_REGIS)),
               RCRAFT = UPPER(TRIM(p_RCRAFT)),
               FCRAFT = UPPER(TRIM(p_FCRAFT)),
               PURPOSE = UPPER(TRIM(p_PURPOSE)),
               P_TYPE = UPPER(TRIM(p_PTYPE)),
               FROM_AIRP = UPPER(TRIM(p_FROM_AIRP)),
               TO_AIRP = UPPER(TRIM(p_TO_AIRP)),
               ATD = TRIM(p_ATD),
               ATA = TRIM(p_ATA),
               VIA = UPPER(TRIM(p_VIA)),
               FPLVIA = UPPER(TRIM(p_FPLVIA)),
               ETD = TRIM(p_ETD),
               ETA = TRIM(p_ETA),
               USERCREATE = NVL(TRIM(p_USERCREATE), USERCREATE),
               DATECREATE = TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS'),
               REMARK = TRIM(p_REMARK)
         WHERE FLIGHT_ID = p_FLIGHT_ID
           AND ISACCEPTED = 0;

        p_ReturnCode := SQL%ROWCOUNT;
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            WRITE_ERROR('UPDATE_FINISHED_AIRSPACE');
            p_ReturnCode := -1;
    END UPDATE_FINISHED_AIRSPACE;

    PROCEDURE DELETE_FINISHED_AIRSPACE
    (
        p_FLIGHT_ID  IN T_FINISHFLIGHTS_AIRSPACE.FLIGHT_ID%TYPE,
        p_USER       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    )
    IS
    BEGIN
        DELETE FROM T_FINISHFLIGHTS_AIRSPACE
         WHERE FLIGHT_ID = p_FLIGHT_ID
           AND ISACCEPTED = 0;

        p_ReturnCode := SQL%ROWCOUNT;

        IF p_ReturnCode > 0 THEN
            INSERT INTO T_ACTIONHISTORY
            (
                USERID, FULLNAME, HOSTIP, DATEMODIFY,
                ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
            )
            VALUES
            (
                0, NVL(p_USER, ' '), ' ', SYSDATE,
                'DELETE FINISHFLIGHTS AIRSPACE', p_FLIGHT_ID,
                'DELETE NEW AIRSPACE FLIGHT', 0
            );
        END IF;

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            WRITE_ERROR('DELETE_FINISHED_AIRSPACE');
            p_ReturnCode := -1;
    END DELETE_FINISHED_AIRSPACE;

    PROCEDURE SUBMIT_FINISHED_AIRSPACE
    (
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        p_User       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    )
    IS
        v_start_date  DATE;
        v_finish_date DATE;
    BEGIN
        v_start_date := PARSE_DATE(p_StartDate);
        v_finish_date := PARSE_DATE(p_FinishDate);

        IF v_finish_date < v_start_date THEN
            RAISE_APPLICATION_ERROR(-20001, 'P_FINISHDATE must be >= P_STARTDATE');
        END IF;

        UPDATE T_FINISHFLIGHTS_AIRSPACE
           SET ISACCEPTED = 2
         WHERE FLIGHTDATE >= v_start_date
           AND FLIGHTDATE < v_finish_date + 1
           AND ISACCEPTED = 0;

        p_ReturnCode := SQL%ROWCOUNT;

        INSERT INTO T_ACTIONHISTORY
        (
            USERID, FULLNAME, HOSTIP, DATEMODIFY,
            ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
        )
        VALUES
        (
            0, NVL(p_User, ' '), ' ', SYSDATE,
            'SUBMIT FINISHFLIGHTS AIRSPACE', 0,
            p_StartDate || ' - ' || p_FinishDate
                || '; ROWS=' || TO_CHAR(p_ReturnCode),
            0
        );

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            WRITE_ERROR('SUBMIT_FINISHED_AIRSPACE');
            p_ReturnCode := -1;
    END SUBMIT_FINISHED_AIRSPACE;

    PROCEDURE APPROVE_FINISHED_AIRSPACE
    (
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        p_User       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    )
    IS
        v_start_date  DATE;
        v_finish_date DATE;
    BEGIN
        v_start_date := PARSE_DATE(p_StartDate);
        v_finish_date := PARSE_DATE(p_FinishDate);

        IF v_finish_date < v_start_date THEN
            RAISE_APPLICATION_ERROR(-20001, 'P_FINISHDATE must be >= P_STARTDATE');
        END IF;

        UPDATE T_FINISHFLIGHTS_AIRSPACE
           SET ISACCEPTED = 1
         WHERE FLIGHTDATE >= v_start_date
           AND FLIGHTDATE < v_finish_date + 1
           AND ISACCEPTED = 2;

        p_ReturnCode := SQL%ROWCOUNT;

        INSERT INTO T_ACTIONHISTORY
        (
            USERID, FULLNAME, HOSTIP, DATEMODIFY,
            ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
        )
        VALUES
        (
            0, NVL(p_User, ' '), ' ', SYSDATE,
            'APPROVE FINISHFLIGHTS AIRSPACE', 0,
            p_StartDate || ' - ' || p_FinishDate
                || '; ROWS=' || TO_CHAR(p_ReturnCode),
            0
        );

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            WRITE_ERROR('APPROVE_FINISHED_AIRSPACE');
            p_ReturnCode := -1;
    END APPROVE_FINISHED_AIRSPACE;

    PROCEDURE EXPORT_AIRSPACE_MESSAGE
    (
        p_StartDate  IN VARCHAR2,
        p_FinishDate IN VARCHAR2,
        p_User       IN VARCHAR2 DEFAULT NULL,
        p_ReturnCode OUT NUMBER
    )
    IS
        TYPE t_text_parts IS TABLE OF VARCHAR2(32767)
            INDEX BY PLS_INTEGER;

        v_parts         t_text_parts;
        v_part_ids      t_text_parts;
        v_start_date    DATE;
        v_finish_date   DATE;
        v_line          VARCHAR2(32767);
        v_content       VARCHAR2(32767);
        v_part_count    PLS_INTEGER;
        v_row_number    PLS_INTEGER;
        v_total_flights PLS_INTEGER := 0;
        v_total_parts   PLS_INTEGER := 0;

        FUNCTION CLEAN_TEXT
        (
            p_value      IN VARCHAR2,
            p_max_length IN PLS_INTEGER
        )
        RETURN VARCHAR2
        IS
        BEGIN
            RETURN SUBSTR(
                REPLACE(
                    REPLACE(
                        NVL(TRIM(p_value), '-'),
                        CHR(13),
                        ' '
                    ),
                    CHR(10),
                    ' '
                ),
                1,
                p_max_length
            );
        END CLEAN_TEXT;
    BEGIN
        v_start_date := PARSE_DATE(p_StartDate);
        v_finish_date := PARSE_DATE(p_FinishDate);

        IF v_finish_date < v_start_date THEN
            RAISE_APPLICATION_ERROR(-20001, 'P_FINISHDATE must be >= P_STARTDATE');
        END IF;

        -- Chỉ thay điện văn Airspace chưa gửi trong đúng khoảng ngày export.
        DELETE FROM T_PLAN_MESSAGE
         WHERE STATUS = 0
           AND MESS_TYPE = 'AIRSPACE MESSAGE'
           AND FLIGHTDATE >= v_start_date
           AND FLIGHTDATE < v_finish_date + 1;

        FOR d IN
        (
            SELECT TRUNC(FLIGHTDATE) AS FLIGHT_DATE
              FROM T_FINISHFLIGHTS_AIRSPACE
             WHERE ISACCEPTED = 1
               AND FLIGHTDATE >= v_start_date
               AND FLIGHTDATE < v_finish_date + 1
             GROUP BY TRUNC(FLIGHTDATE)
             ORDER BY TRUNC(FLIGHTDATE)
        )
        LOOP
            v_parts.DELETE;
            v_part_ids.DELETE;
            v_part_count := 1;
            v_row_number := 0;
            v_parts(1) := NULL;
            v_part_ids(1) := NULL;

            FOR f IN
            (
                SELECT FLIGHT_ID,
                       CALLSIGN,
                       REGIS,
                       PURPOSE,
                       FROM_AIRP,
                       TO_AIRP,
                       ETD,
                       ETA,
                       VIA,
                       FPLVIA,
                       REMARK
                  FROM T_FINISHFLIGHTS_AIRSPACE
                 WHERE ISACCEPTED = 1
                   AND FLIGHTDATE >= d.FLIGHT_DATE
                   AND FLIGHTDATE < d.FLIGHT_DATE + 1
                 ORDER BY CALLSIGN,
                          FROM_AIRP,
                          TO_AIRP,
                          ETD,
                          FLIGHT_ID
            )
            LOOP
                v_row_number := v_row_number + 1;

                v_line :=
                       TO_CHAR(v_row_number)
                    || ' '
                    || CLEAN_TEXT(f.REGIS, 10)
                    || ' '
                    || CLEAN_TEXT(f.CALLSIGN, 12)
                    || ' '
                    || CLEAN_TEXT(f.FROM_AIRP, 4)
                    || ' '
                    || CLEAN_TEXT(f.TO_AIRP, 4)
                    || ' '
                    || CLEAN_TEXT(f.ETD, 6)
                    || ' '
                    || CLEAN_TEXT(f.ETA, 6)
                    || ' '
                    || CLEAN_TEXT(f.PURPOSE, 10)
                    || ' '
                    || CLEAN_TEXT(f.VIA, 120)
                    || ' '
                    || CLEAN_TEXT(f.FPLVIA, 120)
                    || ' '
                    || CLEAN_TEXT(f.REMARK, 900);

                v_line := RTRIM(SUBSTR(v_line, 1, 1400)) || CHR(10);

                IF v_parts(v_part_count) IS NOT NULL
                   AND LENGTHB(v_parts(v_part_count))
                       + LENGTHB(v_line) > 1650
                THEN
                    v_part_count := v_part_count + 1;
                    v_parts(v_part_count) := NULL;
                    v_part_ids(v_part_count) := NULL;
                END IF;

                v_parts(v_part_count) :=
                    v_parts(v_part_count) || v_line;

                v_part_ids(v_part_count) :=
                    CASE
                        WHEN v_part_ids(v_part_count) IS NULL
                        THEN TO_CHAR(f.FLIGHT_ID)
                        ELSE v_part_ids(v_part_count)
                             || ',' || TO_CHAR(f.FLIGHT_ID)
                    END;
            END LOOP;

            IF v_row_number > 0 THEN
                FOR i IN 1 .. v_part_count
                LOOP
                    v_content :=
                           'PART ' || TO_CHAR(i)
                        || ' OF ' || TO_CHAR(v_part_count)
                        || CHR(10)
                        || 'FPL ON:'
                        || TO_CHAR(
                               d.FLIGHT_DATE,
                               'DD-MON-YYYY',
                               'NLS_DATE_LANGUAGE=ENGLISH'
                           )
                        || ': AIRSPACE MESSAGE'
                        || CHR(10)
                        || v_parts(i)
                        || CHR(10)
                        || 'NNNN';

                    IF LENGTHB(v_content) > 2000 THEN
                        RAISE_APPLICATION_ERROR(
                            -20002,
                            'AIRSPACE MESSAGE content exceeds 2000 bytes'
                        );
                    END IF;

                    INSERT INTO T_PLAN_MESSAGE
                    (
                        FLIGHTDATE,
                        PART_NO,
                        CONTENT,
                        MESS_TYPE,
                        STATUS,
                        LISTFLIGHTID
                    )
                    VALUES
                    (
                        d.FLIGHT_DATE,
                        i,
                        v_content,
                        'AIRSPACE MESSAGE',
                        0,
                        v_part_ids(i)
                    );

                    v_total_parts := v_total_parts + 1;
                END LOOP;

                v_total_flights := v_total_flights + v_row_number;
            END IF;
        END LOOP;

        INSERT INTO T_ACTIONHISTORY
        (
            USERID, FULLNAME, HOSTIP, DATEMODIFY,
            ACTIONSCODE, NEWS_ID, NOTES, MENU_ID
        )
        VALUES
        (
            0, NVL(p_User, ' '), ' ', SYSDATE,
            'EXPORT AIRSPACE MESSAGE', 0,
            p_StartDate || ' - ' || p_FinishDate
                || '; FLIGHTS=' || TO_CHAR(v_total_flights)
                || '; PARTS=' || TO_CHAR(v_total_parts),
            0
        );

        p_ReturnCode := v_total_flights;
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            WRITE_ERROR('EXPORT_AIRSPACE_MESSAGE');
            p_ReturnCode := -1;
    END EXPORT_AIRSPACE_MESSAGE;
END AIRSPACE_PKG;
/

BEGIN
    DBMS_STATS.GATHER_TABLE_STATS(
        ownname          => USER,
        tabname          => 'T_FINISHFLIGHTS_AIRSPACE',
        cascade          => TRUE,
        estimate_percent => DBMS_STATS.AUTO_SAMPLE_SIZE
    );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
       (
           'T_FINISHFLIGHTS_AIRSPACE',
           'SEQ_FINISHFLIGHTS_AIRSPACE',
           'AIRSPACE_PKG'
       )
 ORDER BY OBJECT_TYPE, OBJECT_NAME;
