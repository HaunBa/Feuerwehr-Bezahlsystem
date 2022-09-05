using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    public partial class ChangedSmth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "Identity",
                table: "BoughtArticles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInVending",
                schema: "Identity",
                table: "BoughtArticles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VendingMachineNumber",
                schema: "Identity",
                table: "BoughtArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VendingSlot",
                schema: "Identity",
                table: "BoughtArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                schema: "Identity",
                table: "BoughtArticles");

            migrationBuilder.DropColumn(
                name: "IsInVending",
                schema: "Identity",
                table: "BoughtArticles");

            migrationBuilder.DropColumn(
                name: "VendingMachineNumber",
                schema: "Identity",
                table: "BoughtArticles");

            migrationBuilder.DropColumn(
                name: "VendingSlot",
                schema: "Identity",
                table: "BoughtArticles");
        }
    }
}
