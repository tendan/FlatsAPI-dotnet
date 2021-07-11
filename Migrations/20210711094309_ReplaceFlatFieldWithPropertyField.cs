using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatsAPI.Migrations
{
    public partial class ReplaceFlatFieldWithPropertyField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Flats_FlatId",
                table: "Rents");

            migrationBuilder.AlterColumn<int>(
                name: "FlatId",
                table: "Rents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Property",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentId",
                table: "BlockOfFlats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlockOfFlats_RentId",
                table: "BlockOfFlats",
                column: "RentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockOfFlats_Rents_RentId",
                table: "BlockOfFlats",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Flats_FlatId",
                table: "Rents",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockOfFlats_Rents_RentId",
                table: "BlockOfFlats");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Flats_FlatId",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_BlockOfFlats_RentId",
                table: "BlockOfFlats");

            migrationBuilder.DropColumn(
                name: "Property",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "RentId",
                table: "BlockOfFlats");

            migrationBuilder.AlterColumn<int>(
                name: "FlatId",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Flats_FlatId",
                table: "Rents",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
