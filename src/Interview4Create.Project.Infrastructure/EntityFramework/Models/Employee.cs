namespace Interview4Create.Project.Infrastructure.EntityFramework.Models;
public class Employee : BaseEntity<Guid>
{
    public byte EmployeeTitleId { get; set; }
    public string Email { get; set; }

    public EmployeeTitle EmployeeTitle { get; set; }
    public ICollection<CompanyEmployee> CompanyEmployees { get; set; }
}
