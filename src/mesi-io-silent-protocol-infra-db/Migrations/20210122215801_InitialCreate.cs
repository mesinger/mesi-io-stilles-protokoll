using Microsoft.EntityFrameworkCore.Migrations;

namespace Mesi.Io.SilentProtocol.Infrastructure.Db.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_silent_protocol_entries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Suspect = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Entry = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    TimeStamp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_silent_protocol_entries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_silent_protocol_entries");
        }
    }
}
