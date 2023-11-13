using Interview4Create.Project.Application.Errors;
using Interview4Create.Project.Domain.Common.Errors;
using Interview4Create.Project.Domain.Common.OperationResult;
using Interview4Create.Project.Domain.Common.UnitOfWork;
using Interview4Create.Project.Domain.Companies;
using Interview4Create.Project.Domain.Employees;
using Interview4Create.Project.Domain.Employees.Factories;
using MediatR;

namespace Interview4Create.Project.Application.UseCases.Employees.Commands;
public class CreateEmployeeCommandRequestHandler : IRequestHandler<CreateEmployeeCommandRequest, Result<CreateEmployeeCommandResponse>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandRequestHandler(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        IEmployeeFactory employeeFactory)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _employeeFactory = employeeFactory ?? throw new ArgumentNullException(nameof(employeeFactory));
    }

    public async Task<Result<CreateEmployeeCommandResponse>> Handle(CreateEmployeeCommandRequest request, CancellationToken cancellationToken)
    {
        if (!await _employeeRepository.IsEmailUnique(request.Email))
        {
            return Result<CreateEmployeeCommandResponse>.Failure(ApplicationErrors.Employee.EmailIsNotUnique());
        }

        var companyIdsFromRequest = request.CompanyIds.Select(e => new CompanyIdentity(e));
        var companies = await _companyRepository.GetByIds(companyIdsFromRequest);

        if (!DoesEveryCompanyFromRequestExist(companyIdsFromRequest, companies))
        {
            return Result<CreateEmployeeCommandResponse>.Failure(ApplicationErrors.Company.CompanyDoesNotExist());
        }

        var employee = _employeeFactory.Create(request.Email, request.Title);

        var results = new List<Result>();
        foreach (var company in companies)
        {
            var result = company.AddEmployee(employee);
            results.Add(result);
        }

        if (results.AreUnsuccessful())
        {
            var errors = results.GetErrors();
            return Result<CreateEmployeeCommandResponse>.Failure(errors);
        }

        _unitOfWork.Enlist(() => _employeeRepository.Add(employee));
        _unitOfWork.Enlist(() => _companyRepository.UpdateRange(companies));

        await _unitOfWork.Complete();

        var response = new CreateEmployeeCommandResponse
        {
            CreatedId = employee.Id.Value,
        };

        return Result<CreateEmployeeCommandResponse>.Success(response);
    }

    private bool DoesEveryCompanyFromRequestExist(IEnumerable<CompanyIdentity> existingCompanyIds, IEnumerable<Company> companies)
    {
        return companies.Count() == existingCompanyIds.Count();
    }
}
