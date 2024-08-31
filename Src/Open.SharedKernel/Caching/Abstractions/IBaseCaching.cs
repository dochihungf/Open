namespace Open.SharedKernel.Caching.Abstractions;

public interface IBaseCaching
{
    TimeSpan? DefaultAbsoluteExpireTime { get; }
    
    Task<bool> ExistsAsync(string key);
    
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, bool keepTtl = false);
    
    Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false);
    
    Task<bool> DeleteAsync(string key);
    
    Task<bool> DeleteByPatternAsync(string pattern);
    
    Task<T?> GetAsync<T>(string key);
    
    Task<string> GetStringAsync(string key);
    
    Task<bool> ReplaceAsync(string key, object value);
}
