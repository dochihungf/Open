using Microsoft.Extensions.Localization;
using MySqlConnector;
using Open.Core.Repositories.Dapper;
using Open.Core.SeedWork.Interfaces;
using Open.Core.UnitOfWork;
using Open.Security.Auth;
using Open.SharedKernel.Caching.Sequence;
using Open.SharedKernel.Constants;
using Open.SharedKernel.MySQL;

namespace Open.SharedKernel.Repositories.Dapper;

public class WriteOnlyRepository<TEntity> : IWriteOnlyRepository<TEntity> where TEntity : IEntityBase
{
    protected IDbConnection _connection;
    protected readonly string _tableName;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly bool _isSystemTable;
    
    public WriteOnlyRepository(IDbConnection connection,
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching) 
    {
        _connection = connection;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _tableName = ((TEntity)Activator.CreateInstance(typeof(TEntity))!).GetTableName();
        _isSystemTable = typeof(TEntity).GetProperty("OwnerId") == null;
    }
    
    public IUnitOfWork UnitOfWork => _connection;
    public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TEntity>> SaveAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, IList<string>? updateFields = default, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TEntity>> DeleteAsync(List<string> ids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<MySqlBulkCopyResult> BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    protected virtual async Task ClearCacheWhenChangesAsync(List<object>? ids, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();
        var fullRecordKey = _isSystemTable ? BaseCacheKeys.GetSystemFullRecordsKey(_tableName) : BaseCacheKeys.GetFullRecordsKey(_tableName, _currentUser.Context.OwnerId);
        tasks.Add(_sequenceCaching.DeleteAsync(fullRecordKey));

        if (ids != null && ids.Any())
        {
            foreach (var id in ids)
            {
                var recordByIdKey = _isSystemTable ? BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id) : BaseCacheKeys.GetRecordByIdKey(_tableName, id, _currentUser.Context.OwnerId);
                tasks.Add(_sequenceCaching.DeleteAsync(recordByIdKey));
            }
        }
        await Task.WhenAll(tasks);
    }
}
