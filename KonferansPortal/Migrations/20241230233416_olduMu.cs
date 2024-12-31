using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class olduMu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KatilimciKonferans");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Duyurular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Duyurular",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Duyurular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Duyurular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<int>(
                name: "KonferansId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_KonferansId",
                table: "AspNetUsers",
                column: "KonferansId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Konferanslar_KonferansId",
                table: "AspNetUsers",
                column: "KonferansId",
                principalTable: "Konferanslar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Konferanslar_KonferansId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_KonferansId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Duyurular");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Duyurular");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Duyurular");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Duyurular");

            migrationBuilder.DropColumn(
                name: "KonferansId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.CreateTable(
                name: "KatilimciKonferans",
                columns: table => new
                {
                    KatilimcilarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KatilinanKonferansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KatilimciKonferans", x => new { x.KatilimcilarId, x.KatilinanKonferansId });
                    table.ForeignKey(
                        name: "FK_KatilimciKonferans_AspNetUsers_KatilimcilarId",
                        column: x => x.KatilimcilarId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KatilimciKonferans_Konferanslar_KatilinanKonferansId",
                        column: x => x.KatilinanKonferansId,
                        principalTable: "Konferanslar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KatilimciKonferans_KatilinanKonferansId",
                table: "KatilimciKonferans",
                column: "KatilinanKonferansId");
        }
    }
}
