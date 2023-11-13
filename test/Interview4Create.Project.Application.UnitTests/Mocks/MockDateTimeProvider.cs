using Interview4Create.Project.Domain.Common.Dates;

namespace Interview4Create.Project.Application.UnitTests.Mocks;
public class MockDateTimeProvider : IDateTimeProvider
{
    private DateTime _dateTime;

    public MockDateTimeProvider(DateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public DateTime Now()
    {
        return _dateTime;
    }

    public void SetNow(DateTime now)
    {
        _dateTime = now;
    }
}
