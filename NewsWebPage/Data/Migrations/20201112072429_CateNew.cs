using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebPage.Data.Migrations
{
    public partial class CateNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CategoryId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CateId",
                table: "Articles",
                column: "CateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CateId",
                table: "Articles",
                column: "CateId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CateId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CateId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CategoryId",
                table: "Articles",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
