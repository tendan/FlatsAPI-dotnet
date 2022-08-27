using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace FlatsAPI.Migrations;

public partial class Create : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Permissions",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Email = table.Column<string>(type: "text", nullable: false),
                Username = table.Column<string>(type: "text", nullable: false),
                FirstName = table.Column<string>(type: "text", nullable: false),
                LastName = table.Column<string>(type: "text", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Password = table.Column<string>(type: "text", nullable: false),
                BillingAddress = table.Column<string>(type: "text", nullable: true),
                PhoneNumber = table.Column<string>(type: "text", nullable: true),
                RoleId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Accounts_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PermissionRole",
            columns: table => new
            {
                PermissionsId = table.Column<int>(type: "integer", nullable: false),
                RolesId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsId, x.RolesId });
                table.ForeignKey(
                    name: "FK_PermissionRole_Permissions_PermissionsId",
                    column: x => x.PermissionsId,
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PermissionRole_Roles_RolesId",
                    column: x => x.RolesId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BlockOfFlats",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Address = table.Column<string>(type: "text", nullable: false),
                PostalCode = table.Column<string>(type: "text", nullable: false),
                Floors = table.Column<int>(type: "integer", nullable: false),
                Margin = table.Column<float>(type: "real", nullable: false),
                OwnerId = table.Column<int>(type: "integer", nullable: true),
                Price = table.Column<float>(type: "real", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BlockOfFlats", x => x.Id);
                table.ForeignKey(
                    name: "FK_BlockOfFlats_Accounts_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Accounts",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Flats",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Area = table.Column<int>(type: "integer", nullable: false),
                Number = table.Column<int>(type: "integer", nullable: false),
                NumberOfRooms = table.Column<int>(type: "integer", nullable: false),
                Floor = table.Column<int>(type: "integer", nullable: false),
                BlockOfFlatsId = table.Column<int>(type: "integer", nullable: false),
                OwnerId = table.Column<int>(type: "integer", nullable: true),
                PriceWhenBought = table.Column<float>(type: "real", nullable: false),
                PricePerMeterSquaredWhenRented = table.Column<float>(type: "real", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Flats", x => x.Id);
                table.ForeignKey(
                    name: "FK_Flats_Accounts_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Accounts",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Flats_BlockOfFlats_BlockOfFlatsId",
                    column: x => x.BlockOfFlatsId,
                    principalTable: "BlockOfFlats",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AccountFlat",
            columns: table => new
            {
                RentedFlatsId = table.Column<int>(type: "integer", nullable: false),
                TenantsId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountFlat", x => new { x.RentedFlatsId, x.TenantsId });
                table.ForeignKey(
                    name: "FK_AccountFlat_Accounts_TenantsId",
                    column: x => x.TenantsId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AccountFlat_Flats_RentedFlatsId",
                    column: x => x.RentedFlatsId,
                    principalTable: "Flats",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Rents",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                PayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Paid = table.Column<bool>(type: "boolean", nullable: false),
                Price = table.Column<float>(type: "real", nullable: false),
                OwnerShip = table.Column<int>(type: "integer", nullable: false),
                PriceWithTax = table.Column<float>(type: "real", nullable: false),
                RentIssuerId = table.Column<int>(type: "integer", nullable: false),
                PropertyId = table.Column<int>(type: "integer", nullable: false),
                BlockOfFlatsPropertyId = table.Column<int>(type: "integer", nullable: true),
                FlatPropertyId = table.Column<int>(type: "integer", nullable: true),
                PropertyType = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rents", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rents_Accounts_RentIssuerId",
                    column: x => x.RentIssuerId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Rents_BlockOfFlats_BlockOfFlatsPropertyId",
                    column: x => x.BlockOfFlatsPropertyId,
                    principalTable: "BlockOfFlats",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Rents_Flats_FlatPropertyId",
                    column: x => x.FlatPropertyId,
                    principalTable: "Flats",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_AccountFlat_TenantsId",
            table: "AccountFlat",
            column: "TenantsId");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_RoleId",
            table: "Accounts",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_BlockOfFlats_OwnerId",
            table: "BlockOfFlats",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Flats_BlockOfFlatsId",
            table: "Flats",
            column: "BlockOfFlatsId");

        migrationBuilder.CreateIndex(
            name: "IX_Flats_OwnerId",
            table: "Flats",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_PermissionRole_RolesId",
            table: "PermissionRole",
            column: "RolesId");

        migrationBuilder.CreateIndex(
            name: "IX_Rents_BlockOfFlatsPropertyId",
            table: "Rents",
            column: "BlockOfFlatsPropertyId");

        migrationBuilder.CreateIndex(
            name: "IX_Rents_FlatPropertyId",
            table: "Rents",
            column: "FlatPropertyId");

        migrationBuilder.CreateIndex(
            name: "IX_Rents_RentIssuerId",
            table: "Rents",
            column: "RentIssuerId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AccountFlat");

        migrationBuilder.DropTable(
            name: "PermissionRole");

        migrationBuilder.DropTable(
            name: "Rents");

        migrationBuilder.DropTable(
            name: "Permissions");

        migrationBuilder.DropTable(
            name: "Flats");

        migrationBuilder.DropTable(
            name: "BlockOfFlats");

        migrationBuilder.DropTable(
            name: "Accounts");

        migrationBuilder.DropTable(
            name: "Roles");
    }
}
