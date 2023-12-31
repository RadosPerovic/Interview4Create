﻿// <auto-generated />
using System;
using Interview4Create.Project.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Interview4Create.Project.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTs")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"),
                            CreatedTs = new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "TestCompany"
                        });
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.CompanyEmployee", b =>
                {
                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.HasKey("CompanyId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("CompanyEmployees");

                    b.HasData(
                        new
                        {
                            CompanyId = new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"),
                            EmployeeId = new Guid("8a8b2856-ec42-460c-90c9-d51580c806de")
                        },
                        new
                        {
                            CompanyId = new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"),
                            EmployeeId = new Guid("5b0a28a7-9430-4c54-ad15-a3b708f91303")
                        });
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTs")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<byte>("EmployeeTitleId")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EmployeeTitleId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8a8b2856-ec42-460c-90c9-d51580c806de"),
                            CreatedTs = new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "test1@email.com",
                            EmployeeTitleId = (byte)1
                        },
                        new
                        {
                            Id = new Guid("5b0a28a7-9430-4c54-ad15-a3b708f91303"),
                            CreatedTs = new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "test2@email.com",
                            EmployeeTitleId = (byte)2
                        });
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.EmployeeTitle", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("EmployeeTitles");

                    b.HasData(
                        new
                        {
                            Id = (byte)0,
                            Name = "None"
                        },
                        new
                        {
                            Id = (byte)1,
                            Name = "Developer"
                        },
                        new
                        {
                            Id = (byte)2,
                            Name = "Manager"
                        },
                        new
                        {
                            Id = (byte)3,
                            Name = "Tester"
                        });
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.SystemLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AffectedColumns")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NewValues")
                        .HasColumnType("text");

                    b.Property<string>("OldValues")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryKey")
                        .HasColumnType("text");

                    b.Property<string>("TableName")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SystemLogs");
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.CompanyEmployee", b =>
                {
                    b.HasOne("Interview4Create.Project.Infrastructure.EntityFramework.Models.Company", "Company")
                        .WithMany("CompanyEmployees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Interview4Create.Project.Infrastructure.EntityFramework.Models.Employee", "Employee")
                        .WithMany("CompanyEmployees")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.Employee", b =>
                {
                    b.HasOne("Interview4Create.Project.Infrastructure.EntityFramework.Models.EmployeeTitle", "EmployeeTitle")
                        .WithMany("Employees")
                        .HasForeignKey("EmployeeTitleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("EmployeeTitle");
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.Company", b =>
                {
                    b.Navigation("CompanyEmployees");
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.Employee", b =>
                {
                    b.Navigation("CompanyEmployees");
                });

            modelBuilder.Entity("Interview4Create.Project.Infrastructure.EntityFramework.Models.EmployeeTitle", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
