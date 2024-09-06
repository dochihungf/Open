using Open.Identity.Infrastructure.Options;

namespace Open.Identity.Infrastructure.Persistence;

public class ApplicationDbContextFactory: IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=localhost; Port=3306; Database=identity_db; Uid=root; Pwd=root; SslMode=Preferred;";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        var identityStoreOptions = new IdentityStoreOptions();
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true);
        
        var context = new ApplicationDbContext(options.Options, identityStoreOptions);
        
        return context;
    }
}
