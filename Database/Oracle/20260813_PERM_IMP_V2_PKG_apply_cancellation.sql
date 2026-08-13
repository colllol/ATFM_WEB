-- Toi uu buoc XAC NHAN HUY CHUYEN cua ImportsPermSC_LD_V2.
-- Khong sua PERM_IMP_PKG.impToPerm_Huy va ConvertPermScToSchedule_Delete cu.

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TSDF_CANCEL_MATCH';

    IF v_count > 0 THEN
        EXECUTE IMMEDIATE 'DROP INDEX IX_TSDF_CANCEL_MATCH';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TSDF_CANCEL_MATCH_V2';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_TSDF_CANCEL_MATCH_V2 ON T_SCHEDULE_DAYFLIGHTS '
            || '(FLIGHTNBR, FROM_AIRP, TO_AIRP, ETD, FLIGHTDATE, ISRENDER, ETA) ONLINE';
    END IF;
END;
/

CREATE OR REPLACE PACKAGE PERM_IMP_V2_PKG AS
    TYPE T_CURSOR IS REF CURSOR;

    PROCEDURE GET_CANCELLATION_MATCHES(P_OUT OUT T_CURSOR);

    PROCEDURE APPLY_CANCELLATIONS
    (
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    );
END PERM_IMP_V2_PKG;
/

CREATE OR REPLACE PACKAGE BODY PERM_IMP_V2_PKG AS
    PROCEDURE GET_CANCELLATION_MATCHES(P_OUT OUT T_CURSOR) IS
    BEGIN
        OPEN P_OUT FOR
            SELECT DISTINCT
                   i.ID AS STAGING_ID, d.FLIGHTNBR AS CALLSIGN, m.PERMNBR_ID,
                   d.FROM_AIRP, d.TO_AIRP, d.ETD, d.ETA, m.OPER_ID AS OPER,
                   m.SEASON, m.PERMTYPE, d.REMARK, d.PURPOSE_ID AS PURPOSE,
                   d.FLIGHT_PK AS IDPERMDETAIL, i."PERMNBR" AS THAMCHIEU,
                   d.BEGINDATE AS FROMDATE, d.ENDDATE AS TODATE,
                   TO_DATE(i.FROMDATE, 'DD-MM-YYYY') AS HUY_FROMDATE,
                   TO_DATE(i.TODATE, 'DD-MM-YYYY') AS HUY_TODATE,
                   REPLACE(
                       d.DAY1 || d.DAY2 || d.DAY3 || d.DAY4 ||
                       d.DAY5 || d.DAY6 || d.DAY7,
                       '0', '.'
                   ) AS DAILY_PHEP,
                   i.DAILY, 'HUY' AS LOAIPHEP
              FROM T_PERMSC_IMP i
              JOIN T_PERMDETAIL_SC d
                ON UPPER(TRIM(d.FLIGHTNBR)) = UPPER(TRIM(i.CALLSIGN))
               AND UPPER(TRIM(d.FROM_AIRP)) = UPPER(TRIM(i.FROM_AIRP))
               AND UPPER(TRIM(d.TO_AIRP)) = UPPER(TRIM(i.TO_AIRP))
               AND TRIM(d.ETD) = TRIM(i.ETD)
               AND (
                    d.ETA IS NULL OR TRIM(d.ETA) IS NULL OR
                    i.ETA IS NULL OR TRIM(d.ETA) = TRIM(i.ETA)
               )
               AND d.STATUS IN (0, 1)
               AND d.BEGINDATE <= TO_DATE(i.TODATE, 'DD-MM-YYYY')
               AND d.ENDDATE >= TO_DATE(i.FROMDATE, 'DD-MM-YYYY')
               AND (INSTR(i.DAILY, '1') = 0 OR d.DAY1 = '1')
               AND (INSTR(i.DAILY, '2') = 0 OR d.DAY2 = '2')
               AND (INSTR(i.DAILY, '3') = 0 OR d.DAY3 = '3')
               AND (INSTR(i.DAILY, '4') = 0 OR d.DAY4 = '4')
               AND (INSTR(i.DAILY, '5') = 0 OR d.DAY5 = '5')
               AND (INSTR(i.DAILY, '6') = 0 OR d.DAY6 = '6')
               AND (INSTR(i.DAILY, '7') = 0 OR d.DAY7 = '7')
              JOIN T_PERMMASTER_SC m
                ON m.PERM_ID = d.PERM_ID
             WHERE i.ACTION = 'HuyChuyen'
             ORDER BY STAGING_ID, FROMDATE, PERMNBR_ID;
    END GET_CANCELLATION_MATCHES;

    PROCEDURE APPLY_CANCELLATIONS
    (
        P_STAGING_IDS IN VARCHAR2,
        P_OUT         OUT NUMBER
    ) IS
        v_selected_count PLS_INTEGER;
        v_updated_count  PLS_INTEGER;
        v_error          VARCHAR2(2000);
    BEGIN
        P_OUT := -1;

        IF P_STAGING_IDS IS NULL OR
           NOT REGEXP_LIKE(TRIM(P_STAGING_IDS), '^[0-9]+(,[0-9]+)*$') THEN
            RAISE_APPLICATION_ERROR(-20831, 'Danh sach staging ID khong hop le');
        END IF;

        SELECT COUNT(*)
          INTO v_selected_count
          FROM T_PERMSC_IMP i
         WHERE i.ACTION = 'HuyChuyen'
           AND INSTR(
                   ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                   ',' || TO_CHAR(i.ID) || ','
               ) > 0;

        IF v_selected_count = 0 THEN
            RAISE_APPLICATION_ERROR(-20832, 'Khong con du lieu huy chuyen de xu ly');
        END IF;

        SAVEPOINT APPLY_CANCEL_V2;

        -- Mot master cho moi so phep; khong lap lai tren tung chuyen bay.
        FOR h IN
        (
            SELECT *
              FROM
              (
                  SELECT i.*,
                         FNCREATEPERMSC_NBR_ID(
                             i.AUTHOR,
                             i.PERMTYPE,
                             i."PERMNBR",
                             i.SEASON,
                             CASE
                                 WHEN i.PERMDATE IS NULL THEN TRUNC(SYSDATE)
                                 ELSE TO_DATE(i.PERMDATE, 'DD-MM-YYYY')
                             END
                         ) AS GENERATED_PERMNBR_ID,
                         ROW_NUMBER() OVER
                         (
                             PARTITION BY FNCREATEPERMSC_NBR_ID(
                                 i.AUTHOR,
                                 i.PERMTYPE,
                                 i."PERMNBR",
                                 i.SEASON,
                                 CASE
                                     WHEN i.PERMDATE IS NULL THEN TRUNC(SYSDATE)
                                     ELSE TO_DATE(i.PERMDATE, 'DD-MM-YYYY')
                                 END
                             )
                             ORDER BY i.ID
                         ) AS RN
                    FROM T_PERMSC_IMP i
                   WHERE i.ACTION = 'HuyChuyen'
                     AND INSTR(
                             ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                             ',' || TO_CHAR(i.ID) || ','
                         ) > 0
              )
             WHERE RN = 1
        ) LOOP
            INSERT INTO T_PERMMASTER_SC
            (
                PERMNBR_ID, AUTHOR_ID, PERMTYPE, FLIGHTTYPE,
                PERMNBR, VERSION, PERMDATE, OPER_ID,
                REFERENCE, VALIDHOURS, LASTUSER,
                BILLINGADDRESS, PERMCONTENT, SEASON
            )
            SELECT h.GENERATED_PERMNBR_ID,
                   h.AUTHOR,
                   h.PERMTYPE,
                   h.FLIGHTTYPE,
                   h."PERMNBR",
                   NVL(NULLIF(TRIM(h.VERSION), ''), 'A'),
                   CASE
                       WHEN h.PERMDATE IS NULL THEN TRUNC(SYSDATE)
                       ELSE TO_DATE(h.PERMDATE, 'DD-MM-YYYY')
                   END,
                   h.OPER,
                   h.REFERENCE,
                   CASE WHEN h.PERMTYPE = 'LD' THEN 24 ELSE 72 END,
                   'SYSTEM',
                   h.BILLINGADDRESS,
                   h.PERMCONTENT,
                   h.SEASON
              FROM DUAL
             WHERE NOT EXISTS
                   (
                       SELECT 1
                         FROM T_PERMMASTER_SC m
                        WHERE TRIM(m.PERMNBR_ID) = TRIM(h.GENERATED_PERMNBR_ID)
                   );
        END LOOP;

        -- Insert theo tap; DAILY rut gon 26/37/2457 duoc tach dung bang INSTR.
        INSERT INTO T_PERMDETAIL_SC
        (
            PERM_ID, PURPOSE_ID, CRAFT_ID, FLIGHTNBR, REGISTRATION,
            DAY1, DAY2, DAY3, DAY4, DAY5, DAY6, DAY7,
            FROM_AIRP, TO_AIRP, ETD, ETA, VIA,
            STATUS, LASTUSER, BEGINDATE, ENDDATE, REMARK
        )
        SELECT m.PERM_ID,
               i.PURPOSE,
               (
                   SELECT MIN(c.CRAFT_ID)
                     FROM M_CRAFT_TYPE c
                    WHERE TRIM(c.MA) = TRIM(REPLACE(LPAD(i.CRAFT, 4), '/', ''))
               ),
               i.CALLSIGN,
               i.REGISTRATION,
               CASE WHEN INSTR(i.DAILY, '1') > 0 THEN '1' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '2') > 0 THEN '2' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '3') > 0 THEN '3' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '4') > 0 THEN '4' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '5') > 0 THEN '5' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '6') > 0 THEN '6' ELSE '0' END,
               CASE WHEN INSTR(i.DAILY, '7') > 0 THEN '7' ELSE '0' END,
               i.FROM_AIRP,
               i.TO_AIRP,
               TRIM(i.ETD),
               TRIM(i.ETA),
               i.VIA,
               '2',
               'SYSTEM',
               TO_DATE(i.FROMDATE, 'DD-MM-YYYY'),
               TO_DATE(i.TODATE, 'DD-MM-YYYY'),
               'PHEP HUY ' || i.REMARK || ' '
          FROM T_PERMSC_IMP i
          JOIN T_PERMMASTER_SC m
            ON TRIM(m.PERMNBR_ID) = TRIM(
                FNCREATEPERMSC_NBR_ID(
                    i.AUTHOR,
                    i.PERMTYPE,
                    i."PERMNBR",
                    i.SEASON,
                    CASE
                        WHEN i.PERMDATE IS NULL THEN TRUNC(SYSDATE)
                        ELSE TO_DATE(i.PERMDATE, 'DD-MM-YYYY')
                    END
                )
            )
         WHERE i.ACTION = 'HuyChuyen'
           AND INSTR(
                   ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                   ',' || TO_CHAR(i.ID) || ','
               ) > 0
           AND NOT EXISTS
               (
                   SELECT 1
                     FROM T_PERMDETAIL_SC d
                    WHERE d.PERM_ID = m.PERM_ID
                      AND TRIM(d.FLIGHTNBR) = TRIM(i.CALLSIGN)
                      AND TRIM(d.FROM_AIRP) = TRIM(i.FROM_AIRP)
                      AND TRIM(d.TO_AIRP) = TRIM(i.TO_AIRP)
                      AND TRIM(d.ETD) = TRIM(i.ETD)
                      AND NVL(TRIM(d.ETA), '#') = NVL(TRIM(i.ETA), '#')
                      AND d.BEGINDATE = TO_DATE(i.FROMDATE, 'DD-MM-YYYY')
                      AND d.ENDDATE = TO_DATE(i.TODATE, 'DD-MM-YYYY')
                      AND d.STATUS = '2'
                      AND d.DAY1 = CASE WHEN INSTR(i.DAILY, '1') > 0 THEN '1' ELSE '0' END
                      AND d.DAY2 = CASE WHEN INSTR(i.DAILY, '2') > 0 THEN '2' ELSE '0' END
                      AND d.DAY3 = CASE WHEN INSTR(i.DAILY, '3') > 0 THEN '3' ELSE '0' END
                      AND d.DAY4 = CASE WHEN INSTR(i.DAILY, '4') > 0 THEN '4' ELSE '0' END
                      AND d.DAY5 = CASE WHEN INSTR(i.DAILY, '5') > 0 THEN '5' ELSE '0' END
                      AND d.DAY6 = CASE WHEN INSTR(i.DAILY, '6') > 0 THEN '6' ELSE '0' END
                      AND d.DAY7 = CASE WHEN INSTR(i.DAILY, '7') > 0 THEN '7' ELSE '0' END
               );

        -- Mot UPDATE theo tap thay cho COUNT + UPDATE + COMMIT tren tung ngay.
        MERGE INTO T_SCHEDULE_DAYFLIGHTS s
        USING
        (
            SELECT schedule_id, MIN(permnbr) AS permnbr
              FROM
              (
                  SELECT s2.ID AS schedule_id, i."PERMNBR" AS permnbr
                    FROM T_PERMSC_IMP i
                    JOIN T_SCHEDULE_DAYFLIGHTS s2
                      ON s2.FLIGHTNBR = i.CALLSIGN
                     AND s2.FROM_AIRP = i.FROM_AIRP
                     AND s2.TO_AIRP = i.TO_AIRP
                     AND s2.ETD = TRIM(i.ETD)
                     AND (
                         s2.ETA IS NULL OR TRIM(s2.ETA) IS NULL OR
                         i.ETA IS NULL OR TRIM(s2.ETA) = TRIM(i.ETA)
                     )
                     AND s2.FLIGHTDATE >= TO_DATE(i.FROMDATE, 'DD-MM-YYYY')
                     AND s2.FLIGHTDATE < TO_DATE(i.TODATE, 'DD-MM-YYYY') + 1
                     AND s2.ISRENDER IN (0, 1)
                     AND INSTR(
                         i.DAILY,
                         TO_CHAR(TRUNC(s2.FLIGHTDATE) - TRUNC(s2.FLIGHTDATE, 'IW') + 1)
                     ) > 0
                   WHERE i.ACTION = 'HuyChuyen'
                     AND INSTR(
                             ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                             ',' || TO_CHAR(i.ID) || ','
                         ) > 0
              )
             GROUP BY schedule_id
        ) x
           ON (s.ID = x.schedule_id)
        WHEN MATCHED THEN
            UPDATE SET s.STATUS = '1',
                       s.ISRENDER = 1,
                       s.REMARK = 'HUY CHUYEN THEO PHEP ' || x.permnbr;

        v_updated_count := SQL%ROWCOUNT;

        DELETE FROM T_PERMSC_IMP i
         WHERE i.ACTION = 'HuyChuyen'
           AND INSTR(
                   ',' || REPLACE(P_STAGING_IDS, ' ', '') || ',',
                   ',' || TO_CHAR(i.ID) || ','
               ) > 0;

        COMMIT;
        P_OUT := 1;

        BEGIN
            PROCESS_PKG.ADD_ERROR_LOG(
                'PERM_IMP_V2_APPLY_OK',
                v_selected_count,
                'Updated schedule rows=' || v_updated_count
            );
        EXCEPTION
            WHEN OTHERS THEN NULL;
        END;
    EXCEPTION
        WHEN OTHERS THEN
            v_error := SUBSTR(
                SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
                1,
                2000
            );

            BEGIN
                ROLLBACK TO APPLY_CANCEL_V2;
            EXCEPTION
                WHEN OTHERS THEN NULL;
            END;

            BEGIN
                PROCESS_PKG.ADD_ERROR_LOG(
                    'PERM_IMP_V2_APPLY_ERROR',
                    SQLCODE,
                    SUBSTR(v_error, 1, 200)
                );
            EXCEPTION
                WHEN OTHERS THEN NULL;
            END;
            P_OUT := -1;
    END APPLY_CANCELLATIONS;
END PERM_IMP_V2_PKG;
/

SHOW ERRORS PACKAGE PERM_IMP_V2_PKG;
SHOW ERRORS PACKAGE BODY PERM_IMP_V2_PKG;

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERM_IMP_V2_PKG'
 ORDER BY OBJECT_TYPE;

SELECT INDEX_NAME, STATUS
  FROM USER_INDEXES
 WHERE INDEX_NAME = 'IX_TSDF_CANCEL_MATCH_V2';
