using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class onKayitkatilimcifix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnKayit_AspNetUsers_uyeId",
                table: "OnKayit");

            migrationBuilder.DropForeignKey(
                name: "FK_OnKayit_Konferanslar_konferansId",
                table: "OnKayit");

            migrationBuilder.RenameColumn(
                name: "uyeId",
                table: "OnKayit",
                newName: "UyeId");

            migrationBuilder.RenameColumn(
                name: "konferansId",
                table: "OnKayit",
                newName: "KonferansId");

            migrationBuilder.RenameIndex(
                name: "IX_OnKayit_uyeId",
                table: "OnKayit",
                newName: "IX_OnKayit_UyeId");

            migrationBuilder.RenameIndex(
                name: "IX_OnKayit_konferansId",
                table: "OnKayit",
                newName: "IX_OnKayit_KonferansId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnKayit_AspNetUsers_UyeId",
                table: "OnKayit",
                column: "UyeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnKayit_Konferanslar_KonferansId",
                table: "OnKayit",
                column: "KonferansId",
                principalTable: "Konferanslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnKayit_AspNetUsers_UyeId",
                table: "OnKayit");

            migrationBuilder.DropForeignKey(
                name: "FK_OnKayit_Konferanslar_KonferansId",
                table: "OnKayit");

            migrationBuilder.RenameColumn(
                name: "UyeId",
                table: "OnKayit",
                newName: "uyeId");

            migrationBuilder.RenameColumn(
                name: "KonferansId",
                table: "OnKayit",
                newName: "konferansId");

            migrationBuilder.RenameIndex(
                name: "IX_OnKayit_UyeId",
                table: "OnKayit",
                newName: "IX_OnKayit_uyeId");

            migrationBuilder.RenameIndex(
                name: "IX_OnKayit_KonferansId",
                table: "OnKayit",
                newName: "IX_OnKayit_konferansId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnKayit_AspNetUsers_uyeId",
                table: "OnKayit",
                column: "uyeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnKayit_Konferanslar_konferansId",
                table: "OnKayit",
                column: "konferansId",
                principalTable: "Konferanslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
