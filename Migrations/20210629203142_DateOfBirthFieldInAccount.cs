using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatsAPI.Migrations
{
    public partial class DateOfBirthFieldInAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Accounts",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2003, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Accounts",
                type: "datetime",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Accounts");
        }
    }
}
