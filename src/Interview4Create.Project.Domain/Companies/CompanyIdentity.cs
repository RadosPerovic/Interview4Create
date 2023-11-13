using Interview4Create.Project.Domain.Common.Base;

namespace Interview4Create.Project.Domain.Companies;
public class CompanyIdentity : BaseIdentity<Guid>
{
    public CompanyIdentity(Guid value) : base(value)
    {
    }
}
