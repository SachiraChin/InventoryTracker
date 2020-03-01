using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryTracker.DataContext.Migrations
{
    public partial class ReturnedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Returned",
                table: "InventoryItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Returned",
                table: "InventoryItems");
        }
    }
}
