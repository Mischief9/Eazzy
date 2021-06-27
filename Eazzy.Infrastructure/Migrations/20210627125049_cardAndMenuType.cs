using Microsoft.EntityFrameworkCore.Migrations;

namespace Eazzy.Infrastructure.Migrations
{
    public partial class cardAndMenuType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cards_CustomerId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "MenuItemType",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemTypeId",
                table: "MenuItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MenuItemType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuItemTypeId",
                table: "MenuItems",
                column: "MenuItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CustomerId",
                table: "Cards",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuItemType_MenuItemTypeId",
                table: "MenuItems",
                column: "MenuItemTypeId",
                principalTable: "MenuItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuItemType_MenuItemTypeId",
                table: "MenuItems");

            migrationBuilder.DropTable(
                name: "MenuItemType");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_MenuItemTypeId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CustomerId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "MenuItemTypeId",
                table: "MenuItems");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemType",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CustomerId",
                table: "Cards",
                column: "CustomerId",
                unique: true);
        }
    }
}
