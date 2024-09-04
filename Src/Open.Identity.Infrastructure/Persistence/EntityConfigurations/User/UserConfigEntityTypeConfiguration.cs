using Open.Identity.Domain.Entities;
using Open.Identity.Domain.Enums;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class UserConfigEntityTypeConfiguration : PersonalizedEntityAuditBaseEntityTypeConfiguration<UserConfig>
{
    public override void Configure(EntityTypeBuilder<UserConfig> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);
    }
}
