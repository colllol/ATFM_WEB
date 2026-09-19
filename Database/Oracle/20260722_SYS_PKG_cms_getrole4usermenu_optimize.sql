-- Fix and optimize SYS_PKG.CMS_GetRole4UserMenu.
-- Target: ATFM@PDBORCL (Oracle 12c). Run as schema owner ATFM in SQL*Plus.

set serveroutput on size unlimited
set pagesize 200
set linesize 240
set timing on
whenever sqlerror exit sql.sqlcode rollback

prompt [1/6] Check for a duplicate index and create IX_TUM_USER_MENU as INVISIBLE
declare
    l_named_count      number;
    l_equivalent_count number := 0;
    l_equivalent_name  user_indexes.index_name%type;
    l_ddl              varchar2(32767);
    l_normalized_ddl   varchar2(32767);
begin
    select count(*) into l_named_count
      from user_indexes where index_name = 'IX_TUM_USER_MENU';

    for i in (
        select index_name from user_indexes
         where table_name = 'T_USERMENU' and index_type <> 'LOB'
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
            'T_USERMENU(USER_ID,MENU_ID)') > 0 then
            l_equivalent_count := l_equivalent_count + 1;
            l_equivalent_name := i.index_name;
        end if;
    end loop;

    if l_equivalent_count > 0
       and l_equivalent_name = 'IX_TUM_USER_MENU' then
        dbms_output.put_line('Equivalent index already exists: ' || l_equivalent_name);
    elsif l_equivalent_count > 0 then
        raise_application_error(-20301,
            'Equivalent index already exists as ' || l_equivalent_name ||
            '; review it instead of creating a duplicate.');
    elsif l_named_count > 0 then
        raise_application_error(-20302,
            'IX_TUM_USER_MENU exists with an unexpected definition.');
    else
        execute immediate
            'create index IX_TUM_USER_MENU on T_USERMENU ' ||
            '(USER_ID, MENU_ID) invisible';
        dbms_output.put_line('Created IX_TUM_USER_MENU as INVISIBLE.');
    end if;
end;
/

prompt [2/6] Validate the filtered query with the invisible index
alter session set optimizer_use_invisible_indexes = true;
delete from plan_table where statement_id = 'CMS_ROLE_PRE';
explain plan set statement_id = 'CMS_ROLE_PRE' for
select um.*
  from t_users u
  join t_usermenu um on um.user_id = u.userid
 where u.username = 'trucbantruong'
   and um.menu_id = 89;

select plan_table_output
  from table(dbms_xplan.display(null, 'CMS_ROLE_PRE', 'BASIC +PREDICATE'));

declare
    l_index_count number;
begin
    select count(*) into l_index_count from plan_table
     where statement_id = 'CMS_ROLE_PRE'
       and object_name = 'IX_TUM_USER_MENU';
    if l_index_count = 0 then
        raise_application_error(-20303,
            'IX_TUM_USER_MENU was not selected in the validation plan.');
    end if;
end;
/

prompt [3/6] Replace only CMS_GetRole4UserMenu in SYS_PKG
declare
    l_source       clob;
    l_replacement  varchar2(32767) := q'~
    PROCEDURE CMS_GetRole4UserMenu(
        P_UserName varchar2,
        p_Menu_ID number,
        P_OUT_CURSOR out T_CURSOR)
    is
    begin
        open P_OUT_CURSOR for
            select um.*
              from T_USERS u
              join T_USERMENU um on um.USER_ID = u.USERID
             where u.USERNAME = P_UserName
               and um.MENU_ID = p_Menu_ID;
    end CMS_GetRole4UserMenu;
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
        select text from user_source
         where name = 'SYS_PKG' and type = 'PACKAGE BODY'
         order by line
    ) loop
        if not l_in_target
           and regexp_like(r.text,
               '^\s*PROCEDURE\s+CMS_GetRole4UserMenu\s*\(', 'i') then
            l_match_count := l_match_count + 1;
            l_in_target := true;
            append_text(l_replacement);
        elsif l_in_target then
            if regexp_like(r.text,
                '^\s*end\s+CMS_GetRole4UserMenu\s*;', 'i') then
                l_in_target := false;
            end if;
        else
            append_text(r.text);
        end if;
    end loop;

    if l_match_count <> 1 or l_in_target then
        raise_application_error(-20304,
            'Expected exactly one complete CMS_GetRole4UserMenu procedure.');
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

prompt [4/6] Stop if SYS_PKG compilation failed
declare
    l_error_count number;
begin
    select count(*) into l_error_count from user_errors
     where name = 'SYS_PKG' and type = 'PACKAGE BODY';
    if l_error_count > 0 then
        for r in (
            select line, position, text from user_errors
             where name = 'SYS_PKG' and type = 'PACKAGE BODY'
             order by sequence
        ) loop
            dbms_output.put_line('Line ' || r.line || ':' || r.position || ' - ' || r.text);
        end loop;
        raise_application_error(-20305,
            'SYS_PKG compilation failed. Run the rollback script.');
    end if;
end;
/

prompt [5/6] Publish IX_TUM_USER_MENU and gather its statistics
declare
    l_visibility user_indexes.visibility%type;
begin
    select visibility into l_visibility from user_indexes
     where index_name = 'IX_TUM_USER_MENU';
    if l_visibility = 'INVISIBLE' then
        execute immediate 'alter index IX_TUM_USER_MENU visible';
    end if;
end;
/

begin
    dbms_stats.gather_index_stats(user, 'IX_TUM_USER_MENU');
end;
/

alter session set optimizer_use_invisible_indexes = false;

prompt [6/6] Verify final object status and execution plan
select object_name, object_type, status
  from user_objects
 where object_name in ('SYS_PKG', 'IX_TUM_USER_MENU')
 order by object_type, object_name;

delete from plan_table where statement_id = 'CMS_ROLE_POST';
explain plan set statement_id = 'CMS_ROLE_POST' for
select um.*
  from t_users u
  join t_usermenu um on um.user_id = u.userid
 where u.username = 'trucbantruong'
   and um.menu_id = 89;

select plan_table_output
  from table(dbms_xplan.display(null, 'CMS_ROLE_POST', 'BASIC +PREDICATE'));

commit;
prompt Deployment completed successfully.
