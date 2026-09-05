-- Bang staging rieng cho chuc nang Tool/ImportsPermSC_LD_V2.aspx.
-- Khong thay doi T_PERMSC_IMP va khong anh huong cac chuc nang import cu.

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count
      FROM USER_TABLES
     WHERE TABLE_NAME = 'T_PERMSC_CANCEL_V2';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE q'~
            CREATE TABLE T_PERMSC_CANCEL_V2
            (
                ID                NUMBER(19)       NOT NULL,
                IMPORT_BATCH_ID   VARCHAR2(36 CHAR) NOT NULL,
                CALLSIGN          VARCHAR2(30 CHAR) NOT NULL,
                FROMDATE          DATE             NOT NULL,
                TODATE            DATE             NOT NULL,
                DAILY             VARCHAR2(20 CHAR) NOT NULL,
                CRAFT             VARCHAR2(20 CHAR),
                FROM_AIRP         VARCHAR2(4 CHAR)  NOT NULL,
                TO_AIRP           VARCHAR2(4 CHAR)  NOT NULL,
                ETD               VARCHAR2(10 CHAR) NOT NULL,
                ETA               VARCHAR2(10 CHAR),
                VIA               VARCHAR2(1000 CHAR),
                PERMTYPE          VARCHAR2(10 CHAR) DEFAULT 'LD' NOT NULL,
                FLIGHTTYPE        VARCHAR2(10 CHAR),
                REMARK            VARCHAR2(1000 CHAR),
                PERMDATE          DATE,
                AUTHOR            VARCHAR2(20 CHAR),
                OPER              VARCHAR2(10 CHAR),
                SEASON            VARCHAR2(10 CHAR),
                PERMNBR           VARCHAR2(8 CHAR),
                REFERENCE         VARCHAR2(1000 CHAR),
                BILLINGADDRESS    VARCHAR2(1000 CHAR),
                PERMCONTENT       VARCHAR2(4000 BYTE),
                VERSION           VARCHAR2(10 CHAR),
                PURPOSE           VARCHAR2(20 CHAR),
                REGISTRATION      VARCHAR2(30 CHAR),
                CREATED_BY        VARCHAR2(100 CHAR) NOT NULL,
                CREATED_AT        DATE DEFAULT SYSDATE NOT NULL,
                PROCESS_STATUS    VARCHAR2(20 CHAR) DEFAULT 'PENDING' NOT NULL,
                ERROR_MESSAGE     VARCHAR2(1000 CHAR),
                PROCESSED_AT      DATE,
                CONSTRAINT PK_T_PERMSC_CANCEL_V2 PRIMARY KEY (ID),
                CONSTRAINT CK_TPSC_V2_DATE CHECK (TODATE >= FROMDATE),
                CONSTRAINT CK_TPSC_V2_STATUS CHECK
                (
                    PROCESS_STATUS IN
                    ('PENDING', 'ERROR', 'PROCESSING', 'DONE', 'CANCELLED')
                )
            )
        ~';
    END IF;
END;
/

DECLARE
    v_count PLS_INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count
      FROM USER_SEQUENCES
     WHERE SEQUENCE_NAME = 'SEQ_T_PERMSC_CANCEL_V2';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE
            'CREATE SEQUENCE SEQ_T_PERMSC_CANCEL_V2 '
            || 'START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE';
    END IF;
END;
/

DECLARE
    PROCEDURE create_index
    (
        p_name IN VARCHAR2,
        p_sql  IN VARCHAR2
    ) IS
        v_count PLS_INTEGER;
    BEGIN
        SELECT COUNT(*) INTO v_count
          FROM USER_INDEXES
         WHERE INDEX_NAME = UPPER(p_name);

        IF v_count = 0 THEN
            EXECUTE IMMEDIATE p_sql;
        END IF;
    END create_index;
BEGIN
    create_index(
        'IX_TPSC_V2_USER_STATUS',
        'CREATE INDEX IX_TPSC_V2_USER_STATUS ON T_PERMSC_CANCEL_V2 '
        || '(CREATED_BY, PROCESS_STATUS, CREATED_AT)'
    );
    create_index(
        'IX_TPSC_V2_BATCH',
        'CREATE INDEX IX_TPSC_V2_BATCH ON T_PERMSC_CANCEL_V2 '
        || '(IMPORT_BATCH_ID, CREATED_BY)'
    );
    create_index(
        'IX_TPSC_V2_MATCH',
        'CREATE INDEX IX_TPSC_V2_MATCH ON T_PERMSC_CANCEL_V2 '
        || '(CALLSIGN, FROM_AIRP, TO_AIRP, ETD, FROMDATE, TODATE)'
    );
END;
/

COMMENT ON TABLE T_PERMSC_CANCEL_V2 IS
    'Staging rieng cua Cancel Permission SC V2; khong dung chung T_PERMSC_IMP';
COMMENT ON COLUMN T_PERMSC_CANCEL_V2.PROCESS_STATUS IS
    'PENDING/ERROR/PROCESSING/DONE/CANCELLED';

SELECT TABLE_NAME
  FROM USER_TABLES
 WHERE TABLE_NAME = 'T_PERMSC_CANCEL_V2';

SELECT INDEX_NAME, STATUS
  FROM USER_INDEXES
 WHERE TABLE_NAME = 'T_PERMSC_CANCEL_V2'
 ORDER BY INDEX_NAME;
