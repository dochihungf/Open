using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.User.Domain.UserAggregate;

namespace Open.User.Infrastructure.Master.Configurations;

public class UserConfigConfiguration : BaseConfiguration<UserConfig>
{
    public override void Configure(EntityTypeBuilder<UserConfig> builder)
    {
        base.Configure(builder);
    }
}
