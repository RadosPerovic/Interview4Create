using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Employees;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Extensions;
public static class EmployeeModelExtensions
{
    public static Models.Employee ToDatabaseModel(this Employee employee)
    {
        return new Models.Employee
        {
            Id = employee.Id.Value,
            EmployeeTitleId = (byte)employee.Title,
            Email = employee.Email,
            CreatedTs = employee.CreatedTs,
        };
    }

    public static Employee ToDomain(this Models.Employee dbEmployee)
    {
        var identity = new EmployeeIdentity(dbEmployee.Id);
        var title = (EmployeeTitle)dbEmployee.EmployeeTitleId;

        return new Employee(identity, dbEmployee.Email, title, dbEmployee.CreatedTs);
    }
}
