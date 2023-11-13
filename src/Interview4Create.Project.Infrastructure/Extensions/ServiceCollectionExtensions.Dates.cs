using Interview4Create.Project.Domain.Common.Dates;
using Interview4Create.Project.Infrastructure.Dates;
using Microsoft.Extensions.DependencyInjection;

namespace Interview4Create.Project.Infrastructure.Extensions;
public static partial class ServiceCollectionExtensions
{
    public static void AddUtcDateTimeProvider(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, UtcDateTimeProvider>();
    }
}
