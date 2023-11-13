using Interview4Create.Project.Domain.Common.Generators;

namespace Interview4Create.Project.Infrastructure.Generators;
public class GuidGenerator : IGenerator<Guid>
{
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}
