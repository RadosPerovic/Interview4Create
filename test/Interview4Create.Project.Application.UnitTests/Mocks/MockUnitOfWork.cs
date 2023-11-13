using Interview4Create.Project.Domain.Common.UnitOfWork;

namespace Interview4Create.Project.Application.UnitTests.Mocks;
public class MockUnitOfWork : IUnitOfWork
{
    private readonly IList<Func<Task>> _operations;

    public MockUnitOfWork()
    {
        _operations = new List<Func<Task>>();
    }

    public async Task Complete()
    {
        try
        {
            foreach (var operation in _operations)
            {
                await operation();
            }
        }
        finally
        {
            _operations.Clear();
        }
    }

    public void Enlist(Func<Task> operation)
    {
        _operations.Add(operation);
    }
}
