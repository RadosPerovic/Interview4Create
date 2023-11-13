using FluentValidation;
using Interview4Create.Project.Application.PipelineBehavior;
using Interview4Create.Project.Infrastructure.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interview4Create.Project.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        AddDomainDependencies(services);
        AddApplicationDependencies(services);
        AddInfrastructureDependencies(services, configuration);
    }

    private static void AddDomainDependencies(IServiceCollection services)
    {
        AddDomainFactories(services);
    }

    private static void AddApplicationDependencies(IServiceCollection services)
    {
        services.AddMediatR(opt => opt.RegisterServicesFromAssemblies(typeof(Application.Module).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(Application.Module).Assembly);
    }

    private static void AddInfrastructureDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFramework(configuration);
        services.AddGuidGenerator();
        services.AddUtcDateTimeProvider();
    }

    private static void AddDomainFactories(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(new[] { typeof(Domain.Module).Assembly })
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Factory")))
            .AsMatchingInterface()
            .WithScopedLifetime());
    }
}

