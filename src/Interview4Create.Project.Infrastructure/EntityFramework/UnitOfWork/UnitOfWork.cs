using Interview4Create.Project.Domain.Common.UnitOfWork;

namespace Interview4Create.Project.Infrastructure.EntityFramework.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _dbContext;
    private readonly IList<Func<Task>> _operations;

    public UnitOfWork(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        _operations = new List<Func<Task>>();
    }

    public async Task Complete()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            foreach (var operation in _operations)
            {
                await operation();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
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
