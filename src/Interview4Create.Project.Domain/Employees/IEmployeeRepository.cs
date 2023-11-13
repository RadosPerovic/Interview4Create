namespace Interview4Create.Project.Domain.Employees;
public interface IEmployeeRepository
{
    Task<bool> IsEmailUnique(string email);
    Task<bool> AreEmailsUnique(IEnumerable<string> emails);
    Task Add(Employee employee);
    Task AddRange(IEnumerable<Employee> employees);
    Task<IEnumerable<Employee>> GetByIds(IEnumerable<EmployeeIdentity> ids);
}
