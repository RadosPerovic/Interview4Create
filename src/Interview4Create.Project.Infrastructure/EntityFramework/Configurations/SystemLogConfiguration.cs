using Interview4Create.Project.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Configurations;
public class SystemLogConfiguration : IEntityTypeConfiguration<SystemLog>
{
    public void Configure(EntityTypeBuilder<SystemLog> builder)
    {
        builder.Property(e => e.Id)
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Type)
            .IsRequired(false);

        builder.Property(e => e.TableName)
            .IsRequired(false);

        builder.Property(e => e.PrimaryKey)
            .IsRequired(false);

        builder.Property(e => e.OldValues)
            .IsRequired(false);

        builder.Property(e => e.NewValues)
            .IsRequired(false);

        builder.Property(e => e.AffectedColumns)
            .IsRequired(false);
    }
}
