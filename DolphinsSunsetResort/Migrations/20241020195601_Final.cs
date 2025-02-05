using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DolphinsSunsetResort.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingRooms",
                table: "BookingRooms");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameTable(
                name: "BookingRooms",
                newName: "BookingRoom");

            migrationBuilder.AlterColumn<bool>(
                name: "DiscountIsActive",
                table: "Prices",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingRoom",
                table: "BookingRoom",
                column: "BookingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                table: "Booking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_BookingId",
                table: "BookingRoom",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_RoomId",
                table: "BookingRoom",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AplicationUser",
                table: "Booking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRoom_Booking",
                table: "BookingRoom",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRoom_Room",
                table: "BookingRoom",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AplicationUser",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRoom_Booking",
                table: "BookingRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRoom_Room",
                table: "BookingRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingRoom",
                table: "BookingRoom");

            migrationBuilder.DropIndex(
                name: "IX_BookingRoom_BookingId",
                table: "BookingRoom");

            migrationBuilder.DropIndex(
                name: "IX_BookingRoom_RoomId",
                table: "BookingRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_UserId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "BookingRoom",
                newName: "BookingRooms");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.AlterColumn<bool>(
                name: "DiscountIsActive",
                table: "Prices",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingRooms",
                table: "BookingRooms",
                column: "BookingRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");
        }
    }
}
