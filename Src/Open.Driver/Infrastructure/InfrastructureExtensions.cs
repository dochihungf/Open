using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Open.Core.GuardClauses;
using Open.Driver.Infrastructure.Master;
using Open.Driver.Infrastructure.Master.Interceptors;
using Open.Driver.Infrastructure.S3ObjectStorage;
using Open.Driver.Infrastructure.S3ObjectStorage.Providers;
using Open.SharedKernel.Caching.Distributed;
using Open.SharedKernel.Caching.Extensions;
using Open.SharedKernel.Caching.InMemory;
using Open.SharedKernel.Caching.Sequence;
using Open.SharedKernel.Settings;

namespace Open.Driver.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Master Database
        Guard.Against.Null(CoreSettings.ConnectionStrings, nameof(CoreSettings.ConnectionStrings));
        Guard.Against.Null(CoreSettings.ConnectionStrings["Master"], message: "Connection string 'Master' not found.");
        
        services.AddScoped<ISaveChangesInterceptor, EntityBaseInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        services.AddMigration<ApplicationDbContext>();
        
        services.ConfigureMySqlRetryOptions(configuration);
        
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var mySqlRetryOptions = provider.GetRequiredService<IOptionsMonitor<MySqlRetryOptions>>();
            
            options.AddInterceptors(provider.GetServices<ISaveChangesInterceptor>());
            
            options.UseMySql(CoreSettings.ConnectionStrings["Master"],
                    ServerVersion.AutoDetect(CoreSettings.ConnectionStrings["Master"]),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: mySqlRetryOptions.CurrentValue.MaxRetryCount,
                        maxRetryDelay: mySqlRetryOptions.CurrentValue.MaxRetryDelay,
                        errorNumbersToAdd: mySqlRetryOptions.CurrentValue.ErrorNumbersToAdd))
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true);
        });
        
        services.AddScoped<ApplicationDbContextSeed>();

        
        // S3 Object Storage
        services.ConfigureS3StorageSettings(configuration);
        
        services.AddSingleton<AmazonS3Client>(s =>
        {
            var s3StorageSettings = s.GetRequiredService<IOptionsMonitor<S3StorageSettings>>();
            return new AmazonS3Client(s3StorageSettings.CurrentValue.AccessKey,
                s3StorageSettings.CurrentValue.SecretKey,
                new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.APSoutheast1,
                    RetryMode = RequestRetryMode.Standard,
                    MaxErrorRetry = s3StorageSettings.CurrentValue.MaxErrorRetry,
                    ServiceURL = s3StorageSettings.CurrentValue.ServiceURL,
                    UseHttp = false,
                    SignatureVersion = SignatureVersion.SigV4.ToString(),
                    Timeout = TimeSpan.FromSeconds(10)
                });
        });
        
        services.AddScoped<IS3StorageProvider, S3StorageProvider>();
        
        // Redis Caching
        services.AddInMemoryCaching(configuration);
        services.AddDistributedRedisCache(configuration);
        services.AddTransient<ISequenceCaching, SequenceCaching>();
        
        // ...
        services.AddSingleton(TimeProvider.System);
        
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

    public static OptionsBuilder<S3StorageSettings> ConfigureS3StorageSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection(nameof(S3StorageSettings));
        return services.AddOptions<S3StorageSettings>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
    
   

}
