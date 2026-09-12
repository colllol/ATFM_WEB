-- Rollback chuc nang tra cuu lich su T_SEND_AMHS.
-- Khong xoa du lieu T_SEND_AMHS va khong thay doi cac package gui AMHS.

BEGIN
    EXECUTE IMMEDIATE 'DROP PACKAGE SEND_AMHS_HISTORY_PKG';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE <> -4043 THEN
            RAISE;
        END IF;
END;
/
