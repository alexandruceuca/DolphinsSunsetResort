using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DolphinsSunsetResort.Migrations
{
    /// <inheritdoc />
    public partial class MenuItemFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrinceId",
                table: "MenuItems",
                newName: "PriceId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_PrinceId",
                table: "MenuItems",
                newName: "IX_MenuItems_PriceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceId",
                table: "MenuItems",
                newName: "PrinceId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_PriceId",
                table: "MenuItems",
                newName: "IX_MenuItems_PrinceId");
        }
    }
}
