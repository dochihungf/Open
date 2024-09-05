using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class PermissionEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(a => a.Id);
        
        builder
            .Property(a => a.Code)
            .HasMaxLength(DataSchemaLength.Medium);

        builder
            .Property(a => a.Name)
            .HasMaxLength(DataSchemaLength.Large);

        builder
            .Property(a => a.Description)
            .HasMaxLength(DataSchemaLength.ExtraLarge);
    }
}
