-- Optimize the FLIGHT_DAYFLIGHT read procedures for T_PLAN_MESSAGE_NEW.
-- Target: ATFM@PDBORCL (Oracle 12c). Run as schema owner ATFM in SQL*Plus.

set serveroutput on size unlimited
set pagesize 200
set linesize 240
set timing on
whenever sqlerror exit sql.sqlcode rollback

prompt [1/6] Check for a duplicate index and create the new index as INVISIBLE
declare
    l_named_count      number;
    l_equivalent_count number := 0;
    l_equivalent_name  user_indexes.index_name%type;
    l_index_ddl        varchar2(32767);
    l_normalized_ddl   varchar2(32767);
begin
    select count(*) into l_named_count
      from user_indexes
     where index_name = 'IX_TPMN_VIEW_MESSAGE';

    for i in (
        select index_name
          from user_indexes
         where table_name = 'T_PLAN_MESSAGE_NEW'
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
            'T_PLAN_MESSAGE_NEW(TRUNC(FLIGHTDATE),MESS_TYPE,PART_NO,ID,STATUS)') > 0 then
            l_equivalent_count := l_equivalent_count + 1;
            l_equivalent_name := i.index_name;
        end if;
    end loop;

    if l_equivalent_count > 0
       and l_equivalent_name = 'IX_TPMN_VIEW_MESSAGE' then
        dbms_output.put_line('Equivalent index already exists: ' || l_equivalent_name);
    elsif l_equivalent_count > 0 then
        raise_application_error(-20101,
            'Equivalent index already exists as ' || l_equivalent_name ||
            '; review it instead of creating a duplicate.');
    elsif l_named_count > 0 then
        raise_application_error(-20102,
            'IX_TPMN_VIEW_MESSAGE exists with an unexpected definition.');
    else
        execute immediate
            'create index IX_TPMN_VIEW_MESSAGE on T_PLAN_MESSAGE_NEW ' ||
            '(TRUNC(FLIGHTDATE), MESS_TYPE, PART_NO, ID, STATUS) invisible';
        dbms_output.put_line('Created IX_TPMN_VIEW_MESSAGE as INVISIBLE.');
    end if;
end;
/

prompt [2/6] Validate the index plan before publishing it
alter session set optimizer_use_invisible_indexes = true;
delete from plan_table where statement_id = 'TPMN_VIEW_PRE';
explain plan set statement_id = 'TPMN_VIEW_PRE' for
select part_no,
       mess_type,
       case status when 1 then 'Red' when 0 then '#000' end as status
  from t_plan_message_new
 where trunc(flightdate) = date '2026-07-11'
   and length(mess_type) > 4
 order by mess_type, part_no;

select plan_table_output
  from table(dbms_xplan.display(null, 'TPMN_VIEW_PRE', 'BASIC +PREDICATE'));

prompt [3/6] Rewrite the five date-based read procedures in FLIGHT_DAYFLIGHT
declare
    l_source       clob;
    l_target_name  varchar2(128);
    l_match_count  number := 0;
    l_cursor       integer := null;
    l_result       integer;

    l_airport varchar2(32767) := q'~
    PROCEDURE View_MessageAirPort(p_date varchar2, P_OUT_CURSOR out T_CURSOR) is
        v_flight_day date;
    begin
        v_flight_day := to_date(trim(p_date), 'FXDD/MM/YYYY');
        open P_OUT_CURSOR for
            select ID, PART_NO, MESS_TYPE,
                   case STATUS when 1 then 'Red' when 0 then '#000' end as Status
              from T_PLAN_MESSAGE_NEW
             where trunc(FLIGHTDATE) = v_flight_day
               and length(MESS_TYPE) = 4
             order by Status desc, MESS_TYPE asc, PART_NO asc;
    end View_MessageAirPort;
~';

    l_airport_2023 varchar2(32767) := q'~
    PROCEDURE View_MessageAirPort2023(p_date varchar2, P_MESSTYPE varchar2,
                                      P_OUT_CURSOR out T_CURSOR) is
        v_flight_day date;
    begin
        v_flight_day := to_date(trim(p_date), 'FXDD/MM/YYYY');
        open P_OUT_CURSOR for
            select ID, PART_NO, MESS_TYPE,
                   case STATUS when 1 then 'Red' when 0 then '#000' end as Status
              from T_PLAN_MESSAGE_NEW
             where trunc(FLIGHTDATE) = v_flight_day
               and length(MESS_TYPE) = 4
               and MESS_TYPE = P_MESSTYPE
             order by Status desc, MESS_TYPE asc, PART_NO asc;
    end View_MessageAirPort2023;
~';

    l_detail_airport varchar2(32767) := q'~
    PROCEDURE View_MessageDetailAirPort(p_date varchar2, P_PARTNO varchar2,
                                        P_MESSTYPE varchar2,
                                        P_OUT_CURSOR out T_CURSOR) is
        v_flight_day date;
    begin
        v_flight_day := to_date(trim(p_date), 'FXDD/MM/YYYY');
        open P_OUT_CURSOR for
            select CONTENT,
                   (select ADDRESS
                      from M_G_A_M a
                      inner join M_G_A_D b on a.ID = b.G_A_M_ID
                     where GROUP_NAME = P_MESSTYPE
                       and rownum = 1) as ADDRESS
              from T_PLAN_MESSAGE_NEW
             where trunc(FLIGHTDATE) = v_flight_day
               and ID = P_PARTNO
               and MESS_TYPE = P_MESSTYPE
               and length(MESS_TYPE) = 4
             order by MESS_TYPE asc, PART_NO asc;
    end View_MessageDetailAirPort;
~';

    l_zone varchar2(32767) := q'~
    PROCEDURE View_MessageZone(p_date varchar2, P_OUT_CURSOR out T_CURSOR) is
        v_flight_day date;
    begin
        v_flight_day := to_date(trim(p_date), 'FXDD/MM/YYYY');
        open P_OUT_CURSOR for
            select PART_NO, MESS_TYPE,
                   case STATUS when 1 then 'Red' when 0 then '#000' end as Status
              from T_PLAN_MESSAGE_NEW
             where trunc(FLIGHTDATE) = v_flight_day
               and length(MESS_TYPE) > 4
             order by MESS_TYPE asc, PART_NO asc;
    end View_MessageZone;
~';

    l_detail_zone varchar2(32767) := q'~
    PROCEDURE View_MessageDetailZone(p_date varchar2, P_PARTNO varchar2,
                                     P_MESSTYPE varchar2,
                                     P_OUT_CURSOR out T_CURSOR) is
        v_flight_day date;
    begin
        v_flight_day := to_date(trim(p_date), 'FXDD/MM/YYYY');
        open P_OUT_CURSOR for
            select CONTENT
              from T_PLAN_MESSAGE_NEW
             where trunc(FLIGHTDATE) = v_flight_day
               and PART_NO = P_PARTNO
               and MESS_TYPE = P_MESSTYPE
               and length(MESS_TYPE) > 4
             order by MESS_TYPE asc, PART_NO asc;
    end View_MessageDetailZone;
~';

    procedure append_text(p_text varchar2) is
    begin
        dbms_lob.writeappend(l_source, length(p_text), p_text);
    end;

    procedure start_target(p_name varchar2, p_replacement varchar2) is
    begin
        l_target_name := p_name;
        l_match_count := l_match_count + 1;
        append_text(p_replacement);
    end;
begin
    dbms_lob.createtemporary(l_source, true);
    append_text('create or replace ');

    for r in (
        select text from user_source
         where name = 'FLIGHT_DAYFLIGHT' and type = 'PACKAGE BODY'
         order by line
    ) loop
        if l_target_name is null then
            if regexp_like(r.text, '^\s*PROCEDURE\s+View_MessageAirPort\s*\(', 'i') then
                start_target('View_MessageAirPort', l_airport);
            elsif regexp_like(r.text, '^\s*PROCEDURE\s+View_MessageAirPort2023\s*\(', 'i') then
                start_target('View_MessageAirPort2023', l_airport_2023);
            elsif regexp_like(r.text, '^\s*PROCEDURE\s+View_MessageDetailAirPort\s*\(', 'i') then
                start_target('View_MessageDetailAirPort', l_detail_airport);
            elsif regexp_like(r.text, '^\s*PROCEDURE\s+View_MessageZone\s*\(', 'i') then
                start_target('View_MessageZone', l_zone);
            elsif regexp_like(r.text, '^\s*PROCEDURE\s+View_MessageDetailZone\s*\(', 'i') then
                start_target('View_MessageDetailZone', l_detail_zone);
            else
                append_text(r.text);
            end if;
        elsif regexp_like(r.text,
              '^\s*end\s+' || l_target_name || '\s*;', 'i') then
            l_target_name := null;
        end if;
    end loop;

    if l_match_count <> 5 or l_target_name is not null then
        raise_application_error(-20103,
            'Expected five complete T_PLAN_MESSAGE_NEW read procedures.');
    end if;

    l_cursor := dbms_sql.open_cursor;
    dbms_sql.parse(l_cursor, l_source, dbms_sql.native);
    l_result := dbms_sql.execute(l_cursor);
    dbms_sql.close_cursor(l_cursor);
    dbms_lob.freetemporary(l_source);
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

prompt [4/6] Stop if package compilation failed
declare
    l_error_count number;
begin
    select count(*) into l_error_count
      from user_errors
     where name = 'FLIGHT_DAYFLIGHT' and type = 'PACKAGE BODY';
    if l_error_count > 0 then
        for r in (
            select line, position, text from user_errors
             where name = 'FLIGHT_DAYFLIGHT' and type = 'PACKAGE BODY'
             order by sequence
        ) loop
            dbms_output.put_line('Line ' || r.line || ':' || r.position || ' - ' || r.text);
        end loop;
        raise_application_error(-20104,
            'FLIGHT_DAYFLIGHT compilation failed. Run the rollback script.');
    end if;
end;
/

prompt [5/6] Publish the index and gather index statistics
declare
    l_visibility user_indexes.visibility%type;
begin
    select visibility into l_visibility
      from user_indexes where index_name = 'IX_TPMN_VIEW_MESSAGE';
    if l_visibility = 'INVISIBLE' then
        execute immediate 'alter index IX_TPMN_VIEW_MESSAGE visible';
    end if;
end;
/

begin
    dbms_stats.gather_index_stats(user, 'IX_TPMN_VIEW_MESSAGE');
end;
/

alter session set optimizer_use_invisible_indexes = false;

prompt [6/6] Verify final object status and execution plan
select object_name, object_type, status
  from user_objects
 where object_name in ('FLIGHT_DAYFLIGHT', 'IX_TPMN_VIEW_MESSAGE')
 order by object_type, object_name;

delete from plan_table where statement_id = 'TPMN_VIEW_POST';
explain plan set statement_id = 'TPMN_VIEW_POST' for
select part_no,
       mess_type,
       case status when 1 then 'Red' when 0 then '#000' end as status
  from t_plan_message_new
 where trunc(flightdate) = date '2026-07-11'
   and length(mess_type) > 4
 order by mess_type, part_no;

select plan_table_output
  from table(dbms_xplan.display(null, 'TPMN_VIEW_POST', 'BASIC +PREDICATE'));

commit;
prompt Deployment completed successfully.
