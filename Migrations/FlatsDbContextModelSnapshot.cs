﻿// <auto-generated />
using System;
using FlatsAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlatsAPI.Migrations
{
    [DbContext(typeof(FlatsDbContext))]
    partial class FlatsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AccountFlat", b =>
                {
                    b.Property<int>("RentedFlatsId")
                        .HasColumnType("integer");

                    b.Property<int>("TenantsId")
                        .HasColumnType("integer");

                    b.HasKey("RentedFlatsId", "TenantsId");

                    b.HasIndex("TenantsId");

                    b.ToTable("AccountFlat");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BillingAddress")
                        .HasColumnType("text")
                        .HasColumnName("BillingAddress");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DateOfBirth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("LastName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Password");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("PhoneNumber");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("RoleId");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Username");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FlatsAPI.Entities.BlockOfFlats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Floors")
                        .HasColumnType("integer");

                    b.Property<float>("Margin")
                        .HasColumnType("real");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("BlockOfFlats");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Flat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Area")
                        .HasColumnType("integer");

                    b.Property<int>("BlockOfFlatsId")
                        .HasColumnType("integer");

                    b.Property<int>("Floor")
                        .HasColumnType("integer");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfRooms")
                        .HasColumnType("integer");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<float?>("PricePerMeterSquaredWhenRented")
                        .HasColumnType("real");

                    b.Property<float>("PriceWhenBought")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("BlockOfFlatsId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Flats");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Rent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BlockOfFlatsPropertyId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FlatPropertyId")
                        .HasColumnType("integer");

                    b.Property<int>("OwnerShip")
                        .HasColumnType("integer")
                        .HasColumnName("OwnerShip");

                    b.Property<bool>("Paid")
                        .HasColumnType("boolean")
                        .HasColumnName("Paid");

                    b.Property<DateTime>("PayDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("Price")
                        .HasColumnType("real")
                        .HasColumnName("Price");

                    b.Property<float>("PriceWithTax")
                        .HasColumnType("real")
                        .HasColumnName("PriceWithTax");

                    b.Property<int>("PropertyId")
                        .HasColumnType("integer")
                        .HasColumnName("PropertyId");

                    b.Property<int>("PropertyType")
                        .HasColumnType("integer");

                    b.Property<int>("RentIssuerId")
                        .HasColumnType("integer")
                        .HasColumnName("RentIssuerId");

                    b.HasKey("Id");

                    b.HasIndex("BlockOfFlatsPropertyId");

                    b.HasIndex("FlatPropertyId");

                    b.HasIndex("RentIssuerId");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<int>("PermissionsId")
                        .HasColumnType("integer");

                    b.Property<int>("RolesId")
                        .HasColumnType("integer");

                    b.HasKey("PermissionsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("AccountFlat", b =>
                {
                    b.HasOne("FlatsAPI.Entities.Flat", null)
                        .WithMany()
                        .HasForeignKey("RentedFlatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlatsAPI.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("TenantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlatsAPI.Entities.Account", b =>
                {
                    b.HasOne("FlatsAPI.Entities.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("FlatsAPI.Entities.BlockOfFlats", b =>
                {
                    b.HasOne("FlatsAPI.Entities.Account", "Owner")
                        .WithMany("OwnedBlocksOfFlats")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Flat", b =>
                {
                    b.HasOne("FlatsAPI.Entities.BlockOfFlats", "BlockOfFlats")
                        .WithMany("Flats")
                        .HasForeignKey("BlockOfFlatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlatsAPI.Entities.Account", "Owner")
                        .WithMany("OwnedFlats")
                        .HasForeignKey("OwnerId");

                    b.Navigation("BlockOfFlats");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Rent", b =>
                {
                    b.HasOne("FlatsAPI.Entities.BlockOfFlats", "BlockOfFlatsProperty")
                        .WithMany("Rents")
                        .HasForeignKey("BlockOfFlatsPropertyId");

                    b.HasOne("FlatsAPI.Entities.Flat", "FlatProperty")
                        .WithMany("Rents")
                        .HasForeignKey("FlatPropertyId");

                    b.HasOne("FlatsAPI.Entities.Account", "RentIssuer")
                        .WithMany("Rents")
                        .HasForeignKey("RentIssuerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlockOfFlatsProperty");

                    b.Navigation("FlatProperty");

                    b.Navigation("RentIssuer");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("FlatsAPI.Entities.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlatsAPI.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlatsAPI.Entities.Account", b =>
                {
                    b.Navigation("OwnedBlocksOfFlats");

                    b.Navigation("OwnedFlats");

                    b.Navigation("Rents");
                });

            modelBuilder.Entity("FlatsAPI.Entities.BlockOfFlats", b =>
                {
                    b.Navigation("Flats");

                    b.Navigation("Rents");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Flat", b =>
                {
                    b.Navigation("Rents");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Role", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
