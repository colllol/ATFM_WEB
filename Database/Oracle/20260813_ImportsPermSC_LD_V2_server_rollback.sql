-- ============================================================================
-- ROLLBACK DATABASE CANCEL PERMISSION SC V2
-- Khong xoa menu de tranh mat lien ket/quyen da cau hinh tren may chu.
-- Khong rollback du lieu phep/chuyen bay da duoc nguoi dung xac nhan huy.
-- ============================================================================

SET SERVEROUTPUT ON SIZE UNLIMITED
SET ECHO ON
SET FEEDBACK ON
SET VERIFY OFF
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK

SPOOL 20260813_ImportsPermSC_LD_V2_server_rollback.log

PROMPT 1. BO APPLY_CANCELLATIONS VA INDEX V2; GIU GET_CANCELLATION_MATCHES
@@20260813_PERM_IMP_V2_PKG_apply_cancellation_rollback.sql

PROMPT 2. THU HEP PERM NBR STAGING VE 5 KY TU NEU DU LIEU CHO PHEP
@@20260813_T_PERMSC_IMP_permnbr_8_rollback.sql

PROMPT ROLLBACK OBJECT HOAN TAT. MENU VA DU LIEU NGHIEP VU DUOC GIU NGUYEN.

SPOOL OFF
EXIT SUCCESS

