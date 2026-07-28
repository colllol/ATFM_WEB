-- Chỉ chạy rollback khi chắc chắn không cần giữ trạng thái Accepted.
-- Việc DROP COLUMN sẽ xóa toàn bộ trạng thái đã ghi nhận.

DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'IX_TFFM_ACCEPT_DATE';

    IF v_count > 0 THEN
        EXECUTE IMMEDIATE 'DROP INDEX IX_TFFM_ACCEPT_DATE';
    END IF;
END;
/

DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_FINISHFLIGHTS_MILITARY'
       AND COLUMN_NAME = 'ISACCEPTED';

    IF v_count > 0 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_FINISHFLIGHTS_MILITARY '
            || 'DROP COLUMN ISACCEPTED';
    END IF;
END;
/

-- Sau đó gỡ p_ISACCEPTED, điều kiện ISACCEPTED và
-- ACCEPT_FIN_FLIGHTS_MILITARY khỏi package A_TEST_SEARCH.
