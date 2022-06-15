using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    public partial class AddNullabilityToPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_User_ExecutorId",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "ExecutorId",
                schema: "Identity",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User_ExecutorId",
                schema: "Identity",
                table: "Payments",
                column: "ExecutorId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_User_ExecutorId",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "ExecutorId",
                schema: "Identity",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User_ExecutorId",
                schema: "Identity",
                table: "Payments",
                column: "ExecutorId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
