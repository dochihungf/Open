namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class EntityBaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>  where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .HasKey(e => e.Id);
    }
}
