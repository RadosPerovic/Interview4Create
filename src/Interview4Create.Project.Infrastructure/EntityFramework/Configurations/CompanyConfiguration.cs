using Interview4Create.Project.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Configurations;
internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    private const int MAX_LENGTH = 100;
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(MAX_LENGTH);

        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}
