using Interview4Create.Project.Application.Errors;
using Interview4Create.Project.Domain.Common.OperationResult;
using Interview4Create.Project.Domain.Common.UnitOfWork;
using Interview4Create.Project.Domain.Companies;
using Interview4Create.Project.Domain.Companies.Factory;
using Interview4Create.Project.Domain.Employees;
using Interview4Create.Project.Domain.Employees.Factories;
using MediatR;

namespace Interview4Create.Project.Application.UseCases.Companies.Commands;
public class CreateCompanyCommandRequestHandler : IRequestHandler<CreateCompanyCommandRequest, Result<CreateCompanyCommandResponse>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyFactory _companyFactory;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCompanyCommandRequestHandler(
        ICompanyRepository companyRepository,
        ICompanyFactory companyFactory,
        IEmployeeRepository employeeRepository,
        IEmployeeFactory employeeFactory,
        IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        _companyFactory = companyFactory ?? throw new ArgumentNullException(nameof(companyFactory));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _employeeFactory = employeeFactory ?? throw new ArgumentNullException(nameof(employeeFactory));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<CreateCompanyCommandResponse>> Handle(CreateCompanyCommandRequest request, CancellationToken cancellationToken)
    {
        if (!await _companyRepository.IsNameUnique(request.Name))
        {
            return Result<CreateCompanyCommandResponse>.Failure(ApplicationErrors.Company.NameIsNotUnique());
        }

        var company = _companyFactory.Create(request.Name);

        var employeeIdsFromRequest = request.Employees
            .Where(e => e.Id is not null)
            .Select(e => new EmployeeIdentity(e.Id!.Value))
            .ToList();

        var employeesForAdding = new List<Employee>();
        if (employeeIdsFromRequest.Any())
        {
            var existingEmployees = await _employeeRepository.GetByIds(employeeIdsFromRequest);

            if (!DoesEveryEmployeeFromRequestExist(employeeIdsFromRequest, existingEmployees))
            {
                return Result<CreateCompanyCommandResponse>.Failure(ApplicationErrors.Employee.EmployeeDoesNotExist());
            }

            employeesForAdding.AddRange(existingEmployees);
        }

        var unknownEmployees = request.Employees
            .Where(e => e.Id is null)
            .ToList();

        if (unknownEmployees.Any())
        {
            if (!await _employeeRepository.AreEmailsUnique(unknownEmployees.Select(e => e.Email)!))
            {
                return Result<CreateCompanyCommandResponse>.Failure(ApplicationErrors.Employee.EmailIsNotUnique());
            }

            var newEmployees = CreateNewEmployees(unknownEmployees);
            employeesForAdding.AddRange(newEmployees);

            _unitOfWork.Enlist(() => _employeeRepository.AddRange(newEmployees));
        }

        var result = company.AddEmployees(employeesForAdding);
        if (!result.IsSuccessful)
        {
            return Result<CreateCompanyCommandResponse>.Failure(result.Errors);
        }

        _unitOfWork.Enlist(() => _companyRepository.Add(company));
        await _unitOfWork.Complete();

        var response = new CreateCompanyCommandResponse
        {
            CreatedId = company.Id.Value
        };

        return Result<CreateCompanyCommandResponse>.Success(response);
    }

    private bool DoesEveryEmployeeFromRequestExist(IEnumerable<EmployeeIdentity> existingIds, IEnumerable<Employee> employees)
    {
        return employees.Count() == existingIds.Count();
    }

    private IEnumerable<Employee> CreateNewEmployees(List<CreateCompanyEmployeeCommandRequest> unknownEmployees)
    {
        var newEmployees = new List<Employee>();
        foreach (var unknownEmployee in unknownEmployees)
        {
            var newEmployee = _employeeFactory.Create(unknownEmployee.Email!, unknownEmployee.Title!.Value);
            newEmployees.Add(newEmployee);
        }

        return newEmployees;
    }
}
