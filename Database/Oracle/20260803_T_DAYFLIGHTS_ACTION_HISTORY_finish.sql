-- Record fnFinishFlight/A_TEST_SEARCH.MOVEFINISH_GOINGON in flight history.

SET SERVEROUTPUT ON;

PROMPT === 1. Allow FINISH action type ===

ALTER TABLE T_DAYFLIGHTS_ACTION_HISTORY
    DROP CONSTRAINT CK_TDF_ACTION_HIS_TYPE;

ALTER TABLE T_DAYFLIGHTS_ACTION_HISTORY
    ADD CONSTRAINT CK_TDF_ACTION_HIS_TYPE
        CHECK (ACTION_TYPE IN ('INSERT', 'UPDATE', 'MOVE_DATE', 'FINISH'));

PROMPT === 2. Classify MOVEFINISH changes as FINISH ===

CREATE OR REPLACE TRIGGER TRG_TDF_GOINGON_ACTION_HIS
AFTER INSERT OR UPDATE OF LASTUSER ON T_DAY_FLIGHTS_GOINGON
FOR EACH ROW
DECLARE
    l_action_type T_DAYFLIGHTS_ACTION_HISTORY.ACTION_TYPE%TYPE;
    l_action_user T_DAYFLIGHTS_ACTION_HISTORY.ACTION_USER%TYPE;
    l_change_detail T_DAYFLIGHTS_ACTION_HISTORY.CHANGE_DETAIL%TYPE;

    PROCEDURE APPEND_CHANGE
    (
        p_field_name IN VARCHAR2,
        p_old_value  IN VARCHAR2,
        p_new_value  IN VARCHAR2
    )
    IS
    BEGIN
        IF p_old_value <> p_new_value
           OR (p_old_value IS NULL AND p_new_value IS NOT NULL)
           OR (p_old_value IS NOT NULL AND p_new_value IS NULL)
        THEN
            IF l_change_detail IS NOT NULL THEN
                l_change_detail := l_change_detail || CHR(10);
            END IF;

            l_change_detail := l_change_detail
                || p_field_name
                || ': [' || NVL(p_old_value, '(empty)') || ']'
                || ' -> '
                || '[' || NVL(p_new_value, '(empty)') || ']';
        END IF;
    END APPEND_CHANGE;
BEGIN
    IF INSERTING THEN
        IF :NEW.CODE = 'CS'
           AND :NEW.DATE_OLD IS NOT NULL
           AND :NEW.FLIGHTDATE IS NOT NULL
           AND TRUNC(:NEW.FLIGHTDATE) = TRUNC(:NEW.DATE_OLD) + 1
        THEN
            l_action_type := 'MOVE_DATE';
            l_change_detail :=
                   'P_DATE: ['
                || TO_CHAR(:NEW.DATE_OLD, 'DD-MM-YYYY')
                || '] -> ['
                || TO_CHAR(:NEW.FLIGHTDATE, 'DD-MM-YYYY')
                || ']';
        ELSE
            l_action_type := 'INSERT';
            l_change_detail := 'NEW FLIGHT';
        END IF;
    ELSE
        IF NVL(:OLD.MOVEFINISH, 0) <> NVL(:NEW.MOVEFINISH, 0)
           AND NVL(:NEW.MOVEFINISH, 0) = 1
        THEN
            l_action_type := 'FINISH';
        ELSE
            l_action_type := 'UPDATE';
        END IF;

        APPEND_CHANGE('LETTER', :OLD.LETTER_TYPE, :NEW.LETTER_TYPE);
        APPEND_CHANGE('PERM', :OLD.PERMNBR, :NEW.PERMNBR);
        APPEND_CHANGE('REGIS', :OLD.REGISTRATION, :NEW.REGISTRATION);
        APPEND_CHANGE('CALLSIGN', :OLD.FLIGHTNBR, :NEW.FLIGHTNBR);
        APPEND_CHANGE('FROM', :OLD.FROM_AIRP, :NEW.FROM_AIRP);
        APPEND_CHANGE('TO', :OLD.TO_AIRP, :NEW.TO_AIRP);
        APPEND_CHANGE('ETD', :OLD.ETD, :NEW.ETD);
        APPEND_CHANGE('EOBT', :OLD.EOBT, :NEW.EOBT);
        APPEND_CHANGE('ETA', :OLD.ETA, :NEW.ETA);
        APPEND_CHANGE('ATD', :OLD.ATD, :NEW.ATD);
        APPEND_CHANGE('ATA', :OLD.ATA, :NEW.ATA);
        APPEND_CHANGE('ROUTE', :OLD.VIA, :NEW.VIA);
        APPEND_CHANGE('ROUTE_FPL', :OLD.ROUTE_TT, :NEW.ROUTE_TT);
        APPEND_CHANGE('CRAFT_ID', TO_CHAR(:OLD.CRAFT_ID), TO_CHAR(:NEW.CRAFT_ID));
        APPEND_CHANGE('CRAFT_P', :OLD.CRAFT_TYPE, :NEW.CRAFT_TYPE);
        APPEND_CHANGE('P_TYPE', :OLD.PERMTYPE, :NEW.PERMTYPE);
        APPEND_CHANGE('F_TYPE', :OLD.FLIGHT_TYPE, :NEW.FLIGHT_TYPE);
        APPEND_CHANGE('OPER', :OLD.OPER_ID, :NEW.OPER_ID);
        APPEND_CHANGE('PURPOSE', :OLD.PURPOSE, :NEW.PURPOSE);
        APPEND_CHANGE('VALID', TO_CHAR(:OLD.VALIDHOURS), TO_CHAR(:NEW.VALIDHOURS));
        APPEND_CHANGE(
            'P_DATE',
            TO_CHAR(:OLD.FLIGHTDATE, 'DD-MM-YYYY'),
            TO_CHAR(:NEW.FLIGHTDATE, 'DD-MM-YYYY')
        );
        APPEND_CHANGE('REMARK', :OLD.REMARK, :NEW.REMARK);
        APPEND_CHANGE('MOVE_FINISHED', TO_CHAR(:OLD.MOVEFINISH), TO_CHAR(:NEW.MOVEFINISH));

        IF l_change_detail IS NULL THEN
            l_change_detail := 'NO BUSINESS DATA CHANGED';
        END IF;
    END IF;

    l_action_user := NVL(
        TRIM(:NEW.LASTUSER),
        NVL(TRIM(:OLD.LASTUSER), 'SYSTEM')
    );

    INSERT INTO T_DAYFLIGHTS_ACTION_HISTORY
    (
        HISTORY_ID,
        SOURCE_ROW_ID,
        FLIGHT_ID,
        ACTION_TYPE,
        CALLSIGN,
        FLIGHTDATE,
        ACTION_USER,
        CHANGE_DETAIL,
        ACTION_DATE
    )
    VALUES
    (
        SEQ_T_DAYFLIGHTS_ACTION_HIS.NEXTVAL,
        :NEW.ID,
        :NEW.FLIGHT_ID,
        l_action_type,
        :NEW.FLIGHTNBR,
        :NEW.FLIGHTDATE,
        l_action_user,
        l_change_detail,
        SYSTIMESTAMP
    );
END;
/

ALTER TRIGGER TRG_TDF_GOINGON_ACTION_HIS ENABLE;

PROMPT === 3. Make MOVEFINISH_GOINGON pass P_USER to the trigger ===

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

    IF DBMS_LOB.INSTR(l_ddl, l_new_sql) > 0 THEN
        DBMS_OUTPUT.PUT_LINE('A_TEST_SEARCH.MOVEFINISH_GOINGON already updated.');
    ELSIF DBMS_LOB.INSTR(l_ddl, l_old_sql) > 0 THEN
        l_ddl := REPLACE(l_ddl, l_old_sql, l_new_sql);
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
        DBMS_OUTPUT.PUT_LINE('Updated A_TEST_SEARCH.MOVEFINISH_GOINGON.');
    ELSE
        RAISE_APPLICATION_ERROR(
            -20031,
            'Expected MOVEFINISH_GOINGON UPDATE statement was not found.'
        );
    END IF;
END;
/

PROMPT === 4. Deployment result ===

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('TRG_TDF_GOINGON_ACTION_HIS', 'A_TEST_SEARCH')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT NAME, TYPE, LINE, POSITION, TEXT
  FROM USER_ERRORS
 WHERE NAME IN ('TRG_TDF_GOINGON_ACTION_HIS', 'A_TEST_SEARCH')
 ORDER BY NAME, TYPE, SEQUENCE;
