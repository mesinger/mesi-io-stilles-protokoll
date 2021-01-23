using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mesi.Io.SilentProtocol.Infrastructure.Db.Migrations
{
    public partial class AddCreatedAtRow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Suspect",
                table: "t_silent_protocol_entries",
                newName: "suspect");

            migrationBuilder.RenameColumn(
                name: "Entry",
                table: "t_silent_protocol_entries",
                newName: "entry");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "t_silent_protocol_entries",
                newName: "time_stamp");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                table: "t_silent_protocol_entries",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at_utc",
                table: "t_silent_protocol_entries");

            migrationBuilder.RenameColumn(
                name: "suspect",
                table: "t_silent_protocol_entries",
                newName: "Suspect");

            migrationBuilder.RenameColumn(
                name: "entry",
                table: "t_silent_protocol_entries",
                newName: "Entry");

            migrationBuilder.RenameColumn(
                name: "time_stamp",
                table: "t_silent_protocol_entries",
                newName: "TimeStamp");
        }
    }
}
