-- ============================================================================
-- Sua ATFM.CATDONG69KYTU: ngat dong toi da 69 ky tu tai khoang trang/tab.
--
-- Loi cu: SUBSTR(..., 69) cat giua tu, vi du PUBL<LF>ISHED hoac
-- PUBLI<LF>SHED. CHUANDONG69KYTU va P_FLY.GenFromTable_day_flightsNotAll
-- deu goi ham nay, nen sua o day ap dung dong nhat cho cac dien van.
--
-- Chay bang schema ATFM tren Oracle:
--   sqlplus ATFM@//HOST:1521/PDBORCL @20260923_CATDONG69KYTU_word_wrap.sql
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

CREATE OR REPLACE FUNCTION CATDONG69KYTU(P_IN VARCHAR2)
RETURN VARCHAR2
IS
    c_max_length CONSTANT PLS_INTEGER := 69;
    l_input      VARCHAR2(32767) := REPLACE(P_IN, CHR(13), '');
    l_output     VARCHAR2(32767) := '';
    l_line       VARCHAR2(32767);
    l_remaining  VARCHAR2(32767);
    l_newline    PLS_INTEGER;
    l_cut        PLS_INTEGER;

    PROCEDURE append_wrapped_line(p_line IN VARCHAR2) IS
    BEGIN
        l_remaining := p_line;

        WHILE LENGTH(l_remaining) > c_max_length LOOP
            -- Uu tien ngat sau tu cuoi cung nam trong 69 ky tu dau.
            l_cut := INSTR(
                SUBSTR(l_remaining, 1, c_max_length),
                ' ',
                -1
            );

            IF l_cut = 0 THEN
                l_cut := INSTR(
                    SUBSTR(l_remaining, 1, c_max_length),
                    CHR(9),
                    -1
                );
            END IF;

            IF l_cut > 0 THEN
                l_output := l_output
                    || RTRIM(SUBSTR(l_remaining, 1, l_cut - 1))
                    || CHR(10);
                l_remaining := LTRIM(SUBSTR(l_remaining, l_cut + 1));
            ELSE
                -- Chi co mot token dai hon 69 ky tu: bat buoc cat de dung chuan.
                l_output := l_output
                    || SUBSTR(l_remaining, 1, c_max_length)
                    || CHR(10);
                l_remaining := SUBSTR(l_remaining, c_max_length + 1);
            END IF;
        END LOOP;

        l_output := l_output || l_remaining;
    END append_wrapped_line;
BEGIN
    IF P_IN IS NULL THEN
        RETURN NULL;
    END IF;

    -- Giu nguyen cac dong da co; chi wrap tung dong rieng biet.
    LOOP
        l_newline := INSTR(l_input, CHR(10));

        IF l_newline = 0 THEN
            append_wrapped_line(l_input);
            EXIT;
        END IF;

        l_line := SUBSTR(l_input, 1, l_newline - 1);
        append_wrapped_line(l_line);
        l_output := l_output || CHR(10);
        l_input := SUBSTR(l_input, l_newline + 1);

        EXIT WHEN l_input IS NULL;
    END LOOP;

    RETURN l_output;
END CATDONG69KYTU;
/

ALTER PACKAGE P_FLY COMPILE BODY;

DECLARE
    l_text      VARCHAR2(32767);
    l_line      VARCHAR2(32767);
    l_remaining VARCHAR2(32767);
    l_newline   PLS_INTEGER;
    l_max_len   PLS_INTEGER := 0;
    l_errors    PLS_INTEGER;
BEGIN
    l_text := CHUANDONG69KYTU(
        '130 A321 HVN1371 VVPB VVTS 0600 0735 PAX OTHER PERMITTED AIRWAYS PUBLISHED IN VIET NAM AIP/Q2 320/ATR'
    );

    IF INSTR(l_text, 'PUBL' || CHR(10) || 'ISHED') > 0
       OR INSTR(l_text, 'PUBLI' || CHR(10) || 'SHED') > 0 THEN
        RAISE_APPLICATION_ERROR(-20961, 'Van cat giua tu PUBLISHED');
    END IF;

    l_remaining := l_text;
    LOOP
        l_newline := INSTR(l_remaining, CHR(10));
        IF l_newline = 0 THEN
            l_line := l_remaining;
        ELSE
            l_line := SUBSTR(l_remaining, 1, l_newline - 1);
        END IF;

        l_max_len := GREATEST(l_max_len, NVL(LENGTH(l_line), 0));
        EXIT WHEN l_newline = 0;
        l_remaining := SUBSTR(l_remaining, l_newline + 1);
    END LOOP;

    IF l_max_len > 69 THEN
        RAISE_APPLICATION_ERROR(-20962, 'Co dong vuot qua 69 ky tu: ' || l_max_len);
    END IF;

    SELECT COUNT(*)
      INTO l_errors
      FROM USER_ERRORS
     WHERE NAME IN ('CATDONG69KYTU', 'P_FLY');

    IF l_errors > 0 THEN
        RAISE_APPLICATION_ERROR(-20963, 'Con ' || l_errors || ' loi bien dich');
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: wrap tai ranh gioi tu, dong dai nhat=' || l_max_len
    );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('CATDONG69KYTU', 'P_FLY')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

-- Khong EXIT tai day: cho phep script tong hop server_deploy goi bang @@.
