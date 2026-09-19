-- Rollback cac object moi cua ban toi uu ExportBravo.
-- Luu y: script nay khong khoi phuc du lieu T_DAY_FLIGHTS_BRAVO da export.

BEGIN
    EXECUTE IMMEDIATE 'DROP PACKAGE BRAVO_EXPORT_PKG';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE <> -4043 THEN
            RAISE;
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP INDEX IX_TDF_BRAVO_FLIGHTDATE';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE <> -1418 THEN
            RAISE;
        END IF;
END;
/

