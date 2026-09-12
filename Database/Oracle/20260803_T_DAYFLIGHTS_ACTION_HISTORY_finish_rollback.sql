-- Restore MOVEFINISH_GOINGON and the preceding MOVE_DATE trigger version.

SET SERVEROUTPUT ON;

DECLARE
    l_ddl        CLOB;
    l_old_sql    VARCHAR2(1000) :=
        'Update T_DAY_FLIGHTS_GOINGON set MOVEFINISH=1  WHERE    FLIGHT_ID=P_ID ;';
    l_new_sql    VARCHAR2(1000) :=
        'Update T_DAY_FLIGHTS_GOINGON set MOVEFINISH=1, LASTUSER=P_USER  WHERE    FLIGHT_ID=P_ID ;';
    l_cursor     INTEGER;
BEGIN
    SELECT DBMS_METADATA.GET_DDL('PACKAGE_BODY', 'A_TEST_SEARCH')
      INTO l_ddl
      FROM DUAL;

    IF DBMS_LOB.INSTR(l_ddl, l_old_sql) > 0 THEN
        DBMS_OUTPUT.PUT_LINE('A_TEST_SEARCH.MOVEFINISH_GOINGON already restored.');
    ELSIF DBMS_LOB.INSTR(l_ddl, l_new_sql) > 0 THEN
        l_ddl := REPLACE(l_ddl, l_new_sql, l_old_sql);
        l_cursor := DBMS_SQL.OPEN_CURSOR;
        BEGIN
            DBMS_SQL.PARSE(l_cursor, l_ddl, DBMS_SQL.NATIVE);
            DBMS_SQL.CLOSE_CURSOR(l_cursor);
        EXCEPTION
            WHEN OTHERS THEN
                IF DBMS_SQL.IS_OPEN(l_cursor) THEN
                    DBMS_SQL.CLOSE_CURSOR(l_cursor);
                END IF;
                RAISE;
        END;
    ELSE
        RAISE_APPLICATION_ERROR(
            -20032,
            'Expected MOVEFINISH_GOINGON UPDATE statement was not found.'
        );
    END IF;
END;
/

UPDATE T_DAYFLIGHTS_ACTION_HISTORY
   SET ACTION_TYPE = 'UPDATE'
 WHERE ACTION_TYPE = 'FINISH';

COMMIT;

@@20260803_T_DAYFLIGHTS_ACTION_HISTORY_move_date.sql
