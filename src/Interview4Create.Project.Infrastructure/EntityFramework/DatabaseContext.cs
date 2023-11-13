using Humanizer;
using Interview4Create.Project.Infrastructure.EntityFramework.Models;
using Interview4Create.Project.Infrastructure.EntityFramework.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Interview4Create.Project.Infrastructure.EntityFramework;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }
    public DbSet<Models.EmployeeTitle> EmployeeTitles { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
    public DbSet<SystemLog> SystemLogs { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesWithSystemLogs(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyConfiguration(modelBuilder);
        Seed(modelBuilder);
    }

    private async Task<int> SaveChangesWithSystemLogs(CancellationToken cancellationToken)
    {
        var entriesWithTemporaryProperties = OnBefofeSaveChanges();
        var res1 = await base.SaveChangesAsync(cancellationToken);
        var res2 = await OnAfterSaveChanges(entriesWithTemporaryProperties);
        return res1 + res2;
    }

    private IEnumerable<SystemLogEntry> OnBefofeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var systemLogEntries = new List<SystemLogEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is SystemLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
            {
                continue;
            }

            var systemLogEntry = CreateSystemLogEntry(entry);
            FillSystemLogEntryProperties(entry, systemLogEntry);
            systemLogEntries.Add(systemLogEntry);
        }

        var systemLogs = systemLogEntries.Where(e => !e.HasTemporaryProperties).Select(e => e.ToSystemLog());
        SystemLogs.AddRange(systemLogs);

        return systemLogEntries.Where(e => e.HasTemporaryProperties);
    }

    private SystemLogEntry CreateSystemLogEntry(EntityEntry entry)
    {
        return new SystemLogEntry(entry)
        {
            TableName = entry.Entity.GetType().Name.Pluralize(),
        };
    }

    private void FillSystemLogEntryProperties(EntityEntry entry, SystemLogEntry systemLogEntry)
    {
        foreach (var property in entry.Properties)
        {
            if (property.IsTemporary)
            {
                systemLogEntry.TemporaryProperties.Add(property);
            }

            var propertyName = property.Metadata.Name;
            if (property.Metadata.IsPrimaryKey())
            {
                systemLogEntry.KeyValues[propertyName] = property.CurrentValue!;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    SetAddedProperties(systemLogEntry, property, propertyName);
                    break;
                case EntityState.Modified:
                    if (property.IsModified)
                    {
                        SetModifiedProperties(systemLogEntry, property, propertyName);
                    }
                    break;
                default:
                    if (systemLogEntry.EntityEntry.Entity.GetType() == typeof(CompanyEmployee))
                    {
                        SetAddedProperties(systemLogEntry, property, propertyName);
                    }
                    break;
            }
        }
    }

    private void SetAddedProperties(SystemLogEntry systemLogEntry, PropertyEntry property, string propertyName)
    {
        systemLogEntry.SystemLogType = Enums.SystemLogType.Create;
        systemLogEntry.NewValues[propertyName] = property.CurrentValue!;
    }

    private void SetModifiedProperties(SystemLogEntry systemLogEntry, PropertyEntry property, string propertyName)
    {
        systemLogEntry.ChangedColumns.Add(propertyName);
        systemLogEntry.SystemLogType = Enums.SystemLogType.Update;
        systemLogEntry.OldValues[propertyName] = property.OriginalValue!;
        systemLogEntry.NewValues[propertyName] = property.CurrentValue!;
    }

    private void SetCompanyEmployeeSystemLogType(SystemLogEntry systemLogEntry, PropertyEntry property, string propertyName)
    {
        systemLogEntry.SystemLogType = Enums.SystemLogType.Create;
    }

    private async Task<int> OnAfterSaveChanges(IEnumerable<SystemLogEntry> systemLogEntries)
    {
        if (systemLogEntries is null || !systemLogEntries.Any())
        {
            await Task.FromResult(0);
        }

        foreach (var entry in systemLogEntries!)
        {
            foreach (var property in entry.EntityEntry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                {
                    entry.KeyValues[property.Metadata.Name] = property.CurrentValue!;
                }

                entry.NewValues[property.Metadata.Name] = property.CurrentValue!;
            }

            SystemLogs.AddRange(entry.ToSystemLog());
        }

        return await base.SaveChangesAsync();
    }

    private void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }


    private void Seed(ModelBuilder modelBuilder)
    {
        var companies = DataSeeder.GetDefaultCompaniesData();
        SeedData(modelBuilder, companies.ToList());

        var employees = DataSeeder.GetDefaultEmployeesData();
        SeedData(modelBuilder, employees.ToList());

        var companyEmployees = DataSeeder.GetCompanyEmployeesForConcreteCompanyAndEmployees(companies, employees);
        SeedData(modelBuilder, companyEmployees.ToList());

        SeedEnumData<Domain.Common.Enums.EmployeeTitle, EmployeeTitle>(modelBuilder, e => new Models.EmployeeTitle { Id = (byte)e, Name = e.ToString() });
    }

    private void SeedEnumData<TEnum, TDatabaseModel>(ModelBuilder modelBuilder, Func<TEnum, TDatabaseModel> converter)
        where TDatabaseModel : class
    {
        var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

        var data = values.Select(converter).ToList();

        SeedData(modelBuilder, data);
    }

    private void SeedData<TDatabaseModel>(ModelBuilder modelBuilder, List<TDatabaseModel> data)
        where TDatabaseModel : class
    {
        modelBuilder.Entity<TDatabaseModel>().HasData(data);
    }
}
