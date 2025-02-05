using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DolphinsSunsetResort.Migrations
{
	/// <inheritdoc />
	public partial class Rating : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_News_AppFiles_ImageId",
				table: "News");

			migrationBuilder.DropIndex(
				name: "IX_News_ImageId",
				table: "News");

			migrationBuilder.AddColumn<int>(
				name: "Rating",
				table: "Booking",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "RecommandationId",
				table: "Booking",
				type: "int",
				nullable: false,
				defaultValue: 3);

			migrationBuilder.AddColumn<int>(
				name: "NewsId",
				table: "AppFiles",
				type: "int",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "DictionaryRecommendation",
				columns: table => new
				{
					RecommendationId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RecommendationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_DictionaryRecommendation", x => x.RecommendationId);
				});

			migrationBuilder.InsertData(
	  table: "DictionaryRecommendation",
	  columns: new[] { "RecommendationId", "RecommendationName" },
	  values: new object[,]
	  {
			{ 1, "Yes" },
			{ 2, "No" },
			{ 3, "No Comment" }
	  });

			migrationBuilder.CreateIndex(
				name: "IX_Booking_RecommandationId",
				table: "Booking",
				column: "RecommandationId");

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

			migrationBuilder.AddForeignKey(
				name: "FK_Booking_DictionaryRecommandation",
				table: "Booking",
				column: "RecommandationId",
				principalTable: "DictionaryRecommendation",
				principalColumn: "RecommendationId",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_News_AppFiles",
				table: "AppFiles");

			migrationBuilder.DropForeignKey(
				name: "FK_Booking_DictionaryRecommandation",
				table: "Booking");

			migrationBuilder.DropTable(
				name: "DictionaryRecommendation");

			migrationBuilder.DropIndex(
				name: "IX_Booking_RecommandationId",
				table: "Booking");

			migrationBuilder.DropIndex(
				name: "IX_AppFiles_NewsId",
				table: "AppFiles");

			migrationBuilder.DropColumn(
				name: "Rating",
				table: "Booking");

			migrationBuilder.DropColumn(
				name: "RecommandationId",
				table: "Booking");

			migrationBuilder.DropColumn(
				name: "NewsId",
				table: "AppFiles");

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
	}
}
