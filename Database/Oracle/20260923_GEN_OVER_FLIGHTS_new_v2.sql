-- ============================================================================
-- Them P_FLY.GEN_OVER_FLIGHTS_new_v2 va chi doi FLIGHT2TEXT_new (DANG = 3)
-- sang ham moi. GEN_OVER_FLIGHTS_new cu duoc giu nguyen.
--
-- V2 loc FLIGHTDATE bang DATE range, tinh priority bang SQL mot lan va ghi
-- SQLCODE/SQLERRM/backtrace vao T_LOG neu phat sinh loi.
-- Chay bang schema ATFM qua SQL*Plus/SQLcl.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

DECLARE
    c_pfly                 CONSTANT VARCHAR2(30) := 'P_FLY';
    c_permission2text      CONSTANT VARCHAR2(30) := 'PERMISSION2TEXT';
    c_v2_marker            CONSTANT VARCHAR2(100) :=
        '-- 2026-09-23: GEN_OVER_FLIGHTS_new_v2';
    c_v2_declaration       CONSTANT VARCHAR2(1000) :=
        'FUNCTION GEN_OVER_FLIGHTS_new_v2 (DATE_FLY DATE, '
        || 'PARTS_NUMBER OUT NUMBER, '
        || 'PARTS_CONTENT OUT PERMISSION2TEXT.MESSAGE_PARTS, '
        || 'PART_ID OUT PERMISSION2TEXT.MESSAGE_PARTS) RETURN BOOLEAN;';

    l_pfly_spec_original   CLOB;
    l_pfly_body_original   CLOB;
    l_perm_body_original   CLOB;
    l_pfly_spec_new        CLOB;
    l_pfly_body_new        CLOB;
    l_perm_body_new        CLOB;
    l_v2_impl              CLOB;
    l_call_replacement     CLOB;
    l_pos                  PLS_INTEGER;
    l_perm_func_pos        PLS_INTEGER;
    l_call_pos             PLS_INTEGER;
    l_cursor               INTEGER;
    l_invalid_count        PLS_INTEGER;
    l_error_count          PLS_INTEGER;

    PROCEDURE append_lob
    (
        p_target IN OUT NOCOPY CLOB,
        p_source IN CLOB,
        p_offset IN PLS_INTEGER,
        p_amount IN PLS_INTEGER
    ) IS
        c_chunk_size    CONSTANT PLS_INTEGER := 32000;
        l_source_offset PLS_INTEGER := p_offset;
        l_remaining     PLS_INTEGER := p_amount;
        l_chunk_amount  PLS_INTEGER;
        l_chunk         VARCHAR2(32767);
    BEGIN
        IF p_amount <= 0 THEN
            RETURN;
        END IF;

        WHILE l_remaining > 0 LOOP
            l_chunk_amount := LEAST(l_remaining, c_chunk_size);
            l_chunk := DBMS_LOB.SUBSTR(
                p_source, l_chunk_amount, l_source_offset
            );
            DBMS_LOB.WRITEAPPEND(p_target, LENGTH(l_chunk), l_chunk);
            l_source_offset := l_source_offset + l_chunk_amount;
            l_remaining := l_remaining - l_chunk_amount;
        END LOOP;
    END append_lob;

    PROCEDURE append_text
    (
        p_target IN OUT NOCOPY CLOB,
        p_text IN VARCHAR2
    ) IS
    BEGIN
        IF p_text IS NOT NULL THEN
            DBMS_LOB.WRITEAPPEND(p_target, LENGTH(p_text), p_text);
        END IF;
    END append_text;

    PROCEDURE parse_ddl(p_ddl IN CLOB) IS
    BEGIN
        l_cursor := DBMS_SQL.OPEN_CURSOR;
        DBMS_SQL.PARSE(l_cursor, p_ddl, DBMS_SQL.NATIVE);
        DBMS_SQL.CLOSE_CURSOR(l_cursor);
        l_cursor := NULL;
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -24344 THEN
                FOR e IN
                (
                    SELECT name, type, line, position, text
                      FROM USER_ERRORS
                     WHERE NAME IN (c_pfly, c_permission2text)
                     ORDER BY name, sequence
                ) LOOP
                    DBMS_OUTPUT.PUT_LINE(
                        e.name || ' ' || e.type || ' (' || e.line || ','
                        || e.position || '): ' || e.text
                    );
                END LOOP;
            END IF;
            IF l_cursor IS NOT NULL AND DBMS_SQL.IS_OPEN(l_cursor) THEN
                DBMS_SQL.CLOSE_CURSOR(l_cursor);
            END IF;
            RAISE;
    END parse_ddl;

    PROCEDURE replace_at
    (
        p_source IN CLOB,
        p_position IN PLS_INTEGER,
        p_old_length IN PLS_INTEGER,
        p_replacement IN CLOB,
        p_result OUT NOCOPY CLOB
    ) IS
    BEGIN
        DBMS_LOB.CREATETEMPORARY(p_result, TRUE);
        append_lob(p_result, p_source, 1, p_position - 1);
        append_lob(
            p_result,
            p_replacement,
            1,
            DBMS_LOB.GETLENGTH(p_replacement)
        );
        append_lob(
            p_result,
            p_source,
            p_position + p_old_length,
            DBMS_LOB.GETLENGTH(p_source) - p_position - p_old_length + 1
        );
    END replace_at;

    PROCEDURE restore_original IS
    BEGIN
        parse_ddl(l_pfly_spec_original);
        parse_ddl(l_pfly_body_original);
        parse_ddl(l_perm_body_original);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('WARNING: restore failed: ' || SQLERRM);
    END restore_original;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM, 'SQLTERMINATOR', FALSE
    );

    l_pfly_spec_original := DBMS_METADATA.GET_DDL(
        'PACKAGE_SPEC', c_pfly, USER
    );
    l_pfly_body_original := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY', c_pfly, USER
    );
    l_perm_body_original := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY', c_permission2text, USER
    );

    IF DBMS_LOB.INSTR(l_pfly_body_original, c_v2_marker) > 0 THEN
        DBMS_OUTPUT.PUT_LINE('P_FLY: GEN_OVER_FLIGHTS_new_v2 da ton tai.');
    ELSE
        -- DBMS_METADATA cua schema nay xuat package spec ket thuc bang END;
        -- (khong kem ten package).
        l_pos := DBMS_LOB.INSTR(l_pfly_spec_original, CHR(10) || 'END;');
        IF l_pos = 0 THEN
            RAISE_APPLICATION_ERROR(-20991, 'Khong tim thay END P_FLY trong spec');
        END IF;

        DBMS_LOB.CREATETEMPORARY(l_pfly_spec_new, TRUE);
        append_lob(l_pfly_spec_new, l_pfly_spec_original, 1, l_pos - 1);
        append_text(l_pfly_spec_new, CHR(10) || '  ' || c_v2_declaration || CHR(10));
        append_lob(
            l_pfly_spec_new, l_pfly_spec_original, l_pos,
            DBMS_LOB.GETLENGTH(l_pfly_spec_original) - l_pos + 1
        );

        l_v2_impl := TO_CLOB(q'~
-- 2026-09-23: GEN_OVER_FLIGHTS_new_v2
FUNCTION GEN_OVER_FLIGHTS_new_v2
(
    DATE_FLY      DATE,
    PARTS_NUMBER  OUT NUMBER,
    PARTS_CONTENT OUT PERMISSION2TEXT.MESSAGE_PARTS,
    PART_ID       OUT PERMISSION2TEXT.MESSAGE_PARTS
) RETURN BOOLEAN
IS
    TYPE day_flights_c IS REF CURSOR;
    c               day_flights_c;
    TYPE rc_type IS RECORD
    (
        flight_id    NUMBER,
        ma           VARCHAR2(240),
        flightnbr    VARCHAR2(20),
        from_airp    VARCHAR2(4),
        to_airp      VARCHAR2(4),
        etd          VARCHAR2(6),
        via          VARCHAR2(2000),
        remark       VARCHAR2(2000),
        purpose      VARCHAR2(2000)
    );
    rc              rc_type;
    l_line          VARCHAR2(32767);
    l_base_line     VARCHAR2(32767);
    l_via_remark    VARCHAR2(4000);
    l_on_day        VARCHAR2(10) := TO_CHAR(TRUNC(DATE_FLY), 'DD/MM/YYYY');
    l_stt           PLS_INTEGER := 0;
    l_part_length   PLS_INTEGER := 0;
    c_max_length    CONSTANT PLS_INTEGER := 1600;
    l_row_count     PLS_INTEGER := 0;
    l_is_special    BOOLEAN;
    l_error_code    NUMBER;
    l_error_text    VARCHAR2(4000);
BEGIN
    PARTS_NUMBER := 1;
    PARTS_CONTENT(1) := NULL;
    PART_ID(1) := NULL;

    INSERT INTO T_LOG (NAME, SQLCODE, ERR)
    VALUES ('GEN_OVER_FLIGHTS_new_v2', l_on_day, TO_CHAR(TRUNC(DATE_FLY), 'DD-MON-RR'));
    COMMIT;

    OPEN c FOR
        SELECT a.flight_id,
               craft_type_right(b.ma, a.craft_type) AS ma,
               REGEXP_REPLACE(a.flightnbr, '[^0-9A-Za-z]', '') AS flightnbr,
               a.from_airp,
               a.to_airp,
               a.etd,
               TRIM(a.via) AS via,
               a.remark,
               a.purpose
          FROM t_day_flights a
          LEFT JOIN m_craft_type b ON b.craft_id = a.craft_id
         WHERE a.flightdate >= TRUNC(DATE_FLY)
           AND a.flightdate < TRUNC(DATE_FLY) + 1
           AND a.from_airp NOT LIKE 'VV%'
           AND a.to_airp NOT LIKE 'VV%'
           AND a.status <> '4'
           AND a.status <> '5'
           AND a.permnbr <> 'NoPerm'
           AND a.permnbr <> 'NOPERM'
           AND a.isaccess = 1
           AND a.ismessage = 0
         ORDER BY NVL(
                      (
                          SELECT MIN(r.pr) * 10
                            FROM t_sp_route r
                           WHERE TRIM(a.via) = r.route_name
                              OR INSTR(a.via, r.route_name) > 0
                      ),
                      300002
                  ),
                  lcopy(a.flightnbr, 3),
                  a.craft_id,
                  a.etd;

    LOOP
        FETCH c INTO rc;
        EXIT WHEN c%NOTFOUND;

        l_row_count := l_row_count + 1;
        l_stt := l_stt + 1;
        l_base_line := TO_CHAR(l_stt) || ' '
            || RPAD(NVL(rc.ma, ''), 4)
            || char_loop(1)
            || RPAD(NVL(rc.flightnbr, ''), 8) || ' '
            || RPAD(NVL(rc.from_airp, ''), 4) || ' '
            || RPAD(NVL(rc.to_airp, ''), 4) || ' '
            || LPAD(etd_eta(l_on_day, rc.etd), 5) || ' ';

        l_is_special := INSTR(rc.remark, '777/767') > 0
            OR INSTR(rc.remark, '333/738') > 0
            OR INSTR(rc.remark, '738/333') > 0
            OR INSTR(rc.remark, 'B787/B767/B737') > 0
            OR INSTR(rc.remark, '333') > 0
            OR INSTR(rc.remark, '320/319') > 0
            OR INSTR(rc.remark, '319/320') > 0
            OR INSTR(rc.remark, '777') > 0
            OR INSTR(rc.remark, '33F') > 0
            OR INSTR(rc.remark, '380/350/330/321/777/744') > 0
            OR INSTR(rc.remark, 'M771/M768/R474/W1/B329/M753/M755') > 0
            OR INSTR(rc.remark, '330/340/777/787/380') > 0
            OR INSTR(rc.remark, 'A320/A319/B737/B738/B777/E190') > 0
            OR INSTR(rc.remark, '333/757/767/777/744') > 0
            OR INSTR(rc.remark, '748/773/772/333/748/777/388/738/739') > 0
            OR INSTR(rc.remark, '320/321/319/330') > 0
            OR INSTR(rc.remark, '747/777/7') > 0
            OR INSTR(rc.remark, '97G') > 0;

        IF l_is_special THEN
            l_via_remark := rc.via;
        ELSIF LENGTH(l_base_line) + LENGTH(rc.via) <= 65
              AND LENGTH(l_base_line) + LENGTH(rc.via) + LENGTH(rc.remark) > 65 THEN
            l_via_remark := rc.via || '     ' || rc.remark;
        ELSE
            l_via_remark := rc.via || '  ' || rc.remark;
        END IF;

        l_line := RTRIM(
            l_base_line || NVL(rc.purpose, '')
            || ' Other permitted airways published in Viet Nam AIP/'
            || NVL(l_via_remark, '')
        ) || CHR(10);
        l_line := chuanDong69KyTu(l_line);

        IF LENGTH(l_line) > 2000 THEN
            RAISE_APPLICATION_ERROR(
                -20992,
                'FLIGHT_ID=' || rc.flight_id || ' vuot 2000 ky tu sau khi format'
            );
        END IF;

        IF l_part_length > 0 AND l_part_length + LENGTH(l_line) > c_max_length THEN
            PARTS_NUMBER := PARTS_NUMBER + 1;
            PARTS_CONTENT(PARTS_NUMBER) := NULL;
            PART_ID(PARTS_NUMBER) := NULL;
            l_part_length := 0;
        END IF;

        PARTS_CONTENT(PARTS_NUMBER) := PARTS_CONTENT(PARTS_NUMBER) || l_line;
        IF PART_ID(PARTS_NUMBER) IS NULL THEN
            PART_ID(PARTS_NUMBER) := TO_CHAR(rc.flight_id);
        ELSE
            PART_ID(PARTS_NUMBER) := PART_ID(PARTS_NUMBER) || ',' || rc.flight_id;
        END IF;
        l_part_length := l_part_length + LENGTH(l_line);
    END LOOP;
    CLOSE c;

    IF l_row_count = 0 THEN
        PARTS_NUMBER := 0;
        RETURN FALSE;
    END IF;

    RETURN TRUE;
EXCEPTION
    WHEN OTHERS THEN
        l_error_code := SQLCODE;
        l_error_text := SUBSTR(
            SQLERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE,
            1,
            4000
        );
        BEGIN
            IF c%ISOPEN THEN
                CLOSE c;
            END IF;
        EXCEPTION
            WHEN OTHERS THEN NULL;
        END;
        BEGIN
            INSERT INTO T_LOG (NAME, SQLCODE, ERR)
            VALUES
            (
                'GEN_OVER_FLIGHTS_new_v2_ERROR',
                TO_CHAR(l_error_code),
                l_error_text
            );
            COMMIT;
        EXCEPTION
            WHEN OTHERS THEN NULL;
        END;
        PARTS_NUMBER := 0;
        RETURN FALSE;
END GEN_OVER_FLIGHTS_new_v2;

~');

        l_pos := DBMS_LOB.INSTR(
            l_pfly_body_original, 'FUNCTION GEN_OVER_FLIGHTS_new', 1
        );
        IF l_pos = 0 THEN
            RAISE_APPLICATION_ERROR(-20993, 'Khong tim thay GEN_OVER_FLIGHTS_new');
        END IF;
        DBMS_LOB.CREATETEMPORARY(l_pfly_body_new, TRUE);
        append_lob(l_pfly_body_new, l_pfly_body_original, 1, l_pos - 1);
        append_lob(l_pfly_body_new, l_v2_impl, 1, DBMS_LOB.GETLENGTH(l_v2_impl));
        append_lob(
            l_pfly_body_new, l_pfly_body_original, l_pos,
            DBMS_LOB.GETLENGTH(l_pfly_body_original) - l_pos + 1
        );


        BEGIN
            parse_ddl(l_pfly_spec_new);
            parse_ddl(l_pfly_body_new);
        EXCEPTION
            WHEN OTHERS THEN
                restore_original;
                RAISE;
        END;
        DBMS_OUTPUT.PUT_LINE('P_FLY: da them GEN_OVER_FLIGHTS_new_v2.');
    END IF;

    -- Chi thay call nam trong FLIGHT2TEXT_new; cac function cu khac giu nguyen.
    l_perm_func_pos := DBMS_LOB.INSTR(
        l_perm_body_original, 'FUNCTION FLIGHT2TEXT_new', 1
    );
    IF l_perm_func_pos = 0 THEN
        RAISE_APPLICATION_ERROR(-20994, 'Khong tim thay FLIGHT2TEXT_new');
    END IF;

    l_call_pos := DBMS_LOB.INSTR(
        l_perm_body_original,
        'P_FLY.GEN_OVER_FLIGHTS_new_v2',
        l_perm_func_pos
    );
    IF l_call_pos > 0 THEN
        DBMS_OUTPUT.PUT_LINE('PERMISSION2TEXT: FLIGHT2TEXT_new da goi V2.');
    ELSE
        l_call_pos := DBMS_LOB.INSTR(
            l_perm_body_original,
            'P_FLY.GEN_OVER_FLIGHTS_New',
            l_perm_func_pos
        );
        IF l_call_pos = 0 THEN
            RAISE_APPLICATION_ERROR(-20995, 'Khong tim thay call GEN_OVER_FLIGHTS_New');
        END IF;

        DBMS_LOB.CREATETEMPORARY(l_call_replacement, TRUE);
        append_text(l_call_replacement, 'P_FLY.GEN_OVER_FLIGHTS_new_v2');
        BEGIN
            replace_at(
                l_perm_body_original,
                l_call_pos,
                LENGTH('P_FLY.GEN_OVER_FLIGHTS_New'),
                l_call_replacement,
                l_perm_body_new
            );
            parse_ddl(l_perm_body_new);
        EXCEPTION
            WHEN OTHERS THEN
                restore_original;
                RAISE;
        END;
        DBMS_OUTPUT.PUT_LINE('PERMISSION2TEXT: FLIGHT2TEXT_new da chuyen sang V2.');
    END IF;

    SELECT COUNT(*)
      INTO l_invalid_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN (c_pfly, c_permission2text)
       AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
       AND STATUS <> 'VALID';

    SELECT COUNT(*)
      INTO l_error_count
      FROM USER_ERRORS
     WHERE NAME IN (c_pfly, c_permission2text);

    IF l_invalid_count > 0 OR l_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20996,
            'Compile khong dat: invalid=' || l_invalid_count
            || ', errors=' || l_error_count
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: P_FLY va PERMISSION2TEXT deu VALID.');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('P_FLY', 'PERMISSION2TEXT')
 ORDER BY OBJECT_NAME, OBJECT_TYPE;

SELECT POSITION, ARGUMENT_NAME, DATA_TYPE, IN_OUT
  FROM USER_ARGUMENTS
 WHERE PACKAGE_NAME = 'P_FLY'
   AND OBJECT_NAME = 'GEN_OVER_FLIGHTS_NEW_V2'
 ORDER BY SEQUENCE;

