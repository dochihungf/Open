using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Constants;
using Open.Core.SeedWork;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class EntityAuditBaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityAuditBase
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .Property(e => e.CreatedBy)
            .HasMaxLength(DataSchemaLength.ExtraLarge);
        
        builder
            .Property(e => e.LastModifiedBy)
            .HasMaxLength(DataSchemaLength.ExtraLarge)
            .IsRequired(false);

        builder
            .Property(e => e.LastModifiedDate)
            .IsRequired(false);
        
        builder
            .Property(e => e.DeletedBy)
            .HasMaxLength(DataSchemaLength.ExtraLarge)
            .IsRequired(false);
        
        builder
            .Property(e => e.DeletedDate)
            .IsRequired(false);
        
        builder
            .Property(e => e.IsDeleted)
            .HasDefaultValue(false);
    }
}
