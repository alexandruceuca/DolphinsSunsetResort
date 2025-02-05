using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DolphinsSunsetResort.Migrations
{
    /// <inheritdoc />
    public partial class Rating3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecommandationId",
                table: "Booking",
                newName: "RecommendationId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_RecommandationId",
                table: "Booking",
                newName: "IX_Booking_RecommendationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecommendationId",
                table: "Booking",
                newName: "RecommandationId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_RecommendationId",
                table: "Booking",
                newName: "IX_Booking_RecommandationId");
        }
    }
}
