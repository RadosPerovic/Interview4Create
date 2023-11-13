using Interview4Create.Project.Infrastructure.EntityFramework.Models;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Extensions;
public static class CompanyEmployeeModelExtensions
{
    public static IEnumerable<CompanyEmployee> ToCompanyEmployeeDatabaseModels(this Domain.Companies.Company company)
    {
        foreach (var employee in company.Employees)
        {
            yield return new CompanyEmployee
            {
                CompanyId = company.Id.Value,
                EmployeeId = employee.Id.Value,
            };
        }
    }
}
