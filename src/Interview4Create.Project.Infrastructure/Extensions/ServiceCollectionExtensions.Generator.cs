using Interview4Create.Project.Domain.Common.Generators;
using Interview4Create.Project.Infrastructure.Generators;
using Microsoft.Extensions.DependencyInjection;

namespace Interview4Create.Project.Infrastructure.Extensions;
public static partial class ServiceCollectionExtensions
{
    public static void AddGuidGenerator(this IServiceCollection services)
    {
        services.AddScoped<IGenerator<Guid>, GuidGenerator>();
    }
}
