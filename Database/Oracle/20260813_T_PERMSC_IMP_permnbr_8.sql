-- Mo rong Number cua bang staging huy/thay doi phep SC tu 5 len 8 ky tu.
-- Script idempotent: co the chay lai an toan tren schema ATFM.
SET SERVEROUTPUT ON;

DECLARE
    v_data_type   USER_TAB_COLUMNS.DATA_TYPE%TYPE;
    v_char_length USER_TAB_COLUMNS.CHAR_LENGTH%TYPE;
BEGIN
    SELECT DATA_TYPE, CHAR_LENGTH
      INTO v_data_type, v_char_length
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_PERMSC_IMP'
       AND COLUMN_NAME = 'PERMNBR';

    IF v_data_type <> 'VARCHAR2' THEN
        RAISE_APPLICATION_ERROR(-20831, 'T_PERMSC_IMP.PERMNBR is not VARCHAR2');
    END IF;

    IF v_char_length < 8 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_PERMSC_IMP MODIFY (PERMNBR VARCHAR2(8 CHAR))';
        DBMS_OUTPUT.PUT_LINE('Changed T_PERMSC_IMP.PERMNBR to VARCHAR2(8 CHAR).');
    ELSE
        DBMS_OUTPUT.PUT_LINE('T_PERMSC_IMP.PERMNBR already supports at least 8 characters.');
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20832, 'Missing column T_PERMSC_IMP.PERMNBR');
END;
/

DECLARE
    v_char_length USER_TAB_COLUMNS.CHAR_LENGTH%TYPE;
BEGIN
    SELECT CHAR_LENGTH
      INTO v_char_length
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_PERMSC_IMP'
       AND COLUMN_NAME = 'PERMNBR';

    IF v_char_length < 8 THEN
        RAISE_APPLICATION_ERROR(-20833, 'Verify failed: PERMNBR length=' || v_char_length);
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: T_PERMSC_IMP.PERMNBR length=' || v_char_length);
END;
/
