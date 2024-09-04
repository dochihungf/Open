using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Open.Core.GuardClauses;
using Open.Core.Repositories.Dapper;
using Open.Identity.Infrastructure.Persistence;
using Open.SharedKernel.Repositories.Dapper;
using Open.SharedKernel.Settings;

namespace Open.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        Guard.Against.Null(CoreSettings.ConnectionStrings, nameof(CoreSettings.ConnectionStrings));
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(CoreSettings.ConnectionStrings["Default"], 
                    ServerVersion.AutoDetect(CoreSettings.ConnectionStrings["Default"]))
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
        );
        
        // Base
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
        services.AddScoped(typeof(IWriteOnlyRepository<>), typeof(WriteOnlyRepository<>));
        
        
        return services;
    }
}
