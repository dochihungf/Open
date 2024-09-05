using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class UserRoleEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            
        builder
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}
