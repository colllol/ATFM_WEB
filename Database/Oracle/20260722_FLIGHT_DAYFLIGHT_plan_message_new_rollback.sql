-- Roll back the T_PLAN_MESSAGE_NEW read optimization deployed on 2026-07-22.
-- Target: ATFM@PDBORCL (Oracle 12c). Run as schema owner ATFM in SQL*Plus.

set serveroutput on size unlimited
set timing on
whenever sqlerror exit sql.sqlcode rollback

prompt [1/3] Restore the five original read procedures
declare
    l_source       clob;
    l_target_name  varchar2(128);
    l_match_count  number := 0;
    l_cursor       integer := null;
    l_result       integer;

    l_airport varchar2(32767) := q'~
    PROCEDURE View_MessageAirPort(p_date varchar2, P_OUT_CURSOR out T_CURSOR) is
    begin
        open P_OUT_CURSOR for
            select ID, PART_NO, MESS_TYPE,
                   case STATUS when 1 then 'Red' when 0 then '#000' end as Status
              from T_PLAN_MESSAGE_NEW
             where to_char(FLIGHTDATE, 'dd/mm/yyyy') = p_date
               and length(MESS_TYPE) = 4
             order by Status desc, MESS_TYPE asc, PART_NO asc;
    end View_MessageAirPort;
~';

    l_airport_2023 varchar2(32767) := q'~
    PROCEDURE View_MessageAirPort2023(p_date varchar2, P_MESSTYPE varchar2,
                                      P_OUT_CURSOR out T_CURSOR) is
    begin
        open P_OUT_CURSOR for
            select ID, PART_NO, MESS_TYPE,
                   case STATUS when 1 then 'Red' when 0 then '#000' end as Status
              from T_PLAN_MESSAGE_NEW
             where to_char(FLIGHTDATE, 'dd/mm/yyyy') = p_date
               and length(MESS_TYPE) = 4
               and MESS_TYPE = P_MESSTYPE
             order by Status desc, MESS_TYPE asc, PART_NO asc;
    end View_MessageAirPort2023;
~';

    l_detail_airport varchar2(32767) := q'~
    PROCEDURE View_MessageDetailAirPort(p_date varchar2, P_PARTNO varchar2,
                                        P_MESSTYPE varchar2,
                                        P_OUT_CURSOR out T_CURSOR) is
    begin
        open P_OUT_CURSOR for
            select CONTENT,
                   (select ADDRESS
                      from M_G_A_M a
                      inner join M_G_A_D b on a.ID = b.G_A_M_ID
                     where GROUP_NAME = P_MESSTYPE and rownum = 1) as ADDRESS
              from T_PLAN_MESSAGE_NEW
             where to_char(FLIGHTDATE, 'dd/mm/yyyy') = p_date
               and ID = P_PARTNO
               and MESS_TYPE = P_MESSTYPE
               and length(MESS_TYPE) = 4
             order by MESS_TYPE asc, PART_NO asc;
    end View_MessageDetailAirPort;
~';

    l_zone varchar2(32767) := q'~
    PROCEDURE View_MessageZone(p_date varchar2, P_OUT_CURSOR out T_CURSOR) is
    begin
        open P_OUT_CURSOR for
            select PART_NO, MESS_TYPE,
                   case STATUS when 1 then 'Red' when 0 then '#000' end as Status
              from T_PLAN_MESSAGE_NEW
             where to_char(FLIGHTDATE, 'dd/mm/yyyy') = p_date
               and length(MESS_TYPE) > 4
             order by MESS_TYPE asc, PART_NO asc;
    end View_MessageZone;
~';

    l_detail_zone varchar2(32767) := q'~
    PROCEDURE View_MessageDetailZone(p_date varchar2, P_PARTNO varchar2,
                                     P_MESSTYPE varchar2,
                                     P_OUT_CURSOR out T_CURSOR) is
    begin
        open P_OUT_CURSOR for
            select CONTENT from T_PLAN_MESSAGE_NEW
             where to_char(FLIGHTDATE, 'dd/mm/yyyy') = p_date
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

prompt [2/3] Validate package compilation before dropping the index
declare
    l_error_count number;
begin
    select count(*) into l_error_count
      from user_errors
     where name = 'FLIGHT_DAYFLIGHT' and type = 'PACKAGE BODY';
    if l_error_count > 0 then
        raise_application_error(-20104,
            'Package restore failed; IX_TPMN_VIEW_MESSAGE was not dropped.');
    end if;
end;
/

prompt [3/3] Drop only the index introduced by this optimization
declare
    l_count number;
begin
    select count(*) into l_count
      from user_indexes where index_name = 'IX_TPMN_VIEW_MESSAGE';
    if l_count > 0 then
        execute immediate 'drop index IX_TPMN_VIEW_MESSAGE';
    end if;
end;
/

select object_name, object_type, status
  from user_objects
 where object_name = 'FLIGHT_DAYFLIGHT'
 order by object_type;

commit;
prompt Rollback completed successfully.
