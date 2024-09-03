using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Core.SeedWork;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class EntityBaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>  where TEntity : EntityBase
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .HasKey(e => e.Id);
    }
}
