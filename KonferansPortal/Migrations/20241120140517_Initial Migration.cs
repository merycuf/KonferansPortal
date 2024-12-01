 using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Konferanslar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konferanslar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uyeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EgitmenKonferans",
                columns: table => new
                {
                    EgitilenKonferansId = table.Column<int>(type: "int", nullable: false),
                    EgitmenlerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgitmenKonferans", x => new { x.EgitilenKonferansId, x.EgitmenlerId });
                    table.ForeignKey(
                        name: "FK_EgitmenKonferans_Konferanslar_EgitilenKonferansId",
                        column: x => x.EgitilenKonferansId,
                        principalTable: "Konferanslar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EgitmenKonferans_Uyeler_EgitmenlerId",
                        column: x => x.EgitmenlerId,
                        principalTable: "Uyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KatilimciKonferans",
                columns: table => new
                {
                    KatilimcilarId = table.Column<int>(type: "int", nullable: false),
                    KatilinanKonferansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KatilimciKonferans", x => new { x.KatilimcilarId, x.KatilinanKonferansId });
                    table.ForeignKey(
                        name: "FK_KatilimciKonferans_Konferanslar_KatilinanKonferansId",
                        column: x => x.KatilinanKonferansId,
                        principalTable: "Konferanslar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KatilimciKonferans_Uyeler_KatilimcilarId",
                        column: x => x.KatilimcilarId,
                        principalTable: "Uyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EgitmenKonferans_EgitmenlerId",
                table: "EgitmenKonferans",
                column: "EgitmenlerId");

            migrationBuilder.CreateIndex(
                name: "IX_KatilimciKonferans_KatilinanKonferansId",
                table: "KatilimciKonferans",
                column: "KatilinanKonferansId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EgitmenKonferans");

            migrationBuilder.DropTable(
                name: "KatilimciKonferans");

            migrationBuilder.DropTable(
                name: "Konferanslar");

            migrationBuilder.DropTable(
                name: "Uyeler");
        }
    }
}
