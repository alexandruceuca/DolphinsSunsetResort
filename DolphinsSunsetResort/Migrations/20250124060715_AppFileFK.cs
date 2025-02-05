using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DolphinsSunsetResort.Migrations
{
    /// <inheritdoc />
    public partial class AppFileFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AppFiles",
                table: "News");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AppFiles",
                table: "News",
                column: "ImageId",
                principalTable: "AppFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AppFiles",
                table: "News");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AppFiles",
                table: "News",
                column: "ImageId",
                principalTable: "AppFiles",
                principalColumn: "Id");
        }
    }
}
