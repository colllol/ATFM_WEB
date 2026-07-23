-- Archive all T_DAY_FLIGHTS rows, then delete source rows before 2026.
-- Target: ATFM@PDBORCL (Oracle 12c). Run as schema owner ATFM in SQL*Plus.
--
-- IMPORTANT:
--   1. Stop IIS/API/jobs that insert, update or delete T_DAY_FLIGHTS.
--   2. Take an RMAN/database backup before running this script.
--   3. Ensure tablespace USERS has enough free space.
--   4. CTAS copies columns and data, but not indexes, triggers, foreign keys,
--      column defaults or comments. This is intentional for an archive table.
--   5. DELETE releases rows but does not automatically shrink the source
--      table/index segments. Space reclamation must be planned separately.
--
-- Measured on 2026-07-22 before creating this script:
--   T_DAY_FLIGHTS rows             : 11,340,890
--   Rows before 2026-01-01         :  9,941,629
--   Rows from 2026-01-01           :  1,399,261
--   T_DAY_FLIGHTS table segment    : ~2,426 MB
--   T_DAY_FLIGHTS index segments   :   ~944 MB
--   USERS free space               : ~4,898 MB

set serveroutput on size unlimited
set pagesize 200
set linesize 240
set timing on
set verify off
whenever sqlerror exit sql.sqlcode rollback

-- Safety lock: change NO to YES only during the approved maintenance window.
define CONFIRM_APPLICATION_STOPPED = 'NO'
define CONFIRM_ARCHIVE_AND_DELETE = 'NO'
define DELETE_BATCH_SIZE = 100000

prompt [1/7] Preflight validation
declare
    l_application_stopped varchar2(10) := upper(trim('&&CONFIRM_APPLICATION_STOPPED'));
    l_confirm             varchar2(10) := upper(trim('&&CONFIRM_ARCHIVE_AND_DELETE'));
    l_backup_exists       number;
    l_source_rows         number;
    l_old_rows            number;
    l_null_dates          number;
    l_source_bytes        number;
    l_free_bytes          number;
begin
    if l_application_stopped <> 'YES' or l_confirm <> 'YES' then
        raise_application_error(-20401,
            'Execution not confirmed. Stop the application and set both confirmation flags to YES.');
    end if;

    select count(*) into l_backup_exists
      from user_tables where table_name = 'T_DAY_FLIGHTS_BAK2026';
    if l_backup_exists > 0 then
        raise_application_error(-20402,
            'T_DAY_FLIGHTS_BAK2026 already exists. No object was changed.');
    end if;

    select count(*),
           sum(case when flightdate < date '2026-01-01' then 1 else 0 end),
           sum(case when flightdate is null then 1 else 0 end)
      into l_source_rows, l_old_rows, l_null_dates
      from t_day_flights;

    select bytes into l_source_bytes
      from user_segments
     where segment_name = 'T_DAY_FLIGHTS'
       and segment_type = 'TABLE';

    select sum(bytes) into l_free_bytes
      from user_free_space
     where tablespace_name = (
         select tablespace_name from user_tables
          where table_name = 'T_DAY_FLIGHTS');

    -- Keep at least 20% headroom above the current source table segment.
    if nvl(l_free_bytes, 0) < l_source_bytes * 1.20 then
        raise_application_error(-20403,
            'Insufficient tablespace headroom. Required at least ' ||
            round(l_source_bytes * 1.20 / 1024 / 1024) ||
            ' MB; available ' || round(nvl(l_free_bytes, 0) / 1024 / 1024) || ' MB.');
    end if;

    dbms_output.put_line('SOURCE_ROWS=' || l_source_rows);
    dbms_output.put_line('ROWS_BEFORE_2026=' || l_old_rows);
    dbms_output.put_line('NULL_FLIGHTDATE_ROWS=' || l_null_dates);
    dbms_output.put_line('SOURCE_SEGMENT_MB=' || round(l_source_bytes / 1024 / 1024, 2));
    dbms_output.put_line('TABLESPACE_FREE_MB=' || round(l_free_bytes / 1024 / 1024, 2));
end;
/

prompt [2/7] Create the archive table and copy the complete source snapshot
create table T_DAY_FLIGHTS_BAK2026
tablespace USERS
as
select *
  from T_DAY_FLIGHTS;

comment on table T_DAY_FLIGHTS_BAK2026 is
    'Full T_DAY_FLIGHTS snapshot created before deleting FLIGHTDATE values earlier than 2026-01-01';

prompt [3/7] Verify the complete archive before allowing any delete
declare
    l_source_rows       number;
    l_backup_rows       number;
    l_source_old_rows   number;
    l_backup_old_rows   number;
    l_source_null_dates number;
    l_backup_null_dates number;
    l_source_min_id     number;
    l_backup_min_id     number;
    l_source_max_id     number;
    l_backup_max_id     number;
    l_source_min_date   date;
    l_backup_min_date   date;
    l_source_max_date   date;
    l_backup_max_date   date;
begin
    select count(*),
           sum(case when flightdate < date '2026-01-01' then 1 else 0 end),
           sum(case when flightdate is null then 1 else 0 end),
           min(flight_id), max(flight_id), min(flightdate), max(flightdate)
      into l_source_rows, l_source_old_rows, l_source_null_dates,
           l_source_min_id, l_source_max_id, l_source_min_date, l_source_max_date
      from T_DAY_FLIGHTS;

    select count(*),
           sum(case when flightdate < date '2026-01-01' then 1 else 0 end),
           sum(case when flightdate is null then 1 else 0 end),
           min(flight_id), max(flight_id), min(flightdate), max(flightdate)
      into l_backup_rows, l_backup_old_rows, l_backup_null_dates,
           l_backup_min_id, l_backup_max_id, l_backup_min_date, l_backup_max_date
      from T_DAY_FLIGHTS_BAK2026;

    if l_source_rows <> l_backup_rows
       or l_source_old_rows <> l_backup_old_rows
       or l_source_null_dates <> l_backup_null_dates
       or nvl(l_source_min_id, -1) <> nvl(l_backup_min_id, -1)
       or nvl(l_source_max_id, -1) <> nvl(l_backup_max_id, -1)
       or nvl(l_source_min_date, date '0001-01-01') <>
          nvl(l_backup_min_date, date '0001-01-01')
       or nvl(l_source_max_date, date '0001-01-01') <>
          nvl(l_backup_max_date, date '0001-01-01') then
        raise_application_error(-20404,
            'Archive verification failed. T_DAY_FLIGHTS was NOT deleted. ' ||
            'Keep both tables for investigation.');
    end if;

    dbms_output.put_line('ARCHIVE_VERIFIED_ROWS=' || l_backup_rows);
    dbms_output.put_line('ARCHIVE_VERIFIED_ROWS_BEFORE_2026=' || l_backup_old_rows);
    dbms_output.put_line('ARCHIVE_MIN_FLIGHT_ID=' || l_backup_min_id);
    dbms_output.put_line('ARCHIVE_MAX_FLIGHT_ID=' || l_backup_max_id);
end;
/

prompt [4/7] Delete pre-2026 rows from T_DAY_FLIGHTS in controlled batches
declare
    l_batch_size   pls_integer := &&DELETE_BATCH_SIZE;
    l_batch_rows   pls_integer;
    l_total_rows   number := 0;
    l_batch_number number := 0;
begin
    if l_batch_size < 1000 or l_batch_size > 500000 then
        raise_application_error(-20405,
            'DELETE_BATCH_SIZE must be between 1,000 and 500,000.');
    end if;

    loop
        delete from T_DAY_FLIGHTS
         where rowid in (
             select rowid
               from T_DAY_FLIGHTS
              where flightdate < date '2026-01-01'
                and rownum <= l_batch_size);

        l_batch_rows := sql%rowcount;
        commit;

        l_total_rows := l_total_rows + l_batch_rows;
        l_batch_number := l_batch_number + 1;

        if mod(l_batch_number, 10) = 0 or l_batch_rows = 0 then
            dbms_output.put_line(
                'BATCH=' || l_batch_number ||
                '; LAST_ROWS=' || l_batch_rows ||
                '; TOTAL_DELETED=' || l_total_rows);
        end if;

        exit when l_batch_rows = 0;
    end loop;

    dbms_output.put_line('DELETE_COMPLETED_TOTAL=' || l_total_rows);
exception
    when others then
        rollback;
        dbms_output.put_line(
            'DELETE_STOPPED_AFTER_COMMITTED_ROWS=' || l_total_rows);
        raise;
end;
/

prompt [5/7] Verify the source retention boundary
declare
    l_old_rows       number;
    l_remaining_rows number;
    l_backup_rows    number;
begin
    select count(*) into l_old_rows
      from T_DAY_FLIGHTS
     where flightdate < date '2026-01-01';

    select count(*) into l_remaining_rows from T_DAY_FLIGHTS;
    select count(*) into l_backup_rows from T_DAY_FLIGHTS_BAK2026;

    if l_old_rows <> 0 then
        raise_application_error(-20406,
            'Post-delete verification failed: ' || l_old_rows ||
            ' rows before 2026 remain in T_DAY_FLIGHTS.');
    end if;

    dbms_output.put_line('SOURCE_ROWS_BEFORE_2026=' || l_old_rows);
    dbms_output.put_line('SOURCE_REMAINING_ROWS=' || l_remaining_rows);
    dbms_output.put_line('ARCHIVE_TOTAL_ROWS=' || l_backup_rows);
end;
/

prompt [6/7] Refresh optimizer statistics
begin
    dbms_stats.gather_table_stats(
        ownname => user,
        tabname => 'T_DAY_FLIGHTS',
        estimate_percent => dbms_stats.auto_sample_size,
        method_opt => 'FOR ALL COLUMNS SIZE AUTO',
        cascade => true);

    dbms_stats.gather_table_stats(
        ownname => user,
        tabname => 'T_DAY_FLIGHTS_BAK2026',
        estimate_percent => dbms_stats.auto_sample_size,
        method_opt => 'FOR ALL COLUMNS SIZE AUTO',
        cascade => false);
end;
/

prompt [7/7] Final report
select table_name, num_rows, blocks, avg_row_len, tablespace_name, last_analyzed
  from user_tables
 where table_name in ('T_DAY_FLIGHTS', 'T_DAY_FLIGHTS_BAK2026')
 order by table_name;

select 'T_DAY_FLIGHTS' table_name,
       count(*) total_rows,
       sum(case when flightdate < date '2026-01-01' then 1 else 0 end) rows_before_2026,
       sum(case when flightdate >= date '2026-01-01' then 1 else 0 end) rows_from_2026,
       sum(case when flightdate is null then 1 else 0 end) null_flightdate
  from T_DAY_FLIGHTS
union all
select 'T_DAY_FLIGHTS_BAK2026',
       count(*),
       sum(case when flightdate < date '2026-01-01' then 1 else 0 end),
       sum(case when flightdate >= date '2026-01-01' then 1 else 0 end),
       sum(case when flightdate is null then 1 else 0 end)
  from T_DAY_FLIGHTS_BAK2026;

prompt Archive and retention process completed successfully.

-- Optional recovery template (manual review is mandatory before use):
-- insert into T_DAY_FLIGHTS
-- select b.*
--   from T_DAY_FLIGHTS_BAK2026 b
--  where b.flightdate < date '2026-01-01'
--    and not exists (
--        select 1 from T_DAY_FLIGHTS s where s.flight_id = b.flight_id);
-- commit;
