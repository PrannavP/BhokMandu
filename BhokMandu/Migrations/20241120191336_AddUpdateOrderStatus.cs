using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BhokMandu.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdateOrderStatusRequest",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateOrderStatusRequest");
        }
    }
}
