using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Open.Core.Caching.Distributed;
using Open.Core.Caching.Extensions;
using Open.Core.Caching.InMemory;
using Open.Core.Caching.Sequence;
using Open.Core.Caching.Settings;
using StackExchange.Redis;

namespace Open.Core.Caching;

public static class CachingDependencyInjection
{
    public static IServiceCollection AddInMemoryCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCaching>(s =>
            new MemoryCaching(s.GetRequiredService<IMemoryCache>()));
        
        return services;
    }

    public static IServiceCollection AddDistributedRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var configSection = configuration.GetSection(nameof(DistributedCacheSettings));
        services.Configure<DistributedCacheSettings>(configSection);
        
        var asyncPolicy = PollyExtensions.CreateDefaultPolicy(cfg =>
        {
            cfg.Or<RedisServerException>()
                .Or<RedisConnectionException>();
        });
        
        services.AddSingleton<IDistributedRedisCache>(s => new DistributedRedisCache(
            s.GetRequiredService<IOptions<DistributedCacheSettings>>(),
            asyncPolicy));

        return services;
    }
    
    public static IServiceCollection AddCoreCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInMemoryCaching(configuration)
            .AddDistributedRedisCaching(configuration)
            .AddSingleton<ISequenceCaching, SequenceCaching>();
        
        return services;
    }
}