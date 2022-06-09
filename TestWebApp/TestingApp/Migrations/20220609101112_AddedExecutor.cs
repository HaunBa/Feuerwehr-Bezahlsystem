using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    public partial class AddedExecutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_User_PersonId",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_TopUps_User_PersonId",
                schema: "Identity",
                table: "TopUps");

            migrationBuilder.AddColumn<string>(
                name: "ExecutorId",
                schema: "Identity",
                table: "TopUps",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExecutorId",
                schema: "Identity",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TopUps_ExecutorId",
                schema: "Identity",
                table: "TopUps",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ExecutorId",
                schema: "Identity",
                table: "Payments",
                column: "ExecutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User_ExecutorId",
                schema: "Identity",
                table: "Payments",
                column: "ExecutorId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User_PersonId",
                schema: "Identity",
                table: "Payments",
                column: "PersonId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopUps_User_ExecutorId",
                schema: "Identity",
                table: "TopUps",
                column: "ExecutorId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopUps_User_PersonId",
                schema: "Identity",
                table: "TopUps",
                column: "PersonId",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_User_PersonId",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_TopUps_User_ExecutorId",
                schema: "Identity",
                table: "TopUps");

            migrationBuilder.DropForeignKey(
                name: "FK_TopUps_User_PersonId",
                schema: "Identity",
                table: "TopUps");

            migrationBuilder.DropIndex(
                name: "IX_TopUps_ExecutorId",
                schema: "Identity",
                table: "TopUps");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ExecutorId",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ExecutorId",
                schema: "Identity",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "ExecutorId",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User_PersonId",
                schema: "Identity",
                table: "Payments",
                column: "PersonId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopUps_User_PersonId",
                schema: "Identity",
                table: "TopUps",
                column: "PersonId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
