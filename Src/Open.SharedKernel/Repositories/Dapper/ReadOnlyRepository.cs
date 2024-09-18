using MassTransit.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Open.Constants;
using Open.Core.Exceptions;
using Open.Core.Models;
using Open.Core.Repositories.Dapper;
using Open.Core.Results;
using Open.Core.SeedWork.Interfaces;
using Open.Security.Auth;
using Open.SharedKernel.Attributes;
using Open.SharedKernel.Caching.Sequence;
using Open.SharedKernel.Extensions;
using Open.SharedKernel.Libraries.Security;
using Open.SharedKernel.Libraries.Utilities;
using Open.SharedKernel.MySQL;
using Open.SharedKernel.Properties;

namespace Open.SharedKernel.Repositories.Dapper;

public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IEntity
{
    protected IDbConnection _connection;
    protected readonly string _tableName;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly IServiceProvider _provider;
    protected readonly bool _isSystemTable;

    public ReadOnlyRepository(IDbConnection connection,
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching,
        IServiceProvider provider)
    {
        _connection = connection;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _tableName = (((TEntity)Activator.CreateInstance(typeof(TEntity))!)!).GetTableName();
        _provider = provider;
        _isSystemTable = !typeof(TEntity).HasInterface(typeof(IPersonalizeEntity));
    }

    #region Cache

    public virtual async Task<CacheResult<List<TResult>>> GetAllCacheAsync<TResult>(
        CancellationToken cancellationToken = default)
    {
        string key = _isSystemTable
            ? BaseCacheKeys.GetSystemFullRecordsKey(_tableName)
            : BaseCacheKeys.GetFullRecordsKey(_tableName, _currentUser.Context.OwnerId);

        return new CacheResult<List<TResult>>(key, await _sequenceCaching.GetAsync<List<TResult>>(key));
    }

    public virtual async Task<CacheResult<TResult>> GetByIdCacheAsync<TResult>(object id,
        CancellationToken cancellationToken = default)
    {
        string key = _isSystemTable
            ? BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id)
            : BaseCacheKeys.GetRecordByIdKey(_tableName, _currentUser.Context.OwnerId, id);

        return new CacheResult<TResult>(key, await _sequenceCaching.GetAsync<TResult>(key));
    }

    #endregion

    public virtual async Task<IEnumerable<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken = default)
    {
        var cacheResult = await GetAllCacheAsync<TResult>(cancellationToken);
        if (cacheResult.Value != null && cacheResult.Value.Any())
        {
            return cacheResult.Value;
        }

        var cmd = $"SELECT * FROM {_tableName} as T WHERE 1=1";
        if (typeof(TEntity).HasInterface(typeof(IPersonalizeEntity)))
        {
            cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        }
        
        if (typeof(TEntity).HasInterface(typeof(ISoftDelete)))
        {
            cmd += " AND T.IsDeleted = 0";
        }
        
        cmd += " ORDER BY CASE WHEN T.LastModifiedDate > T.CreatedDate THEN T.LastModifiedDate ELSE T.CreatedDate END DESC";
        
        var result = await _connection.QueryAsync<TResult>(cmd);
        if (result.Any())
        {
            await _sequenceCaching.SetAsync(cacheResult.Key, result, TimeSpan.FromDays(7));
        }

        return result;
    }

    public virtual async Task<TResult?> GetByIdAsync<TResult>(object id, CancellationToken cancellationToken = default)
    {
        var cacheResult = await GetByIdCacheAsync<TResult>(id, cancellationToken);
        if (cacheResult.Value != null)
        {
            return cacheResult.Value;
        }

        var cmd = $"SELECT * FROM {_tableName} as T WHERE T.Id = @Id";
        if (typeof(TEntity).HasInterface(typeof(IPersonalizeEntity)))
        {
            cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        }
        
        if (typeof(TEntity).HasInterface(typeof(ISoftDelete)))
        {
            cmd += " AND T.IsDeleted = 0";
        }

        var result = await _connection.QuerySingleOrDefaultAsync<TResult>(cmd, new { Id = id });
        if (result != null)
        {
            await _sequenceCaching.SetAsync(cacheResult.Key, result, TimeSpan.FromDays(7));
        }

        return result;
    }

    public virtual async Task<IPagedList<TResult>> GetPagingAsync<TResult>(PagingRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = $"SELECT * FROM {_tableName} as T WHERE 1 = 1";
        var countCmd = $"SELECT Count(Id) FROM {_tableName} as T WHERE 1 = 1";

        if (typeof(TEntity).HasInterface(typeof(IPersonalizeEntity)))
        {
            cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
            countCmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        }
        
        if (typeof(TEntity).HasInterface(typeof(ISoftDelete)))
        {
            cmd += " AND T.IsDeleted = 0";
            countCmd += " AND T.IsDeleted = 0";
        }

        // Filter
        var param = new Dictionary<string, object>();
        if (request.Filter != null && request.Filter?.Fields != null && request.Filter.Fields.Any())
        {
            var formula = request.Filter.Formula;
            for (int i = 0; i < request.Filter.Fields.Count; i++)
            {
                var field = request.Filter.Fields[i];
                var property = typeof(TEntity).GetProperty(field.FieldName);
                if (property == null || !Attribute.IsDefined(property, typeof(FilterableAttribute)))
                {
                    var localize = _provider.GetRequiredService<IStringLocalizer<Resources>>();
                    throw new BadRequestException(localize["repository_filter_is_invalid"].Value);
                }

                var hasUnicode = field.Value.HasUnicode();
                var replaceValue =
                    $" T.{field.FieldName} {GetOperatorWithValue(field, out string paramName, hasUnicode)}";
                formula = formula.Replace("{" + i + "}", replaceValue);
                param[paramName] = field.Value;
            }

            cmd += $" AND ({formula}) ";
            countCmd += $" AND ({formula}) ";
        }

        // Order by
        var limit = $"LIMIT {request.Offset}, {request.PageSize}";
        if (request.Sorts != null && request.Sorts.Any())
        {
            var tmp = new List<string>();
            foreach (var sort in request.Sorts)
            {
                if (Secure.DetectSqlInjection(sort.FieldName))
                {
                    throw new SqlInjectionException();
                }

                tmp.Add($" T.{sort.FieldName} {(sort.SortAscending ? "ASC" : "DESC")} ");
            }

            var order = $" ORDER BY {string.Join(",", tmp)} ";
            cmd += $" {order} {limit}";
        }
        else
        {
            cmd += $" ORDER BY CASE WHEN T.LastModifiedDate > T.CreatedDate THEN T.LastModifiedDate ELSE T.CreatedDate END DESC {limit}";
        }

        var dataTask = _connection.QueryAsync<TResult>(cmd, param);
        var countTask = _connection.QuerySingleOrDefaultAsync<int>(countCmd, param);

        await Task.WhenAll(dataTask, countTask);
        return new PagedList<TResult>(request.PageIndex,
            request.PageSize,
            request.From,
            (int)Math.Ceiling(countTask.Result / (double)request.PageSize),
            countTask.Result,
            dataTask.Result);
    }

    public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken)
    {
        var cmd = $"SELECT COUNT(*) FROM {_tableName} as T WHERE 1=1";
        if (typeof(TEntity).HasInterface(typeof(IPersonalizeEntity)))
        {
            cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        }

        if (typeof(TEntity).HasInterface(typeof(ISoftDelete)))
        {
            cmd += " AND T.IsDeleted = 0";
        }

        return await _connection.QuerySingleOrDefaultAsync<long>(cmd);
    }

    private static string GetOperatorWithValue(Field field, out string paramName, bool hasUnicode = false)
    {
        paramName = $"{field.FieldName}{Utilities.RandomString(6, false)}";
        var suffix = string.Empty;

        switch (field.Condition)
        {
            case WhereType.E:
                return $"= @{paramName}";
            case WhereType.NE:
                return $"<> @{paramName}";
            case WhereType.GT:
                return $"> @{paramName}";
            case WhereType.GE:
                return $">= @{paramName}";
            case WhereType.LT:
                return $"< @{paramName}";
            case WhereType.LE:
                return $"<= @{paramName}";
            case WhereType.C:
                return $"LIKE CONCAT('%', @{paramName} {suffix}, '%')";
            case WhereType.NC:
                return $"NOT LIKE CONCAT('%', @{paramName} {suffix}, '%')";
            case WhereType.SW:
                return $"LIKE CONCAT(@{paramName} {suffix}, '%')";
            case WhereType.NSW:
                return $"NOT LIKE CONCAT(@{paramName} {suffix}, '%')";
            case WhereType.EW:
                return $"LIKE CONCAT('%', @{paramName} {suffix})";
            case WhereType.NEW:
                return $"NOT LIKE CONCAT('%', @{paramName} {suffix})";
            default:
                return string.Empty;
        }
    }
}
