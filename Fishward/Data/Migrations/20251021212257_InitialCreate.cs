using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fishward.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    DiscordID = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.DiscordID);
                });

            migrationBuilder.CreateTable(
                name: "player_resources",
                columns: table => new
                {
                    DiscordID = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Money = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_resources", x => x.DiscordID);
                    table.ForeignKey(
                        name: "FK_player_resources_players_DiscordID",
                        column: x => x.DiscordID,
                        principalTable: "players",
                        principalColumn: "DiscordID",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_resources");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
