using FluentValidation;
using Interview4Create.Project.Application.Errors;
using Interview4Create.Project.Application.Extensions;

namespace Interview4Create.Project.Application.UseCases.Companies.Commands;
public class CreateCompanyCommandRequestValidator : AbstractValidator<CreateCompanyCommandRequest>
{
    private const int MAX_LENGTH = 50;
    public CreateCompanyCommandRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithError(ApplicationErrors.Company.NameCannotBeEmpty())
            .MaximumLength(MAX_LENGTH)
            .WithError(ApplicationErrors.Company.NameMaxLength(MAX_LENGTH));

        RuleForEach(r => r.Employees)
            .SetValidator(new CreateCompanyEmployeeCommandRequestValidator());

    }
}

public class CreateCompanyEmployeeCommandRequestValidator : AbstractValidator<CreateCompanyEmployeeCommandRequest>
{
    public CreateCompanyEmployeeCommandRequestValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .When(r => r.Email is null && r.Title is null)
            .WithError(ApplicationErrors.Company.EmployeeIdMustNotBeNullWhenEmailAndTitleAreNull());

        RuleFor(r => r.Id)
            .Null()
            .When(r => r.Email is not null && r.Title is not null)
            .WithError(ApplicationErrors.Company.EmployeeIdMustBeNullWhenEmailAndTitleAreNotNull());

        RuleFor(r => r.Email)
            .NotNull()
            .When(e => e.Id is null)
            .WithError(ApplicationErrors.Company.EmployeeEmailMustNotBeNullWhenIdIsNull());

        RuleFor(e => e.Email)
            .Null()
            .When(e => e.Id is not null)
            .WithError(ApplicationErrors.Company.EmployeeEmailMustBeNullWhenIdIsNotNull());

        RuleFor(e => e.Title)
            .NotNull()
            .When(e => e.Id is null)
            .WithError(ApplicationErrors.Company.EmployeeTitleMustNotBeNullWhenIdIsNull());

        RuleFor(e => e.Title)
            .Null()
            .When(e => e.Id is not null)
            .WithError(ApplicationErrors.Company.EmployeeTitleMustBeNullWhenIdIsNotNull());
    }
}
