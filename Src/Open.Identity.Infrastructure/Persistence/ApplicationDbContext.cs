using System.Reflection;
using Open.Core.EntityFrameworkCore;
using Open.Identity.Infrastructure.Extensions;
using Open.Identity.Infrastructure.Options;

namespace Open.Identity.Infrastructure.Persistence;

public class ApplicationDbContext : AppDbContext, IUnitOfWork
{
    private readonly IdentityStoreOptions _storeOptions;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IdentityStoreOptions storeOptions) : base(options)
    {
        _storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureIdentityContext(_storeOptions);
        
        base.OnModelCreating(modelBuilder);
    }
}
