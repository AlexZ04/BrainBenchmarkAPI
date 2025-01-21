using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainBenchmarkAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adminsList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adminsList", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adminsList");
        }
    }
}
