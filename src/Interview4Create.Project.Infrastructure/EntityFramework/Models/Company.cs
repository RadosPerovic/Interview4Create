#nullable disable

namespace Interview4Create.Project.Infrastructure.EntityFramework.Models;
public class Company : BaseEntity<Guid>
{
    public string Name { get; set; }

    public ICollection<CompanyEmployee> CompanyEmployees { get; set; }
}
