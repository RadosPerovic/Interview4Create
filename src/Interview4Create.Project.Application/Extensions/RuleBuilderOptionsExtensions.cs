using FluentValidation;
using Interview4Create.Project.Domain.Common.Errors;

namespace Interview4Create.Project.Application.Extensions;
public static class RuleBuilderOptionsExtensions
{
    public static IRuleBuilder<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> builder, Error error)
    {
        return builder
            .WithMessage(error.ErrorMessage)
            .WithErrorCode(error.ErrorCode);
    }


}
