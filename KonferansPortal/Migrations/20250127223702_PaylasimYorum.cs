using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class PaylasimYorum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Konferanslar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Duyurular",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Paylasim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublisherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KonferansId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paylasim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paylasim_AspNetUsers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Paylasim_Konferanslar_KonferansId",
                        column: x => x.KonferansId,
                        principalTable: "Konferanslar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Yorum",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublisherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CevaplananId = table.Column<int>(type: "int", nullable: true),
                    PaylasimId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yorum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yorum_AspNetUsers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Yorum_Paylasim_PaylasimId",
                        column: x => x.PaylasimId,
                        principalTable: "Paylasim",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Yorum_Yorum_CevaplananId",
                        column: x => x.CevaplananId,
                        principalTable: "Yorum",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paylasim_KonferansId",
                table: "Paylasim",
                column: "KonferansId");

            migrationBuilder.CreateIndex(
                name: "IX_Paylasim_PublisherId",
                table: "Paylasim",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorum_CevaplananId",
                table: "Yorum",
                column: "CevaplananId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorum_PaylasimId",
                table: "Yorum",
                column: "PaylasimId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorum_PublisherId",
                table: "Yorum",
                column: "PublisherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Yorum");

            migrationBuilder.DropTable(
                name: "Paylasim");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Konferanslar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Duyurular",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
