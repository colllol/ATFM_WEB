-- ============================================================================
-- Sua PERMISSION2TEXT.FLIGHT2TEXT_new de chi tach PART tai ranh gioi chuyen bay.
--
-- Hien trang: code cu xem moi dong 69 ky tu la mot chuyen bay. Khi mot chuyen
-- co dong tiep theo (vi du PUBLISHED IN VIET NAM...), PART moi co the bat dau
-- bang dong tiep theo thay vi dong STT cua chuyen bay.
--
-- Quy tac moi: mot record la tu dong co STT den truoc dong STT tiep theo.
-- Neu record ke tiep vuot gioi han PART, chuyen ca record sang PART moi.
-- Script chi patch FLIGHT2TEXT_new, la ham RenderKhb/KHBN2TEXT dang goi.
-- Co the chay lai an toan.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

DECLARE
    c_package_name       CONSTANT VARCHAR2(30) := 'PERMISSION2TEXT';
    c_function_marker    CONSTANT VARCHAR2(100) := 'FUNCTION FLIGHT2TEXT_new';
    c_declare_marker     CONSTANT VARCHAR2(100) := 'ListID clob;';
    c_start_marker       CONSTANT VARCHAR2(100) := 'len:=length(str);';
    c_end_marker         CONSTANT VARCHAR2(100) := 'PARTS_NUMBER:=PARTS_CONTENT.last;';
    c_patch_marker       CONSTANT VARCHAR2(100) :=
        '-- 2026-09-23: keep each flight record in one PART.';

    l_body               CLOB;
    l_new_body           CLOB;
    l_body_after_declare CLOB;
    l_function_start     PLS_INTEGER;
    l_declare_start      PLS_INTEGER;
    l_start              PLS_INTEGER;
    l_end                PLS_INTEGER;
    l_cursor             INTEGER;
    l_error_count        PLS_INTEGER;
    l_declaration        VARCHAR2(2000);
    -- Ma va co the dai hon gioi han VARCHAR2 khi Oracle ghep bieu thuc ||.
    -- Dung CLOB de DBMS_LOB.PARSE nhan day du package body sau khi patch.
    l_replacement        CLOB;

    PROCEDURE append_lob
    (
        p_target IN OUT NOCOPY CLOB,
        p_source IN CLOB,
        p_offset IN PLS_INTEGER,
        p_amount IN PLS_INTEGER
    ) IS
        c_chunk_size    CONSTANT PLS_INTEGER := 32000;
        l_target_length PLS_INTEGER;
        l_source_offset PLS_INTEGER := p_offset;
        l_remaining     PLS_INTEGER := p_amount;
        l_chunk_amount  PLS_INTEGER;
        l_chunk          VARCHAR2(32767);
    BEGIN
        IF p_amount <= 0 THEN
            RETURN;
        END IF;

        -- DBMS_LOB.COPY khong giu duoc noi dung CLOB metadata lon tren Oracle
        -- 12.1 trong moi truong nay. Doc/ghi tung buffer VARCHAR2 de ghep body.
        WHILE l_remaining > 0 LOOP
            l_chunk_amount := LEAST(l_remaining, c_chunk_size);
            l_chunk := DBMS_LOB.SUBSTR(
                p_source,
                l_chunk_amount,
                l_source_offset
            );
            DBMS_LOB.WRITEAPPEND(p_target, LENGTH(l_chunk), l_chunk);
            l_source_offset := l_source_offset + l_chunk_amount;
            l_remaining := l_remaining - l_chunk_amount;
        END LOOP;
    END append_lob;

    PROCEDURE parse_ddl(p_ddl IN CLOB) IS
    BEGIN
        l_cursor := DBMS_SQL.OPEN_CURSOR;
        DBMS_SQL.PARSE(l_cursor, p_ddl, DBMS_SQL.NATIVE);
        DBMS_SQL.CLOSE_CURSOR(l_cursor);
        l_cursor := NULL;
    EXCEPTION
        WHEN OTHERS THEN
            IF l_cursor IS NOT NULL AND DBMS_SQL.IS_OPEN(l_cursor) THEN
                DBMS_SQL.CLOSE_CURSOR(l_cursor);
            END IF;
            RAISE;
    END parse_ddl;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    l_body := DBMS_METADATA.GET_DDL('PACKAGE_BODY', c_package_name, USER);

    IF DBMS_LOB.INSTR(l_body, c_patch_marker) > 0 THEN
        DBMS_OUTPUT.PUT_LINE('PERMISSION2TEXT: da co patch tach PART theo chuyen bay.');
    ELSE
        l_function_start := DBMS_LOB.INSTR(l_body, c_function_marker);
        IF l_function_start = 0 THEN
            RAISE_APPLICATION_ERROR(-20971, 'Khong tim thay FLIGHT2TEXT_new');
        END IF;

        l_declare_start := DBMS_LOB.INSTR(
            l_body,
            c_declare_marker,
            l_function_start
        );
        l_start := DBMS_LOB.INSTR(l_body, c_start_marker, l_function_start);
        l_end := DBMS_LOB.INSTR(l_body, c_end_marker, l_start);

        IF l_declare_start = 0 OR l_start = 0 OR l_end = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20972,
                'Source FLIGHT2TEXT_new khong dung marker de patch an toan'
            );
        END IF;

        l_declaration := c_declare_marker || CHR(10)
            || '  l_record_start PLS_INTEGER;' || CHR(10)
            || '  l_next_record  PLS_INTEGER;' || CHR(10)
            || '  l_part_length  PLS_INTEGER;' || CHR(10)
            || '  l_comma        PLS_INTEGER;' || CHR(10)
            || '  l_record       VARCHAR2(32767);' || CHR(10)
            || '  l_id_token     VARCHAR2(4000);';

        -- Chen declaration chi trong FLIGHT2TEXT_new, khong thay the cac ham khac.
        DBMS_LOB.CREATETEMPORARY(l_body_after_declare, TRUE);
        append_lob(
            l_body_after_declare,
            l_body,
            1,
            l_declare_start - 1
        );
        DBMS_LOB.WRITEAPPEND(
            l_body_after_declare,
            LENGTH(l_declaration),
            l_declaration
        );
        append_lob(
            l_body_after_declare,
            l_body,
            l_declare_start + LENGTH(c_declare_marker),
            DBMS_LOB.GETLENGTH(l_body)
                - l_declare_start - LENGTH(c_declare_marker) + 1
        );
        l_body := l_body_after_declare;

        -- Tim lai marker sau khi them declaration vao body.
        l_function_start := DBMS_LOB.INSTR(l_body, c_function_marker);
        l_start := DBMS_LOB.INSTR(l_body, c_start_marker, l_function_start);
        l_end := DBMS_LOB.INSTR(l_body, c_end_marker, l_start);

        -- q-quote giu chuoi vao mot literal nho hon 32 KB; tranh loi 06502
        -- do Oracle 12.1 danh gia chuoi noi dai bang toan tu ||.
        l_replacement := TO_CLOB(q'~
-- 2026-09-23: keep each flight record in one PART.
        -- A record gom tat ca dong cua mot chuyen bay (STT, dong tiep theo).
        ListID := SUBSTR(p_ListFlightId, 2, LENGTH(p_ListFlightId));
        nb := 1;
        l_part_length := 0;
        PARTS_CONTENT(nb) := NULL;
        PartId(nb) := NULL;
        l_record_start := 1;

        WHILE l_record_start <= LENGTH(str) LOOP
            -- Dong moi bat dau bang STT, vi du: " 260 A350 HVN18 ...".
            l_next_record := REGEXP_INSTR(
                str,
                CHR(10) || ' *[0-9]{1,4} +[[:alnum:]]',
                l_record_start + 1,
                1,
                0,
                'c'
            );

            IF l_next_record > 0 THEN
                -- Bao gom LF truoc STT cua record sau vao record hien tai.
                l_record := SUBSTR(
                    str, l_record_start, l_next_record - l_record_start + 1
                );
                l_record_start := l_next_record + 1;
            ELSE
                l_record := SUBSTR(str, l_record_start);
                l_record_start := LENGTH(str) + 1;
            END IF;

            l_comma := INSTR(ListID, ',');
            IF l_comma > 0 THEN
                l_id_token := SUBSTR(ListID, 1, l_comma - 1);
                ListID := SUBSTR(ListID, l_comma + 1);
            ELSE
                l_id_token := ListID;
                ListID := NULL;
            END IF;

            IF l_part_length > 0
               AND l_part_length + LENGTH(l_record) > max_byte THEN
                nb := nb + 1;
                PARTS_CONTENT(nb) := NULL;
                PartId(nb) := NULL;
                l_part_length := 0;
            END IF;

            PARTS_CONTENT(nb) := PARTS_CONTENT(nb) || l_record;
            IF PartId(nb) IS NULL THEN
                PartId(nb) := l_id_token;
            ELSIF l_id_token IS NOT NULL THEN
                PartId(nb) := PartId(nb) || ',' || l_id_token;
            END IF;
            l_part_length := l_part_length + LENGTH(l_record);
        END LOOP;

        PARTS_NUMBER := CASE WHEN l_part_length > 0 THEN nb ELSE 0 END;~');

        DBMS_LOB.CREATETEMPORARY(l_new_body, TRUE);
        append_lob(l_new_body, l_body, 1, l_start - 1);
        append_lob(
            l_new_body,
            l_replacement,
            1,
            DBMS_LOB.GETLENGTH(l_replacement)
        );
        append_lob(
            l_new_body,
            l_body,
            l_end + LENGTH(c_end_marker),
            DBMS_LOB.GETLENGTH(l_body) - l_end - LENGTH(c_end_marker) + 1
        );

        parse_ddl(l_new_body);
        DBMS_LOB.FREETEMPORARY(l_new_body);
        DBMS_OUTPUT.PUT_LINE('PERMISSION2TEXT: da patch FLIGHT2TEXT_new.');
    END IF;

    SELECT COUNT(*)
      INTO l_error_count
      FROM USER_ERRORS
     WHERE NAME = c_package_name;

    IF l_error_count > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20973,
            'PERMISSION2TEXT con ' || l_error_count || ' loi bien dich'
        );
    END IF;
END;
/

-- Kiem chung khong ghi du lieu: tao PART cho ngay mau va kiem tra moi PART
-- bat dau bang STT chuyen bay, khong bat dau bang dong tiep theo PUBLISHED.
DECLARE
    l_parts         PERMISSION2TEXT.MESSAGE_PARTS;
    l_part_ids      PERMISSION2TEXT.MESSAGE_PARTS;
    l_part_count    NUMBER;
    l_flight_ids    CLOB;
    l_success       BOOLEAN;
    l_record_count  NUMBER;
    l_id_count      NUMBER;
BEGIN
    l_success := PERMISSION2TEXT.FLIGHT2TEXT_new(
        DATE '2026-09-24',
        l_part_count,
        l_parts,
        1,
        NULL,
        l_flight_ids,
        l_part_ids
    );

    IF NOT l_success OR NVL(l_part_count, 0) = 0 THEN
        RAISE_APPLICATION_ERROR(-20974, 'Khong tao duoc PART de kiem chung');
    END IF;

    FOR i IN 1 .. l_part_count LOOP
        IF REGEXP_LIKE(l_parts(i), '^[[:space:]]*PUBLISHED', 'c') THEN
            RAISE_APPLICATION_ERROR(
                -20975,
                'PART ' || i || ' bat dau bang dong tiep theo PUBLISHED'
            );
        END IF;

        IF NOT REGEXP_LIKE(
            l_parts(i),
            '^[ ]*[0-9]{1,4} +[[:alnum:]]',
            'c'
        ) THEN
            RAISE_APPLICATION_ERROR(
                -20976,
                'PART ' || i || ' khong bat dau bang STT chuyen bay'
            );
        END IF;

        l_record_count := REGEXP_COUNT(
            l_parts(i),
            '(^|' || CHR(10) || ')[ ]*[0-9]{1,4} +[[:alnum:]]',
            1,
            'c'
        );
        l_id_count := CASE
            WHEN l_part_ids(i) IS NULL THEN 0
            ELSE REGEXP_COUNT(l_part_ids(i), ',') + 1
        END;

        IF l_record_count <> l_id_count THEN
            RAISE_APPLICATION_ERROR(
                -20977,
                'PART ' || i || ': so record=' || l_record_count
                || ', so FLIGHT_ID=' || l_id_count
            );
        END IF;

        DBMS_OUTPUT.PUT_LINE(
            'PART ' || i || ': records=' || l_record_count
            || ', length=' || LENGTH(l_parts(i))
        );
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: PART chi tach tai ranh gioi chuyen bay.');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'PERMISSION2TEXT'
 ORDER BY OBJECT_TYPE;

-- Khong EXIT tai day: cho phep script tong hop server_deploy goi bang @@.
