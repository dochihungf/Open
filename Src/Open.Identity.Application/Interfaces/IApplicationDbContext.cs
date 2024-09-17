using Microsoft.EntityFrameworkCore;
using Open.Core.EntityFrameworkCore;
using Open.Identity.Domain.Entities;

namespace Open.Identity.Application.Interfaces;

public interface IApplicationDbContext : IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<UserPermission> UserPermissions { get; set; }
    DbSet<RolePermission> RolePermissions { get; set; }
    DbSet<SignInHistory> SignInHistories { get; set; }
    DbSet<SecretKey> SecretKeys { get; set; }
    DbSet<OTP> OTPs { get; set; }
    DbSet<MFA> MFAs { get; set; }
}
