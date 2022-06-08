using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    public partial class AddedImageforArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                schema: "Identity",
                table: "Articles",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                schema: "Identity",
                table: "Articles");
        }
    }
}
