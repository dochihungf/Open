using MySqlConnector;
using Open.Core.SeedWork.Interfaces;
using Open.Core.UnitOfWork;

namespace Open.Core.Repositories.Dapper;

public interface IWriteOnlyRepository<TEntity> where TEntity : IEntityAuditBase
{
    IUnitOfWork UnitOfWork { get; }

    Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<List<TEntity>> SaveAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, IList<string>? updateFields = default, CancellationToken cancellationToken = default);

    Task<List<TEntity>> DeleteAsync(List<string> ids, CancellationToken cancellationToken = default);

    ValueTask<MySqlBulkCopyResult> BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
}
