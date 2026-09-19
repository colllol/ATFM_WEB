-- Optimize P_FLY.GenFromTable_day_flightsNotAll.
-- Target: ATFM@PDBORCL (Oracle 12c). Run as schema owner ATFM in SQL*Plus.
-- Reuses T_DAY_FLIGHTS_IDX_TUNE2 and adds only the missing cancel lookup index.

set serveroutput on size unlimited
set pagesize 200
set linesize 240
set timing on
whenever sqlerror exit sql.sqlcode rollback

prompt [1/6] Check or create the T_FLIGHTCANCEL lookup index as INVISIBLE
declare
    l_named_count      number;
    l_equivalent_count number := 0;
    l_equivalent_name  user_indexes.index_name%type;
    l_ddl              varchar2(32767);
    l_normalized_ddl   varchar2(32767);
begin
    select count(*) into l_named_count
      from user_indexes where index_name = 'IX_TFC_GEN_NOTALL';

    for i in (
        select index_name from user_indexes
         where table_name = 'T_FLIGHTCANCEL' and index_type <> 'LOB'
    ) loop
        l_ddl := dbms_lob.substr(
            dbms_metadata.get_ddl('INDEX', i.index_name), 32767, 1);
        l_normalized_ddl := upper(l_ddl);
        l_normalized_ddl := replace(l_normalized_ddl, '"', '');
        l_normalized_ddl := replace(l_normalized_ddl, ' ', '');
        l_normalized_ddl := replace(l_normalized_ddl, chr(9), '');
        l_normalized_ddl := replace(l_normalized_ddl, chr(10), '');
        l_normalized_ddl := replace(l_normalized_ddl, chr(13), '');

        if instr(l_normalized_ddl,
            'T_FLIGHTCANCEL(FLIGHTDATE,CALLSIGN,FROM_AIRP,TO_AIRP)') > 0 then
            l_equivalent_count := l_equivalent_count + 1;
            l_equivalent_name := i.index_name;
        end if;
    end loop;

    if l_equivalent_count > 0
       and l_equivalent_name = 'IX_TFC_GEN_NOTALL' then
        dbms_output.put_line('Equivalent index already exists: ' || l_equivalent_name);
    elsif l_equivalent_count > 0 then
        raise_application_error(-20201,
            'Equivalent index already exists as ' || l_equivalent_name ||
            '; review it instead of creating a duplicate.');
    elsif l_named_count > 0 then
        raise_application_error(-20202,
            'IX_TFC_GEN_NOTALL exists with an unexpected definition.');
    else
        execute immediate
            'create index IX_TFC_GEN_NOTALL on T_FLIGHTCANCEL ' ||
            '(FLIGHTDATE, CALLSIGN, FROM_AIRP, TO_AIRP) invisible';
        dbms_output.put_line('Created IX_TFC_GEN_NOTALL as INVISIBLE.');
    end if;
end;
/

prompt [2/6] Validate both indexes with the rewritten representative query
alter session set optimizer_use_invisible_indexes = true;
delete from plan_table where statement_id = 'PFLY_NOTALL_PRE';
explain plan set statement_id = 'PFLY_NOTALL_PRE' for
select a.flight_id
  from t_day_flights a
  left join m_craft_type b on a.craft_id = b.craft_id
 where substr(a.flightnbr, 1, 3) = 'HVN'
   and a.flightdate >= date '2026-07-22'
   and a.flightdate <  date '2026-07-23'
   and a.status <> '4'
   and a.status <> '5'
   and a.permnbr <> 'NoPerm'
   and a.permnbr <> 'NOPERM'
   and a.isaccess = 1
   and a.ismessage = 0
   and not exists (
       select 1 from t_flightcancel f
        where f.flightdate = to_char(a.flightdate, 'DD-Mon-YY')
          and f.callsign = a.flightnbr
          and f.from_airp = a.from_airp
          and f.to_airp = a.to_airp
          and substr(f.callsign, 1, 3) = 'HVN');

select plan_table_output
  from table(dbms_xplan.display(null, 'PFLY_NOTALL_PRE', 'BASIC +PREDICATE'));

declare
    l_day_index_count    number;
    l_cancel_index_count number;
begin
    select count(*) into l_day_index_count from plan_table
     where statement_id = 'PFLY_NOTALL_PRE'
       and object_name = 'T_DAY_FLIGHTS_IDX_TUNE2';
    select count(*) into l_cancel_index_count from plan_table
     where statement_id = 'PFLY_NOTALL_PRE'
       and object_name = 'IX_TFC_GEN_NOTALL';

    if l_day_index_count = 0 or l_cancel_index_count = 0 then
        raise_application_error(-20203,
            'Expected both T_DAY_FLIGHTS_IDX_TUNE2 and IX_TFC_GEN_NOTALL in the plan.');
    end if;
end;
/

prompt [3/6] Rewrite the date predicates only inside GenFromTable_day_flightsNotAll
declare
    l_source           clob;
    l_in_function      boolean := false;
    l_function_count   number := 0;
    l_date_decl_count  number := 0;
    l_date_init_count  number := 0;
    l_predicate_count  number := 0;
    l_text             varchar2(32767);
    l_before           varchar2(32767);
    l_cursor           integer := null;
    l_result           integer;

    procedure append_text(p_text varchar2) is
    begin
        dbms_lob.writeappend(l_source, length(p_text), p_text);
    end;
begin
    dbms_lob.createtemporary(l_source, true);
    append_text('create or replace ');

    for r in (
        select text from user_source
         where name = 'P_FLY' and type = 'PACKAGE BODY'
         order by line
    ) loop
        l_text := r.text;

        if regexp_like(l_text,
            '^\s*Function\s+GenFromTable_day_flightsNotAll\s*\(', 'i') then
            l_in_function := true;
            l_function_count := l_function_count + 1;
        end if;

        if l_in_function then
            if instr(l_text, 'P_Date varchar(200);') > 0 then
                l_text := replace(l_text, 'P_Date varchar(200);', 'P_Date date;');
                l_date_decl_count := l_date_decl_count + 1;
            end if;

            if instr(l_text, 'P_Date:=fn_convert_date_pfly(on_day);') > 0 then
                l_text := replace(l_text,
                    'P_Date:=fn_convert_date_pfly(on_day);',
                    'P_Date := to_date(trim(on_day), ''FXDD/MM/YYYY'');');
                l_date_init_count := l_date_init_count + 1;
            end if;

            l_before := l_text;
            l_text := replace(l_text,
                'to_char(a.flightdate,''dd/mm/yyyy'')=on_day',
                'a.flightdate >= P_Date and a.flightdate < P_Date + 1');
            l_text := replace(l_text,
                'to_char(flightdate,''dd/mm/yyyy'')=on_day',
                'flightdate >= P_Date and flightdate < P_Date + 1');
            if l_text <> l_before then
                l_predicate_count := l_predicate_count + 1;
            end if;
        end if;

        append_text(l_text);
    end loop;

    if l_function_count <> 1
       or l_date_decl_count <> 1
       or l_date_init_count <> 1
       or l_predicate_count <> 44 then
        raise_application_error(-20204,
            'Source mismatch: function=' || l_function_count ||
            ', declaration=' || l_date_decl_count ||
            ', initialization=' || l_date_init_count ||
            ', predicates=' || l_predicate_count || ' (expected 44).');
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

prompt [4/6] Stop if P_FLY compilation failed
declare
    l_error_count number;
begin
    select count(*) into l_error_count from user_errors
     where name = 'P_FLY' and type = 'PACKAGE BODY';
    if l_error_count > 0 then
        for r in (
            select line, position, text from user_errors
             where name = 'P_FLY' and type = 'PACKAGE BODY'
             order by sequence
        ) loop
            dbms_output.put_line('Line ' || r.line || ':' || r.position || ' - ' || r.text);
        end loop;
        raise_application_error(-20205,
            'P_FLY compilation failed. Run the rollback script.');
    end if;
end;
/

prompt [5/6] Publish IX_TFC_GEN_NOTALL and gather its statistics
declare
    l_visibility user_indexes.visibility%type;
begin
    select visibility into l_visibility from user_indexes
     where index_name = 'IX_TFC_GEN_NOTALL';
    if l_visibility = 'INVISIBLE' then
        execute immediate 'alter index IX_TFC_GEN_NOTALL visible';
    end if;
end;
/

begin
    dbms_stats.gather_index_stats(user, 'IX_TFC_GEN_NOTALL');
end;
/

alter session set optimizer_use_invisible_indexes = false;

prompt [6/6] Verify final object status and execution plan
select object_name, object_type, status
  from user_objects
 where object_name in ('P_FLY', 'IX_TFC_GEN_NOTALL')
 order by object_type, object_name;

delete from plan_table where statement_id = 'PFLY_NOTALL_POST';
explain plan set statement_id = 'PFLY_NOTALL_POST' for
select a.flight_id
  from t_day_flights a
  left join m_craft_type b on a.craft_id = b.craft_id
 where substr(a.flightnbr, 1, 3) = 'HVN'
   and a.flightdate >= date '2026-07-22'
   and a.flightdate <  date '2026-07-23'
   and a.status <> '4' and a.status <> '5'
   and a.permnbr <> 'NoPerm' and a.permnbr <> 'NOPERM'
   and a.isaccess = 1 and a.ismessage = 0
   and not exists (
       select 1 from t_flightcancel f
        where f.flightdate = to_char(a.flightdate, 'DD-Mon-YY')
          and f.callsign = a.flightnbr
          and f.from_airp = a.from_airp
          and f.to_airp = a.to_airp
          and substr(f.callsign, 1, 3) = 'HVN');

select plan_table_output
  from table(dbms_xplan.display(null, 'PFLY_NOTALL_POST', 'BASIC +PREDICATE'));

commit;
prompt Deployment completed successfully.
