using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryTracker.Migrations
{
    public partial class ReturnedAndBorrowedDatesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BorrowedDate",
                table: "InventoryItems",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReturnedDate",
                table: "InventoryItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowedDate",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "InventoryItems");
        }
    }
}
