using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    public partial class AddedBoughtArticlesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Payments_PaymentId",
                schema: "Identity",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_PaymentId",
                schema: "Identity",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "Identity",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "BoughtArticles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoughtArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoughtArticles_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "Identity",
                        principalTable: "Payments",
                        principalColumn: "PaymentId");
                    table.ForeignKey(
                        name: "FK_BoughtArticles_Prices_PriceId",
                        column: x => x.PriceId,
                        principalSchema: "Identity",
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoughtArticles_PaymentId",
                schema: "Identity",
                table: "BoughtArticles",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BoughtArticles_PriceId",
                schema: "Identity",
                table: "BoughtArticles",
                column: "PriceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoughtArticles",
                schema: "Identity");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                schema: "Identity",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PaymentId",
                schema: "Identity",
                table: "Articles",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Payments_PaymentId",
                schema: "Identity",
                table: "Articles",
                column: "PaymentId",
                principalSchema: "Identity",
                principalTable: "Payments",
                principalColumn: "PaymentId");
        }
    }
}
