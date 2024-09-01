using Open.SharedKernel.Caching.Distributed;
using Open.SharedKernel.Caching.Enums;
using Open.SharedKernel.Caching.InMemory;

namespace Open.SharedKernel.Caching.Sequence;

public class SequenceCaching(IMemoryCaching memCaching, IDistributedRedisCache distributedRedisCache)
    : ISequenceCaching
{
    #region Declares + Constructor
    
    private readonly IMemoryCaching _memCaching = memCaching;
    private readonly IDistributedRedisCache _distributedRedisCache = distributedRedisCache;
    
    public TimeSpan DefaultAbsoluteExpireTime => TimeSpan.FromHours(24);
    
    #endregion
    
    public async Task<object?> GetAsync(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await _memCaching.GetAsync<object>(key);
                if (result is null)
                {
                    result = await _distributedRedisCache.GetAsync<object>(key);
                    if (result is not null)
                    {
                        await _memCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await _memCaching.GetAsync<object>(key);
            case CachingType.Redis:
                return await _distributedRedisCache.GetAsync<object>(key);
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task<T?> GetAsync<T>(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await _memCaching.GetAsync<T>(key);
                if (result is null)
                {
                    result = await _distributedRedisCache.GetAsync<T>(key);
                    if (result is not null)
                    {
                        await _memCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await _memCaching.GetAsync<T>(key);
            case CachingType.Redis:
                return await _distributedRedisCache.GetAsync<T>(key);
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task<string?> GetStringAsync(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await _memCaching.GetStringAsync(key);
                if (string.IsNullOrEmpty(result))
                {
                    result = await _distributedRedisCache.GetStringAsync(key);
                    if (!string.IsNullOrEmpty(result))
                    {
                        await _memCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await _memCaching.GetStringAsync(key);
            case CachingType.Redis:
                return await _distributedRedisCache.GetStringAsync(key);
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null, bool keepTtl = false,
        CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                var a = await _memCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                await _distributedRedisCache.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
            case CachingType.Memory:
                await _memCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
            case CachingType.Redis:
                await _distributedRedisCache.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task DeleteAsync(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                await _memCaching.DeleteAsync(key);
                await _distributedRedisCache.DeleteAsync(key);
                return;
            case CachingType.Memory:
                await _memCaching.DeleteAsync(key);
                return;
            case CachingType.Redis:
                await _distributedRedisCache.DeleteAsync(key);
                return;
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }

    public async Task DeleteByPatternAsync(string pattern, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                await _memCaching.DeleteByPatternAsync(pattern);
                await _distributedRedisCache.DeleteByPatternAsync(pattern);
                return;
            case CachingType.Memory:
                await _memCaching.DeleteByPatternAsync(pattern);
                return;
            case CachingType.Redis:
                await _distributedRedisCache.DeleteByPatternAsync(pattern);
                return;
        }
        throw new Exception("The caching type is invalid. Please re-check!!!");
    }
}
