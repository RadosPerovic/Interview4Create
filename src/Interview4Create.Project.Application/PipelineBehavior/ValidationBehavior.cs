using FluentValidation;
using Interview4Create.Project.Domain.Common.OperationResult;
using MediatR;

namespace Interview4Create.Project.Application.PipelineBehavior;
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(_validators.Select(e => e.ValidateAsync(context, cancellationToken)));
        var failures = results
            .SelectMany(vr => vr.Errors)
            .Where(er => er != null)
            .ToArray();

        if (failures.Any())
        {
            var result = new TResponse()
            {
                IsSuccessful = false,
                Message = "Validation failed",
                Errors = failures
                            .Select(e => new Domain.Common.Errors.Error(e.ErrorCode, e.ErrorMessage))
                            .ToList(),
            };

            return result;
        }

        return await next();
    }
}
