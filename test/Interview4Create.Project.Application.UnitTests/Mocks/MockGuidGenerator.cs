using Interview4Create.Project.Domain.Common.Generators;

namespace Interview4Create.Project.Application.UnitTests.Mocks;
public class MockGuidGenerator : IGenerator<Guid>
{
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}
