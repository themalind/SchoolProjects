using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fourth_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Restaurants_RestaurantId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_RestaurantId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "RestaurantIdentifier",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantIdentifier",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_RestaurantId",
                table: "OrderItems",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Restaurants_RestaurantId",
                table: "OrderItems",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
