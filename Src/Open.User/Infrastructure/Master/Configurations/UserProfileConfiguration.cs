using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.User.Domain.UserAggregate;

namespace Open.User.Infrastructure.Master.Configurations;

public class UserProfileConfiguration : BaseConfiguration<UserProfile>
{
    public override void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        base.Configure(builder);
    }
}
