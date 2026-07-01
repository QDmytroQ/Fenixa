using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "content");

            migrationBuilder.CreateTable(
                name: "decks",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TargetLanguage = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_decks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FrontText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    BackText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ContextExample = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    AudioUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cards_decks_DeckId",
                        column: x => x.DeckId,
                        principalSchema: "content",
                        principalTable: "decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "card_tags",
                schema: "content",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_tags", x => new { x.CardId, x.TagId });
                    table.ForeignKey(
                        name: "FK_card_tags_cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "content",
                        principalTable: "cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_card_tags_tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "content",
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_tags_TagId",
                schema: "content",
                table: "card_tags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_cards_DeckId",
                schema: "content",
                table: "cards",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_decks_IsPublic_TargetLanguage",
                schema: "content",
                table: "decks",
                columns: new[] { "IsPublic", "TargetLanguage" });

            migrationBuilder.CreateIndex(
                name: "IX_decks_UserId",
                schema: "content",
                table: "decks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tags_UserId_Name",
                schema: "content",
                table: "tags",
                columns: new[] { "UserId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_tags",
                schema: "content");

            migrationBuilder.DropTable(
                name: "cards",
                schema: "content");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "content");

            migrationBuilder.DropTable(
                name: "decks",
                schema: "content");
        }
    }
}
