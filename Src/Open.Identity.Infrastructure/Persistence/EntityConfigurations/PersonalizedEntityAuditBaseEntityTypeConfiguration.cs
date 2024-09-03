namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations; 
public class PersonalizedEntityAuditBaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : PersonalizedEntityAuditBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .Property(e => e.CreatedBy)
            .HasMaxLength(255);
        
        builder
            .Property(e => e.LastModifiedBy)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(e => e.LastModifiedDate)
            .IsRequired(false);
        
        builder
            .Property(e => e.DeletedBy)
            .HasMaxLength(255)
            .IsRequired(false);
        
        builder
            .Property(e => e.DeletedDate)
            .IsRequired(false);
        
        builder
            .Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        builder
            .Property(e => e.OwnerId)
            .IsRequired();
    }
}
