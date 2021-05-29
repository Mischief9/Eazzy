using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eazzy.Infrastructure.Migrations
{
    public partial class orderCreatedOnAndOrderTotalWithoutTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "OrderTotalWithoutTax",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTotalWithoutTax",
                table: "Orders");
        }
    }
}
