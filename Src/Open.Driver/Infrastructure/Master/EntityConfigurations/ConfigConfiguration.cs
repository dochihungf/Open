using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Driver.Domain.Aggregates;

namespace Open.Driver.Infrastructure.Master.EntityConfigurations;

public class ConfigConfiguration : EntityBaseConfiguration<Configuration>
{
    public override void Configure(EntityTypeBuilder<Configuration> builder)
    {
        base.Configure(builder);
        
        builder.Property(c => c.MaxCapacity)
            .HasDefaultValue(unchecked(10 * 1024 * 1024 * 1024)); // 10 GB

        builder.Property(c => c.MaxFileSize)
            .HasDefaultValue(100 * 1024 * 1024); // 100 MB
        
    }
}
