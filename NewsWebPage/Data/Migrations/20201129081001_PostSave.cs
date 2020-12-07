using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebPage.Data.Migrations
{
    public partial class PostSave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostSaves",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    DateRead = table.Column<DateTime>(nullable: false),
                    ReaderID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostSaves_AspNetUsers_ReaderID",
                        column: x => x.ReaderID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostSaveDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostSaveID = table.Column<int>(nullable: false),
                    PostID = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_PostSaves_ReaderID",
                table: "PostSaves",
                column: "ReaderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostSaveDetails");

            migrationBuilder.DropTable(
                name: "PostSaves");
        }
    }
}
