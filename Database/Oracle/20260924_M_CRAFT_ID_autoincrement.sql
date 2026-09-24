-- ============================================================================
-- Tao unique index va tu tang ID cho bang M_CRAFT da ton tai (Oracle 12c).
-- Sequence bat dau tu MAX(ID)+1; trigger chi cap ID neu INSERT bo trong ID.
-- Script co the chay lai. Kiem tra du lieu truoc khi tao unique index.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET DEFINE OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

DECLARE
    v_count       PLS_INTEGER;
    v_null_ids    PLS_INTEGER;
    v_rows        PLS_INTEGER;
    v_distinct    PLS_INTEGER;
    v_next_id     NUMBER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'M_CRAFT';

    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20961, 'Table M_CRAFT does not exist');
    END IF;

    SELECT COUNT(*), COUNT(ID), COUNT(DISTINCT ID), NVL(MAX(ID), 0) + 1
      INTO v_rows, v_count, v_distinct, v_next_id
      FROM M_CRAFT;

    v_null_ids := v_rows - v_count;
    IF v_null_ids > 0 OR v_count <> v_distinct THEN
        RAISE_APPLICATION_ERROR(
            -20962,
            'M_CRAFT contains NULL or duplicate ID values; correct data first'
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_SEQUENCES
     WHERE SEQUENCE_NAME = 'M_CRAFT_ID_SEQ';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE SEQUENCE M_CRAFT_ID_SEQ START WITH '
            || TO_CHAR(v_next_id, 'FM9999999999999999999999999990')
            || ' INCREMENT BY 1 NOCACHE NOCYCLE';
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'UX_M_CRAFT_ID';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE UNIQUE INDEX UX_M_CRAFT_ID ON M_CRAFT (ID)';
    END IF;

    EXECUTE IMMEDIATE q'~
        CREATE OR REPLACE TRIGGER TRG_M_CRAFT_ID
        BEFORE INSERT ON M_CRAFT
        FOR EACH ROW
        WHEN (NEW.ID IS NULL)
        BEGIN
            SELECT M_CRAFT_ID_SEQ.NEXTVAL INTO :NEW.ID FROM DUAL;
        END;~';

    DBMS_OUTPUT.PUT_LINE(
        'M_CRAFT ID setup complete. Next sequence value starts at '
        || TO_CHAR(v_next_id)
    );
END;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*)
      INTO v_count
      FROM USER_OBJECTS
     WHERE OBJECT_NAME IN ('M_CRAFT_ID_SEQ', 'TRG_M_CRAFT_ID', 'UX_M_CRAFT_ID')
       AND STATUS = 'VALID';

    IF v_count <> 3 THEN
        RAISE_APPLICATION_ERROR(
            -20963,
            'Expected valid sequence, trigger and index objects'
        );
    END IF;

    SELECT COUNT(*)
      INTO v_count
      FROM USER_INDEXES
     WHERE INDEX_NAME = 'UX_M_CRAFT_ID'
       AND STATUS = 'VALID'
       AND UNIQUENESS = 'UNIQUE';

    IF v_count <> 1 THEN
        RAISE_APPLICATION_ERROR(-20964, 'UX_M_CRAFT_ID is not a valid unique index');
    END IF;

    DBMS_OUTPUT.PUT_LINE('VERIFY OK: sequence, trigger and unique index are valid.');
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME IN ('M_CRAFT_ID_SEQ', 'TRG_M_CRAFT_ID', 'UX_M_CRAFT_ID')
 ORDER BY OBJECT_TYPE, OBJECT_NAME;
