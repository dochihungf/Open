using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Constants;
using Open.User.Domain.UserAggregate;

namespace Open.User.Infrastructure.Master.Configurations;

public class UserAvatarConfiguration : BaseConfiguration<UserAvatar>
{
    public override void Configure(EntityTypeBuilder<UserAvatar> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.FileName)
            .HasMaxLength(DataSchemaLength.SuperLarge);
        
        builder.Property(e => e.Url)
            .HasMaxLength(DataSchemaLength.SuperLarge);
        
        builder.HasIndex(e => e.FileName)
            .IsUnique();
    }
}
