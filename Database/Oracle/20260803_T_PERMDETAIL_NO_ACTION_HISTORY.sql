-- Luu lich su thay doi chi tiet phep bay NO theo tung chuyen bay.

SET SERVEROUTPUT ON;

DECLARE
    l_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO l_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_PERMDETAIL_NO_ACTION_HISTORY';

    IF l_count = 0 THEN
        EXECUTE IMMEDIATE q'~
            CREATE TABLE T_PERMDETAIL_NO_ACTION_HISTORY
            (
                HISTORY_ID     NUMBER(19) NOT NULL,
                PERM_DETAIL_ID NUMBER NOT NULL,
                FLIGHT_PK      NUMBER,
                PERM_ID        NUMBER,
                ACTION_TYPE    VARCHAR2(10 CHAR) NOT NULL,
                FLIGHTNBR      VARCHAR2(30 CHAR),
                ACTION_USER    VARCHAR2(100 CHAR) NOT NULL,
                ACTION_DATE    TIMESTAMP(6) WITH TIME ZONE
                               DEFAULT SYSTIMESTAMP NOT NULL,
                CHANGE_DETAIL  CLOB,
                CONSTRAINT PK_T_PERMDETAIL_NO_ACTION_HIS
                    PRIMARY KEY (HISTORY_ID),
                CONSTRAINT CK_T_PERMDETAIL_NO_ACTION_TYP
                    CHECK (ACTION_TYPE IN ('INSERT', 'UPDATE', 'DELETE'))
            )
        ~';
        DBMS_OUTPUT.PUT_LINE('Created T_PERMDETAIL_NO_ACTION_HISTORY.');
    ELSE
        DBMS_OUTPUT.PUT_LINE('T_PERMDETAIL_NO_ACTION_HISTORY already exists.');
    END IF;
END;
/

DECLARE
    l_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO l_count
      FROM USER_SEQUENCES
     WHERE SEQUENCE_NAME = 'SEQ_T_PERMDETAIL_NO_ACT_HIS';
    IF l_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE SEQUENCE SEQ_T_PERMDETAIL_NO_ACT_HIS ' ||
            'START WITH 1 INCREMENT BY 1 CACHE 50 NOCYCLE';
    END IF;

    SELECT COUNT(*) INTO l_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TPDNO_HIS_DETAIL_DATE';
    IF l_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_TPDNO_HIS_DETAIL_DATE ' ||
            'ON T_PERMDETAIL_NO_ACTION_HISTORY ' ||
            '(PERM_DETAIL_ID, ACTION_DATE DESC)';
    END IF;

    SELECT COUNT(*) INTO l_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TPDNO_HIS_PERM_DATE';
    IF l_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE INDEX IX_TPDNO_HIS_PERM_DATE ' ||
            'ON T_PERMDETAIL_NO_ACTION_HISTORY ' ||
            '(PERM_ID, ACTION_DATE DESC)';
    END IF;
END;
/

CREATE OR REPLACE TRIGGER TRG_T_PERMDETAIL_NO_ACTION_HIS
AFTER INSERT OR UPDATE OR DELETE ON T_PERMDETAIL_NO
FOR EACH ROW
DECLARE
    l_action_type    T_PERMDETAIL_NO_ACTION_HISTORY.ACTION_TYPE%TYPE;
    l_action_user    T_PERMDETAIL_NO_ACTION_HISTORY.ACTION_USER%TYPE;
    l_change_detail  CLOB;
    l_perm_detail_id T_PERMDETAIL_NO_ACTION_HISTORY.PERM_DETAIL_ID%TYPE;
    l_flight_pk      T_PERMDETAIL_NO_ACTION_HISTORY.FLIGHT_PK%TYPE;
    l_perm_id        T_PERMDETAIL_NO_ACTION_HISTORY.PERM_ID%TYPE;
    l_flightnbr      T_PERMDETAIL_NO_ACTION_HISTORY.FLIGHTNBR%TYPE;

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
        l_action_type := 'INSERT';
        l_action_user := NVL(TRIM(:NEW.LASTUSER), 'SYSTEM');
        l_change_detail := 'NEW FLIGHT';
        l_perm_detail_id := :NEW.ID;
        l_flight_pk := :NEW.FLIGHT_PK;
        l_perm_id := :NEW.PERM_ID;
        l_flightnbr := :NEW.FLIGHTNBR;
    ELSIF DELETING THEN
        l_action_type := 'DELETE';
        l_action_user := NVL(TRIM(:OLD.LASTUSER), 'SYSTEM');
        l_change_detail := 'DELETED FLIGHT';
        l_perm_detail_id := :OLD.ID;
        l_flight_pk := :OLD.FLIGHT_PK;
        l_perm_id := :OLD.PERM_ID;
        l_flightnbr := :OLD.FLIGHTNBR;
    ELSE
        l_action_type := 'UPDATE';
        l_action_user := NVL(
            TRIM(:NEW.LASTUSER),
            NVL(TRIM(:OLD.LASTUSER), 'SYSTEM')
        );
        l_perm_detail_id := :NEW.ID;
        l_flight_pk := :NEW.FLIGHT_PK;
        l_perm_id := :NEW.PERM_ID;
        l_flightnbr := :NEW.FLIGHTNBR;

        APPEND_CHANGE('CALLSIGN', :OLD.FLIGHTNBR, :NEW.FLIGHTNBR);
        APPEND_CHANGE('REGISTRATION', :OLD.REGISTRATION, :NEW.REGISTRATION);
        APPEND_CHANGE('FROM', :OLD.FROM_AIRP, :NEW.FROM_AIRP);
        APPEND_CHANGE('TO', :OLD.TO_AIRP, :NEW.TO_AIRP);
        APPEND_CHANGE('ETD', :OLD.ETD, :NEW.ETD);
        APPEND_CHANGE('ETA', :OLD.ETA, :NEW.ETA);
        APPEND_CHANGE('DAY_FLIGHT', :OLD.DAYSFLIGHT, :NEW.DAYSFLIGHT);
        APPEND_CHANGE('CRAFT_ID', TO_CHAR(:OLD.CRAFT_ID), TO_CHAR(:NEW.CRAFT_ID));
        APPEND_CHANGE('PURPOSE_ID', :OLD.PURPOSE_ID, :NEW.PURPOSE_ID);
        APPEND_CHANGE('MTOW', TO_CHAR(:OLD.MTOW), TO_CHAR(:NEW.MTOW));
        APPEND_CHANGE(
            'MAX_DATE',
            TO_CHAR(:OLD.MAX_DATE, 'DD-MM-YYYY'),
            TO_CHAR(:NEW.MAX_DATE, 'DD-MM-YYYY')
        );
        APPEND_CHANGE('VIA', :OLD.VIA, :NEW.VIA);
        APPEND_CHANGE('REMARK', :OLD.REMARK, :NEW.REMARK);
        APPEND_CHANGE('REMARK_SEND', :OLD.REMARK_SEND, :NEW.REMARK_SEND);
        APPEND_CHANGE('STATUS', :OLD.STATUS, :NEW.STATUS);
        APPEND_CHANGE('IS_REN', TO_CHAR(:OLD.IS_REN), TO_CHAR(:NEW.IS_REN));

        -- Khong tao log neu chi LASTMODIFY/LASTUSER bi thay doi.
        IF l_change_detail IS NULL THEN
            RETURN;
        END IF;
    END IF;

    INSERT INTO T_PERMDETAIL_NO_ACTION_HISTORY
    (
        HISTORY_ID,
        PERM_DETAIL_ID,
        FLIGHT_PK,
        PERM_ID,
        ACTION_TYPE,
        FLIGHTNBR,
        ACTION_USER,
        ACTION_DATE,
        CHANGE_DETAIL
    )
    VALUES
    (
        SEQ_T_PERMDETAIL_NO_ACT_HIS.NEXTVAL,
        l_perm_detail_id,
        l_flight_pk,
        l_perm_id,
        l_action_type,
        l_flightnbr,
        l_action_user,
        SYSTIMESTAMP,
        l_change_detail
    );
END;
/

ALTER TRIGGER TRG_T_PERMDETAIL_NO_ACTION_HIS ENABLE;

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
       (
           'T_PERMDETAIL_NO_ACTION_HISTORY',
           'SEQ_T_PERMDETAIL_NO_ACT_HIS',
           'TRG_T_PERMDETAIL_NO_ACTION_HIS'
       )
 ORDER BY OBJECT_TYPE, OBJECT_NAME;
