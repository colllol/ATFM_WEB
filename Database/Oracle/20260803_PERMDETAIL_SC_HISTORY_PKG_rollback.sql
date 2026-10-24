-- Rollback API package tra cuu lich su chi tiet phep bay SC.

DECLARE
    l_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO l_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'PERMDETAIL_SC_HISTORY_PKG'
       AND OBJECT_TYPE = 'PACKAGE';

    IF l_count > 0 THEN
        EXECUTE IMMEDIATE 'DROP PACKAGE PERMDETAIL_SC_HISTORY_PKG';
    END IF;
END;
/
