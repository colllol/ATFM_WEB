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
                ACTION_DATE   TIMESTAMP(6) WITH TIME ZONE
                              DEFAULT SYSTIMESTAMP NOT NULL,
                CONSTRAINT PK_T_DAYFLIGHTS_ACTION_HIS
                    PRIMARY KEY (HISTORY_ID),
                CONSTRAINT CK_TDF_ACTION_HIS_TYPE
                    CHECK (ACTION_TYPE IN ('INSERT', 'UPDATE'))
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
BEGIN
    IF INSERTING THEN
        l_action_type := 'INSERT';
    ELSE
        l_action_type := 'UPDATE';
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
