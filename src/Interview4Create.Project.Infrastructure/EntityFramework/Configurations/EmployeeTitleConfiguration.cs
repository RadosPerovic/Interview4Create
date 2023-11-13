using Interview4Create.Project.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Configurations;
internal class EmployeeTitleConfiguration : IEntityTypeConfiguration<EmployeeTitle>
{
    private const int MAX_LENGTH = 50;

    public void Configure(EntityTypeBuilder<EmployeeTitle> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Name)
            .HasMaxLength(MAX_LENGTH);
    }
}
