-- Chỉ chạy rollback khi chắc chắn không cần giữ trạng thái Accepted.
-- Việc DROP COLUMN sẽ xóa toàn bộ trạng thái đã ghi nhận.
--
-- BẮT BUỘC trước khi chạy file này:
--   1. Gỡ p_ISACCEPTED và điều kiện ISACCEPTED khỏi
--      GET_FINISHED_FLIGHTS_MINITARY.
--   2. Gỡ ACCEPT_FIN_FLIGHTS_MILITARY và
--      EXPORT_QS_PLAN_MESSAGE khỏi package A_TEST_SEARCH.
--   3. Biên dịch package specification/body thành VALID.
--
-- Các T_PLAN_MESSAGE có MESS_TYPE='QS MESSAGE' không bị xóa tự động
-- để tránh mất điện văn đã gửi. Chỉ xóa thủ công khi nghiệp vụ cho phép.

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
