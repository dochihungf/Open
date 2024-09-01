using System.Text;
using Newtonsoft.Json;
using Open.Core.GuardClauses;
using Polly.Retry;
using StackExchange.Redis;

namespace Open.SharedKernel.Caching.Distributed;

public class DistributedRedisCache : IDistributedRedisCache
{
    #region Declares + Constructor
    
    private readonly IDatabase _cache;
    private readonly string _instance;
    private readonly AsyncRetryPolicy? _retryPolicy;
    private readonly IServer _server;

    public DistributedRedisCache(string connectionString, 
        string prefix, 
        int database = 0,
        AsyncRetryPolicy? asyncRetryPolicy = null)
    {
        var connection = ConnectionMultiplexer.Connect(connectionString);
        _cache = connection.GetDatabase(database);
        _server = connection.GetServer(connectionString);
        _instance = prefix;
        _retryPolicy = asyncRetryPolicy;
    }
    
    public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromHours(24);
    
    #endregion

    #region Implements

     public async Task<bool> ExistsAsync(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        
        return await RunWithPolicyAsync(async () => await _cache.KeyExistsAsync(key));
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(value, nameof(value));
        
        var absoluteExpireTime = expiry ?? DefaultAbsoluteExpireTime;
        var valueString = JsonConvert.SerializeObject(value);
        
        return await RunWithPolicyAsync(async () => await SetStringAsync(key, valueString, absoluteExpireTime, keepTtl));
    }

    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        
        var absoluteExpireTime = expiry ?? DefaultAbsoluteExpireTime;
        
        return await RunWithPolicyAsync(async () =>
            await _cache.StringSetAsync(key, Encoding.UTF8.GetBytes(value), absoluteExpireTime, keepTtl));
    }

    public async Task<bool> DeleteAsync(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        
        return await RunWithPolicyAsync(async () => await _cache.KeyDeleteAsync(key));
    }

    public async Task<bool> DeleteByPatternAsync(string pattern)
    {
        Guard.Against.Null(_server, nameof(_server));
        Guard.Against.NullOrEmpty(pattern, nameof(pattern));
        
        var keys = _server.Keys(pattern: pattern).ToArray();

        await RunWithPolicyAsync(async () =>
        {
            await _cache.KeyDeleteAsync(keys);
        });

        return true;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));

        return await RunWithPolicyAsync(async () =>
        {
            var value = await GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            
            return JsonConvert.DeserializeObject<T>(value);
        });
    }

    public async Task<string> GetStringAsync(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));

        return await RunWithPolicyAsync(async () =>
        {
            var value = await _cache.StringGetAsync(key);

            if (!value.HasValue)
            {
                return string.Empty;
            }
            
            return Encoding.UTF8.GetString(value);
        });
    }

    public async Task<bool> ReplaceAsync(string key, object value)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));

        return await RunWithPolicyAsync(async () =>
        {
            if (await ExistsAsync(key) && !await DeleteAsync(key))
            {
                return false;
            }

            return await SetAsync(key, value);
        });
    }

    #endregion
    
    #region Private methods

    private async Task<T> RunWithPolicyAsync<T>(Func<Task<T>> action)
    {
        if (_retryPolicy is null)
            return await action();

        return await _retryPolicy
            .ExecuteAsync(async () => await action())
            .ConfigureAwait(false);
    }

    private async Task RunWithPolicyAsync(Func<Task> action)
    {
        if (_retryPolicy is null)
        {
            await action();
            return;
        }

        await _retryPolicy
            .ExecuteAsync(async () =>
            {
                await action();
            }).ConfigureAwait(false);
    }

    private string GetKeyForRedis(string key) => $"{_instance}{key}";
    
    #endregion
}
