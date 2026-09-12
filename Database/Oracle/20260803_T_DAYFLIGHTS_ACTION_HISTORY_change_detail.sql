-- Add field-level change details to the DaylyFlight action history.

SET SERVEROUTPUT ON;

PROMPT === 1. Add CHANGE_DETAIL column ===

DECLARE
    l_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO l_count
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_DAYFLIGHTS_ACTION_HISTORY'
       AND COLUMN_NAME = 'CHANGE_DETAIL';

    IF l_count = 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_DAYFLIGHTS_ACTION_HISTORY ' ||
            'ADD (CHANGE_DETAIL CLOB)';
        DBMS_OUTPUT.PUT_LINE('Added CHANGE_DETAIL.');
    ELSE
        DBMS_OUTPUT.PUT_LINE('CHANGE_DETAIL already exists.');
    END IF;
END;
/

PROMPT === 2. Replace audit trigger ===

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

PROMPT === 3. Return CHANGE_DETAIL through the API package ===

CREATE OR REPLACE PACKAGE BODY DAYFLIGHT_HISTORY_PKG AS
    PROCEDURE GET_BY_FLIGHT_ID
    (
        P_FLIGHT_ID IN T_DAYFLIGHTS_ACTION_HISTORY.FLIGHT_ID%TYPE,
        P_OUT_CURSOR OUT T_CURSOR
    )
    IS
    BEGIN
        OPEN P_OUT_CURSOR FOR
            SELECT HISTORY_ID,
                   SOURCE_ROW_ID,
                   FLIGHT_ID,
                   ACTION_TYPE,
                   CALLSIGN,
                   TO_CHAR(FLIGHTDATE, 'DD-MM-YYYY') AS FLIGHTDATE,
                   ACTION_USER,
                   DBMS_LOB.SUBSTR(
                       CHANGE_DETAIL,
                       4000,
                       1
                   ) AS CHANGE_DETAIL,
                   TO_CHAR(
                       ACTION_DATE,
                       'DD-MM-YYYY HH24:MI:SS'
                   ) AS ACTION_DATE
              FROM T_DAYFLIGHTS_ACTION_HISTORY
             WHERE FLIGHT_ID = P_FLIGHT_ID
             ORDER BY HISTORY_ID DESC;
    END GET_BY_FLIGHT_ID;
END DAYFLIGHT_HISTORY_PKG;
/

PROMPT === 4. Deployment result ===

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
       (
           'T_DAYFLIGHTS_ACTION_HISTORY',
           'TRG_TDF_GOINGON_ACTION_HIS',
           'DAYFLIGHT_HISTORY_PKG'
       )
 ORDER BY OBJECT_TYPE, OBJECT_NAME;
