using Interview4Create.Project.Domain.Employees;
using Interview4Create.Project.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Repositories;
public class EmployeeRepository : IEmployeeRepository
{
    private readonly DatabaseContext _dbContext;

    public EmployeeRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Add(Employee employee)
    {
        var dbEmployee = employee.ToDatabaseModel();

        await _dbContext.AddAsync(dbEmployee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddRange(IEnumerable<Employee> employees)
    {
        var dbEmployees = employees.Select(e => e.ToDatabaseModel());

        await _dbContext.AddRangeAsync(dbEmployees);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> AreEmailsUnique(IEnumerable<string> emails)
    {
        return !await _dbContext.Employees.AnyAsync(e => emails.Contains(e.Email));
    }

    public async Task<IEnumerable<Employee>> GetByIds(IEnumerable<EmployeeIdentity> ids)
    {
        var idValues = ids.Select(e => e.Value);
        var dbEmployees = await _dbContext.Employees
            .Where(e => idValues.Contains(e.Id))
            .AsNoTracking()
            .ToListAsync();

        var employees = dbEmployees.Select(e => e.ToDomain());

        return employees;
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        return !await _dbContext.Employees.AnyAsync(e => e.Email == email);
    }
}
