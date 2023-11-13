namespace Interview4Create.Project.Infrastructure.EntityFramework.Models;
public class CompanyEmployee
{
    public Guid CompanyId { get; set; }
    public Guid EmployeeId { get; set; }

    public Company Company { get; set; }
    public Employee Employee { get; set; }
}
