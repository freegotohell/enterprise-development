using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoService.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueOrderMechanicIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_order_mechanics_repair_order_id_mechanic_id",
                table: "order_mechanics",
                columns: new[] { "repair_order_id", "mechanic_id" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_order_mechanics_repair_order_id",
                table: "order_mechanics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_order_mechanics_repair_order_id_mechanic_id",
                table: "order_mechanics");

            migrationBuilder.CreateIndex(
                name: "IX_order_mechanics_repair_order_id",
                table: "order_mechanics",
                column: "repair_order_id");
        }
    }
}
