using Interview4Create.Project.Domain.Companies;
using Interview4Create.Project.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Repositories;
internal class CompanyRepository : ICompanyRepository
{
    private readonly DatabaseContext _dbContext;

    public CompanyRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Add(Company company)
    {
        var dbCompany = company.ToDatabaseModel();

        await _dbContext.AddAsync(dbCompany);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Company>> GetByIds(IEnumerable<CompanyIdentity> ids)
    {
        var idValues = ids.Select(e => e.Value);
        var dbCompanies = await _dbContext.Companies
                .Include(c => c.CompanyEmployees)
                    .ThenInclude(ce => ce.Employee)
                .Where(c => idValues.Contains(c.Id))
                .ToListAsync();

        var companies = dbCompanies
            .Select(c => c.ToDomain())
            .ToList();

        return companies;
    }

    public async Task UpdateRange(IEnumerable<Company> companies)
    {
        var companyIds = companies.Select(e => e.Id.Value);
        var dbCompanies = await _dbContext.Companies
                .Where(c => companyIds.Contains(c.Id))
                .ToListAsync();

        UpdateRange(companies, dbCompanies);

        _dbContext.Companies.UpdateRange(dbCompanies);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsNameUnique(string name)
    {
        return !await _dbContext.Companies.AnyAsync(c => c.Name == name);
    }

    private void UpdateRange(IEnumerable<Company> companies, List<Models.Company> dbCompanies)
    {
        foreach (var dbCompany in dbCompanies)
        {
            var company = companies.Single(c => c.Id.Value == dbCompany.Id);

            dbCompany.Name = company!.Name;
            dbCompany.CreatedTs = company.CreatedTs;

            dbCompany.CompanyEmployees = company.ToCompanyEmployeeDatabaseModels().ToList();
        }
    }
}