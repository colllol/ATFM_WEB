-- Roll back the View_Message optimization deployed on 2026-07-22.
-- Target: ATFM@PDBORCL (Oracle 12c)
-- Run as schema owner ATFM in SQL*Plus.

set serveroutput on size unlimited
set timing on
whenever sqlerror exit sql.sqlcode rollback

prompt [1/3] Restore the original View_Message implementation
declare
    l_source       clob;
    l_replacement  varchar2(32767) := q'~
    PROCEDURE View_Message(p_date varchar2, P_OUT_CURSOR out T_CURSOR) is
    begin
        open P_OUT_CURSOR for
            select ID,
                   PART_NO,
                   MESS_TYPE,
                   case STATUS
                       when 1 then 'Red'
                       when 0 then '#000'
                   end as Status
              from T_PLAN_MESSAGE
             where to_char(FLIGHTDATE, 'dd/mm/yyyy') = p_date
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

prompt [2/3] Validate package compilation before removing the index
declare
    l_error_count number;
begin
    select count(*)
      into l_error_count
      from user_errors
     where name = 'FLIGHT_DAYFLIGHT'
       and type = 'PACKAGE BODY';

    if l_error_count > 0 then
        raise_application_error(-20003,
            'Package restore failed; IX_TPM_VIEW_MESSAGE was not dropped.');
    end if;
end;
/

prompt [3/3] Drop only the index introduced by the optimization
declare
    l_count number;
begin
    select count(*)
      into l_count
      from user_indexes
     where index_name = 'IX_TPM_VIEW_MESSAGE';

    if l_count > 0 then
        execute immediate 'drop index IX_TPM_VIEW_MESSAGE';
    end if;
end;
/

select object_name, object_type, status
  from user_objects
 where object_name = 'FLIGHT_DAYFLIGHT'
 order by object_type;

commit;
prompt Rollback completed successfully.
