-- Them moc thoi gian cham FIR dau tien va lan chuyen sang FIR tiep theo.
-- Script idempotent, chay bang tai khoan co quyen ALTER tren ATFM.T_TRACKS_LOG.
DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM ALL_TAB_COLUMNS
     WHERE OWNER = 'ATFM'
       AND TABLE_NAME = 'T_TRACKS_LOG'
       AND COLUMN_NAME = 'TIME_IN';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'ALTER TABLE ATFM.T_TRACKS_LOG ADD (TIME_IN VARCHAR2(19))';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM ALL_TAB_COLUMNS
     WHERE OWNER = 'ATFM'
       AND TABLE_NAME = 'T_TRACKS_LOG'
       AND COLUMN_NAME = 'TIME_OUT';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'ALTER TABLE ATFM.T_TRACKS_LOG ADD (TIME_OUT VARCHAR2(19))';
    END IF;
END;
/
