namespace Interview4Create.Project.Domain.Companies;
public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetByIds(IEnumerable<CompanyIdentity> ids);
    Task UpdateRange(IEnumerable<Company> companies);
    Task Add(Company company);
    Task<bool> IsNameUnique(string name);
}
