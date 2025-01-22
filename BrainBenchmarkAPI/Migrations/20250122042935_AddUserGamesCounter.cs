using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainBenchmarkAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGamesCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GamesPlayed",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamesPlayed",
                table: "users");
        }
    }
}
