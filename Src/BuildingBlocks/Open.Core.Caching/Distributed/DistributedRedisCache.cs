using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Open.Core.Caching.Settings;
using Polly.Retry;
using StackExchange.Redis;

namespace Open.Core.Caching.Distributed;

public class DistributedRedisCache : IDistributedRedisCache
{
    private readonly IDatabase _cache;
    private readonly string _instance;
    private readonly AsyncRetryPolicy? _retryPolicy;
    private readonly IServer _server;

    public DistributedRedisCache(IOptions<DistributedCacheSettings> options, AsyncRetryPolicy? asyncRetryPolicy)
    {
        var connection = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        _cache = connection.GetDatabase(options.Value.DatabaseIndex);
        _server = connection.GetServer(options.Value.ConnectionString);
        _instance = options.Value.InstanceName;
        _retryPolicy = asyncRetryPolicy;
    }
    
    public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromHours(24);
    
    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        return await RunWithPolicyAsync(async () => await _cache.KeyExistsAsync(key));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiry">Thời gian hết hạn</param>
    /// <param name="keepTtl">Một giá trị boolean chỉ ra liệu có giữ nguyên thời gian sống (TTL).</param>
    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        var absoluteExpireTime = expiry ?? DefaultAbsoluteExpireTime;
        var valueRaw = JsonConvert.SerializeObject(value);
        return await RunWithPolicyAsync(async () => await SetStringAsync(key, valueRaw, absoluteExpireTime, keepTtl));
        
    }

    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        var absoluteExpireTime = expiry ?? DefaultAbsoluteExpireTime;
        return await RunWithPolicyAsync(async () =>
            await _cache.StringSetAsync(key, Encoding.UTF8.GetBytes(value), absoluteExpireTime, keepTtl));
    }

    public async Task<bool> DeleteAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        return await RunWithPolicyAsync(async () => await _cache.KeyDeleteAsync(key));
    }

    public async Task<bool> DeleteByPatternAsync(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return false;
        }
        
        var keys = _server.Keys(pattern: pattern).ToArray();
        await RunWithPolicyAsync(async () =>
        {
            await _cache.KeyDeleteAsync(keys);
        });
        
        return true;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        return await RunWithPolicyAsync(async () =>
        {
            var value = await GetStringAsync(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }
            
            return JsonConvert.DeserializeObject<T>(value);
        });
    }

    public async Task<string?> GetStringAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        return await RunWithPolicyAsync(async () =>
        {
            var value = await _cache.StringGetAsync(key);
            
            return !value.HasValue ? string.Empty : Encoding.UTF8.GetString(value);
        });
    }

    public async Task<bool> ReplaceAsync(string key, object value)
    {
        if(string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        return await RunWithPolicyAsync(async () =>
        {
            if (await ExistsAsync(key) && !await DeleteAsync(key))
            {
                return false;
            }
            
            return await SetAsync(key, value);
        });
    }
    
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
    
    private string GetKeyForRedisCache(string key) => $"{_instance}{key}";
}