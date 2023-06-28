using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Acrobatt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstName = table.Column<string>(type: "text", nullable: false),
                    lastName = table.Column<string>(type: "text", nullable: false),
                    pseudo = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_id", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "game_configurations",
                columns: table => new
                {
                    game_configuration_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_name = table.Column<string>(type: "text", nullable: false),
                    max_players = table.Column<int>(type: "integer", nullable: false),
                    max_flags = table.Column<int>(type: "integer", nullable: false),
                    duration = table.Column<double>(type: "double precision", nullable: false),
                    gameMode = table.Column<int>(type: "integer", nullable: false),
                    is_private = table.Column<bool>(type: "boolean", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_configuration_id", x => x.game_configuration_id);
                    table.ForeignKey(
                        name: "FK_game_configurations_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    map_id = table.Column<int>(type: "integer", nullable: false),
                    map_name = table.Column<string>(type: "text", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_map_id", x => x.map_id);
                    table.ForeignKey(
                        name: "FK_maps_game_configurations_map_id",
                        column: x => x.map_id,
                        principalTable: "game_configurations",
                        principalColumn: "game_configuration_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    team_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    color = table.Column<string>(type: "text", nullable: false),
                    nb_player = table.Column<int>(type: "integer", nullable: false),
                    GameConfigurationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_id", x => x.team_id);
                    table.ForeignKey(
                        name: "FK_teams_game_configurations_GameConfigurationId",
                        column: x => x.GameConfigurationId,
                        principalTable: "game_configurations",
                        principalColumn: "game_configuration_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "email",
                table: "accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_game_configurations_AccountId",
                table: "game_configurations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "map_name",
                table: "maps",
                column: "map_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teams_GameConfigurationId",
                table: "teams",
                column: "GameConfigurationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maps");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "game_configurations");

            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
