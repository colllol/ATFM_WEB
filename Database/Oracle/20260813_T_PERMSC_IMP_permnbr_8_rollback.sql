-- Chi rollback khi chac chan khong con Number dai hon 5 ky tu trong staging.
SET SERVEROUTPUT ON;

DECLARE
    v_max_length NUMBER;
BEGIN
    SELECT NVL(MAX(LENGTH(PERMNBR)), 0)
      INTO v_max_length
      FROM T_PERMSC_IMP;

    IF v_max_length > 5 THEN
        RAISE_APPLICATION_ERROR(
            -20834,
            'Cannot rollback: T_PERMSC_IMP contains PERMNBR length=' || v_max_length
        );
    END IF;

    EXECUTE IMMEDIATE
        'ALTER TABLE T_PERMSC_IMP MODIFY (PERMNBR VARCHAR2(5 CHAR))';
    DBMS_OUTPUT.PUT_LINE('Rollback complete: T_PERMSC_IMP.PERMNBR=VARCHAR2(5 CHAR).');
END;
/
