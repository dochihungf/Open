using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Open.Core.GuardClauses;
using Open.SharedKernel.Caching.Distributed;
using Open.SharedKernel.Caching.Settings;
using StackExchange.Redis;

namespace Open.SharedKernel.Extensions;

public static class CachingDependencyInjectionExtensions
{
    public static IServiceCollection AddDistributedRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCacheSettings = configuration.GetSection(RedisCacheSettings.SectionName).Get<RedisCacheSettings>();
        Guard.Against.Null(redisCacheSettings, nameof(redisCacheSettings));
        
        var asyncPolicy = PollyExtensions.CreateDefaultPolicy(cfg =>
        {
            cfg.Or<RedisServerException>()
                .Or<RedisConnectionException>();
        });
        
        services.AddSingleton<IDistributedRedisCache>(s => new DistributedRedisCache(
            redisCacheSettings.ConnectionString,
            redisCacheSettings.InstanceName, 
            redisCacheSettings.DatabaseIndex,
            asyncPolicy));

        return services;
    }
}
