using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class onKayitandTartismaFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Konferanslar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OnKayit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isChecked = table.Column<bool>(type: "bit", nullable: false),
                    konferansId = table.Column<int>(type: "int", nullable: false),
                    uyeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    dekontFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    isPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnKayit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnKayit_AspNetUsers_uyeId",
                        column: x => x.uyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnKayit_Konferanslar_konferansId",
                        column: x => x.konferansId,
                        principalTable: "Konferanslar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnKayit_konferansId",
                table: "OnKayit",
                column: "konferansId");

            migrationBuilder.CreateIndex(
                name: "IX_OnKayit_uyeId",
                table: "OnKayit",
                column: "uyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnKayit");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Konferanslar");
        }
    }
}
