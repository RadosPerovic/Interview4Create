using Interview4Create.Project.Domain.Common.Enums;

namespace Interview4Create.Project.Domain.Employees.Factories;
public interface IEmployeeFactory
{
    Employee Create(string email, EmployeeTitle title);
}
