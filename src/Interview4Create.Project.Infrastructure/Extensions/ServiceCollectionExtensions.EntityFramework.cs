using System.Reflection;
using Interview4Create.Project.Domain.Common.UnitOfWork;
using Interview4Create.Project.Infrastructure.EntityFramework;
using Interview4Create.Project.Infrastructure.EntityFramework.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interview4Create.Project.Infrastructure.Extensions;
public static partial class ServiceCollectionExtensions
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterDbContext(services, configuration);
        RegisterRepositories(services);
        RegisterUnitOfWork(services);
    }

    private static void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("PostgreSql:ConnectionString");
        services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository")))
            .AsMatchingInterface()
            .WithScopedLifetime());
    }

    private static void RegisterUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}