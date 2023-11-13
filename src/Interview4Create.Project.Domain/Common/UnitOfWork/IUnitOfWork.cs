namespace Interview4Create.Project.Domain.Common.UnitOfWork;
public interface IUnitOfWork
{
    void Enlist(Func<Task> operation);
    Task Complete();
}
