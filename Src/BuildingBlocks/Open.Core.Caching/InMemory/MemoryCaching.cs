using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Open.Core.Caching.InMemory;

public class MemoryCaching(IMemoryCache caching) : IMemoryCaching
{
    public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromHours(2);
    
    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        return await Task.FromResult(caching.TryGetValue(key, out _));
    }

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
        
        await SetStringAsync(key, JsonConvert.SerializeObject(value), absoluteExpireTime, keepTtl);
        
        return await ExistsAsync(key);
    }

    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        if (!expiry.HasValue)
        {
            caching.Set(key, value, DefaultAbsoluteExpireTime ?? TimeSpan.FromMinutes(5));
        }
        else
        {
            caching.Set(key, value, expiry.Value);
        }

        return await ExistsAsync(key);
        
    }

    public async Task<bool> DeleteAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        caching.Remove(key);

        return !await ExistsAsync(key);
    }

    public async Task<bool> DeleteByPatternAsync(string pattern)
    {
        var allKey = GetAllCacheKeys();
        var removeKeys = allKey.Where(x => x.StartsWith(pattern)).ToList();

        if (removeKeys.Any())
        {
            foreach (var key in removeKeys)
            {
                await DeleteAsync(key);
            }
        }

        return true;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var resultRaw = await GetStringAsync(key);
        if (string.IsNullOrWhiteSpace(resultRaw))
        {
            return default;
        }
        
        return JsonConvert.DeserializeObject<T>(resultRaw);
    }

    public async Task<string?> GetStringAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (caching.Get(key) is not string result)
        {
            return string.Empty;
        }

        return await Task.FromResult(result);
    }

    public async Task<bool> ReplaceAsync(string key, object value)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var expression = await ExistsAsync(key) || !await DeleteAsync(key);
        if (expression)
        {
            return false;
        }
        
        return await SetAsync(key, value);
    }

    private List<string> GetAllCacheKeys()
    {
        var keys = new List<string>();
        return keys;
    }
}
