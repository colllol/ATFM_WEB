-- ============================================================================
-- Rollback: go bo ADSB_PERFORMANCE_PKG.
-- Trang AdsBPerformanceReport.aspx van chay binh thuong bang SQL inline cu
-- chung nao code-behind chua chuyen sang goi package nay.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    V_COUNT PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO V_COUNT
      FROM USER_OBJECTS
     WHERE OBJECT_NAME = 'ADSB_PERFORMANCE_PKG'
       AND OBJECT_TYPE = 'PACKAGE';

    IF V_COUNT > 0 THEN
        EXECUTE IMMEDIATE 'DROP PACKAGE ADSB_PERFORMANCE_PKG';
        DBMS_OUTPUT.PUT_LINE('Da drop ADSB_PERFORMANCE_PKG');
    ELSE
        DBMS_OUTPUT.PUT_LINE('ADSB_PERFORMANCE_PKG khong ton tai, bo qua');
    END IF;
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'ADSB_PERFORMANCE_PKG'
 ORDER BY OBJECT_TYPE;
