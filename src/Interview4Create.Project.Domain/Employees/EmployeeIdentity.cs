using Interview4Create.Project.Domain.Common.Base;

namespace Interview4Create.Project.Domain.Employees;
public class EmployeeIdentity : BaseIdentity<Guid>
{
    public EmployeeIdentity(Guid value) : base(value)
    {

    }
}
