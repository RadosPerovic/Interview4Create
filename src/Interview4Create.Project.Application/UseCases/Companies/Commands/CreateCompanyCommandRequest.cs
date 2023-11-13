using Interview4Create.Project.Domain.Common.Enums;
using Interview4Create.Project.Domain.Common.OperationResult;
using MediatR;

namespace Interview4Create.Project.Application.UseCases.Companies.Commands;
public class CreateCompanyCommandRequest : IRequest<Result<CreateCompanyCommandResponse>>
{
    public string Name { get; set; }
    public IEnumerable<CreateCompanyEmployeeCommandRequest> Employees { get; set; }
}

public class CreateCompanyEmployeeCommandRequest
{
    public Guid? Id { get; set; }
    public string? Email { get; set; }
    public EmployeeTitle? Title { get; set; }
}
