/*
  Optional supporting indexes for PERM_PKG.PAcceptedPermSC.
  Date: 2026-07-31

  Findings before creation:
    - T_SCHEDULE_DAYFLIGHTS: about 6.95 million rows / 1.44 GB.
    - DELETE by PERM_ID + FLIGHT_PK + ISRENDER used TABLE ACCESS FULL,
      optimizer cost 51071.
    - T_PERMDETAIL_SC: about 414 thousand rows; lookup by FLIGHT_PK also used
      TABLE ACCESS FULL, optimizer cost 1983.

  Expected size of IX_SCHEDULE_PERM_FLIGHT_RENDER is in the same range as the
  current schedule indexes (roughly 150-300 MB). Create during a controlled
  deployment window. ONLINE is used to reduce blocking, but it still consumes
  CPU, I/O, redo and temporary space while building.

  This script has NOT been deployed automatically.
*/

CREATE INDEX IX_SCHEDULE_PERM_FLIGHT_RENDER
    ON T_SCHEDULE_DAYFLIGHTS (PERM_ID, FLIGHT_PK, ISRENDER)
    ONLINE;

CREATE INDEX IX_PERMDETAIL_SC_FLIGHT_PK
    ON T_PERMDETAIL_SC (FLIGHT_PK)
    ONLINE;

BEGIN
    DBMS_STATS.GATHER_INDEX_STATS
    (
        ownname => USER,
        indname => 'IX_SCHEDULE_PERM_FLIGHT_RENDER'
    );

    DBMS_STATS.GATHER_INDEX_STATS
    (
        ownname => USER,
        indname => 'IX_PERMDETAIL_SC_FLIGHT_PK'
    );
END;
/

/* Verify access paths after creation:

EXPLAIN PLAN SET STATEMENT_ID = 'ACC_DELETE_INDEXED' FOR
DELETE FROM T_SCHEDULE_DAYFLIGHTS s
WHERE s.ISRENDER = 0
  AND s.PERM_ID = 202724
  AND s.FLIGHT_PK = 1332540;

SELECT plan_table_output
FROM TABLE
(
    DBMS_XPLAN.DISPLAY
    (
        NULL,
        'ACC_DELETE_INDEXED',
        'BASIC +PREDICATE +COST'
    )
);

EXPLAIN PLAN SET STATEMENT_ID = 'ACC_DETAIL_INDEXED' FOR
SELECT *
FROM T_PERMDETAIL_SC
WHERE FLIGHT_PK = 1332540;

SELECT plan_table_output
FROM TABLE
(
    DBMS_XPLAN.DISPLAY
    (
        NULL,
        'ACC_DETAIL_INDEXED',
        'BASIC +PREDICATE +COST'
    )
);
*/

/* Rollback indexes:

DROP INDEX IX_SCHEDULE_PERM_FLIGHT_RENDER ONLINE;
DROP INDEX IX_PERMDETAIL_SC_FLIGHT_PK ONLINE;
*/
