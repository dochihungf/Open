using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Constants;
using Open.Driver.Domain.SeedWork;
using Open.SharedKernel.Libraries.Helpers;

namespace Open.Driver.Infrastructure.Master.EntityConfigurations;

public abstract class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(DataSchemaLength.ExtraLarge);
        
        builder.Property(e => e.CreatedDate)
            .HasDefaultValue(DateHelper.Now);
        
        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(DataSchemaLength.ExtraLarge)
            .IsRequired(false);
        
        builder.Property(e => e.LastModifiedDate)
            .HasDefaultValue(DateHelper.Now)
            .IsRequired(false);
        
        builder.Property(e => e.DeletedBy)
            .HasMaxLength(DataSchemaLength.ExtraLarge)
            .IsRequired(false);
        
        builder.Property(e => e.DeletedDate)
            .HasDefaultValue(DateHelper.Now)
            .IsRequired(false);
        
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(e => e.Version)
            .IsConcurrencyToken();
    }
}
