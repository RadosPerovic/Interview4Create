namespace Interview4Create.Project.Infrastructure.EntityFramework.Models;
public class BaseEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedTs { get; set; }
}
