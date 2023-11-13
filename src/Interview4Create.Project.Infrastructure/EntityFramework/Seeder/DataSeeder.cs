using Interview4Create.Project.Domain.Common.Enums;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Seeder;
public static class DataSeeder
{
    public static IEnumerable<Models.Company> GetDefaultCompaniesData()
    {
        return new List<Models.Company>
        {
            new Models.Company
            {
                Id = new Guid("17dafea2-e371-4d07-8389-e752b5a3d9f4"),
                Name = "TestCompany",
                CreatedTs = new DateTime(2023, 11, 11, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }

    public static IEnumerable<Models.Employee> GetDefaultEmployeesData()
    {
        return new List<Models.Employee>
        {
            new Models.Employee
            {
                Id = new Guid("8a8b2856-ec42-460c-90c9-d51580c806de"),
                Email = "test1@email.com",
                EmployeeTitleId = (byte)EmployeeTitle.Developer,
                CreatedTs = new DateTime(2023, 11, 11, 0, 0, 0, DateTimeKind.Utc)
            },
            new Models.Employee
            {
                Id = new Guid("5b0a28a7-9430-4c54-ad15-a3b708f91303"),
                Email = "test2@email.com",
                EmployeeTitleId = (byte)EmployeeTitle.Manager,
                CreatedTs = new DateTime(2023, 11, 11, 0, 0, 0, DateTimeKind.Utc)
            }
        };
    }

    public static IEnumerable<Models.CompanyEmployee> GetCompanyEmployeesForConcreteCompanyAndEmployees(IEnumerable<Models.Company> companies, IEnumerable<Models.Employee> employees)
    {
        foreach (var company in companies)
        {
            foreach (var employee in employees)
            {
                yield return new Models.CompanyEmployee
                {
                    CompanyId = company.Id,
                    EmployeeId = employee.Id,
                };
            }
        }
    }
}
