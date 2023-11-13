namespace Interview4Create.Project.Infrastructure.EntityFramework.Models;

public class EmployeeTitle
{
    public byte Id { get; set; }
    public string Name { get; set; }

    public ICollection<Employee> Employees { get; set; }
}
