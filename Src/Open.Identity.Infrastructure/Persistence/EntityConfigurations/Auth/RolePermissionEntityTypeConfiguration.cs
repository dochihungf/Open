using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class RolePermissionEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<RolePermission>
{
    public override void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(ur => new { ur.PermissionId, ur.RoleId });

        builder.HasOne(ur => ur.Role)
            .WithMany(u => u.RolePermissions)
            .HasForeignKey(ur => ur.RoleId);

        builder.HasOne(ur => ur.Permission)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(ur => ur.PermissionId);
    }
}
