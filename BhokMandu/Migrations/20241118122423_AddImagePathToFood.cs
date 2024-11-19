using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BhokMandu.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Food",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Food");
        }
    }
}
