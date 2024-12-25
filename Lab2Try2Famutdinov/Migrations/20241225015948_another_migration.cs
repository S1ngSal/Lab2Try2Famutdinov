using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab2Try2Famutdinov.Migrations
{
    /// <inheritdoc />
    public partial class another_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishOrder_Dish_DishesId",
                table: "DishOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_DishOrder_Order_OrdersId",
                table: "DishOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishOrder",
                table: "DishOrder");

            migrationBuilder.RenameTable(
                name: "DishOrder",
                newName: "OrderDishes");

            migrationBuilder.RenameIndex(
                name: "IX_DishOrder_OrdersId",
                table: "OrderDishes",
                newName: "IX_OrderDishes_OrdersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDishes",
                table: "OrderDishes",
                columns: new[] { "DishesId", "OrdersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDishes_Dish_DishesId",
                table: "OrderDishes",
                column: "DishesId",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDishes_Order_OrdersId",
                table: "OrderDishes",
                column: "OrdersId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDishes_Dish_DishesId",
                table: "OrderDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDishes_Order_OrdersId",
                table: "OrderDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDishes",
                table: "OrderDishes");

            migrationBuilder.RenameTable(
                name: "OrderDishes",
                newName: "DishOrder");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDishes_OrdersId",
                table: "DishOrder",
                newName: "IX_DishOrder_OrdersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishOrder",
                table: "DishOrder",
                columns: new[] { "DishesId", "OrdersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DishOrder_Dish_DishesId",
                table: "DishOrder",
                column: "DishesId",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishOrder_Order_OrdersId",
                table: "DishOrder",
                column: "OrdersId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
