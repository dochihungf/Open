using Open.Core.Models;
using Open.Core.Results;
using Open.Core.SeedWork;
using Open.Core.SeedWork.Interfaces;

namespace Open.Core.Repositories.Dapper;

public interface IReadOnlyRepository<TEntity> where TEntity : IEntityBase
{
    Task<CacheResult<List<TResult>>> GetAllCacheAsync<TResult>(CancellationToken cancellationToken = default);

    Task<CacheResult<TResult>> GetByIdCacheAsync<TResult>(object id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken = default);

    Task<TResult?> GetByIdAsync<TResult>(object id, CancellationToken cancellationToken = default);

    Task<IPagedList<TResult>> GetPagingAsync<TResult>(PagingRequest request, CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(CancellationToken cancellationToken = default);
}
