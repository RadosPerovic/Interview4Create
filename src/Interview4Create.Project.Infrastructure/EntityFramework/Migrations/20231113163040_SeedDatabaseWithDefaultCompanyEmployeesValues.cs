using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Interview4Create.Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabaseWithDefaultCompanyEmployeesValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "CreatedTs", "Name" },
                values: new object[] { new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"), new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), "TestCompany" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedTs", "Email", "EmployeeTitleId" },
                values: new object[,]
                {
                    { new Guid("5b0a28a7-9430-4c54-ad15-a3b708f91303"), new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), "test2@email.com", (byte)2 },
                    { new Guid("8a8b2856-ec42-460c-90c9-d51580c806de"), new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), "test1@email.com", (byte)1 }
                });

            migrationBuilder.InsertData(
                table: "CompanyEmployees",
                columns: new[] { "CompanyId", "EmployeeId" },
                values: new object[,]
                {
                    { new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"), new Guid("5b0a28a7-9430-4c54-ad15-a3b708f91303") },
                    { new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"), new Guid("8a8b2856-ec42-460c-90c9-d51580c806de") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyEmployees",
                keyColumns: new[] { "CompanyId", "EmployeeId" },
                keyValues: new object[] { new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"), new Guid("5b0a28a7-9430-4c54-ad15-a3b708f91303") });

            migrationBuilder.DeleteData(
                table: "CompanyEmployees",
                keyColumns: new[] { "CompanyId", "EmployeeId" },
                keyValues: new object[] { new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"), new Guid("8a8b2856-ec42-460c-90c9-d51580c806de") });

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("5b0a28a7-9430-4c54-ad15-a3b708f91303"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("8a8b2856-ec42-460c-90c9-d51580c806de"));
        }
    }
}
