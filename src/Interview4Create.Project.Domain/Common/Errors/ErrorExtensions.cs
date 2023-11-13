using Interview4Create.Project.Domain.Common.OperationResult;

namespace Interview4Create.Project.Domain.Common.Errors;
public static class ErrorExtensions
{
    public static bool AreUnsuccessful(this IEnumerable<Result> results)
    {
        return results.Any(e => e.IsSuccessful is false);
    }

    public static IEnumerable<Error> GetErrors(this IEnumerable<Result> results)
    {
        return results
                .Where(e => e.IsSuccessful is false)
                .SelectMany(e => e.Errors);
    }
}
