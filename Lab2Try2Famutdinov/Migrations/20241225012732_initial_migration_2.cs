using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab2Try2Famutdinov.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Order_OrderId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_OrderId",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Dish");

            migrationBuilder.CreateTable(
                name: "DishOrder",
                columns: table => new
                {
                    DishesId = table.Column<int>(type: "int", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishOrder", x => new { x.DishesId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_DishOrder_Dish_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishOrder_Order_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishOrder_OrdersId",
                table: "DishOrder",
                column: "OrdersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishOrder");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Dish",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_OrderId",
                table: "Dish",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Order_OrderId",
                table: "Dish",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
