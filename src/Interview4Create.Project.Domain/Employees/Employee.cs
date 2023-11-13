using Interview4Create.Project.Domain.Common.Base;
using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Common.Guard;

namespace Interview4Create.Project.Domain.Employees;
public class Employee : Entity<EmployeeIdentity>
{
    private string _email;
    private EmployeeTitle _title;

    public Employee(
        EmployeeIdentity id,
        string email,
        EmployeeTitle title,
        DateTime createdTs)
        : base(id, createdTs)
    {
        Email = email;
        Title = title;
    }

    public string Email
    {
        get
        {
            return _email;
        }

        private set
        {
            CommonGuard.NotNull(value);
            _email = value;
        }
    }

    public EmployeeTitle Title
    {
        get
        {
            return _title;
        }
        set
        {
            CommonGuard.NotNone(value, EmployeeTitle.None);
            _title = value;
        }
    }
}
