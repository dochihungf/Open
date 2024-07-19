namespace ConsoleApp.Units;

public class Cache(TimeSpan cacheTime)
{
    public record Item(string Url, string Content, DateTime LastCollected);

    private readonly Dictionary<string, Item> _cache = new();

    public bool Contains(string url)
    {
        if (_cache.TryGetValue(url, out var item))
        {
            return DateTime.UtcNow.Subtract(item.LastCollected) < cacheTime;
        }

        return false;
    }

    public void Add(Item item) => _cache.Add(item.Url, item);
}