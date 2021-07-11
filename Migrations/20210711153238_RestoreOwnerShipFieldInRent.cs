using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatsAPI.Migrations
{
    public partial class RestoreOwnerShipFieldInRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerShip",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Accounts_RentIssuerId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "OwnerShip",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BlockOfFlats");

            migrationBuilder.RenameColumn(
                name: "RentIssuerId",
                table: "Rents",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Rents_RentIssuerId",
                table: "Rents",
                newName: "IX_Rents_OwnerId");

            migrationBuilder.CreateTable(
                name: "AccountRent",
                columns: table => new
                {
                    RentsId = table.Column<int>(type: "int", nullable: false),
                    TenantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRent", x => new { x.RentsId, x.TenantsId });
                    table.ForeignKey(
                        name: "FK_AccountRent_Accounts_TenantsId",
                        column: x => x.TenantsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRent_Rents_RentsId",
                        column: x => x.RentsId,
                        principalTable: "Rents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRent_TenantsId",
                table: "AccountRent",
                column: "TenantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Accounts_OwnerId",
                table: "Rents",
                column: "OwnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
