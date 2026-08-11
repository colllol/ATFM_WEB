-- Rollback package duyet PermNo. Khong xoa du lieu schedule da tao.

BEGIN
    EXECUTE IMMEDIATE 'DROP PACKAGE PERM_NO_APPROVAL_PKG';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE <> -4043 THEN
            RAISE;
        END IF;
END;
/

