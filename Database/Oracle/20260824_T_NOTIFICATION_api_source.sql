-- Tach khoa chong trung sang SOURCE_HASH de SOURCE_TYPE/SOURCE_KEY giu dung
-- gia tri source_type/source_key do API bao cao email tra ve.
-- Chay script nay truoc khi deploy ma Notification.ashx.cs moi.

DECLARE
    V_COLUMN_COUNT NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO V_COLUMN_COUNT
    FROM USER_TAB_COLUMNS
    WHERE TABLE_NAME = 'T_NOTIFICATION'
      AND COLUMN_NAME = 'SOURCE_HASH';

    IF V_COLUMN_COUNT = 0 THEN
        EXECUTE IMMEDIATE '
            ALTER TABLE T_NOTIFICATION
            ADD SOURCE_HASH VARCHAR2(80 CHAR) NULL';
    END IF;
END;
/

-- Chuyen hash Message-ID dang nam trong SOURCE_KEY sang SOURCE_HASH.
UPDATE T_NOTIFICATION
SET SOURCE_HASH = SOURCE_KEY
WHERE SOURCE_TYPE = 'EMAIL_API'
  AND SOURCE_HASH IS NULL
  AND SOURCE_KEY IS NOT NULL;

COMMIT;

-- Unique index cu dua tren (SOURCE_TYPE, SOURCE_KEY) khong con dung duoc vi
-- API tra source_key giong nhau cho moi email.
DECLARE
    V_INDEX_COUNT NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO V_INDEX_COUNT
    FROM USER_INDEXES
    WHERE INDEX_NAME = 'UX_T_NOTIFICATION_SOURCE';

    IF V_INDEX_COUNT > 0 THEN
        EXECUTE IMMEDIATE 'DROP INDEX UX_T_NOTIFICATION_SOURCE';
    END IF;
END;
/

-- Khoa chong trung moi: chi dua tren hash Message-ID.
DECLARE
    V_INDEX_COUNT NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO V_INDEX_COUNT
    FROM USER_INDEXES
    WHERE INDEX_NAME = 'UX_T_NOTIFICATION_SOURCE_HASH';

    IF V_INDEX_COUNT = 0 THEN
        EXECUTE IMMEDIATE '
            CREATE UNIQUE INDEX UX_T_NOTIFICATION_SOURCE_HASH
            ON T_NOTIFICATION (SOURCE_HASH)';
    END IF;
END;
/

-- Ghi lai source_type/source_key thuc te cua API cho cac thong bao email da co.
UPDATE T_NOTIFICATION
SET SOURCE_TYPE = 'email',
    SOURCE_KEY = '1'
WHERE SOURCE_TYPE = 'EMAIL_API'
  AND SOURCE_HASH IS NOT NULL;

COMMIT;

COMMENT ON COLUMN T_NOTIFICATION.SOURCE_TYPE IS
    'Gia tri source_type do API nguon tra ve, vi du email';
COMMENT ON COLUMN T_NOTIFICATION.SOURCE_KEY IS
    'Gia tri source_key do API nguon tra ve';
COMMENT ON COLUMN T_NOTIFICATION.SOURCE_HASH IS
    'Hash SHA-256 cua Message-ID, khoa duy nhat dung de dong bo khong trung lap';

