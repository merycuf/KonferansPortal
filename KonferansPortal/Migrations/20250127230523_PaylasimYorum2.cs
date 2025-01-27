using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class PaylasimYorum2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paylasim_Konferanslar_KonferansId",
                table: "Paylasim");

            migrationBuilder.DropIndex(
                name: "IX_Paylasim_KonferansId",
                table: "Paylasim");

            migrationBuilder.DropColumn(
                name: "KonferansId",
                table: "Paylasim");

            migrationBuilder.AddColumn<int>(
                name: "TartismaId",
                table: "Yorum",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Paylasim",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "Tartisma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublisherId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tartisma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tartisma_AspNetUsers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tartisma_Konferanslar_Id",
                        column: x => x.Id,
                        principalTable: "Konferanslar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Yorum_TartismaId",
                table: "Yorum",
                column: "TartismaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tartisma_PublisherId",
                table: "Tartisma",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paylasim_Konferanslar_Id",
                table: "Paylasim",
                column: "Id",
                principalTable: "Konferanslar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Yorum_Tartisma_TartismaId",
                table: "Yorum",
                column: "TartismaId",
                principalTable: "Tartisma",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paylasim_Konferanslar_Id",
                table: "Paylasim");

            migrationBuilder.DropForeignKey(
                name: "FK_Yorum_Tartisma_TartismaId",
                table: "Yorum");

            migrationBuilder.DropTable(
                name: "Tartisma");

            migrationBuilder.DropIndex(
                name: "IX_Yorum_TartismaId",
                table: "Yorum");

            migrationBuilder.DropColumn(
                name: "TartismaId",
                table: "Yorum");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Paylasim",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "KonferansId",
                table: "Paylasim",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paylasim_KonferansId",
                table: "Paylasim",
                column: "KonferansId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paylasim_Konferanslar_KonferansId",
                table: "Paylasim",
                column: "KonferansId",
                principalTable: "Konferanslar",
                principalColumn: "Id");
        }
    }
}
