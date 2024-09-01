using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Open.Core.GuardClauses;

namespace Open.SharedKernel.Caching.InMemory;

public class MemoryCaching(IMemoryCache cache) : IMemoryCaching
{
    #region Declares + Constructor

    private readonly IMemoryCache _cache = cache;
    
    public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromHours(2);

    #endregion
    
    #region Implements
    
    public async Task<bool> ExistsAsync(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        return await Task.FromResult(_cache.TryGetValue(key, out _));
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        await SetStringAsync(key, JsonConvert.SerializeObject(value), expiry, keepTtl);

        return await ExistsAsync(key);
    }

    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(value, nameof(value));
        
        if (!expiry.HasValue)
        {
            _cache.Set(key, value, DefaultAbsoluteExpireTime ?? TimeSpan.FromMinutes(5));
        }
        else
        {
            _cache.Set(key, value, expiry.Value);
        }

        return await ExistsAsync(key);
    }

    public async Task<bool> DeleteAsync(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        
        _cache.Remove(key);

        return !(await ExistsAsync(key));
    }

    public async Task<bool> DeleteByPatternAsync(string pattern)
    {
        var allKey = GetAllCacheKeys();
        var removeKeys = allKey.Where(x => x.StartsWith(pattern)).ToList();
        var tasks = new List<Task>();
        if (removeKeys.Any())
        {
            foreach (var key in removeKeys)
            {
                tasks.Add(DeleteAsync(key));
            }
        }
        await Task.WhenAll(tasks);

        return true;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));

        var result = await GetStringAsync(key);
        if (string.IsNullOrEmpty(result))
        {
            return default(T);
        }
        
        return JsonConvert.DeserializeObject<T>(result);
    }

    public async Task<string> GetStringAsync(string key)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        
        if (_cache.Get(key) is not string result)
        {
            return string.Empty;
        }

        return await Task.FromResult(result);
    }

    public async Task<bool> ReplaceAsync(string key, object value)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(value, nameof(value));

        if (await ExistsAsync(key) && !await DeleteAsync(key))
        {
            return false;
        }

        return await SetAsync(key, value);
    }
    
    #endregion

    #region Private methods

    private List<string> GetAllCacheKeys()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var typeOfCache = _cache.GetType();
        var fieldEntries = typeOfCache?.GetField("_entries", flags);
        var entriesValue = fieldEntries?.GetValue(_cache);
        var cacheItems = entriesValue as IDictionary;

        var keys = new List<string>();
        if (cacheItems is null || cacheItems.Count <= 0)
        {
            return keys;
        }
        
        foreach (DictionaryEntry cacheItem in cacheItems)
        {
            keys.Add(cacheItem.Key.ToString());
        }
        return keys;
    }

    #endregion
}
