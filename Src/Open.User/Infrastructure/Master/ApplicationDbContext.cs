using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Open.Core.EntityFrameworkCore;
using Open.User.Domain.UserAggregate;
using AppUser = Open.User.Domain.UserAggregate.User;

namespace Open.User.Infrastructure.Master;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : AppDbContext(options), IAppDbContext
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserProfile> Profiles { get; set; }
    public DbSet<UserAvatar> Avatars { get; set; }
    public DbSet<UserAddress> Addresses { get; set; }
    public DbSet<UserConfig> Configs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
