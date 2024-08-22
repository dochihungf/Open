namespace Open.Core.Results;

public interface IPagedList<T>
{
    int From { get; }
    int Index { get; }
    int Size { get; }
    int Count { get; }
    int Pages { get; }
    IEnumerable<T> Items { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }
}
