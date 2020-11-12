using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebPage.Data.Migrations
{
    public partial class NewTypeContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nội dung",
                table: "Posts",
                type: "Nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nội dung",
                table: "Posts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Nvarchar(max)",
                oldNullable: true);
        }
    }
}
