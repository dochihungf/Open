using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Open.User.Infrastructure.Master;

public class ApplicationDbContextFactory: IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=localhost; Port=3306; Database=user_db; Uid=root; Pwd=root; SslMode=Preferred;";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true);
        
        var context = new ApplicationDbContext(options.Options);
        
        return context;
    }
}
