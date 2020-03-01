using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryTracker.DataContext.Migrations
{
    public partial class IsWorkingAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Returned",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "InventoryItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsWorking",
                table: "InventoryItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWorking",
                table: "InventoryItems");

            migrationBuilder.AddColumn<bool>(
                name: "Returned",
                table: "InventoryItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReturnedDate",
                table: "InventoryItems",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
