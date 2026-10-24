-- ============================================================================
-- Seed dữ liệu Airspace từ 10 ngày gần nhất có dữ liệu Military.
-- Có thể chạy lại: các dòng đã seed với cùng khóa nghiệp vụ sẽ không bị chèn lại.
-- Toàn bộ dữ liệu seed ở trạng thái ISACCEPTED = 0 (CHƯA GỬI DUYỆT).
-- Marker rollback: SEED_AIRSPACE_10D_20260728
-- ============================================================================

INSERT INTO T_FINISHFLIGHTS_AIRSPACE
(
    FLIGHTDATE,
    CALLSIGN,
    P_TYPE,
    FROM_AIRP,
    TO_AIRP,
    ETD,
    ETA,
    ATD,
    ATA,
    VIA,
    REMARK,
    FLIGHT_ID,
    OPER,
    REGIS,
    RCRAFT,
    FCRAFT,
    PURPOSE,
    FPLVIA,
    USERCREATE,
    DATECREATE,
    ISACCEPTED
)
WITH selected_days AS
(
    SELECT flight_day
      FROM
      (
          SELECT DISTINCT TRUNC(FLIGHTDATE) AS flight_day
            FROM T_FINISHFLIGHTS_MILITARY
           WHERE FLIGHTDATE IS NOT NULL
           ORDER BY TRUNC(FLIGHTDATE) DESC
      )
     WHERE ROWNUM <= 10
),
source_rows AS
(
    SELECT m.*
      FROM T_FINISHFLIGHTS_MILITARY m
      JOIN selected_days d
        ON d.flight_day = TRUNC(m.FLIGHTDATE)
)
SELECT
    s.FLIGHTDATE,
    s.CALLSIGN,
    s.P_TYPE,
    s.FROM_AIRP,
    s.TO_AIRP,
    s.ETD,
    s.ETA,
    s.ATD,
    s.ATA,
    s.VIA,
    s.REMARK,
    SEQ_FINISHFLIGHTS_AIRSPACE.NEXTVAL,
    s.OPER,
    s.REGIS,
    s.RCRAFT,
    s.FCRAFT,
    s.PURPOSE,
    s.FPLVIA,
    'SEED_AIRSPACE_10D_20260728',
    TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS'),
    0
  FROM source_rows s
 WHERE NOT EXISTS
       (
           SELECT 1
             FROM T_FINISHFLIGHTS_AIRSPACE a
            WHERE a.USERCREATE = 'SEED_AIRSPACE_10D_20260728'
              AND a.FLIGHTDATE = s.FLIGHTDATE
              AND NVL(a.CALLSIGN, '#') = NVL(s.CALLSIGN, '#')
              AND NVL(a.REGIS, '#') = NVL(s.REGIS, '#')
              AND NVL(a.FROM_AIRP, '#') = NVL(s.FROM_AIRP, '#')
              AND NVL(a.TO_AIRP, '#') = NVL(s.TO_AIRP, '#')
              AND NVL(a.ETD, '#') = NVL(s.ETD, '#')
              AND NVL(a.ETA, '#') = NVL(s.ETA, '#')
       );

COMMIT;

BEGIN
    DBMS_STATS.GATHER_TABLE_STATS(
        ownname          => USER,
        tabname          => 'T_FINISHFLIGHTS_AIRSPACE',
        cascade          => TRUE,
        estimate_percent => DBMS_STATS.AUTO_SAMPLE_SIZE
    );
END;
/

SELECT TO_CHAR(TRUNC(FLIGHTDATE), 'DD-MM-YYYY') AS FLIGHT_DAY,
       COUNT(*) AS ROW_COUNT,
       MIN(ISACCEPTED) AS MIN_STATUS,
       MAX(ISACCEPTED) AS MAX_STATUS
  FROM T_FINISHFLIGHTS_AIRSPACE
 WHERE USERCREATE = 'SEED_AIRSPACE_10D_20260728'
 GROUP BY TRUNC(FLIGHTDATE)
 ORDER BY TRUNC(FLIGHTDATE);

SELECT COUNT(*) AS TOTAL_SEEDED_ROWS
  FROM T_FINISHFLIGHTS_AIRSPACE
 WHERE USERCREATE = 'SEED_AIRSPACE_10D_20260728';

