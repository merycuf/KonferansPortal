using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddKatilimcilarToKonferans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KonferansUye_AspNetUsers_KatilimcilarId",
                table: "KonferansUye");

            migrationBuilder.DropForeignKey(
                name: "FK_KonferansUye_Konferanslar_katilinanKonferanslarId",
                table: "KonferansUye");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KonferansUye",
                table: "KonferansUye");

            migrationBuilder.DropIndex(
                name: "IX_KonferansUye_katilinanKonferanslarId",
                table: "KonferansUye");

            migrationBuilder.RenameColumn(
                name: "katilinanKonferanslarId",
                table: "KonferansUye",
                newName: "KonferansId");

            migrationBuilder.RenameColumn(
                name: "KatilimcilarId",
                table: "KonferansUye",
                newName: "UyeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KonferansUye",
                table: "KonferansUye",
                columns: new[] { "KonferansId", "UyeId" });

            migrationBuilder.CreateIndex(
                name: "IX_KonferansUye_UyeId",
                table: "KonferansUye",
                column: "UyeId");

            migrationBuilder.AddForeignKey(
                name: "FK_KonferansUye_AspNetUsers_UyeId",
                table: "KonferansUye",
                column: "UyeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KonferansUye_Konferanslar_KonferansId",
                table: "KonferansUye",
                column: "KonferansId",
                principalTable: "Konferanslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KonferansUye_AspNetUsers_UyeId",
                table: "KonferansUye");

            migrationBuilder.DropForeignKey(
                name: "FK_KonferansUye_Konferanslar_KonferansId",
                table: "KonferansUye");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KonferansUye",
                table: "KonferansUye");

            migrationBuilder.DropIndex(
                name: "IX_KonferansUye_UyeId",
                table: "KonferansUye");

            migrationBuilder.RenameColumn(
                name: "UyeId",
                table: "KonferansUye",
                newName: "KatilimcilarId");

            migrationBuilder.RenameColumn(
                name: "KonferansId",
                table: "KonferansUye",
                newName: "katilinanKonferanslarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KonferansUye",
                table: "KonferansUye",
                columns: new[] { "KatilimcilarId", "katilinanKonferanslarId" });

            migrationBuilder.CreateIndex(
                name: "IX_KonferansUye_katilinanKonferanslarId",
                table: "KonferansUye",
                column: "katilinanKonferanslarId");

            migrationBuilder.AddForeignKey(
                name: "FK_KonferansUye_AspNetUsers_KatilimcilarId",
                table: "KonferansUye",
                column: "KatilimcilarId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KonferansUye_Konferanslar_katilinanKonferanslarId",
                table: "KonferansUye",
                column: "katilinanKonferanslarId",
                principalTable: "Konferanslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
