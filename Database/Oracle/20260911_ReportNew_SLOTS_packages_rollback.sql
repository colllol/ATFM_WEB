-- ============================================================================
-- Rollback: go bo cac package tao ngay 11-09-2026 cho ReportNew + SLOTS.
--   FLIGHT_STATUS_PKG, DELAY_ALERT_PKG, DAYPLAN_COMPARE_PKG,
--   TRACKING_MAP_PKG, SLOT_COMPARE_PKG.
-- Cac trang van chay binh thuong bang SQL inline cu chung nao code-behind
-- chua chuyen sang goi package.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF

DECLARE
    TYPE T_NAMES IS TABLE OF VARCHAR2(60);
    V_NAMES T_NAMES := T_NAMES(
        'FLIGHT_STATUS_PKG',
        'DELAY_ALERT_PKG',
        'DAYPLAN_COMPARE_PKG',
        'TRACKING_MAP_PKG',
        'SLOT_COMPARE_PKG'
    );
    V_COUNT PLS_INTEGER;
BEGIN
    FOR i IN 1 .. V_NAMES.COUNT LOOP
        SELECT COUNT(*)
          INTO V_COUNT
          FROM USER_OBJECTS
         WHERE OBJECT_NAME = V_NAMES(i)
           AND OBJECT_TYPE = 'PACKAGE';

        IF V_COUNT > 0 THEN
            EXECUTE IMMEDIATE 'DROP PACKAGE ' || V_NAMES(i);
            DBMS_OUTPUT.PUT_LINE('Da drop ' || V_NAMES(i));
        ELSE
            DBMS_OUTPUT.PUT_LINE(V_NAMES(i) || ' khong ton tai, bo qua');
        END IF;
    END LOOP;
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('FLIGHT_STATUS_PKG', 'DELAY_ALERT_PKG', 'DAYPLAN_COMPARE_PKG',
                       'TRACKING_MAP_PKG', 'SLOT_COMPARE_PKG')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;
