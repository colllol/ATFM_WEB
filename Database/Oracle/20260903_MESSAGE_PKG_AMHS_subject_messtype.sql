-- ============================================================================
-- Gan OUTBOX_ORACLE.SUBJECT = messType khi gui AMHS tu MessManagement.aspx.
--
-- Web truyen messType vao tham so P_HEADER cua MESSAGE_PKG.message_send_one_amhs.
-- Script nay chi thay phan gan PRIORITY/SUBJECT trong insertOutbox_AMHS, khong
-- thay doi package specification hay chu ky API hien tai.
--
-- Chay bang user ATFM tren dung PDB/service. DBeaver: Execute SQL Script (Alt+X).
-- ============================================================================

DECLARE
    v_body          CLOB;
    v_new_body      CLOB;
    v_old_count     PLS_INTEGER;
    v_new_count     PLS_INTEGER;
    v_compile_error PLS_INTEGER;

    c_old_pattern CONSTANT VARCHAR2(1000) :=
        '\(case[[:space:]]+when[[:space:]]+nvl\(p_Header,''FF''\)[[:space:]]*=[[:space:]]*''FF''[[:space:]]+then[[:space:]]+0[[:space:]]+when[[:space:]]+nvl\(p_Header,''FF''\)[[:space:]]*=[[:space:]]*''GG''[[:space:]]+then[[:space:]]+1[[:space:]]+end\),[[:space:]]*''object'',[[:space:]]*sysdate';

    c_new_pattern CONSTANT VARCHAR2(1000) :=
        '\(case[[:space:]]+when[[:space:]]+upper\(trim\(p_Header\)\)[[:space:]]*=[[:space:]]*''GG''[[:space:]]+then[[:space:]]+1[[:space:]]+else[[:space:]]+0[[:space:]]+end\),[[:space:]]*nvl\(trim\(p_Header\),[[:space:]]*''object''\),[[:space:]]*sysdate';

    c_replacement CONSTANT VARCHAR2(1000) :=
        '(case when upper(trim(p_Header)) = ''GG'' then 1 else 0 end),' || CHR(10) ||
        '              nvl(trim(p_Header), ''object''), sysdate';
BEGIN
    IF USER <> 'ATFM' THEN
        RAISE_APPLICATION_ERROR(
            -20931,
            'Hay ket noi bang user ATFM tren dung PDB/service truoc khi chay'
        );
    END IF;

    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    v_body := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY',
        'MESSAGE_PKG',
        USER
    );

    v_old_count := REGEXP_COUNT(v_body, c_old_pattern, 1, 'in');
    v_new_count := REGEXP_COUNT(v_body, c_new_pattern, 1, 'in');

    IF v_old_count = 0 AND v_new_count = 1 THEN
        DBMS_OUTPUT.PUT_LINE(
            'MESSAGE_PKG da dung P_HEADER lam SUBJECT; khong can cap nhat.'
        );
    ELSIF v_old_count <> 1 THEN
        RAISE_APPLICATION_ERROR(
            -20932,
            'Khong tim thay duy nhat doan SUBJECT=''object''. OLD_COUNT=' ||
            v_old_count || ', NEW_COUNT=' || v_new_count
        );
    ELSE
        v_new_body := REGEXP_REPLACE(
            v_body,
            c_old_pattern,
            c_replacement,
            1,
            1,
            'in'
        );

        BEGIN
            EXECUTE IMMEDIATE v_new_body;

            SELECT COUNT(*)
              INTO v_compile_error
              FROM USER_ERRORS
             WHERE NAME = 'MESSAGE_PKG'
               AND TYPE = 'PACKAGE BODY';

            IF v_compile_error > 0 THEN
                EXECUTE IMMEDIATE v_body;
                RAISE_APPLICATION_ERROR(
                    -20933,
                    'Ban sua MESSAGE_PKG BODY co ' || v_compile_error ||
                    ' loi bien dich; da phuc hoi body truoc do'
                );
            END IF;

            DBMS_OUTPUT.PUT_LINE(
                'Da cap nhat MESSAGE_PKG: SUBJECT lay tu P_HEADER (messType).'
            );
        EXCEPTION
            WHEN OTHERS THEN
                BEGIN
                    EXECUTE IMMEDIATE v_body;
                EXCEPTION
                    WHEN OTHERS THEN
                        DBMS_OUTPUT.PUT_LINE(
                            'WARNING: khong tu phuc hoi duoc MESSAGE_PKG BODY: ' ||
                            SQLERRM
                        );
                END;
                RAISE;
        END;
    END IF;

    SELECT COUNT(*)
      INTO v_compile_error
      FROM USER_ERRORS
     WHERE NAME = 'MESSAGE_PKG'
       AND TYPE = 'PACKAGE BODY';

    IF v_compile_error > 0 THEN
        RAISE_APPLICATION_ERROR(
            -20934,
            'MESSAGE_PKG BODY con ' || v_compile_error || ' loi bien dich'
        );
    END IF;
END;
/

-- Ket qua phai co PACKAGE va PACKAGE BODY deu VALID.
SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
FROM USER_OBJECTS
WHERE OBJECT_NAME = 'MESSAGE_PKG'
  AND OBJECT_TYPE IN ('PACKAGE', 'PACKAGE BODY')
ORDER BY OBJECT_TYPE;

-- Phai thay dong gan SUBJECT bang NVL(TRIM(P_HEADER), 'object').
SELECT LINE, TEXT
FROM USER_SOURCE
WHERE NAME = 'MESSAGE_PKG'
  AND TYPE = 'PACKAGE BODY'
  AND
  (
      UPPER(TEXT) LIKE '%INSERT INTO OUTBOX_ORACLE%'
      OR UPPER(TEXT) LIKE '%NVL(TRIM(P_HEADER), ''OBJECT'')%'
  )
ORDER BY LINE;

-- Sau khi gui thu tu giao dien, doi chieu ban ghi moi nhat:
-- SELECT ID, SUBJECT, PRIORITY, TIME, CONTENT
-- FROM OUTBOX_ORACLE
-- ORDER BY ID DESC
-- FETCH FIRST 10 ROWS ONLY;
