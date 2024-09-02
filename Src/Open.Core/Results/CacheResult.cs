namespace Open.Core.Results;

public class CacheResult<T>(string key, T? value)
{
    public string Key { get; set; } = key;

    public T? Value { get; set; } = value;
}

public class CacheResult(string key, object? value) : CacheResult<object>(key, value);
