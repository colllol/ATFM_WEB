/*
  PERM_PKG.PermDetail_SC_GetBySerach - optimized implementation
  Date: 2026-07-31

  IMPORTANT
  =========
  This is a package procedure. Keep the declaration in PACKAGE SPEC and
  replace the existing implementation in PACKAGE BODY with the implementation
  below. Do not add it as a second overload.

  The public signature intentionally remains identical to the legacy version.
  The API oDataProvider reads ALL_ARGUMENTS and expects:
    - P_PERM_ID, P_CRAFT_ID, P_MTOW as VARCHAR2.
    - P_PAGESIZE and P_PAGEINDEX as NUMBER (declared as INT).

  Do not change P_PAGESIZE/P_PAGEINDEX to PLS_INTEGER or BINARY_INTEGER.
*/

/* ================================================================
   1. Declaration kept in PACKAGE SPEC PERM_PKG
   ================================================================ */
PROCEDURE PermDetail_SC_GetBySerach
(
    p_PERM_ID      IN VARCHAR2,
    p_PURPOSE_ID   IN VARCHAR2,
    p_CRAFT_ID     IN VARCHAR2,
    p_MTOW         IN VARCHAR2,
    p_FLIGHTNBR    IN VARCHAR2,
    p_REGISTRATION IN VARCHAR2,
    p_DAY1         IN VARCHAR2,
    p_DAY2         IN VARCHAR2,
    p_DAY3         IN VARCHAR2,
    p_DAY4         IN VARCHAR2,
    p_DAY5         IN VARCHAR2,
    p_DAY6         IN VARCHAR2,
    p_DAY7         IN VARCHAR2,
    p_FROM_AIRP    IN VARCHAR2,
    p_TO_AIRP      IN VARCHAR2,
    p_ETD          IN VARCHAR2,
    p_ETA          IN VARCHAR2,
    p_VIA          IN VARCHAR2,
    p_STATUS       IN VARCHAR2,
    p_LASTUSER     IN VARCHAR2,
    p_BEGINDATE    IN DATE,
    p_ENDDATE      IN DATE,
    p_REMARK       IN VARCHAR2,
    p_PageSize     IN INT,
    p_PageIndex    IN INT,
    p_rStart       IN NUMBER,
    p_rFinish      IN NUMBER,
    P_RETURN_CODE  OUT T_CURSOR
);

/* ================================================================
   2. Implementation replacing the old block in PACKAGE BODY
   ================================================================ */
PROCEDURE PermDetail_SC_GetBySerach
(
    p_PERM_ID      IN VARCHAR2,
    p_PURPOSE_ID   IN VARCHAR2,
    p_CRAFT_ID     IN VARCHAR2,
    p_MTOW         IN VARCHAR2,
    p_FLIGHTNBR    IN VARCHAR2,
    p_REGISTRATION IN VARCHAR2,
    p_DAY1         IN VARCHAR2,
    p_DAY2         IN VARCHAR2,
    p_DAY3         IN VARCHAR2,
    p_DAY4         IN VARCHAR2,
    p_DAY5         IN VARCHAR2,
    p_DAY6         IN VARCHAR2,
    p_DAY7         IN VARCHAR2,
    p_FROM_AIRP    IN VARCHAR2,
    p_TO_AIRP      IN VARCHAR2,
    p_ETD          IN VARCHAR2,
    p_ETA          IN VARCHAR2,
    p_VIA          IN VARCHAR2,
    p_STATUS       IN VARCHAR2,
    p_LASTUSER     IN VARCHAR2,
    p_BEGINDATE    IN DATE,
    p_ENDDATE      IN DATE,
    p_REMARK       IN VARCHAR2,
    p_PageSize     IN INT,
    p_PageIndex    IN INT,
    p_rStart       IN NUMBER,
    p_rFinish      IN NUMBER,
    P_RETURN_CODE  OUT T_CURSOR
)
IS
    v_perm_id       NUMBER;
    v_craft_id      NUMBER;
    v_mtow          NUMBER;
    v_first_index   NUMBER;
    v_last_index    NUMBER;
    v_page_size     NUMBER;
    v_page_index    NUMBER;
    v_purpose_id    VARCHAR2(4000);
    v_flightnbr     VARCHAR2(4000);
    v_registration VARCHAR2(4000);
    v_from_airp     VARCHAR2(4000);
    v_to_airp       VARCHAR2(4000);
    v_etd           VARCHAR2(4000);
    v_eta           VARCHAR2(4000);
    v_via           VARCHAR2(4000);
    v_remark        VARCHAR2(4000);
BEGIN
    /* PERM_ID is mandatory and drives IX_PERMDETAIL_SC_PERM_ID_ID. */
    IF TRIM(p_PERM_ID) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'P_PERM_ID is required');
    END IF;

    BEGIN
        v_perm_id := TO_NUMBER(TRIM(p_PERM_ID));
    EXCEPTION
        WHEN VALUE_ERROR THEN
            RAISE_APPLICATION_ERROR(-20002, 'P_PERM_ID must be numeric');
    END;

    /* CRAFT_ID=0 and MTOW=0 retain the legacy meaning: no filter. */
    IF TRIM(p_CRAFT_ID) IS NOT NULL
       AND TRIM(p_CRAFT_ID) <> '0'
    THEN
        BEGIN
            v_craft_id := TO_NUMBER(TRIM(p_CRAFT_ID));
        EXCEPTION
            WHEN VALUE_ERROR THEN
                RAISE_APPLICATION_ERROR(-20003, 'P_CRAFT_ID must be numeric');
        END;
    END IF;

    IF TRIM(p_MTOW) IS NOT NULL
       AND TRIM(p_MTOW) <> '0'
    THEN
        BEGIN
            v_mtow := TO_NUMBER(
                REPLACE(TRIM(p_MTOW), ',', '.'),
                '99999999999999999999999999999999999999D99999999999999999999',
                'NLS_NUMERIC_CHARACTERS=''.,'''
            );
        EXCEPTION
            WHEN VALUE_ERROR THEN
                RAISE_APPLICATION_ERROR(-20004, 'P_MTOW must be numeric');
        END;
    END IF;

    /* Normalize text filters once instead of once per returned row. */
    v_purpose_id    := NULLIF(UPPER(TRIM(p_PURPOSE_ID)), '0');
    v_flightnbr     := NULLIF(UPPER(TRIM(p_FLIGHTNBR)), '0');
    v_registration := NULLIF(UPPER(TRIM(p_REGISTRATION)), '0');
    v_from_airp     := NULLIF(UPPER(TRIM(p_FROM_AIRP)), '0');
    v_to_airp       := NULLIF(UPPER(TRIM(p_TO_AIRP)), '0');
    v_etd           := NULLIF(UPPER(TRIM(p_ETD)), '0');
    v_eta           := NULLIF(UPPER(TRIM(p_ETA)), '0');
    v_via           := NULLIF(UPPER(TRIM(p_VIA)), '0');
    v_remark        := NULLIF(UPPER(TRIM(p_REMARK)), '0');

    /*
      The current web page sends RSTART=0 and RFINISH=page size.
      Preserve that contract. PageSize/PageIndex are used only as fallback.
    */
    IF NVL(p_rFinish, 0) > 0 THEN
        v_first_index := GREATEST(NVL(p_rStart, 0), 1);
        v_last_index  := GREATEST(p_rFinish, v_first_index);
    ELSE
        v_page_size   := GREATEST(NVL(p_PageSize, 100), 1);
        v_page_index  := GREATEST(NVL(p_PageIndex, 0), 0);
        v_first_index := (v_page_size * v_page_index) + 1;
        v_last_index  := v_page_size * (v_page_index + 1);
    END IF;

    OPEN P_RETURN_CODE FOR
        WITH
        purpose_lookup AS
        (
            SELECT
                PURPOSE_CODE,
                MAX(PURPOSE_NAME) AS PURPOSE_NAME
            FROM M_FLY_PURPOSE
            GROUP BY PURPOSE_CODE
        ),
        craft_lookup AS
        (
            SELECT
                CRAFT_ID,
                MAX(MA) AS MA,
                MAX(TAITRONG) AS TAITRONG
            FROM M_CRAFT_TYPE
            GROUP BY CRAFT_ID
        ),
        aero_lookup AS
        (
            /* M_AERO currently contains duplicate AE_CODE values. */
            SELECT AE_CODE
            FROM M_AERO
            GROUP BY AE_CODE
        ),
        perm_lookup AS
        (
            SELECT
                PERM_ID,
                MAX(PERMNBR_ID) AS PERMNBR_ID
            FROM T_PERMMASTER_SC
            GROUP BY PERM_ID
        ),
        numbered AS
        (
            SELECT
                d.ID,
                d.FLIGHT_PK,
                d.PERM_ID,
                d.PURPOSE_ID,
                d.CRAFT_ID,
                d.MTOW,
                d.FLIGHTNBR,
                d.REGISTRATION,
                d.DAY1,
                d.DAY2,
                d.DAY3,
                d.DAY4,
                d.DAY5,
                d.DAY6,
                d.DAY7,
                d.FROM_AIRP,
                d.TO_AIRP,
                d.ETA,
                d.ETD,
                d.VIA,
                d.LASTMODIFY,
                d.LASTUSER,
                d.BEGINDATE,
                d.ENDDATE,
                d.REMARK,
                d.REMARK_SEND,
                fp.PURPOSE_NAME AS PURPOSE_NAME1,
                ct.MA AS CRAFT_NAME,
                ct.TAITRONG AS TAITRONG,
                af.AE_CODE AS FROM_NAME,
                atp.AE_CODE AS TO_NAME,
                pm.PERMNBR_ID,
                0 AS PURPOSE_NAME,
                COUNT(*) OVER () AS RECORD_SUM,
                ROW_NUMBER() OVER (ORDER BY d.ID ASC) AS RNUM,
                CASE
                    WHEN d.ENDDATE > SYSDATE THEN 0
                    ELSE 1
                END AS HETHAN,
                d.STATUS
            FROM T_PERMDETAIL_SC d
            LEFT JOIN purpose_lookup fp
              ON fp.PURPOSE_CODE = d.PURPOSE_ID
            LEFT JOIN craft_lookup ct
              ON ct.CRAFT_ID = d.CRAFT_ID
            LEFT JOIN aero_lookup af
              ON af.AE_CODE = d.FROM_AIRP
            LEFT JOIN aero_lookup atp
              ON atp.AE_CODE = d.TO_AIRP
            LEFT JOIN perm_lookup pm
              ON pm.PERM_ID = d.PERM_ID
            WHERE d.PERM_ID = v_perm_id
              AND (
                    v_purpose_id IS NULL
                    OR INSTR(UPPER(d.PURPOSE_ID), v_purpose_id) > 0
                  )
              AND (
                    v_craft_id IS NULL
                    OR d.CRAFT_ID = v_craft_id
                  )
              AND (
                    v_mtow IS NULL
                    OR d.MTOW = v_mtow
                  )
              AND (
                    v_flightnbr IS NULL
                    OR INSTR(UPPER(d.FLIGHTNBR), v_flightnbr) > 0
                  )
              AND (
                    v_registration IS NULL
                    OR INSTR(UPPER(d.REGISTRATION), v_registration) > 0
                  )
              AND (
                    v_from_airp IS NULL
                    OR INSTR(UPPER(d.FROM_AIRP), v_from_airp) > 0
                  )
              AND (
                    v_to_airp IS NULL
                    OR INSTR(UPPER(d.TO_AIRP), v_to_airp) > 0
                  )
              AND (
                    v_etd IS NULL
                    OR INSTR(UPPER(d.ETD), v_etd) > 0
                  )
              AND (
                    v_eta IS NULL
                    OR INSTR(UPPER(d.ETA), v_eta) > 0
                  )
              AND (
                    v_via IS NULL
                    OR INSTR(UPPER(d.VIA), v_via) > 0
                  )
              AND (
                    v_remark IS NULL
                    OR INSTR(UPPER(d.REMARK), v_remark) > 0
                  )
              AND (
                    p_BEGINDATE IS NULL
                    OR d.BEGINDATE = p_BEGINDATE
                  )
              AND (
                    p_ENDDATE IS NULL
                    OR d.ENDDATE = p_ENDDATE
                  )
        )
        SELECT
            ID,
            FLIGHT_PK,
            PERM_ID,
            PURPOSE_ID,
            CRAFT_ID,
            MTOW,
            FLIGHTNBR,
            REGISTRATION,
            DAY1,
            DAY2,
            DAY3,
            DAY4,
            DAY5,
            DAY6,
            DAY7,
            FROM_AIRP,
            TO_AIRP,
            ETA,
            ETD,
            VIA,
            LASTMODIFY,
            LASTUSER,
            BEGINDATE,
            ENDDATE,
            REMARK,
            REMARK_SEND,
            PURPOSE_NAME1,
            CRAFT_NAME,
            TAITRONG,
            FROM_NAME,
            TO_NAME,
            PERMNBR_ID,
            PURPOSE_NAME,
            RECORD_SUM,
            RNUM,
            HETHAN,
            STATUS
        FROM numbered
        WHERE RNUM BETWEEN v_first_index AND v_last_index
        ORDER BY RNUM;
END PermDetail_SC_GetBySerach;

/* ================================================================
   3. Verification after replacing and compiling PERM_PKG
   ================================================================ */
-- ALTER PACKAGE ATFM.PERM_PKG COMPILE SPECIFICATION;
-- ALTER PACKAGE ATFM.PERM_PKG COMPILE BODY;

-- SELECT type, line, position, text
-- FROM user_errors
-- WHERE name = 'PERM_PKG'
-- ORDER BY sequence;

-- SELECT position, argument_name, in_out, data_type
-- FROM user_arguments
-- WHERE package_name = 'PERM_PKG'
--   AND object_name = 'PERMDETAIL_SC_GETBYSERACH'
-- ORDER BY sequence;

/* Expected metadata relevant to the legacy API:
   P_PERM_ID/P_CRAFT_ID/P_MTOW = VARCHAR2
   P_PAGESIZE/P_PAGEINDEX/P_RSTART/P_RFINISH = NUMBER
*/
