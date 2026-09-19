-- Optimize FLIGHT_DAYFLIGHT.View_Message without changing its public signature.
-- Target: ATFM@PDBORCL (Oracle 12c)
-- Run as schema owner ATFM in SQL*Plus.
-- The script is idempotent for the intended index and procedure rewrite.

set serveroutput on size unlimited
set pagesize 200
set linesize 240
set timing on
whenever sqlerror exit sql.sqlcode rollback

prompt [1/6] Check current indexes and create the query-specific index as INVISIBLE
declare
    l_named_index_count number;
    l_equivalent_count  number := 0;
    l_equivalent_name   user_indexes.index_name%type;
    l_index_ddl         varchar2(32767);
    l_normalized_ddl    varchar2(32767);
begin
    select count(*)
      into l_named_index_count
      from user_indexes
     where index_name = 'IX_TPM_VIEW_MESSAGE';

    -- USER_IND_EXPRESSIONS.COLUMN_EXPRESSION is LONG on Oracle 12c, so inspect
    -- the short index DDL instead of applying string functions to that column.
    for i in (
        select index_name
          from user_indexes
         where table_name = 'T_PLAN_MESSAGE'
           and index_type <> 'LOB'
    ) loop
        l_index_ddl := dbms_lob.substr(
            dbms_metadata.get_ddl('INDEX', i.index_name), 32767, 1);
        l_normalized_ddl := upper(l_index_ddl);
        l_normalized_ddl := replace(l_normalized_ddl, '"', '');
        l_normalized_ddl := replace(l_normalized_ddl, ' ', '');
        l_normalized_ddl := replace(l_normalized_ddl, chr(9), '');
        l_normalized_ddl := replace(l_normalized_ddl, chr(10), '');
        l_normalized_ddl := replace(l_normalized_ddl, chr(13), '');

        if instr(l_normalized_ddl,
            'T_PLAN_MESSAGE(TRUNC(FLIGHTDATE),MESS_TYPE,PART_NO,ID,STATUS)') > 0 then
            l_equivalent_count := l_equivalent_count + 1;
            l_equivalent_name := i.index_name;
        end if;
    end loop;

    if l_equivalent_count > 0
       and l_equivalent_name = 'IX_TPM_VIEW_MESSAGE' then
        dbms_output.put_line('Equivalent index already exists: ' || l_equivalent_name);
    elsif l_equivalent_count > 0 then
        raise_application_error(-20004,
            'Equivalent index already exists as ' || l_equivalent_name ||
            '; review it instead of creating a duplicate.');
    elsif l_named_index_count > 0 then
        raise_application_error(-20001,
            'IX_TPM_VIEW_MESSAGE exists but does not have the expected definition.');
    else
        execute immediate
            'create index IX_TPM_VIEW_MESSAGE on T_PLAN_MESSAGE ' ||
            '(TRUNC(FLIGHTDATE), MESS_TYPE, PART_NO, ID, STATUS) invisible';
        dbms_output.put_line('Created IX_TPM_VIEW_MESSAGE as INVISIBLE.');
    end if;
end;
/

prompt [2/6] Validate the execution plan while the new index is INVISIBLE
alter session set optimizer_use_invisible_indexes = true;
delete from plan_table where statement_id = 'TPM_VIEW_MSG_PRE';
explain plan set statement_id = 'TPM_VIEW_MSG_PRE' for
select id,
       part_no,
       mess_type,
       case status when 1 then 'Red' when 0 then '#000' end as status
  from t_plan_message
 where trunc(flightdate) = date '2026-07-11'
 order by mess_type, part_no;

select plan_table_output
  from table(dbms_xplan.display(null, 'TPM_VIEW_MSG_PRE', 'BASIC +PREDICATE'));

prompt [3/6] Rewrite only View_Message inside the existing package body
declare
    l_source       clob;
    l_replacement  varchar2(32767) := q'~
    PROCEDURE View_Message(p_date varchar2, P_OUT_CURSOR out T_CURSOR) is
        v_flight_day date;
    begin
        v_flight_day := to_date(trim(p_date), 'FXDD/MM/YYYY');

        open P_OUT_CURSOR for
            select ID,
                   PART_NO,
                   MESS_TYPE,
                   case STATUS
                       when 1 then 'Red'
                       when 0 then '#000'
                   end as Status
              from T_PLAN_MESSAGE
             where trunc(FLIGHTDATE) = v_flight_day
             order by MESS_TYPE asc, PART_NO asc;
    end View_Message;
~';
    l_in_target    boolean := false;
    l_match_count  number := 0;
    l_cursor       integer := null;
    l_result       integer;

    procedure append_text(p_text varchar2) is
    begin
        dbms_lob.writeappend(l_source, length(p_text), p_text);
    end;
begin
    dbms_lob.createtemporary(l_source, true);
    append_text('create or replace ');

    for r in (
        select text
          from user_source
         where name = 'FLIGHT_DAYFLIGHT'
           and type = 'PACKAGE BODY'
         order by line
    ) loop
        if not l_in_target
           and regexp_like(r.text,
               '^\s*PROCEDURE\s+View_Message\s*\(', 'i') then
            l_match_count := l_match_count + 1;
            l_in_target := true;
            append_text(l_replacement);
        elsif l_in_target then
            if regexp_like(r.text,
                '^\s*end\s+View_Message\s*;', 'i') then
                l_in_target := false;
            end if;
        else
            append_text(r.text);
        end if;
    end loop;

    if l_match_count <> 1 or l_in_target then
        raise_application_error(-20002,
            'Expected exactly one complete View_Message procedure in FLIGHT_DAYFLIGHT.');
    end if;

    l_cursor := dbms_sql.open_cursor;
    dbms_sql.parse(l_cursor, l_source, dbms_sql.native);
    l_result := dbms_sql.execute(l_cursor);
    dbms_sql.close_cursor(l_cursor);
    dbms_lob.freetemporary(l_source);
    dbms_output.put_line('Recompiled FLIGHT_DAYFLIGHT with optimized View_Message.');
exception
    when others then
        if l_cursor is not null and dbms_sql.is_open(l_cursor) then
            dbms_sql.close_cursor(l_cursor);
        end if;
        if dbms_lob.istemporary(l_source) = 1 then
            dbms_lob.freetemporary(l_source);
        end if;
        raise;
end;
/

prompt [4/6] Stop immediately if the package body has compilation errors
declare
    l_error_count number;
begin
    select count(*)
      into l_error_count
      from user_errors
     where name = 'FLIGHT_DAYFLIGHT'
       and type = 'PACKAGE BODY';

    if l_error_count > 0 then
        for r in (
            select line, position, text
              from user_errors
             where name = 'FLIGHT_DAYFLIGHT'
               and type = 'PACKAGE BODY'
             order by sequence
        ) loop
            dbms_output.put_line(
                'Line ' || r.line || ':' || r.position || ' - ' || r.text);
        end loop;
        raise_application_error(-20003,
            'FLIGHT_DAYFLIGHT package body compilation failed. Run rollback script.');
    end if;
end;
/

prompt [5/6] Publish the validated index and gather its optimizer statistics
declare
    l_visibility user_indexes.visibility%type;
begin
    select visibility
      into l_visibility
      from user_indexes
     where index_name = 'IX_TPM_VIEW_MESSAGE';

    if l_visibility = 'INVISIBLE' then
        execute immediate 'alter index IX_TPM_VIEW_MESSAGE visible';
    end if;
end;
/

begin
    dbms_stats.gather_index_stats(
        ownname => user,
        indname => 'IX_TPM_VIEW_MESSAGE');
end;
/

alter session set optimizer_use_invisible_indexes = false;

prompt [6/6] Verify object status and the final execution plan
select object_name, object_type, status
  from user_objects
 where object_name in ('FLIGHT_DAYFLIGHT', 'IX_TPM_VIEW_MESSAGE')
 order by object_type, object_name;

select index_name, index_type, uniqueness, status, visibility
  from user_indexes
 where table_name = 'T_PLAN_MESSAGE'
 order by index_name;

delete from plan_table where statement_id = 'TPM_VIEW_MSG_POST';
explain plan set statement_id = 'TPM_VIEW_MSG_POST' for
select id,
       part_no,
       mess_type,
       case status when 1 then 'Red' when 0 then '#000' end as status
  from t_plan_message
 where trunc(flightdate) = date '2026-07-11'
 order by mess_type, part_no;

select plan_table_output
  from table(dbms_xplan.display(null, 'TPM_VIEW_MSG_POST', 'BASIC +PREDICATE'));

commit;
prompt Deployment completed successfully.
