using System.Text.Json.Serialization;

namespace Open.Core.Results;

public class PagedList<T> : IPagedList<T>
{
    public PagedList() => Items = Array.Empty<T>();

    public PagedList(int index, int size, int from, int totalCount, int totalPages, IEnumerable<T> items)
    {
        Index = index;
        Size = size;
        From = from;
        Count = totalCount;
        Pages = totalPages;
        Items = items;
    }
    
    [JsonInclude]
    public int From { get; private set; }
    [JsonInclude]
    public int Index { get; private set; }
    [JsonInclude]
    public int Size { get; private set; }
    [JsonInclude]
    public int Count { get; private set; }
    [JsonInclude]
    public int Pages { get; private set; }
    [JsonInclude]
    public IEnumerable<T> Items { get; private set; }
    [JsonInclude]
    public bool HasPrevious => Index - From > 0;
    [JsonInclude]
    public bool HasNext => Index - From + 1 < Pages;
}
