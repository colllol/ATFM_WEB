SET SERVEROUTPUT ON
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

PROMPT === 1. Increase T_PERMMASTER_NO.PERM NBR to 8 characters ===

DECLARE
    l_data_type   USER_TAB_COLUMNS.DATA_TYPE%TYPE;
    l_char_length USER_TAB_COLUMNS.CHAR_LENGTH%TYPE;
BEGIN
    SELECT DATA_TYPE, CHAR_LENGTH
      INTO l_data_type, l_char_length
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'T_PERMMASTER_NO'
       AND COLUMN_NAME = 'PERMNBR';

    IF l_data_type <> 'VARCHAR2' THEN
        RAISE_APPLICATION_ERROR(
            -20201,
            'T_PERMMASTER_NO.PERM NBR must be VARCHAR2 before migration'
        );
    ELSIF l_char_length = 5 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE T_PERMMASTER_NO MODIFY (PERMNBR VARCHAR2(8 CHAR))';
        DBMS_OUTPUT.PUT_LINE('Changed PERMNBR from VARCHAR2(5) to VARCHAR2(8 CHAR).');
    ELSIF l_char_length = 8 THEN
        DBMS_OUTPUT.PUT_LINE('PERMNBR is already VARCHAR2(8); no table change required.');
    ELSE
        RAISE_APPLICATION_ERROR(
            -20202,
            'Unexpected PERMNBR length: ' || l_char_length
        );
    END IF;
END;
/

PROMPT === 1b. Increase PERMMASTER_NO_BK.PERM NBR to 8 characters ===

DECLARE
    l_data_type   USER_TAB_COLUMNS.DATA_TYPE%TYPE;
    l_char_length USER_TAB_COLUMNS.CHAR_LENGTH%TYPE;
BEGIN
    SELECT DATA_TYPE, CHAR_LENGTH
      INTO l_data_type, l_char_length
      FROM USER_TAB_COLUMNS
     WHERE TABLE_NAME = 'PERMMASTER_NO_BK'
       AND COLUMN_NAME = 'PERMNBR';

    IF l_data_type <> 'VARCHAR2' THEN
        RAISE_APPLICATION_ERROR(
            -20203,
            'PERMMASTER_NO_BK.PERM NBR must be VARCHAR2 before migration'
        );
    ELSIF l_char_length = 5 THEN
        EXECUTE IMMEDIATE
            'ALTER TABLE PERMMASTER_NO_BK MODIFY (PERMNBR VARCHAR2(8 CHAR))';
        DBMS_OUTPUT.PUT_LINE('Changed PERMMASTER_NO_BK.PERM NBR to VARCHAR2(8 CHAR).');
    ELSIF l_char_length = 8 THEN
        DBMS_OUTPUT.PUT_LINE('PERMMASTER_NO_BK.PERM NBR is already VARCHAR2(8).');
    ELSE
        RAISE_APPLICATION_ERROR(
            -20204,
            'Unexpected PERMMASTER_NO_BK.PERM NBR length: ' || l_char_length
        );
    END IF;
END;
/

PROMPT === 2. Patch PERM_PKG NO search/duplicate checks ===

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
    l_position         PLS_INTEGER;
    l_context          VARCHAR2(500);
    l_error_count      PLS_INTEGER;

    FUNCTION replace_first(
        p_source      IN CLOB,
        p_search      IN VARCHAR2,
        p_replacement IN VARCHAR2
    ) RETURN CLOB
    IS
        l_result       CLOB;
        l_pos          PLS_INTEGER;
        l_source_len   PLS_INTEGER;
        l_suffix_len   PLS_INTEGER;
    BEGIN
        l_pos := DBMS_LOB.INSTR(p_source, p_search, 1, 1);
        IF l_pos = 0 THEN
            RETURN p_source;
        END IF;

        l_source_len := DBMS_LOB.GETLENGTH(p_source);
        DBMS_LOB.CREATETEMPORARY(l_result, TRUE);

        IF l_pos > 1 THEN
            DBMS_LOB.COPY(l_result, p_source, l_pos - 1, 1, 1);
        END IF;

        DBMS_LOB.WRITEAPPEND(
            l_result,
            LENGTH(p_replacement),
            p_replacement
        );

        l_suffix_len :=
            l_source_len - (l_pos + LENGTH(p_search) - 1);

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
    END replace_first;
BEGIN
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

    IF DBMS_LOB.INSTR(l_ddl, l_old_search, 1, 1) > 0 THEN
        l_new_ddl := replace_first(l_ddl, l_old_search, l_new_search);
        l_ddl := l_new_ddl;
        DBMS_OUTPUT.PUT_LINE('Updated SearchExtentsion_Ext NO condition.');
    ELSIF DBMS_LOB.INSTR(l_ddl, l_new_search, 1, 2) = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20203,
            'Cannot find old or new SearchExtentsion_Ext NO condition'
        );
    ELSE
        DBMS_OUTPUT.PUT_LINE('SearchExtentsion_Ext NO condition is already updated.');
    END IF;

    l_position := DBMS_LOB.INSTR(l_ddl, l_old_valid, 1, 1);
    IF l_position > 0 THEN
        l_context := LOWER(
            DBMS_LOB.SUBSTR(
                l_ddl,
                LEAST(500, l_position - 1),
                GREATEST(1, l_position - 500)
            )
        );

        IF INSTR(l_context, 'from t_permmaster_no') = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20204,
                'Legacy validFlightNbr condition is not the NO branch'
            );
        END IF;

        l_new_ddl := replace_first(l_ddl, l_old_valid, l_new_valid);
        l_ddl := l_new_ddl;
        DBMS_OUTPUT.PUT_LINE('Updated validFlightNbr NO condition.');
    ELSIF DBMS_LOB.INSTR(l_ddl, l_new_valid, 1, 2) = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20205,
            'Cannot find old or new validFlightNbr NO condition'
        );
    ELSE
        DBMS_OUTPUT.PUT_LINE('validFlightNbr NO condition is already updated.');
    END IF;

    EXECUTE IMMEDIATE l_ddl;

    SELECT COUNT(*)
      INTO l_error_count
      FROM USER_ERRORS
     WHERE NAME = 'PERM_PKG'
       AND TYPE IN ('PACKAGE', 'PACKAGE BODY');

    IF l_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20206,
            'PERM_PKG compiled with ' || l_error_count || ' error(s)'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('PERM_PKG compiled successfully.');
END;
/

PROMPT === 3. Recompile objects invalidated by ALTER TABLE ===

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

PROMPT === 4. Result ===

SELECT TABLE_NAME,
       COLUMN_NAME,
       DATA_TYPE,
       DATA_LENGTH,
       CHAR_LENGTH,
       CHAR_USED,
       NULLABLE
  FROM USER_TAB_COLUMNS
 WHERE TABLE_NAME IN ('T_PERMMASTER_NO', 'PERMMASTER_NO_BK')
   AND COLUMN_NAME IN ('PERMNBR', 'PERMNBR_ID')
 ORDER BY TABLE_NAME, COLUMN_ID;

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERM_PKG'
 ORDER BY OBJECT_TYPE;

SELECT TYPE, LINE, TRIM(TEXT) AS TEXT
  FROM USER_SOURCE
 WHERE NAME = 'PERM_PKG'
   AND TYPE = 'PACKAGE BODY'
   AND LOWER(TEXT) LIKE '%upper(trim(permnbr))%'
 ORDER BY LINE;

SELECT OBJECT_TYPE, OBJECT_NAME, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN
       (
           'PERMISSION_HISTORY',
           'PERM_IMP_PKG',
           'A_TEST_SEARCH',
           'FLIGHT_DAYFLIGHT',
           'PERMISSION2TEXT',
           'PERM_NO_HVN_CHANGE',
           'PERM_NO_HVN_HUYCHUYEN',
           'SPRENDERSCHEDULE_HVN_NO',
           'SPRENDERSCHEDULE_HVN_NO_ADD',
           'SPRENDERSCHEDULE_HVN_NO_CHANGE',
           'SPRENDERSCHEDULE_HVN_NO_CS',
           'PERMMASTER_NO_TRIGGER_HIS'
       )
 ORDER BY STATUS, OBJECT_TYPE, OBJECT_NAME;
