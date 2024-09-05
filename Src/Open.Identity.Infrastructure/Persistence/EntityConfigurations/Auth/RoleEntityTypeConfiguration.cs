using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class RoleEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);
        
        builder
            .HasIndex(r => r.Code)
            .IsClustered();
            
        builder
            .Property(r => r.Code)
            .HasMaxLength(DataSchemaLength.Medium);

        builder
            .Property(r => r.Name)
            .HasMaxLength(DataSchemaLength.Large);
    }
}
