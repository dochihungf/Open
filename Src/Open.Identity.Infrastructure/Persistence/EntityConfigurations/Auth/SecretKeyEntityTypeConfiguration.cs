using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class SecretKeyEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<SecretKey>
{
    public override void Configure(EntityTypeBuilder<SecretKey> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);
        
        builder
            .HasIndex(r => r.Key)
            .IsClustered();

        builder
            .Property(e => e.Key)
            .HasMaxLength(DataSchemaLength.Medium)
            .IsUnicode()
            .IsRequired();
    }
}
