using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.User.Domain.UserAggregate;

namespace Open.User.Infrastructure.Master.Configurations;

public class UserAddressConfiguration : BaseConfiguration<UserAddress>
{
    public override void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        base.Configure(builder);
    }
}
