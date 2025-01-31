using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class EgitmenIdfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EgitmenKonferans_Egitmenler_UyeModelId",
                table: "EgitmenKonferans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans");

            migrationBuilder.DropIndex(
                name: "IX_EgitmenKonferans_UyeModelId",
                table: "EgitmenKonferans");

            migrationBuilder.RenameColumn(
                name: "UyeModelId",
                table: "EgitmenKonferans",
                newName: "EgitmenId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans",
                columns: new[] { "EgitmenId", "KonferansId" });

            migrationBuilder.CreateIndex(
                name: "IX_EgitmenKonferans_KonferansId",
                table: "EgitmenKonferans",
                column: "KonferansId");

            migrationBuilder.AddForeignKey(
                name: "FK_EgitmenKonferans_Egitmenler_EgitmenId",
                table: "EgitmenKonferans",
                column: "EgitmenId",
                principalTable: "Egitmenler",
                principalColumn: "EgitmenId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EgitmenKonferans_Egitmenler_EgitmenId",
                table: "EgitmenKonferans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans");

            migrationBuilder.DropIndex(
                name: "IX_EgitmenKonferans_KonferansId",
                table: "EgitmenKonferans");

            migrationBuilder.RenameColumn(
                name: "EgitmenId",
                table: "EgitmenKonferans",
                newName: "UyeModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans",
                columns: new[] { "KonferansId", "UyeModelId" });

            migrationBuilder.CreateIndex(
                name: "IX_EgitmenKonferans_UyeModelId",
                table: "EgitmenKonferans",
                column: "UyeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_EgitmenKonferans_Egitmenler_UyeModelId",
                table: "EgitmenKonferans",
                column: "UyeModelId",
                principalTable: "Egitmenler",
                principalColumn: "EgitmenId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
