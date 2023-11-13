using Interview4Create.Project.Domain.Companies;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Extensions;
public static class CompanyModelExtensions
{
    public static Models.Company ToDatabaseModel(this Company company)
    {
        var dbCompany = new Models.Company
        {
            Id = company.Id.Value,
            Name = company.Name,
            CreatedTs = company.CreatedTs,

            CompanyEmployees = company.ToCompanyEmployeeDatabaseModels().ToList(),
        };

        return dbCompany;
    }

    public static Company ToDomain(this Models.Company dbCompany)
    {
        var identity = new CompanyIdentity(dbCompany.Id);

        var company = new Company(identity, dbCompany.Name, dbCompany.CreatedTs);

        if (IsEmployeeIncluded(dbCompany))
        {
            FillCompanyWithEmployees(dbCompany, company);
        }

        return company;
    }

    private static bool IsEmployeeIncluded(Models.Company dbCompany)
    {
        return dbCompany.CompanyEmployees.Any();
    }

    private static void FillCompanyWithEmployees(Models.Company dbCompany, Company company)
    {
        foreach (var companyEmployee in dbCompany.CompanyEmployees)
        {
            if (companyEmployee.Employee is null)
            {
                return;
            }

            var employee = companyEmployee.Employee.ToDomain();

            company.AddEmployee(employee);
        }
    }
}
