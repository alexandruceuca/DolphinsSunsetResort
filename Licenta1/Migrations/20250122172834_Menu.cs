using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DolphinsSunsetResort.Migrations
{
    /// <inheritdoc />
    public partial class Menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AppFiles",
                table: "AppFiles");

            migrationBuilder.DropIndex(
                name: "IX_AppFiles_NewsId",
                table: "AppFiles");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "AppFiles");

            migrationBuilder.CreateTable(
                name: "MenuItemCategories",
                columns: table => new
                {
                    MenuItemCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCategories", x => x.MenuItemCategoryId);
                });
            migrationBuilder.InsertData(
table: "MenuItemCategories",
columns: new[] { "MenuItemCategoryId", "MenuItemCategoryName" },
values: new object[,]
{
            { 1, "Breakfast" },
             {2,"Offers" },
            { 3, "Food" },
            { 4, "Beverage" },
            {5, "Dessert " },

});
            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    MenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PrinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.MenuItemId);
                    table.ForeignKey(
                        name: "FK_MenuItemCategory_MenuItem",
                        column: x => x.CategoryId,
                        principalTable: "MenuItemCategories",
                        principalColumn: "MenuItemCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItem_AppFiles",
                        column: x => x.ImageId,
                        principalTable: "AppFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItem_Price",
                        column: x => x.PrinceId,
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_ImageId",
                table: "News",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ImageId",
                table: "MenuItems",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_PrinceId",
                table: "MenuItems",
                column: "PrinceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_News_AppFiles",
                table: "News",
                column: "ImageId",
                principalTable: "AppFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AppFiles",
                table: "News");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "MenuItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_News_ImageId",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "AppFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppFiles_NewsId",
                table: "AppFiles",
                column: "NewsId",
                unique: true,
                filter: "[NewsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AppFiles",
                table: "AppFiles",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id");
        }
    }
}
