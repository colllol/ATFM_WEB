-- Rollback lich su thay doi chi tiet phep bay NO.

SET SERVEROUTPUT ON;

DECLARE
    PROCEDURE DROP_OBJECT
    (
        p_object_type IN VARCHAR2,
        p_object_name IN VARCHAR2
    )
    IS
        l_count PLS_INTEGER;
    BEGIN
        SELECT COUNT(*) INTO l_count
          FROM USER_OBJECTS
         WHERE OBJECT_TYPE = p_object_type
           AND OBJECT_NAME = p_object_name;

        IF l_count > 0 THEN
            EXECUTE IMMEDIATE 'DROP ' || p_object_type || ' ' || p_object_name;
        END IF;
    END DROP_OBJECT;
BEGIN
    DROP_OBJECT('TRIGGER', 'TRG_T_PERMDETAIL_NO_ACTION_HIS');
    DROP_OBJECT('TABLE', 'T_PERMDETAIL_NO_ACTION_HISTORY');
    DROP_OBJECT('SEQUENCE', 'SEQ_T_PERMDETAIL_NO_ACT_HIS');
END;
/
