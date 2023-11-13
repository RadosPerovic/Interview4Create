using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Common.OperationResult;
using MediatR;

namespace Interview4Create.Project.Application.UseCases.Employees.Commands;

public class CreateEmployeeCommandRequest : IRequest<Result<CreateEmployeeCommandResponse>>
{
    public string Email { get; set; }
    public EmployeeTitle Title { get; set; }
    public IEnumerable<Guid> CompanyIds { get; set; }
}
