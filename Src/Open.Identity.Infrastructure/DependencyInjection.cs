using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Open.Core.GuardClauses;
using Open.Core.Repositories.Dapper;
using Open.Identity.Application.Interfaces;
using Open.Identity.Infrastructure.Options;
using Open.Identity.Infrastructure.Persistence;
using Open.SharedKernel.Repositories.Dapper;
using Open.SharedKernel.Settings;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;

namespace Open.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // MySqlRetryOptions
        services.ConfigureMySqlRetryOptions(configuration);
        
        // DbContext
        Guard.Against.Null(CoreSettings.ConnectionStrings, nameof(CoreSettings.ConnectionStrings));
        services.AddIdentityDbContext<ApplicationDbContext>(identityStoreOptions =>
        {
            identityStoreOptions.ResolveDbContextOptions = (provider, builder) =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<MySqlRetryOptions>>();

                builder
                    .UseMySql(CoreSettings.ConnectionStrings["Default"],
                        ServerVersion.AutoDetect(CoreSettings.ConnectionStrings["Default"]),
                        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: options.CurrentValue.MaxRetryCount,
                            maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                            errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                    .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                    .EnableDetailedErrors(true)
                    .EnableSensitiveDataLogging(true);
            };
        });

        // Base
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
        services.AddScoped(typeof(IWriteOnlyRepository<>), typeof(WriteOnlyRepository<>));
        
        // User
        
        return services;
    }

    public static IServiceCollection AddIdentityDbContext<TContext>(this IServiceCollection services,
        Action<IdentityStoreOptions>? storeOptionsAction = null) 
        where TContext : DbContext, IApplicationDbContext
    {
        var options = new IdentityStoreOptions();
        services.AddSingleton(options);
        storeOptionsAction?.Invoke(options);
        
        if (options.ResolveDbContextOptions != null)
        {
            services.AddDbContext<TContext>(options.ResolveDbContextOptions);
        }
        else
        {
            services.AddDbContext<TContext>(dbCtxBuilder =>
            {
                options.IdentityDbContext?.Invoke(dbCtxBuilder);
            });
        }
        
        services.AddScoped<TContext>();

        return services;
    }
    
    public static OptionsBuilder<MySqlRetryOptions> ConfigureMySqlRetryOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection(nameof(MySqlRetryOptions));
        return services.AddOptions<MySqlRetryOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
