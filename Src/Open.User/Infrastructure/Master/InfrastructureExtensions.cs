using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Open.Core.GuardClauses;
using Open.SharedKernel.Settings;
using Open.User.Extensions;
using Open.User.Infrastructure.Options;

namespace Open.User.Infrastructure.Master;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // AddMigration
        services.AddMigration<ApplicationDbContext>();
        
        // ConfigureMySqlRetryOptions
        services.ConfigureMySqlRetryOptions(configuration);
        
        // AddDbContext
        Guard.Against.Null(CoreSettings.ConnectionStrings, nameof(CoreSettings.ConnectionStrings));
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var mySqlRetryOptions = provider.GetRequiredService<IOptionsMonitor<MySqlRetryOptions>>();
            options.UseMySql(CoreSettings.ConnectionStrings["Default"],
                    ServerVersion.AutoDetect(CoreSettings.ConnectionStrings["Default"]),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: mySqlRetryOptions.CurrentValue.MaxRetryCount,
                        maxRetryDelay: mySqlRetryOptions.CurrentValue.MaxRetryDelay,
                        errorNumbersToAdd: mySqlRetryOptions.CurrentValue.ErrorNumbersToAdd))
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true);
        });

    
        return services;
    }
    
    
    private static OptionsBuilder<MySqlRetryOptions> ConfigureMySqlRetryOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection(nameof(MySqlRetryOptions));
        return services.AddOptions<MySqlRetryOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
