START TRANSACTION;

ALTER TABLE t_silent_protocol_entries RENAME COLUMN "Suspect" TO suspect;

ALTER TABLE t_silent_protocol_entries RENAME COLUMN "Entry" TO entry;

ALTER TABLE t_silent_protocol_entries RENAME COLUMN "TimeStamp" TO time_stamp;

ALTER TABLE t_silent_protocol_entries ADD created_at_utc timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210122221724_AddCreatedAtRow', '5.0.2');

COMMIT;

