using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryMangmentSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class adforginkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "transactions");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "transactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId",
                table: "transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_AspNetUsers_UserId",
                table: "transactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_AspNetUsers_UserId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_UserId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "transactions");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
