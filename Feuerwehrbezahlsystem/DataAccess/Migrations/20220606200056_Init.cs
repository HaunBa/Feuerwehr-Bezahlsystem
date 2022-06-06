using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "paymentsystem");

            migrationBuilder.CreateTable(
                name: "payment",
                schema: "paymentsystem",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_date = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    payment_text = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    payment_total = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.payment_id);
                });

            migrationBuilder.CreateTable(
                name: "price",
                schema: "paymentsystem",
                columns: table => new
                {
                    price_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price_value = table.Column<float>(type: "real", nullable: false),
                    price_sinceDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    price_untilDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price", x => x.price_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "paymentsystem",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    balance = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    OpenCheckoutDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    OpenCheckoutAmount = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "article",
                schema: "paymentsystem",
                columns: table => new
                {
                    article_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    article_amount = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    price_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article", x => x.article_id);
                    table.ForeignKey(
                        name: "article$fk_Article_price1",
                        column: x => x.price_id,
                        principalSchema: "paymentsystem",
                        principalTable: "price",
                        principalColumn: "price_id");
                });

            migrationBuilder.CreateTable(
                name: "topups",
                schema: "paymentsystem",
                columns: table => new
                {
                    topup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    topup_date = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    topup_amount = table.Column<int>(type: "int", nullable: false),
                    topup_executor_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topups", x => x.topup_id);
                    table.ForeignKey(
                        name: "topups$fk_TopUps_User1",
                        column: x => x.topup_executor_id,
                        principalSchema: "paymentsystem",
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_has_payment",
                schema: "paymentsystem",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Payment_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_has_payment_user_id", x => new { x.user_id, x.Payment_id });
                    table.ForeignKey(
                        name: "user_has_payment$fk_User_has_Payment_Payment1",
                        column: x => x.Payment_id,
                        principalSchema: "paymentsystem",
                        principalTable: "payment",
                        principalColumn: "payment_id");
                    table.ForeignKey(
                        name: "user_has_payment$fk_User_has_Payment_User1",
                        column: x => x.user_id,
                        principalSchema: "paymentsystem",
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "payment_has_article",
                schema: "paymentsystem",
                columns: table => new
                {
                    Payment_payment_id = table.Column<int>(type: "int", nullable: false),
                    Article_article_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_has_article_Payment_payment_id", x => new { x.Payment_payment_id, x.Article_article_id });
                    table.ForeignKey(
                        name: "payment_has_article$fk_Payment_has_Article_Article1",
                        column: x => x.Article_article_id,
                        principalSchema: "paymentsystem",
                        principalTable: "article",
                        principalColumn: "article_id");
                    table.ForeignKey(
                        name: "payment_has_article$fk_Payment_has_Article_Payment1",
                        column: x => x.Payment_payment_id,
                        principalSchema: "paymentsystem",
                        principalTable: "payment",
                        principalColumn: "payment_id");
                });

            migrationBuilder.CreateTable(
                name: "user_has_topups",
                schema: "paymentsystem",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    topup_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_has_topups_user_id", x => new { x.user_id, x.topup_id });
                    table.ForeignKey(
                        name: "user_has_topups$fk_User_has_TopUps_TopUps1",
                        column: x => x.topup_id,
                        principalSchema: "paymentsystem",
                        principalTable: "topups",
                        principalColumn: "topup_id");
                    table.ForeignKey(
                        name: "user_has_topups$fk_User_has_TopUps_User1",
                        column: x => x.user_id,
                        principalSchema: "paymentsystem",
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "fk_Article_price1_idx",
                schema: "paymentsystem",
                table: "article",
                column: "price_id");

            migrationBuilder.CreateIndex(
                name: "fk_Payment_has_Article_Article1_idx",
                schema: "paymentsystem",
                table: "payment_has_article",
                column: "Article_article_id");

            migrationBuilder.CreateIndex(
                name: "fk_Payment_has_Article_Payment1_idx",
                schema: "paymentsystem",
                table: "payment_has_article",
                column: "Payment_payment_id");

            migrationBuilder.CreateIndex(
                name: "fk_TopUps_User1_idx",
                schema: "paymentsystem",
                table: "topups",
                column: "topup_executor_id");

            migrationBuilder.CreateIndex(
                name: "fk_User_has_Payment_Payment1_idx",
                schema: "paymentsystem",
                table: "user_has_payment",
                column: "Payment_id");

            migrationBuilder.CreateIndex(
                name: "fk_User_has_Payment_User1_idx",
                schema: "paymentsystem",
                table: "user_has_payment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "fk_User_has_TopUps_TopUps1_idx",
                schema: "paymentsystem",
                table: "user_has_topups",
                column: "topup_id");

            migrationBuilder.CreateIndex(
                name: "fk_User_has_TopUps_User1_idx",
                schema: "paymentsystem",
                table: "user_has_topups",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment_has_article",
                schema: "paymentsystem");

            migrationBuilder.DropTable(
                name: "user_has_payment",
                schema: "paymentsystem");

            migrationBuilder.DropTable(
                name: "user_has_topups",
                schema: "paymentsystem");

            migrationBuilder.DropTable(
                name: "article",
                schema: "paymentsystem");

            migrationBuilder.DropTable(
                name: "payment",
                schema: "paymentsystem");

            migrationBuilder.DropTable(
                name: "topups",
                schema: "paymentsystem");

            migrationBuilder.DropTable(
                name: "price",
                schema: "paymentsystem");

            migrationBuilder.DropTable(
                name: "user",
                schema: "paymentsystem");
        }
    }
}
