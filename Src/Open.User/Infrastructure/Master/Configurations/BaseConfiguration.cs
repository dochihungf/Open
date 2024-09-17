using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Constants;
using Open.Core.SeedWork;

namespace Open.User.Infrastructure.Master.Configurations;

internal abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasDefaultValueSql(UniqueType.Algorithm)
            .ValueGeneratedOnAdd();
    }
}
