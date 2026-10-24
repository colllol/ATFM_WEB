-- Chay bang tai khoan co quyen ALTER tren ATFM.T_TRACKS_LOG.
-- Script co the chay lai an toan va xu ly doc lap tung cot.
-- Sau khi them cot, chay logger voi --mode all --full de backfill. Tool chi
-- dat NOT NULL khi khong con dong thieu toa do; log lich su con null duoc giu nguyen.
DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM ALL_TAB_COLUMNS
     WHERE OWNER = 'ATFM'
       AND TABLE_NAME = 'T_TRACKS_LOG'
       AND COLUMN_NAME = 'LAT';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'ALTER TABLE ATFM.T_TRACKS_LOG ADD (LAT NUMBER)';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM ALL_TAB_COLUMNS
     WHERE OWNER = 'ATFM'
       AND TABLE_NAME = 'T_TRACKS_LOG'
       AND COLUMN_NAME = 'LON';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'ALTER TABLE ATFM.T_TRACKS_LOG ADD (LON NUMBER)';
    END IF;
END;
/
