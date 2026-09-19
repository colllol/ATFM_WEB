-- ============================================================================
-- TOI UU DANH SACH PHEP SC/NO THEO PERMDATE TRONG 12 THANG GAN NHAT
-- Schema du kien: ATFM
--
-- DBeaver: mo file va chon Execute SQL Script (Alt+X).
-- SQL*Plus/SQLcl:
--   sqlplus ATFM@//HOST:1521/SERVICE @20260814_PermissionLists_PERMDATE_indexes.sql
--
-- Phan web can publish kem theo:
--   prjApplication/Permission/ListPermissionSC.aspx.cs
--   prjApplication/Permission/ListPermissionNo.aspx.cs
--   prjApplication/bin/prjApplication.dll
--
-- Script co the chay lai. Neu da co index VALID/VISIBLE voi PERMDATE la cot dau tien,
-- script se giu nguyen index hien co va khong tao index trung lap.
-- ============================================================================

DECLARE
    v_missing VARCHAR2(4000);
    v_count   PLS_INTEGER;

    PROCEDURE require_column
    (
        p_table  IN VARCHAR2,
        p_column IN VARCHAR2
    ) IS
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_TAB_COLUMNS
         WHERE TABLE_NAME = UPPER(p_table)
           AND COLUMN_NAME = UPPER(p_column);

        IF v_count = 0 THEN
            v_missing := v_missing || CHR(10)
                || '- COLUMN ' || UPPER(p_table) || '.' || UPPER(p_column);
        END IF;
    END require_column;
BEGIN
    IF UPPER(USER) <> 'ATFM' THEN
        RAISE_APPLICATION_ERROR(
            -20870,
            'Phai chay script bang schema ATFM. Schema hien tai=' || USER
        );
    END IF;

    require_column('T_PERMMASTER_SC', 'PERMDATE');
    require_column('T_PERMMASTER_NO', 'PERMDATE');

    IF v_missing IS NOT NULL THEN
        RAISE_APPLICATION_ERROR(
            -20871,
            'Thieu cau truc de toi uu danh sach phep:'
            || SUBSTR(v_missing, 1, 1800)
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE('PRECHECK OK. Schema=' || USER);
END;
/

DECLARE
    PROCEDURE ensure_permdate_index
    (
        p_table      IN VARCHAR2,
        p_index_name IN VARCHAR2
    ) IS
        v_count PLS_INTEGER;
    BEGIN
        SELECT COUNT(*)
          INTO v_count
          FROM USER_INDEXES i
          JOIN USER_IND_COLUMNS c
            ON c.INDEX_NAME = i.INDEX_NAME
           AND c.TABLE_NAME = i.TABLE_NAME
         WHERE i.TABLE_NAME = UPPER(p_table)
           AND i.STATUS = 'VALID'
           AND i.VISIBILITY = 'VISIBLE'
           AND c.COLUMN_POSITION = 1
           AND c.COLUMN_NAME = 'PERMDATE';

        IF v_count > 0 THEN
            DBMS_OUTPUT.PUT_LINE(
                'SKIP ' || UPPER(p_table)
                || ': da co index VALID bat dau bang PERMDATE.'
            );
            RETURN;
        END IF;

        SELECT COUNT(*)
          INTO v_count
          FROM USER_INDEXES
         WHERE INDEX_NAME = UPPER(p_index_name);

        IF v_count > 0 THEN
            RAISE_APPLICATION_ERROR(
                -20872,
                'Index ' || UPPER(p_index_name)
                || ' da ton tai nhung khong phu hop/khong VALID'
            );
        END IF;

        EXECUTE IMMEDIATE
            'CREATE INDEX ' || DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(p_index_name))
            || ' ON ' || DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(p_table))
            || ' (PERMDATE) ONLINE';

        DBMS_OUTPUT.PUT_LINE(
            'CREATED ' || UPPER(p_index_name)
            || ' ON ' || UPPER(p_table) || '(PERMDATE) ONLINE.'
        );
    END ensure_permdate_index;
BEGIN
    ensure_permdate_index('T_PERMMASTER_SC', 'IX_TPMSC_PERMDATE');
    ensure_permdate_index('T_PERMMASTER_NO', 'IX_TPMNO_PERMDATE');
END;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(DISTINCT i.TABLE_NAME)
      INTO v_count
      FROM USER_INDEXES i
      JOIN USER_IND_COLUMNS c
        ON c.INDEX_NAME = i.INDEX_NAME
       AND c.TABLE_NAME = i.TABLE_NAME
     WHERE i.TABLE_NAME IN ('T_PERMMASTER_SC', 'T_PERMMASTER_NO')
       AND i.STATUS = 'VALID'
       AND i.VISIBILITY = 'VISIBLE'
       AND c.COLUMN_POSITION = 1
       AND c.COLUMN_NAME = 'PERMDATE';

    IF v_count <> 2 THEN
        RAISE_APPLICATION_ERROR(
            -20873,
            'Chua co du index PERMDATE VALID cho hai bang master'
        );
    END IF;

    DBMS_OUTPUT.PUT_LINE(
        'VERIFY OK: T_PERMMASTER_SC va T_PERMMASTER_NO deu co index PERMDATE.'
    );
END;
/

SELECT i.TABLE_NAME,
       i.INDEX_NAME,
       c.COLUMN_NAME,
       c.COLUMN_POSITION,
       c.DESCEND,
       i.STATUS,
       i.LAST_ANALYZED
  FROM USER_INDEXES i
  JOIN USER_IND_COLUMNS c
    ON c.INDEX_NAME = i.INDEX_NAME
   AND c.TABLE_NAME = i.TABLE_NAME
 WHERE i.TABLE_NAME IN ('T_PERMMASTER_SC', 'T_PERMMASTER_NO')
   AND c.COLUMN_POSITION = 1
   AND c.COLUMN_NAME = 'PERMDATE'
 ORDER BY i.TABLE_NAME, i.INDEX_NAME;

BEGIN
    DBMS_OUTPUT.PUT_LINE('============================================================');
    DBMS_OUTPUT.PUT_LINE('HOAN TAT TOI UU DANH SACH PHEP THEO PERMDATE');
    DBMS_OUTPUT.PUT_LINE(
        'ListPermissionSC/NO: 12 thang truoc den het ngay hien tai.'
    );
    DBMS_OUTPUT.PUT_LINE(
        'SearchPermissionAdv: van tim toan bo theo khoang ngay lua chon.'
    );
    DBMS_OUTPUT.PUT_LINE('============================================================');
END;
/
