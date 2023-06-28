using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acrobatt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterTeamAndGameConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "teams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "map_center",
                table: "maps",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "teams");

            migrationBuilder.DropColumn(
                name: "map_center",
                table: "maps");
        }
    }
}
