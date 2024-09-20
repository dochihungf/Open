using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Constants;
using Open.Core.SeedWork;

namespace Open.User.Infrastructure.Master.Configurations;

public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CreatedDate)
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.LastModifiedDate)
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Version)
            .IsConcurrencyToken();
    }
}
