using Interview4Create.Project.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Configurations;
internal class CompanyEmployeeConfiguration : IEntityTypeConfiguration<CompanyEmployee>
{
    public void Configure(EntityTypeBuilder<CompanyEmployee> builder)
    {
        builder.HasKey(e => new { e.CompanyId, e.EmployeeId });

        builder.HasOne(e => e.Company)
            .WithMany(e => e.CompanyEmployees)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Employee)
            .WithMany(e => e.CompanyEmployees)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
