using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonferansPortal.Migrations
{
    /// <inheritdoc />
    public partial class fileExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paylasim_AspNetUsers_PublisherId",
                table: "Paylasim");

            migrationBuilder.DropForeignKey(
                name: "FK_Tartisma_AspNetUsers_PublisherId",
                table: "Tartisma");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "Tartisma",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "Paylasim",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Paylasim",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ContactMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessage", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Paylasim_AspNetUsers_PublisherId",
                table: "Paylasim",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tartisma_AspNetUsers_PublisherId",
                table: "Tartisma",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paylasim_AspNetUsers_PublisherId",
                table: "Paylasim");

            migrationBuilder.DropForeignKey(
                name: "FK_Tartisma_AspNetUsers_PublisherId",
                table: "Tartisma");

            migrationBuilder.DropTable(
                name: "ContactMessage");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Paylasim");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "Tartisma",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "Paylasim",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Paylasim_AspNetUsers_PublisherId",
                table: "Paylasim",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tartisma_AspNetUsers_PublisherId",
                table: "Tartisma",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
