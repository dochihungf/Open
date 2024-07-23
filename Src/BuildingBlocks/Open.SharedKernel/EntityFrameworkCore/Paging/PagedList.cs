namespace Open.SharedKernel.EntityFrameworkCore.Paging;

public class PagedList<T> : IPagedList<T>
{
    public PagedList(IEnumerable<T> source, int index, int size, int from)
    {
        if (from > index)
            throw new ArgumentException($"indexFrom: {from} > pageIndex: {index}, must indexFrom <= pageIndex");

        Index = index;
        Size = size;
        From = from;
        Pages = (int)Math.Ceiling(Count / (double)Size);

        if (source is IQueryable<T> queryable)
        {
            Count = queryable.Count();
            Items = queryable.Skip((Index - From) * Size).Take(Size).ToList();
        }
        else
        {
            T[] enumerable = source as T[] ?? source.ToArray();
            Count = enumerable.Count();
            Items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();
        }
    }

    public PagedList()
    {
        Items = Array.Empty<T>();
    }

    public int From { get; set; }
    public int Index { get; set; }
    public int Size { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public IList<T> Items { get; set; }
    public bool HasPrevious => Index - From > 0;
    public bool HasNext => Index - From + 1 < Pages;
}

public class PagedList<TSource, TResult> : IPagedList<TResult>
{
    public PagedList(
        IEnumerable<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter,
        int index,
        int size,
        int from
    )
    {
        if (from > index)
            throw new ArgumentException($"From: {from} > Index: {index}, must From <= Index");

        Index = index;
        Size = size;
        From = from;
        Pages = (int)Math.Ceiling(Count / (double)Size);

        if (source is IQueryable<TSource> queryable)
        {
            Count = queryable.Count();
            TSource[] items = queryable.Skip((Index - From) * Size).Take(Size).ToArray();
            Items = new List<TResult>(converter(items));
        }
        else
        {
            TSource[] enumerable = source as TSource[] ?? source.ToArray();
            Count = enumerable.Count();
            TSource[] items = enumerable.Skip((Index - From) * Size).Take(Size).ToArray();
            Items = new List<TResult>(converter(items));
        }
    }

    public PagedList(IPagedList<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        Index = source.Index;
        Size = source.Size;
        From = source.From;
        Count = source.Count;
        Pages = source.Pages;

        Items = new List<TResult>(converter(source.Items));
    }

    public int Index { get; }

    public int Size { get; }

    public int Count { get; }

    public int Pages { get; }

    public int From { get; }

    public IList<TResult> Items { get; }

    public bool HasPrevious => Index - From > 0;

    public bool HasNext => Index - From + 1 < Pages;
}

public static class PagedList
{
    public static IPagedList<T> Empty<T>()
    {
        return new PagedList<T>();
    }

    public static IPagedList<TResult> From<TResult, TSource>(
        IPagedList<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter
    )
    {
        return new PagedList<TSource, TResult>(source, converter);
    }
}
