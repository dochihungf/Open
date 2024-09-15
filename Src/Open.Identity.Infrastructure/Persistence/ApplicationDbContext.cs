using System.Reflection;
using Open.Core.EntityFrameworkCore;
using Open.Identity.Application.Interfaces;
using Open.Identity.Domain.Entities;
using Open.Identity.Infrastructure.Extensions;
using Open.Identity.Infrastructure.Options;

namespace Open.Identity.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IdentityStoreOptions storeOptions) : AppDbContext(options), IApplicationDbContext
{
    private readonly IdentityStoreOptions _storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ModelBuilderIdentityContext(_storeOptions);
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserConfig> UserConfigs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<SignInHistory> SignInHistories { get; set; }
    public DbSet<SecretKey> SecretKeys { get; set; }
    public DbSet<OTP> OTPs { get; set; }
    public DbSet<MFA> MFAs { get; set; }
}
