using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DolphinsSunsetResort.Migrations
{
    /// <inheritdoc />
    public partial class AppFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_ImageId",
                table: "News",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AppFiles_ImageId",
                table: "News",
                column: "ImageId",
                principalTable: "AppFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AppFiles_ImageId",
                table: "News");

            migrationBuilder.DropTable(
                name: "AppFiles");

            migrationBuilder.DropIndex(
                name: "IX_News_ImageId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
