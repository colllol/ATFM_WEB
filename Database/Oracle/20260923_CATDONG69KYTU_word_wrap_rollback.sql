-- ============================================================================
-- Rollback CATDONG69KYTU ve dung ma truoc 2026-09-23.
-- Luu y: ban cu cat cung tai moi 69 ky tu va co the cat giua tu.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

CREATE OR REPLACE FUNCTION CATDONG69KYTU(P_IN VARCHAR2)
RETURN VARCHAR2
AS
    vkq     VARCHAR2(1000) := '';
    str     VARCHAR2(1500) := P_IN;
BEGIN
    vkq := SUBSTR(str, 1, 69);

    FOR nLstr IN 1 .. LENGTH(str) LOOP
        IF MOD(nLstr, 69) = 0 THEN
            vkq := vkq || CHR(10) || SUBSTR(str, nLstr + 1, 69);
        END IF;
    END LOOP;

    RETURN vkq;
END CATDONG69KYTU;
/

ALTER PACKAGE P_FLY COMPILE BODY;

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('CATDONG69KYTU', 'P_FLY')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

EXIT SUCCESS
