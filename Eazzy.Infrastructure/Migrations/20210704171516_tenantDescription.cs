using Microsoft.EntityFrameworkCore.Migrations;

namespace Eazzy.Infrastructure.Migrations
{
    public partial class tenantDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tenants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tenants");
        }
    }
}
