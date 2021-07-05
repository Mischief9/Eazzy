using Microsoft.EntityFrameworkCore.Migrations;

namespace Eazzy.Infrastructure.Migrations
{
    public partial class orderTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId",
                table: "Orders",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tenants_TenantId",
                table: "Orders",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tenants_TenantId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TenantId",
                table: "Orders");
        }
    }
}
