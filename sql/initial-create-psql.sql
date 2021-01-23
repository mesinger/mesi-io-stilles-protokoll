CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE t_silent_protocol_entries (
    "Id" text NOT NULL,
    "Suspect" character varying(100) NOT NULL,
    "Entry" character varying(2000) NOT NULL,
    "TimeStamp" character varying(50) NOT NULL,
    CONSTRAINT "PK_t_silent_protocol_entries" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210122215801_InitialCreate', '5.0.2');

COMMIT;

