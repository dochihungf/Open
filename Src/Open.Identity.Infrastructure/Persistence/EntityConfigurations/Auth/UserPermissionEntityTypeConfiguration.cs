using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class UserPermissionEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<UserPermission>
{
    public override void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(ur => new { ur.UserId, ur.PermissionId });

        builder
            .HasOne(up => up.User)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.UserId);
        
        builder
            .HasOne(up => up.Permission)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.PermissionId);
    }
}
