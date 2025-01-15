using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEgitmenKonferansRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Konferanslar_KonferansId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_EgitmenKonferans_AspNetUsers_EgitmenlerId",
                table: "EgitmenKonferans");

            migrationBuilder.DropForeignKey(
                name: "FK_EgitmenKonferans_Konferanslar_EgitilenKonferansId",
                table: "EgitmenKonferans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans");

            migrationBuilder.DropIndex(
                name: "IX_EgitmenKonferans_EgitmenlerId",
                table: "EgitmenKonferans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_KonferansId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KonferansId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EgitmenlerId",
                table: "EgitmenKonferans",
                newName: "EgitmenId");

            migrationBuilder.RenameColumn(
                name: "EgitilenKonferansId",
                table: "EgitmenKonferans",
                newName: "KonferansId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans",
                columns: new[] { "EgitmenId", "KonferansId" });

            migrationBuilder.CreateTable(
                name: "KonferansUye",
                columns: table => new
                {
                    KatilimcilarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    katilinanKonferanslarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KonferansUye", x => new { x.KatilimcilarId, x.katilinanKonferanslarId });
                    table.ForeignKey(
                        name: "FK_KonferansUye_AspNetUsers_KatilimcilarId",
                        column: x => x.KatilimcilarId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KonferansUye_Konferanslar_katilinanKonferanslarId",
                        column: x => x.katilinanKonferanslarId,
                        principalTable: "Konferanslar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EgitmenKonferans_KonferansId",
                table: "EgitmenKonferans",
                column: "KonferansId");

            migrationBuilder.CreateIndex(
                name: "IX_KonferansUye_katilinanKonferanslarId",
                table: "KonferansUye",
                column: "katilinanKonferanslarId");

            migrationBuilder.AddForeignKey(
                name: "FK_EgitmenKonferans_AspNetUsers_EgitmenId",
                table: "EgitmenKonferans",
                column: "EgitmenId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EgitmenKonferans_Konferanslar_KonferansId",
                table: "EgitmenKonferans",
                column: "KonferansId",
                principalTable: "Konferanslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EgitmenKonferans_AspNetUsers_EgitmenId",
                table: "EgitmenKonferans");

            migrationBuilder.DropForeignKey(
                name: "FK_EgitmenKonferans_Konferanslar_KonferansId",
                table: "EgitmenKonferans");

            migrationBuilder.DropTable(
                name: "KonferansUye");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans");

            migrationBuilder.DropIndex(
                name: "IX_EgitmenKonferans_KonferansId",
                table: "EgitmenKonferans");

            migrationBuilder.RenameColumn(
                name: "KonferansId",
                table: "EgitmenKonferans",
                newName: "EgitilenKonferansId");

            migrationBuilder.RenameColumn(
                name: "EgitmenId",
                table: "EgitmenKonferans",
                newName: "EgitmenlerId");

            migrationBuilder.AddColumn<int>(
                name: "KonferansId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EgitmenKonferans",
                table: "EgitmenKonferans",
                columns: new[] { "EgitilenKonferansId", "EgitmenlerId" });

            migrationBuilder.CreateIndex(
                name: "IX_EgitmenKonferans_EgitmenlerId",
                table: "EgitmenKonferans",
                column: "EgitmenlerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EgitmenKonferans_AspNetUsers_EgitmenlerId",
                table: "EgitmenKonferans",
                column: "EgitmenlerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EgitmenKonferans_Konferanslar_EgitilenKonferansId",
                table: "EgitmenKonferans",
                column: "EgitilenKonferansId",
                principalTable: "Konferanslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
