using Interview4Create.Project.Domain.Common.Dates;
using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Common.Generators;

namespace Interview4Create.Project.Domain.Employees.Factories;
public class EmployeeFactory : IEmployeeFactory
{
    private readonly IGenerator<Guid> _generator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public EmployeeFactory(IGenerator<Guid> generator, IDateTimeProvider dateTimeProvider)
    {
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public Employee Create(string email, EmployeeTitle title)
    {
        var newGuid = _generator.Generate();
        var createdTs = _dateTimeProvider.Now();

        var identity = new EmployeeIdentity(newGuid);

        return new Employee(
            identity,
            email,
            title,
            createdTs);
    }
}
