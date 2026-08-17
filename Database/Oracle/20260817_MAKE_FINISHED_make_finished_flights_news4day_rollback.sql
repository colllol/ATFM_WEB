-- Rollback rieng MAKE_FINISHED.make_finished_flights_news4day.
-- Script giu nguyen cac thanh phan khac cua package MAKE_FINISHED.
-- Script thuan Oracle/JDBC, co the chay bang DBeaver Alt+X.

DECLARE
    c_procedure_name CONSTANT VARCHAR2(30) :=
        'MAKE_FINISHED_FLIGHTS_NEWS4DAY';

    v_spec_ddl      CLOB;
    v_body_ddl      CLOB;
    v_original_spec VARCHAR2(32767);
    v_original_body VARCHAR2(32767);
    v_new_spec      VARCHAR2(32767);
    v_new_body      VARCHAR2(32767);
    v_start_pos     PLS_INTEGER;
    v_end_pos       PLS_INTEGER;
    v_next_pos      PLS_INTEGER;
BEGIN
    DBMS_METADATA.SET_TRANSFORM_PARAM(
        DBMS_METADATA.SESSION_TRANSFORM,
        'SQLTERMINATOR',
        FALSE
    );

    v_spec_ddl := DBMS_METADATA.GET_DDL(
        'PACKAGE_SPEC',
        'MAKE_FINISHED',
        USER
    );
    v_body_ddl := DBMS_METADATA.GET_DDL(
        'PACKAGE_BODY',
        'MAKE_FINISHED',
        USER
    );

    IF DBMS_LOB.GETLENGTH(v_spec_ddl) > 32767
       OR DBMS_LOB.GETLENGTH(v_body_ddl) > 32767 THEN
        RAISE_APPLICATION_ERROR(-20881, 'MAKE_FINISHED source vuot 32767 ky tu');
    END IF;

    v_original_spec := DBMS_LOB.SUBSTR(v_spec_ddl, 32767, 1);
    v_original_body := DBMS_LOB.SUBSTR(v_body_ddl, 32767, 1);
    v_new_spec := v_original_spec;
    v_new_body := v_original_body;

    -- Xoa declaration trong spec neu ton tai.
    v_start_pos := INSTR(
        UPPER(v_new_spec),
        'PROCEDURE ' || c_procedure_name
    );
    IF v_start_pos > 0 THEN
        v_end_pos := INSTR(v_new_spec, ';', v_start_pos);
        v_new_spec := SUBSTR(v_new_spec, 1, v_start_pos - 1)
            || SUBSTR(v_new_spec, v_end_pos + 1);
    END IF;

    -- Xoa implementation trong body neu ton tai.
    v_start_pos := INSTR(
        UPPER(v_new_body),
        'PROCEDURE ' || c_procedure_name
    );
    IF v_start_pos > 0 THEN
        v_next_pos := INSTR(
            UPPER(v_new_body),
            'PROCEDURE SP_COPPYBRAVO',
            v_start_pos + 1
        );

        IF v_next_pos = 0 THEN
            RAISE_APPLICATION_ERROR(-20882, 'Khong tim thay SP_COPPYBRAVO');
        END IF;

        v_new_body := SUBSTR(v_new_body, 1, v_start_pos - 1)
            || SUBSTR(v_new_body, v_next_pos);
    END IF;

    BEGIN
        EXECUTE IMMEDIATE v_new_spec;
        EXECUTE IMMEDIATE v_new_body;
    EXCEPTION
        WHEN OTHERS THEN
            BEGIN
                EXECUTE IMMEDIATE v_original_spec;
                EXECUTE IMMEDIATE v_original_body;
            EXCEPTION
                WHEN OTHERS THEN
                    NULL;
            END;
            RAISE;
    END;

    DBMS_OUTPUT.PUT_LINE(
        'Rollback MAKE_FINISHED.make_finished_flights_news4day completed.'
    );
END;
/

SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
  FROM USER_OBJECTS
 WHERE OBJECT_NAME = 'MAKE_FINISHED'
 ORDER BY OBJECT_TYPE;
