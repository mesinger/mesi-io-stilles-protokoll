-- migrate:up
CREATE TABLE t_silent_protocol_entries (
                                           "Id" text NOT NULL,
                                           "Suspect" character varying(100) NOT NULL,
                                           "Entry" character varying(2000) NOT NULL,
                                           "TimeStamp" character varying(50) NOT NULL,
                                           CONSTRAINT "PK_t_silent_protocol_entries" PRIMARY KEY ("Id")
);

ALTER TABLE t_silent_protocol_entries RENAME COLUMN "Id" TO id;
ALTER TABLE t_silent_protocol_entries RENAME COLUMN "Suspect" TO suspect;
ALTER TABLE t_silent_protocol_entries RENAME COLUMN "Entry" TO entry;
ALTER TABLE t_silent_protocol_entries RENAME COLUMN "TimeStamp" TO time_stamp;
ALTER TABLE t_silent_protocol_entries ADD created_at_utc timestamp without time zone NOT NULL DEFAULT (now() at time zone 'utc');

INSERT INTO t_silent_protocol_entries VALUES('c247b3cc-8913-401f-814c-8fc15b927af7', 'shibe', 'inu is a dogge', 'utcnow');
INSERT INTO t_silent_protocol_entries VALUES('c247b3cc-8913-401f-815c-8fc15b927af7', 'shibe', 'inu is a doggo', 'utcnow');
INSERT INTO t_silent_protocol_entries VALUES('c247b3cc-8913-401f-816c-8fc15b927af7', 'shibe', 'inu is a dogg', 'utcnow');
INSERT INTO t_silent_protocol_entries VALUES('c247b3cc-8913-401f-817c-8fc15b927af7', 'shibe', 'inu is no dogge', 'utcnow');
INSERT INTO t_silent_protocol_entries VALUES('c247b3cc-8913-401f-818c-8fc15b927af7', 'shibe', 'inu is a friendly dogge', 'utcnow');

-- migrate:down
DROP TABLE t_silent_protocol_entries;
