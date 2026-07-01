using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "study");

            migrationBuilder.CreateTable(
                name: "card_progress",
                schema: "study",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetLanguage = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    NextReviewDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastReviewedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Stability = table.Column<double>(type: "float", nullable: false),
                    Difficulty = table.Column<double>(type: "float", nullable: false),
                    Lapses = table.Column<int>(type: "int", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_progress", x => new { x.UserId, x.CardId });
                });

            migrationBuilder.CreateTable(
                name: "user_statistics",
                schema: "study",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentStreak = table.Column<int>(type: "int", nullable: false),
                    LongestStreak = table.Column<int>(type: "int", nullable: false),
                    LearnedCards = table.Column<int>(type: "int", nullable: false),
                    ReviewedCards = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_statistics", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_progress_UserId_NextReviewDate",
                schema: "study",
                table: "card_progress",
                columns: new[] { "UserId", "NextReviewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_card_progress_UserId_TargetLanguage",
                schema: "study",
                table: "card_progress",
                columns: new[] { "UserId", "TargetLanguage" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_progress",
                schema: "study");

            migrationBuilder.DropTable(
                name: "user_statistics",
                schema: "study");
        }
    }
}
