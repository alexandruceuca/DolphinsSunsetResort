using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Licenta1.Migrations
{
    /// <inheritdoc />
    public partial class RoomNumberAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Room");
        }
    }
}
