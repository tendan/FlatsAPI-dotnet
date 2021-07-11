﻿// <auto-generated />
using System;
using FlatsAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlatsAPI.Migrations
{
    [DbContext(typeof(FlatsDbContext))]
    partial class FlatsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("FlatsAPI.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("FlatId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FlatsAPI.Entities.BlockOfFlats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Floors")
                        .HasColumnType("int");

                    b.Property<float>("Margin")
                        .HasColumnType("float");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("RentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("RentId");

                    b.ToTable("BlockOfFlats");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Flat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Area")
                        .HasColumnType("int");

                    b.Property<int>("BlockOfFlatsId")
                        .HasColumnType("int");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfRooms")
                        .HasColumnType("int");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.Property<float?>("PricePerMeterSquaredWhenRented")
                        .HasColumnType("float");

                    b.Property<float>("PriceWhenBought")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BlockOfFlatsId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Flats");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

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
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("FlatId")
                        .HasColumnType("int");

                    b.Property<int>("OwnerShip")
                        .HasColumnType("int");

                    b.Property<bool>("Paid")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("PayDate")
                        .HasColumnType("datetime");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<float>("PriceWithTax")
                        .HasColumnType("float");

                    b.Property<int>("Property")
                        .HasColumnType("int");

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.Property<int>("RentIssuerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.HasIndex("RentIssuerId");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<int>("PermissionsId")
                        .HasColumnType("int");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.HasKey("PermissionsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Account", b =>
                {
                    b.HasOne("FlatsAPI.Entities.Flat", null)
                        .WithMany("Tenants")
                        .HasForeignKey("FlatId");

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

                    b.HasOne("FlatsAPI.Entities.Rent", "Rent")
                        .WithMany()
                        .HasForeignKey("RentId");

                    b.Navigation("Owner");

                    b.Navigation("Rent");
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
                    b.HasOne("FlatsAPI.Entities.Flat", null)
                        .WithMany("Rents")
                        .HasForeignKey("FlatId");

                    b.HasOne("FlatsAPI.Entities.Account", "RentIssuer")
                        .WithMany("Rents")
                        .HasForeignKey("RentIssuerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
                });

            modelBuilder.Entity("FlatsAPI.Entities.Flat", b =>
                {
                    b.Navigation("Rents");

                    b.Navigation("Tenants");
                });

            modelBuilder.Entity("FlatsAPI.Entities.Role", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
