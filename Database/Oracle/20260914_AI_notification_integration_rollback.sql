-- Operational rollback preserving events, read history, mappings and cursor.
-- Stop the web application pool first and disable its NL2SQL polling endpoint/key.
-- Then restore the previous web build. Do not execute the old SOURCE_HASH rollback:
-- it would discard imported-event deduplication metadata.
-- Keep the additive schema and safe legacy package wrappers installed.

-- AI rows are now hidden until an operator explicitly reconciles targets again.
-- Resume: deploy fixed build, choose AUDIENCE_MODE, run RECONCILE_TARGETS + COMMIT,
-- restore backend-only API settings. Existing events and read rows are reused.
BEGIN
    UPDATE T_AI_NOTIFICATION_SYNC SET AUDIENCE_MODE = 'MAPPED' WHERE SOURCE_NAME = 'NL2SQL';
    UPDATE T_NOTIFICATION SET TARGET_TYPE = 1 WHERE SOURCE_TYPE = 'AI_QUERY';
    DELETE FROM T_NOTIFICATION_TARGET T
    WHERE EXISTS (SELECT 1 FROM T_NOTIFICATION N WHERE N.ID = T.NOTIFICATION_ID AND N.SOURCE_TYPE = 'AI_QUERY');
    COMMIT;
END;
/
