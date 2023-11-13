using Interview4Create.Project.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Configurations;
internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    private const int MAX_LENGTH = 50;
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(MAX_LENGTH);

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasOne(e => e.EmployeeTitle)
            .WithMany(e => e.Employees)
            .HasForeignKey(e => e.EmployeeTitleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
