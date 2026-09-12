-- Rollback luong gui bulk AMHS rieng.
-- Khong thay doi MESSAGE_PKG va du lieu da ghi vao OUTBOX_ORACLE.

BEGIN
    EXECUTE IMMEDIATE 'DROP PACKAGE MESSAGE_AMHS_PKG';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE <> -4043 THEN
            RAISE;
        END IF;
END;
/
