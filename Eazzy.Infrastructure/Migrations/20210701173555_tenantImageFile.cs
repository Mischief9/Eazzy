using Microsoft.EntityFrameworkCore.Migrations;

namespace Eazzy.Infrastructure.Migrations
{
    public partial class tenantImageFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Tenants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_TenantId",
                table: "Menus",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Tenants_TenantId",
                table: "Menus",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Tenants_TenantId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_TenantId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Tenants");
        }
    }
}
