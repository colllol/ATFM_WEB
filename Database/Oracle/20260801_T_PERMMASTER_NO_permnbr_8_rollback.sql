SET SERVEROUTPUT ON
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

PROMPT === Roll back T_PERMMASTER_NO PERMNBR changes ===

DECLARE
    l_ddl              CLOB;
    l_new_ddl          CLOB;
    l_line             VARCHAR2(32767);
    l_old_search       VARCHAR2(4000) :=
        q'~and (p_PERMNBR is null or p_PERMNBR ='' or upper(PERMNBR) = lpad(upper(p_PERMNBR),5,'0'))~';
    l_new_search       VARCHAR2(4000) :=
        q'~and
              (
                  p_PERMNBR is null
                  or trim(p_PERMNBR) is null
                  or upper(trim(PERMNBR)) like
                     '%' || upper(trim(p_PERMNBR)) || '%'
                  or
                  (
                      length(trim(PERMNBR)) <= 5
                      and length(trim(p_PERMNBR)) <= 5
                      and upper(lpad(trim(PERMNBR), 5, '0')) =
                          upper(lpad(trim(p_PERMNBR), 5, '0'))
                  )
              )~';
    l_old_valid        VARCHAR2(4000) :=
        q'~where upper(lpad(permnbr,5,'0'))= upper(lpad(p_flightnbr,5,'0'))~';
    l_new_valid        VARCHAR2(4000) :=
        q'~where
        (
            upper(trim(permnbr)) = upper(trim(p_flightnbr))
            or
            (
                length(trim(permnbr)) <= 5
                and length(trim(p_flightnbr)) <= 5
                and upper(lpad(trim(permnbr), 5, '0')) =
                    upper(lpad(trim(p_flightnbr), 5, '0'))
            )
        )~';
    l_error_count      PLS_INTEGER;
    l_max_length       PLS_INTEGER;
    l_char_length      PLS_INTEGER;
    l_bk_max_length    PLS_INTEGER;
    l_bk_char_length   PLS_INTEGER;

    FUNCTION replace_nth(
        p_source      IN CLOB,
        p_search      IN VARCHAR2,
        p_replacement IN VARCHAR2,
        p_occurrence  IN PLS_INTEGER
    ) RETURN CLOB
    IS
        l_result       CLOB;
        l_pos          PLS_INTEGER;
        l_source_len   PLS_INTEGER;
        l_suffix_len   PLS_INTEGER;
    BEGIN
        l_pos := DBMS_LOB.INSTR(
            p_source,
            p_search,
            1,
            p_occurrence
        );
        IF l_pos = 0 THEN
            RETURN p_source;
        END IF;

        l_source_len := DBMS_LOB.GETLENGTH(p_source);
        DBMS_LOB.CREATETEMPORARY(l_result, TRUE);

        IF l_pos > 1 THEN
            DBMS_LOB.COPY(l_result, p_source, l_pos - 1, 1, 1);
        END IF;

        DBMS_LOB.WRITEAPPEND(l_result, LENGTH(p_replacement), p_replacement);
        l_suffix_len := l_source_len - (l_pos + LENGTH(p_search) - 1);

        IF l_suffix_len > 0 THEN
            DBMS_LOB.COPY(
                l_result,
                p_source,
                l_suffix_len,
                l_pos + LENGTH(p_replacement),
                l_pos + LENGTH(p_search)
            );
        END IF;

        RETURN l_result;
    END replace_nth;
BEGIN
    SELECT NVL(MAX(LENGTH(PERMNBR)), 0)
      INTO l_max_length
      FROM T_PERMMASTER_NO;

    IF l_max_length > 5 THEN
        RAISE_APPLICATION_ERROR(
            -20301,
            'Rollback blocked: PERMNBR data longer than 5 characters exists'
        );
    END IF;

    SELECT CHAR_LENGTH
      INTO l_char_length
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_PERMMASTER_NO'
       AND COLUMN_NAME = 'PERMNBR';

    IF l_char_length = 8 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_PERMMASTER_NO MODIFY (PERMNBR VARCHAR2(5 BYTE))';
        DBMS_OUTPUT.PUT_LINE('Restored PERMNBR to VARCHAR2(5 BYTE).');
    ELSIF l_char_length <> 5 THEN
        RAISE_APPLICATION_ERROR(
            -20302,
            'Unexpected PERMNBR length: ' || l_char_length
        );
    END IF;

    SELECT NVL(MAX(LENGTH(PERMNBR)), 0)
      INTO l_bk_max_length
      FROM PERMMASTER_NO_BK;

    IF l_bk_max_length > 5 THEN
        RAISE_APPLICATION_ERROR(
            -20303,
            'Rollback blocked: PERMMASTER_NO_BK contains PERMNBR longer than 5 characters'
        );
    END IF;

    SELECT CHAR_LENGTH
      INTO l_bk_char_length
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'PERMMASTER_NO_BK'
       AND COLUMN_NAME = 'PERMNBR';

    IF l_bk_char_length = 8 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE PERMMASTER_NO_BK MODIFY (PERMNBR VARCHAR2(5 BYTE))';
        DBMS_OUTPUT.PUT_LINE('Restored PERMMASTER_NO_BK.PERM NBR to VARCHAR2(5 BYTE).');
    ELSIF l_bk_char_length <> 5 THEN
        RAISE_APPLICATION_ERROR(
            -20304,
            'Unexpected PERMMASTER_NO_BK.PERM NBR length: ' || l_bk_char_length
        );
    END IF;

    DBMS_LOB.CREATETEMPORARY(l_ddl, TRUE);
    l_line := 'CREATE OR REPLACE ';
    DBMS_LOB.WRITEAPPEND(l_ddl, LENGTH(l_line), l_line);

    FOR r IN
    (
        SELECT TEXT
          FROM USER_SOURCE
         WHERE NAME = 'PERM_PKG'
           AND TYPE = 'PACKAGE BODY'
         ORDER BY LINE
    )
    LOOP
        DBMS_LOB.WRITEAPPEND(l_ddl, LENGTH(r.TEXT), r.TEXT);
    END LOOP;

    IF DBMS_LOB.INSTR(l_ddl, l_new_search, 1, 2) > 0 THEN
        l_new_ddl := replace_nth(l_ddl, l_new_search, l_old_search, 2);
        l_ddl := l_new_ddl;
    ELSIF DBMS_LOB.INSTR(l_ddl, l_old_search, 1, 1) = 0 THEN
        RAISE_APPLICATION_ERROR(-20303, 'SearchExtentsion_Ext NO marker not found');
    END IF;

    IF DBMS_LOB.INSTR(l_ddl, l_new_valid, 1, 2) > 0 THEN
        l_new_ddl := replace_nth(l_ddl, l_new_valid, l_old_valid, 2);
        l_ddl := l_new_ddl;
    ELSIF DBMS_LOB.INSTR(l_ddl, l_old_valid, 1, 1) = 0 THEN
        RAISE_APPLICATION_ERROR(-20304, 'validFlightNbr NO marker not found');
    END IF;

    EXECUTE IMMEDIATE l_ddl;

    SELECT COUNT(*)
      INTO l_error_count
      FROM USER_ERRORS
     WHERE NAME = 'PERM_PKG'
       AND TYPE IN ('PACKAGE', 'PACKAGE BODY');

    IF l_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20305, 'PERM_PKG rollback compiled with errors');
    END IF;

    DBMS_OUTPUT.PUT_LINE('PERM_PKG legacy NO conditions restored successfully.');
END;
/

ALTER PACKAGE PERMISSION_HISTORY COMPILE;
ALTER PACKAGE PERMISSION_HISTORY COMPILE BODY;
ALTER PACKAGE PERM_IMP_PKG COMPILE BODY;
ALTER PACKAGE A_TEST_SEARCH COMPILE BODY;
ALTER PACKAGE FLIGHT_DAYFLIGHT COMPILE BODY;
ALTER PACKAGE PERMISSION2TEXT COMPILE BODY;
ALTER FUNCTION PERM_NO_HVN_CHANGE COMPILE;
ALTER FUNCTION PERM_NO_HVN_HUYCHUYEN COMPILE;
ALTER PROCEDURE SPRENDERSCHEDULE_HVN_NO COMPILE;
ALTER PROCEDURE SPRENDERSCHEDULE_HVN_NO_ADD COMPILE;
ALTER PROCEDURE SPRENDERSCHEDULE_HVN_NO_CHANGE COMPILE;
ALTER PROCEDURE SPRENDERSCHEDULE_HVN_NO_CS COMPILE;
ALTER TRIGGER PERMMASTER_NO_TRIGGER_HIS COMPILE;
