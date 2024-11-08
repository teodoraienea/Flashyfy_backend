using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashify_b.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToFlashCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "flashcard",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_flashcard_UserId",
                table: "flashcard",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_flashcard_user_UserId",
                table: "flashcard",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_flashcard_user_UserId",
                table: "flashcard");

            migrationBuilder.DropIndex(
                name: "IX_flashcard_UserId",
                table: "flashcard");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "flashcard");
        }
    }
}
