using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebPage.Data.Migrations
{
    public partial class NewPostSave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostSaveDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PostSaves");

            migrationBuilder.AddColumn<int>(
                name: "PostID",
                table: "PostSaves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostSaves_PostID",
                table: "PostSaves",
                column: "PostID");

            migrationBuilder.AddForeignKey(
                name: "FK_PostSaves_Posts_PostID",
                table: "PostSaves",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostSaves_Posts_PostID",
                table: "PostSaves");

            migrationBuilder.DropIndex(
                name: "IX_PostSaves_PostID",
                table: "PostSaves");

            migrationBuilder.DropColumn(
                name: "PostID",
                table: "PostSaves");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PostSaves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostSaveDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostID = table.Column<int>(type: "int", nullable: false),
                    PostSaveID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSaveDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostSaveDetails_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostSaveDetails_PostSaves_PostSaveID",
                        column: x => x.PostSaveID,
                        principalTable: "PostSaves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostSaveDetails_PostID",
                table: "PostSaveDetails",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_PostSaveDetails_PostSaveID",
                table: "PostSaveDetails",
                column: "PostSaveID");
        }
    }
}
