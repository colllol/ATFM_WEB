-- Doi chieu danh sach da chap nhan voi nguon GOINGON da chuyen.
-- Chay trong schema ATFM bang Execute SQL Script. Chi tao package doc rieng.
-- Bo loc sao tu 20260728_T_FINISHED_FLIGHTS_isaccepted.sql.
-- P_ISACCEPTED/PAGESIZE/PAGEINDEX giu chu ky API, khong ap dung cho nguon.
-- Can doi chieu voi FINISHED_STATUS_PKG tren server neu package do da thay doi.
CREATE OR REPLACE PACKAGE FINISHED_COMPARE_PKG AS
    TYPE T_CURSOR IS REF CURSOR;
    PROCEDURE COUNT_GOINGON
    (
        PERMNBR         IN VARCHAR2 DEFAULT NULL,
        REGISTRATION    IN VARCHAR2 DEFAULT NULL,
        FROM_AIRP       IN VARCHAR2 DEFAULT NULL,
        TO_AIRP         IN VARCHAR2 DEFAULT NULL,
        PURPOSE         IN VARCHAR2 DEFAULT NULL,
        VALIDHOURS      IN NUMBER DEFAULT NULL,
        OPER_ID         IN VARCHAR2 DEFAULT NULL,
        PERMTYPE        IN VARCHAR2 DEFAULT NULL,
        FLIGHT_TYPE     IN VARCHAR2 DEFAULT NULL,
        CRAFT_TYPE      IN VARCHAR2 DEFAULT NULL,
        CRAFT_ID        IN NUMBER DEFAULT NULL,
        REAL_CRAFT_TYPE IN VARCHAR2 DEFAULT NULL,
        FLIGHTNBR       IN VARCHAR2 DEFAULT NULL,
        VIA             IN VARCHAR2 DEFAULT NULL,
        FPL_VIA         IN VARCHAR2 DEFAULT NULL,
        REMARK          IN VARCHAR2 DEFAULT NULL,
        ETA             IN VARCHAR2 DEFAULT NULL,
        ETD             IN VARCHAR2 DEFAULT NULL,
        ATA             IN VARCHAR2 DEFAULT NULL,
        ATD             IN VARCHAR2 DEFAULT NULL,
        STARTDATE       IN VARCHAR2,
        FINISHDATE      IN VARCHAR2,
        KHUNGGIO1       IN VARCHAR2 DEFAULT '0000',
        KHUNGGIO2       IN VARCHAR2 DEFAULT '2359',
        WHECONDITION    IN VARCHAR2 DEFAULT NULL,
        CAT_HA          IN NUMBER DEFAULT 0,
        TYPEOPER        IN NUMBER DEFAULT 0,
        TYPEFLIGHT      IN NUMBER DEFAULT 0,
        FIR             IN VARCHAR2 DEFAULT '0',
        SANBAYDI        IN VARCHAR2 DEFAULT NULL,
        SANBAYDEN       IN VARCHAR2 DEFAULT NULL,
        P_ISACCEPTED    IN NUMBER DEFAULT 0,
        PAGESIZE        IN NUMBER DEFAULT 100,
        PAGEINDEX       IN NUMBER DEFAULT 0,
        OUT_CURSOR      OUT T_CURSOR
    );

END FINISHED_COMPARE_PKG;
/
CREATE OR REPLACE PACKAGE BODY FINISHED_COMPARE_PKG AS
    FUNCTION PARSE_DATE(p_value IN VARCHAR2)
        RETURN DATE
    IS
    BEGIN
        IF REGEXP_LIKE(TRIM(p_value), '^[0-9]{2}-[0-9]{2}-[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(p_value), 'FXDD-MM-YYYY');
        ELSIF REGEXP_LIKE(TRIM(p_value), '^[0-9]{2}/[0-9]{2}/[0-9]{4}$') THEN
            RETURN TO_DATE(TRIM(p_value), 'FXDD/MM/YYYY');
        END IF;

        RAISE_APPLICATION_ERROR(-20001, 'Date must use DD-MM-YYYY');
    END PARSE_DATE;

    PROCEDURE COUNT_GOINGON
    (
        PERMNBR         IN VARCHAR2 DEFAULT NULL,
        REGISTRATION    IN VARCHAR2 DEFAULT NULL,
        FROM_AIRP       IN VARCHAR2 DEFAULT NULL,
        TO_AIRP         IN VARCHAR2 DEFAULT NULL,
        PURPOSE         IN VARCHAR2 DEFAULT NULL,
        VALIDHOURS      IN NUMBER DEFAULT NULL,
        OPER_ID         IN VARCHAR2 DEFAULT NULL,
        PERMTYPE        IN VARCHAR2 DEFAULT NULL,
        FLIGHT_TYPE     IN VARCHAR2 DEFAULT NULL,
        CRAFT_TYPE      IN VARCHAR2 DEFAULT NULL,
        CRAFT_ID        IN NUMBER DEFAULT NULL,
        REAL_CRAFT_TYPE IN VARCHAR2 DEFAULT NULL,
        FLIGHTNBR       IN VARCHAR2 DEFAULT NULL,
        VIA             IN VARCHAR2 DEFAULT NULL,
        FPL_VIA         IN VARCHAR2 DEFAULT NULL,
        REMARK          IN VARCHAR2 DEFAULT NULL,
        ETA             IN VARCHAR2 DEFAULT NULL,
        ETD             IN VARCHAR2 DEFAULT NULL,
        ATA             IN VARCHAR2 DEFAULT NULL,
        ATD             IN VARCHAR2 DEFAULT NULL,
        STARTDATE       IN VARCHAR2,
        FINISHDATE      IN VARCHAR2,
        KHUNGGIO1       IN VARCHAR2 DEFAULT '0000',
        KHUNGGIO2       IN VARCHAR2 DEFAULT '2359',
        WHECONDITION    IN VARCHAR2 DEFAULT NULL,
        CAT_HA          IN NUMBER DEFAULT 0,
        TYPEOPER        IN NUMBER DEFAULT 0,
        TYPEFLIGHT      IN NUMBER DEFAULT 0,
        FIR             IN VARCHAR2 DEFAULT '0',
        SANBAYDI        IN VARCHAR2 DEFAULT NULL,
        SANBAYDEN       IN VARCHAR2 DEFAULT NULL,
        P_ISACCEPTED    IN NUMBER DEFAULT 0,
        PAGESIZE        IN NUMBER DEFAULT 100,
        PAGEINDEX       IN NUMBER DEFAULT 0,
        OUT_CURSOR      OUT T_CURSOR
    )
    IS
        v_start_date  DATE;
        v_finish_date DATE;
        v_filter_date DATE;
    BEGIN
        v_start_date := PARSE_DATE(STARTDATE);
        v_finish_date := PARSE_DATE(FINISHDATE);

        IF v_finish_date < v_start_date THEN
            RAISE_APPLICATION_ERROR(
                -20002,
                'FINISHDATE must be greater than or equal to STARTDATE'
            );
        END IF;

        IF TRIM(WHECONDITION) IS NOT NULL THEN
            v_filter_date := PARSE_DATE(WHECONDITION);
        END IF;

        OPEN OUT_CURSOR FOR
            SELECT COUNT(*) AS GOINGON_COUNT
              FROM T_DAY_FLIGHTS_GOINGON t
             WHERE t.MOVEFINISH = 1
                     AND t.FLIGHTDATE >= v_start_date
                     AND t.FLIGHTDATE < v_finish_date + 1
                     AND
                     (
                         v_filter_date IS NULL
                         OR
                         (
                             t.FLIGHTDATE >= v_filter_date
                             AND t.FLIGHTDATE < v_filter_date + 1
                         )
                     )
                     AND
                     (
                         TRIM(PERMTYPE) IS NULL
                         OR UPPER(t.PERMTYPE) = UPPER(TRIM(PERMTYPE))
                     )
                     AND
                     (
                         TRIM(PURPOSE) IS NULL
                         OR UPPER(t.PURPOSE) = UPPER(TRIM(PURPOSE))
                     )
                     AND
                     (
                         TRIM(PERMNBR) IS NULL
                         OR UPPER(t.PERMNBR) LIKE
                            '%' || UPPER(TRIM(PERMNBR)) || '%'
                     )
                     AND
                     (
                         TRIM(REGISTRATION) IS NULL
                         OR UPPER(t.REGISTRATION) LIKE
                            '%' || UPPER(TRIM(REGISTRATION)) || '%'
                     )
                     AND
                     (
                         TRIM(REMARK) IS NULL
                         OR UPPER(t.REMARK) LIKE
                            '%' || UPPER(TRIM(REMARK)) || '%'
                     )
                     AND
                     (
                         CRAFT_ID IS NULL
                         OR CRAFT_ID = 0
                         OR t.CRAFT_ID = CRAFT_ID
                     )
                     AND
                     (
                         TRIM(CRAFT_TYPE) IS NULL
                         OR UPPER(t.CRAFT_TYPE) = UPPER(TRIM(CRAFT_TYPE))
                     )
                     AND
                     (
                         TRIM(REAL_CRAFT_TYPE) IS NULL
                         OR UPPER(t.REAL_CRAFT_TYPE) LIKE
                            '%' || UPPER(TRIM(REAL_CRAFT_TYPE)) || '%'
                     )
                     AND
                     (
                         TRIM(FLIGHT_TYPE) IS NULL
                         OR UPPER(t.FLIGHT_TYPE) LIKE
                            '%' || UPPER(TRIM(FLIGHT_TYPE)) || '%'
                     )
                     AND
                     (
                         TRIM(FROM_AIRP) IS NULL
                         OR UPPER(t.FROM_AIRP) LIKE
                            '%' || UPPER(TRIM(FROM_AIRP)) || '%'
                     )
                     AND
                     (
                         TRIM(TO_AIRP) IS NULL
                         OR UPPER(t.TO_AIRP) LIKE
                            '%' || UPPER(TRIM(TO_AIRP)) || '%'
                     )
                     AND
                     (
                         TRIM(VIA) IS NULL
                         OR UPPER(t.VIA) LIKE
                            '%' || UPPER(TRIM(VIA)) || '%'
                     )
                     AND
                     (
                         TRIM(FPL_VIA) IS NULL
                         OR UPPER(t.FPL_VIA) LIKE
                            '%' || UPPER(TRIM(FPL_VIA)) || '%'
                     )
                     AND
                     (
                         TRIM(OPER_ID) IS NULL
                         OR UPPER(t.OPER_ID) LIKE
                            '%' || UPPER(TRIM(OPER_ID)) || '%'
                     )
                     AND
                     (
                         VALIDHOURS IS NULL
                         OR VALIDHOURS = 0
                         OR t.VALIDHOURS = VALIDHOURS
                     )
                     AND
                     (
                         TRIM(FLIGHTNBR) IS NULL
                         OR UPPER(t.FLIGHTNBR) LIKE
                            '%' || UPPER(TRIM(FLIGHTNBR)) || '%'
                     )
                     AND
                     (
                         TRIM(ETD) IS NULL
                         OR UPPER(t.ETD) LIKE
                            '%' || UPPER(TRIM(ETD)) || '%'
                     )
                     AND
                     (
                         TRIM(ETA) IS NULL
                         OR UPPER(t.ETA) LIKE
                            '%' || UPPER(TRIM(ETA)) || '%'
                     )
                     AND
                     (
                         TRIM(ATD) IS NULL
                         OR UPPER(t.ATD) LIKE
                            '%' || UPPER(TRIM(ATD)) || '%'
                     )
                     AND
                     (
                         TRIM(ATA) IS NULL
                         OR UPPER(t.ATA) LIKE
                            '%' || UPPER(TRIM(ATA)) || '%'
                     )
                     AND
                     (
                         NVL(TYPEOPER, 0) = 0
                         OR EXISTS
                         (
                             SELECT 1
                               FROM M_OPER op
                              WHERE op.OPER_ICAO = t.OPER_ID
                                AND
                                (
                                    (TYPEOPER = 1 AND op.IS_DOMESTIC = 1)
                                    OR
                                    (TYPEOPER = 2 AND op.IS_DOMESTIC = 0)
                                )
                         )
                     )
                     AND
                     (
                         NVL(TYPEFLIGHT, 0) = 0
                         OR
                         (
                             TYPEFLIGHT = 1
                             AND t.FROM_AIRP LIKE 'VV%'
                             AND t.TO_AIRP LIKE 'VV%'
                         )
                         OR
                         (
                             TYPEFLIGHT = 2
                             AND
                             (
                                 t.FROM_AIRP NOT LIKE 'VV%'
                                 OR t.TO_AIRP NOT LIKE 'VV%'
                             )
                         )
                     )
                     AND
                     (
                         NVL(FIR, '0') = '0'
                         OR
                         (
                             FIR = '1'
                             AND
                             (
                                 (
                                     t.FROM_AIRP IN
                                     (
                                         'VVNB', 'VVDH', 'VVTX', 'VVCB',
                                         'VVVD', 'VVVH', 'VVDB'
                                     )
                                     AND t.ATD IS NOT NULL
                                 )
                                 OR
                                 (
                                     t.TO_AIRP IN
                                     (
                                         'VVNB', 'VVDH', 'VVTX', 'VVCB',
                                         'VVVD', 'VVVH', 'VVDB'
                                     )
                                     AND t.ATA IS NOT NULL
                                 )
                             )
                         )
                         OR
                         (
                             FIR = '2'
                             AND
                             (
                                 (
                                     t.FROM_AIRP IN
                                     (
                                         'VVTS', 'VVCR', 'VVDN', 'VVPB',
                                         'VVCL', 'VVPC', 'VVRG', 'VVDL',
                                         'VVTH', 'VVCS', 'VVBM', 'VVCT',
                                         'VVCM', 'VVPQ', 'VVPK'
                                     )
                                     AND t.ATD IS NOT NULL
                                 )
                                 OR
                                 (
                                     t.TO_AIRP IN
                                     (
                                         'VVTS', 'VVCR', 'VVDN', 'VVPB',
                                         'VVCL', 'VVPC', 'VVRG', 'VVDL',
                                         'VVTH', 'VVCS', 'VVBM', 'VVCT',
                                         'VVCM', 'VVPQ', 'VVPK'
                                     )
                                     AND t.ATA IS NOT NULL
                                 )
                             )
                         )
                     )
                     AND
                     (
                         (
                             TRIM(SANBAYDI) IS NULL
                             AND TRIM(SANBAYDEN) IS NULL
                         )
                         OR
                         (
                             TRIM(SANBAYDI) IS NOT NULL
                             AND UPPER(t.FROM_AIRP) LIKE
                                 '%' || UPPER(TRIM(SANBAYDI)) || '%'
                         )
                         OR
                         (
                             TRIM(SANBAYDEN) IS NOT NULL
                             AND UPPER(t.TO_AIRP) LIKE
                                 '%' || UPPER(TRIM(SANBAYDEN)) || '%'
                         )
                     )
                     AND
                     (
                         NVL(CAT_HA, 0) = 0
                         OR
                         (
                             CAT_HA = 1
                             AND SUBSTR(TRIM(t.ETD), 1, 4)
                                 BETWEEN KHUNGGIO1 AND KHUNGGIO2
                         )
                         OR
                         (
                             CAT_HA = 2
                             AND SUBSTR(TRIM(t.ETA), 1, 4)
                                 BETWEEN KHUNGGIO1 AND KHUNGGIO2
                         )
                         OR
                         (
                             CAT_HA = 3
                             AND SUBSTR(TRIM(t.ATD), 1, 4)
                                 BETWEEN KHUNGGIO1 AND KHUNGGIO2
                         )
                         OR
                         (
                             CAT_HA = 4
                             AND SUBSTR(TRIM(t.ATA), 1, 4)
                                 BETWEEN KHUNGGIO1 AND KHUNGGIO2
                         )
                     )
;
    END COUNT_GOINGON;

END FINISHED_COMPARE_PKG;
/
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM USER_ERRORS
    WHERE NAME = 'FINISHED_COMPARE_PKG' AND ATTRIBUTE = 'ERROR';
    IF v_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20881, 'FINISHED_COMPARE_PKG compilation failed; xem USER_ERRORS');
    END IF;
END;
/
SELECT TYPE, LINE, POSITION, TEXT FROM USER_ERRORS
WHERE NAME = 'FINISHED_COMPARE_PKG' ORDER BY SEQUENCE;

