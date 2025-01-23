using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainBenchmarkAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedAttemptPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "savedAttempts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_savedAttempts_PlayerId",
                table: "savedAttempts",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_savedAttempts_users_PlayerId",
                table: "savedAttempts",
                column: "PlayerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_savedAttempts_users_PlayerId",
                table: "savedAttempts");

            migrationBuilder.DropIndex(
                name: "IX_savedAttempts_PlayerId",
                table: "savedAttempts");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "savedAttempts");
        }
    }
}
