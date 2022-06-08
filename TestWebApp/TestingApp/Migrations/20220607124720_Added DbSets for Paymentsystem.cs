using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    public partial class AddedDbSetsforPaymentsystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                schema: "Identity",
                table: "User",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                schema: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OpenCheckoutDate",
                schema: "Identity",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OpenCheckoutValue",
                schema: "Identity",
                table: "User",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "Identity",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    PersonId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CashAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_User_PersonId1",
                        column: x => x.PersonId1,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                schema: "Identity",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Since = table.Column<DateTime>(type: "datetime2", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
                });

            migrationBuilder.CreateTable(
                name: "TopUps",
                schema: "Identity",
                columns: table => new
                {
                    TopUpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    PersonId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CashAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUps", x => x.TopUpId);
                    table.ForeignKey(
                        name: "FK_TopUps_User_PersonId1",
                        column: x => x.PersonId1,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "Identity",
                        principalTable: "Payments",
                        principalColumn: "PaymentId");
                    table.ForeignKey(
                        name: "FK_Articles_Prices_PriceId",
                        column: x => x.PriceId,
                        principalSchema: "Identity",
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PaymentId",
                schema: "Identity",
                table: "Articles",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PriceId",
                schema: "Identity",
                table: "Articles",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PersonId1",
                schema: "Identity",
                table: "Payments",
                column: "PersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_TopUps_PersonId1",
                schema: "Identity",
                table: "TopUps",
                column: "PersonId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "TopUps",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Prices",
                schema: "Identity");

            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Comment",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OpenCheckoutDate",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OpenCheckoutValue",
                schema: "Identity",
                table: "User");
        }
    }
}
