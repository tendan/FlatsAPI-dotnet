using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatsAPI.Migrations
{
    public partial class RemoveFlatIdColumnFromAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Accounts");
        }
    }
}
