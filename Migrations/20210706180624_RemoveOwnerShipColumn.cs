using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatsAPI.Migrations
{
    public partial class RemoveOwnerShipColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerShip",
                table: "Flats");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerShip",
                table: "Flats",
                type: "int",
                nullable: true);
        }
    }
}
