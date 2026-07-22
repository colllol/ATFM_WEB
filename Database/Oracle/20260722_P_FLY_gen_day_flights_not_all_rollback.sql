-- Roll back the P_FLY.GenFromTable_day_flightsNotAll optimization.
-- Target: ATFM@PDBORCL (Oracle 12c). Run as schema owner ATFM in SQL*Plus.

set serveroutput on size unlimited
set timing on
whenever sqlerror exit sql.sqlcode rollback

prompt [1/3] Restore the original date predicates in GenFromTable_day_flightsNotAll
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
            if instr(l_text, 'P_Date date;') > 0 then
                l_text := replace(l_text, 'P_Date date;', 'P_Date varchar(200);');
                l_date_decl_count := l_date_decl_count + 1;
            end if;

            if instr(l_text,
                'P_Date := to_date(trim(on_day), ''FXDD/MM/YYYY'');') > 0 then
                l_text := replace(l_text,
                    'P_Date := to_date(trim(on_day), ''FXDD/MM/YYYY'');',
                    'P_Date:=fn_convert_date_pfly(on_day);');
                l_date_init_count := l_date_init_count + 1;
            end if;

            l_before := l_text;
            l_text := replace(l_text,
                'a.flightdate >= P_Date and a.flightdate < P_Date + 1',
                'to_char(a.flightdate,''dd/mm/yyyy'')=on_day');
            l_text := replace(l_text,
                'flightdate >= P_Date and flightdate < P_Date + 1',
                'to_char(flightdate,''dd/mm/yyyy'')=on_day');
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
            'Optimized source does not match the expected 44 predicates.');
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

prompt [2/3] Validate package compilation before dropping the index
declare
    l_error_count number;
begin
    select count(*) into l_error_count from user_errors
     where name = 'P_FLY' and type = 'PACKAGE BODY';
    if l_error_count > 0 then
        raise_application_error(-20205,
            'P_FLY restore failed; IX_TFC_GEN_NOTALL was not dropped.');
    end if;
end;
/

prompt [3/3] Drop only the index introduced by this optimization
declare
    l_count number;
begin
    select count(*) into l_count from user_indexes
     where index_name = 'IX_TFC_GEN_NOTALL';
    if l_count > 0 then
        execute immediate 'drop index IX_TFC_GEN_NOTALL';
    end if;
end;
/

select object_name, object_type, status
  from user_objects where object_name = 'P_FLY'
 order by object_type;

commit;
prompt Rollback completed successfully.
