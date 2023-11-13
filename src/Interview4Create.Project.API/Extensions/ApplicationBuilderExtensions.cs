using Interview4Create.Project.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Interview4Create.Project.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UpdateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        context.Database.SetCommandTimeout(TimeSpan.FromSeconds(300));
        context.Database.Migrate();
    }
}
