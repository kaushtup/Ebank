﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MigrationHelper.Data;

namespace MigrationHelper.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MigrationHelper.Models.Currency", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            name = "GBP"
                        },
                        new
                        {
                            ID = 2,
                            name = "Euro"
                        },
                        new
                        {
                            ID = 3,
                            name = "USD"
                        });
                });

            modelBuilder.Entity("MigrationHelper.Models.Register", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AccNum")
                        .HasColumnType("bigint");

                    b.Property<string>("AccountType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Registers");
                });

            modelBuilder.Entity("MigrationHelper.Models.UserAccount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AccNum")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(12,5)");

                    b.Property<DateTime>("CreatDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("RegisterId")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("RegisterId");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("MigrationHelper.Models.UserAccount", b =>
                {
                    b.HasOne("MigrationHelper.Models.Currency", "currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MigrationHelper.Models.Register", "register")
                        .WithMany()
                        .HasForeignKey("RegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("currency");

                    b.Navigation("register");
                });
#pragma warning restore 612, 618
        }
    }
}
