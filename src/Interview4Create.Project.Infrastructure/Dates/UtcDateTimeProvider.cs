using Interview4Create.Project.Domain.Common.Dates;

namespace Interview4Create.Project.Infrastructure.Dates;
public class UtcDateTimeProvider : IDateTimeProvider
{
    public DateTime Now()
    {
        return DateTime.UtcNow;
    }
}
