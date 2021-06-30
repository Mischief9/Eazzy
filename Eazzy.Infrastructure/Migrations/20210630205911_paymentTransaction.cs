using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eazzy.Infrastructure.Migrations
{
    public partial class paymentTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecondaryId = table.Column<Guid>(nullable: false),
                    ExternalTransactionIdentifier = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    StatusCode = table.Column<string>(nullable: true),
                    StatusDescription = table.Column<string>(nullable: true),
                    CardId = table.Column<int>(nullable: true),
                    RawRequest = table.Column<string>(nullable: true),
                    RawResponse = table.Column<string>(nullable: true),
                    CreateDateOnUtc = table.Column<DateTime>(nullable: false),
                    UpdateDateOnutc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTransactions");
        }
    }
}
