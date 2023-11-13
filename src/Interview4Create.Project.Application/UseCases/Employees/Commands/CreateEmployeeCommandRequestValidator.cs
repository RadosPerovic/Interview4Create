using FluentValidation;
using Interview4Create.Project.Application.Errors;
using Interview4Create.Project.Application.Extensions;
using Interview4Create.Project.Domain.Common.Enums;

namespace Interview4Create.Project.Application.UseCases.Employees.Commands;
public class CreateEmployeeCommandRequestValidator : AbstractValidator<CreateEmployeeCommandRequest>
{
    private const int MAX_LENGT = 50;
    public CreateEmployeeCommandRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithError(ApplicationErrors.Employee.EmailMustNotBeEmpty())
            .MaximumLength(MAX_LENGT)
            .WithError(ApplicationErrors.Employee.EmailMaxLength(MAX_LENGT));

        RuleFor(r => r.Title)
            .Must(t => t != EmployeeTitle.None)
            .WithError(ApplicationErrors.Employee.TitleCannotBeNone());
    }
}
