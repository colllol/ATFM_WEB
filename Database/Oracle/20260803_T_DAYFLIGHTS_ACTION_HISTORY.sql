-- Lich su them/cap nhat thu cong chuyen bay tren DaylyFlight.aspx.
-- Trigger chi ghi UPDATE khi cau lenh co cap nhat LASTUSER. Dieu nay giup:
--   1. Ghi nhan moi thao tac cua nguoi dung tu Add / Update Flight Info.
--   2. Khong ghi trung lan UPDATE EOBT/ATD/ATA ngay sau INSERT vi cau lenh
--      do khong cap nhat LASTUSER.
-- Khong tao khoa ngoai den T_DAY_FLIGHTS_GOINGON de giu lich su khi ban ghi
-- nguon bi chuyen sang finished hoac bi xoa.

SET SERVEROUTPUT ON;

PROMPT === 1. Create history table ===

DECLARE
    l_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO l_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_DAYFLIGHTS_ACTION_HISTORY';

    IF l_count = 0 THEN
        EXECUTE IMMEDIATE q'[
            CREATE TABLE T_DAYFLIGHTS_ACTION_HISTORY
            (
                HISTORY_ID    NUMBER(19) NOT NULL,
                SOURCE_ROW_ID NUMBER,
                FLIGHT_ID     NUMBER,
                ACTION_TYPE   VARCHAR2(10 CHAR) NOT NULL,
                CALLSIGN      VARCHAR2(20 CHAR),
                FLIGHTDATE    DATE,
                ACTION_USER   VARCHAR2(100 CHAR) NOT NULL,
                CHANGE_DETAIL CLOB,
                ACTION_DATE   TIMESTAMP(6) WITH TIME ZONE
                              DEFAULT SYSTIMESTAMP NOT NULL,
                CONSTRAINT PK_T_DAYFLIGHTS_ACTION_HIS
                    PRIMARY KEY (HISTORY_ID),
                CONSTRAINT CK_TDF_ACTION_HIS_TYPE
                    CHECK (ACTION_TYPE IN ('INSERT', 'UPDATE', 'MOVE_DATE', 'FINISH'))
            )
        ]';
        DBMS_OUTPUT.PUT_LINE('Created T_DAYFLIGHTS_ACTION_HISTORY.');
    ELSE
        DBMS_OUTPUT.PUT_LINE('T_DAYFLIGHTS_ACTION_HISTORY already exists.');
    END IF;
END;
/

PROMPT === 2. Create sequence ===

DECLARE
    l_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO l_count
      FROM USER_SEQUENCES
     WHERE SEQUENCE_NAME = 'SEQ_T_DAYFLIGHTS_ACTION_HIS';

    IF l_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE SEQUENCE SEQ_T_DAYFLIGHTS_ACTION_HIS ' ||
            'START WITH 1 INCREMENT BY 1 CACHE 50 NOCYCLE';
        DBMS_OUTPUT.PUT_LINE('Created SEQ_T_DAYFLIGHTS_ACTION_HIS.');
    ELSE
        DBMS_OUTPUT.PUT_LINE('SEQ_T_DAYFLIGHTS_ACTION_HIS already exists.');
    END IF;
END;
/

PROMPT === 3. Create search indexes ===

DECLARE
    l_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO l_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TDF_ACTION_HIS_FLIGHT';

    IF l_count = 0 THEN
        EXECUTE IMMEDIATE q'[
            CREATE INDEX IX_TDF_ACTION_HIS_FLIGHT
                ON T_DAYFLIGHTS_ACTION_HISTORY
                   (FLIGHT_ID, ACTION_DATE)
        ]';
    END IF;

    SELECT COUNT(*)
      INTO l_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TDF_ACTION_HIS_SEARCH';

    IF l_count = 0 THEN
        EXECUTE IMMEDIATE q'[
            CREATE INDEX IX_TDF_ACTION_HIS_SEARCH
                ON T_DAYFLIGHTS_ACTION_HISTORY
                   (FLIGHTDATE, CALLSIGN, ACTION_DATE)
        ]';
    END IF;
END;
/

PROMPT === 4. Create audit trigger ===

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

PROMPT === 5. Deployment result ===

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
       (
           'T_DAYFLIGHTS_ACTION_HISTORY',
           'SEQ_T_DAYFLIGHTS_ACTION_HIS',
           'TRG_TDF_GOINGON_ACTION_HIS',
           'IX_TDF_ACTION_HIS_FLIGHT',
           'IX_TDF_ACTION_HIS_SEARCH'
       )
 ORDER BY OBJECT_TYPE, OBJECT_NAME;

SELECT TRIGGER_NAME, STATUS, TRIGGERING_EVENT
  FROM USER_TRIGGERS
 WHERE TRIGGER_NAME = 'TRG_TDF_GOINGON_ACTION_HIS';
