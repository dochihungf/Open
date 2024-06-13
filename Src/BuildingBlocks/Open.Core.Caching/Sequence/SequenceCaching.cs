using Open.Core.Caching.Distributed;
using Open.Core.Caching.Enums;
using Open.Core.Caching.InMemory;

namespace Open.Core.Caching.Sequence;

public class SequenceCaching(IMemoryCaching memoryCaching, IDistributedRedisCache distributedRedisCache) : ISequenceCaching
{
    public TimeSpan DefaultAbsoluteExpireTime => TimeSpan.FromHours(24);
    
    public async Task<object?> GetAsync(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await memoryCaching.GetAsync<object>(key);
                if (result is null)
                {
                    result = await distributedRedisCache.GetAsync<object>(key);
                    if (result is not null)
                    {
                        await memoryCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await memoryCaching.GetAsync<object>(key);
            case CachingType.Redis:
                return await distributedRedisCache.GetAsync<object>(key);
        }
        throw new Exception("The caching type is invalid.");
    }

    public async Task<T?> GetAsync<T>(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await memoryCaching.GetAsync<T>(key);
                if (result is null)
                {
                    result = await distributedRedisCache.GetAsync<T>(key);
                    if (result is not null)
                    {
                        await memoryCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await memoryCaching.GetAsync<T>(key);
            case CachingType.Redis:
                return await distributedRedisCache.GetAsync<T>(key);
        }
        throw new Exception("The caching type is invalid.");
    }

    public async Task<string?> GetStringAsync(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                var result = await memoryCaching.GetStringAsync(key);
                if (string.IsNullOrEmpty(result))
                {
                    result = await distributedRedisCache.GetStringAsync(key);
                    if (!string.IsNullOrEmpty(result))
                    {
                        await memoryCaching.SetAsync(key, result, DefaultAbsoluteExpireTime);
                    }
                }
                return result;
            case CachingType.Memory:
                return await memoryCaching.GetStringAsync(key);
            case CachingType.Redis:
                return await distributedRedisCache.GetStringAsync(key);
        }
        throw new Exception("The caching type is invalid.");
    }

    public async Task SetAsync(string key, 
        object value, 
        TimeSpan? absoluteExpireTime = null, 
        bool keepTtl = false,
        CachingType onlyUseType = CachingType.Couple)
    {
        switch (onlyUseType)
        {
            case CachingType.Couple:
                var a = await memoryCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                await distributedRedisCache.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
            case CachingType.Memory:
                await memoryCaching.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
            case CachingType.Redis:
                await distributedRedisCache.SetAsync(key, value, absoluteExpireTime, keepTtl);
                return;
        }
        throw new Exception("The caching type is invalid.");
    }

    public async Task DeleteAsync(string key, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                await memoryCaching.DeleteAsync(key);
                await distributedRedisCache.DeleteAsync(key);
                return;
            case CachingType.Memory:
                await memoryCaching.DeleteAsync(key);
                return;
            case CachingType.Redis:
                await distributedRedisCache.DeleteAsync(key);
                return;
        }
        throw new Exception("The caching type is invalid.");
    }

    public async Task DeleteByPatternAsync(string pattern, CachingType type = CachingType.Couple)
    {
        switch (type)
        {
            case CachingType.Couple:
                await memoryCaching.DeleteByPatternAsync(pattern);
                await distributedRedisCache.DeleteByPatternAsync(pattern);
                return;
            case CachingType.Memory:
                await memoryCaching.DeleteByPatternAsync(pattern);
                return;
            case CachingType.Redis:
                await distributedRedisCache.DeleteByPatternAsync(pattern);
                return;
        }
        throw new Exception("The caching type is invalid.");
    }
}