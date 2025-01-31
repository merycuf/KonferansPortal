using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class uyeIdInEgitmen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UyeId",
                table: "Egitmenler",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Egitmenler_UyeId",
                table: "Egitmenler",
                column: "UyeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Egitmenler_AspNetUsers_UyeId",
                table: "Egitmenler",
                column: "UyeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Egitmenler_AspNetUsers_UyeId",
                table: "Egitmenler");

            migrationBuilder.DropIndex(
                name: "IX_Egitmenler_UyeId",
                table: "Egitmenler");

            migrationBuilder.DropColumn(
                name: "UyeId",
                table: "Egitmenler");
        }
    }
}
