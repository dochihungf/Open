using Open.SharedKernel.Caching.Enums;

namespace Open.SharedKernel.Caching.Sequence;

public interface ISequenceCaching
{
    public TimeSpan DefaultAbsoluteExpireTime { get; }
    
    Task<object?> GetAsync(string key, CachingType type = CachingType.Couple);

    Task<T?> GetAsync<T>(string key, CachingType type = CachingType.Couple);

    Task<string?> GetStringAsync(string key, CachingType type = CachingType.Couple);

    Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null,  bool keepTtl = false, CachingType onlyUseType = CachingType.Couple);

    Task DeleteAsync(string key, CachingType type = CachingType.Couple);

    Task DeleteByPatternAsync(string pattern, CachingType type = CachingType.Couple);
}
