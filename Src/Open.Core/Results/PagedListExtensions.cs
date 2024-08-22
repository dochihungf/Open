using Microsoft.EntityFrameworkCore;

namespace Open.Core.Results;

public static class PagedListExtensions
{
    public static IPagedList<TEntity> ToPagedList<TEntity>(
        IEnumerable<TEntity> source, 
        int index,
        int size, 
        int from = 1) 
        where TEntity : class
    {
        if (from > index)
        {
            throw new ArgumentException($"indexFrom: {from} > pageIndex: {index}, must indexFrom <= pageIndex");
        }
        
        var totalCount = source.Count();
        var items = source.Skip((index - from) * size).Take(size).ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)size);

        return new PagedList<TEntity>(index, size, from, totalCount, totalPages, items);
    }
    
    public static async Task<IPagedList<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        int index,
        int size,
        int from = 1,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        if (from > index)
        {
            throw new ArgumentException($"indexFrom: {from} > pageIndex: {index}, must indexFrom <= pageIndex");
        }

        int totalCount = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await source.Skip((index - from) * size).Take(size).ToListAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)size);

        return new PagedList<TEntity>(index, size, from, totalCount, totalPages, items);
    }
}
