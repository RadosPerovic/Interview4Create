using Interview4Create.Project.Domain.Common.Dates;
using Interview4Create.Project.Domain.Common.Generators;

namespace Interview4Create.Project.Domain.Companies.Factory;
public class CompanyFactory : ICompanyFactory
{
    private readonly IGenerator<Guid> _generator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CompanyFactory(IGenerator<Guid> generator, IDateTimeProvider dateTimeProvider)
    {
        _generator = generator;
        _dateTimeProvider = dateTimeProvider;
    }

    public Company Create(string name)
    {
        var newGuid = _generator.Generate();
        var createdTs = _dateTimeProvider.Now();

        var identity = new CompanyIdentity(newGuid);

        return new Company(identity, name, createdTs);
    }
}
