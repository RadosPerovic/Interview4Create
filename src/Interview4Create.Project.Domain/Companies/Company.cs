using Interview4Create.Project.Domain.Common.Base;
using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Common.Errors;
using Interview4Create.Project.Domain.Common.Guard;
using Interview4Create.Project.Domain.Common.OperationResult;
using Interview4Create.Project.Domain.Employees;

namespace Interview4Create.Project.Domain.Companies;
public class Company : Entity<CompanyIdentity>
{
    private string _name;
    private readonly ISet<Employee> _employees;

    public Company(CompanyIdentity id, string name, DateTime createdTs) : base(id, createdTs)
    {
        Name = name;

        _employees = new HashSet<Employee>();
    }

    public string Name
    {
        get
        {
            return _name;
        }
        private set
        {
            CommonGuard.NotNull(value);
            _name = value;
        }
    }

    public IEnumerable<Employee> Employees
    {
        get
        {
            return _employees;
        }
    }

    public Result AddEmployee(Employee employee)
    {
        if (DoesEmployeeWithSameTitleExist(employee.Title))
        {
            return Result.Failure(DomainErrors.Company.CompanyPlaceForSpecificTitleFilled(employee.Title));
        }

        _employees.Add(employee);

        return Result.Success();
    }

    public Result AddEmployees(IEnumerable<Employee> employees)
    {
        var results = new List<Result>();

        foreach (var employee in employees)
        {
            var result = AddEmployee(employee);
            results.Add(result);
        }

        if (results.AreUnsuccessful())
        {
            var errors = results.GetErrors();
            return Result.Failure(errors);
        }

        return Result.Success();
    }

    private bool DoesEmployeeWithSameTitleExist(EmployeeTitle title)
    {
        return _employees.Any(e => e.Title == title);
    }
}
