using System.Reflection;
using MassTransit.Internals;
using Microsoft.Extensions.Localization;
using MySqlConnector;
using Open.Constants;
using Open.Core.Exceptions;
using Open.Core.GuardClauses;
using Open.Core.Repositories.Dapper;
using Open.Core.SeedWork;
using Open.Core.SeedWork.Interfaces;
using Open.Core.UnitOfWork;
using Open.Security.Auth;
using Open.SharedKernel.Attributes;
using Open.SharedKernel.Caching.Sequence;
using Open.SharedKernel.Extensions;
using Open.SharedKernel.Libraries.Helpers;
using Open.SharedKernel.Libraries.Security;
using Open.SharedKernel.MySQL;
using Open.SharedKernel.Properties;

namespace Open.SharedKernel.Repositories.Dapper;

public class WriteOnlyRepository<TEntity> : IWriteOnlyRepository<TEntity> where TEntity : IEntityAuditBase
{
    protected IDbConnection _connection;
    protected readonly string _tableName;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly bool _isSystemTable;
    protected readonly IStringLocalizer<Resources> _localizer;
    
    public WriteOnlyRepository(IDbConnection connection,
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching,
        IStringLocalizer<Resources> localizer) 
    {
        _connection = connection;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _tableName = ((TEntity)Activator.CreateInstance(typeof(TEntity))!).GetTableName();
        _isSystemTable = typeof(TEntity).GetProperty("OwnerId") == null;
        _localizer = localizer;
    }
    
    public IUnitOfWork UnitOfWork => _connection;
    
    public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return (await SaveAsync(new List<TEntity> { entity }, cancellationToken)).First();
    }

    public virtual async Task<List<TEntity>> SaveAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        BeforeSave(entities);
        await BulkInsertAsync(entities, cancellationToken);

        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, IList<string>? updateFields = default, CancellationToken cancellationToken = default)
    {
        var queryCmd = $"SELECT * FROM {_tableName} WHERE Id = @Id AND IsDeleted = 0";
        var currentEntity = await _connection.QuerySingleOrDefaultAsync<TEntity>(queryCmd, new { entity.Id })
                            ?? throw new BadRequestException(_localizer["repository_data_does_not_exist_or_was_deleted"].Value);
        
        var sqlCommand = $"UPDATE {_tableName} AS T SET ";
        var columnParams = new List<string>();
        var ignoreFields = new string[]
        {
            nameof(EntityAuditBase.Id),
            nameof(EntityAuditBase.CreatedDate),
            nameof(EntityAuditBase.CreatedBy),
            nameof(EntityAuditBase.IsDeleted),
            nameof(PersonalizedEntityBase.OwnerId),
        };
        
        var properties = entity.GetPropertyInfos();
        
        // Map lại giá trị cũ vào entity
        if (updateFields != null)
        {
            updateFields.Add("LastModifiedDate");
            updateFields.Add("LastModifiedBy");
            updateFields = updateFields.Distinct().ToList();
        }
        
        foreach (var prop in properties)
        {
            if (updateFields != null && updateFields.Any() && !updateFields.Contains(prop.Name))
            {
                entity[prop.Name] = currentEntity[prop.Name];
            }
            if (!ignoreFields.Contains(prop.Name))
            {
                columnParams.Add($"T.{prop.Name} = @{prop.Name}");
            }
        }
        
        BeforeUpdate(entity);
        
        sqlCommand += string.Join(", ", columnParams) + " WHERE T.Id = @Id AND T.IsDeleted = 0";
        if (typeof(TEntity).GetProperty("OwnerId") != null)
        {
            sqlCommand += $" AND IF(OwnerId = '{_currentUser.Context.OwnerId}', TRUE, IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE));";
        }
        else if (typeof(TEntity).HasInterface(typeof(IAuditable)))
        {
            sqlCommand += $" AND IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE);";
        }
        
        await _connection.ExecuteAsync(sqlCommand, entity, cancellationToken: cancellationToken);
        
        await ClearCacheWhenChangesAsync(new List<object> { entity.Id }, cancellationToken);
        
        return entity;

    }

    public async Task<List<TEntity>> DeleteAsync(List<string> ids, CancellationToken cancellationToken = default)
    {
        var joinedIds = string.Join(", ", ids);
        if (Secure.DetectSqlInjection(joinedIds))
        {
            throw new SqlInjectionException();
        }
        
        var sqlCommand = $"SELECT * FROM {_tableName} AS T WHERE T.Id In ( {joinedIds}) AND T.IsDeleted = 0";
        var deleteCommand = $"UPDATE {_tableName} AS T SET T.IsDeleted = 1, T.DeletedDate = @DeletedDate, T.DeletedBy = @DeletedBy WHERE T.Id IN( {joinedIds} )";
        
        if (typeof(TEntity).GetProperty("OwnerId") != null)
        {
            sqlCommand += $" AND IF(OwnerId = '{_currentUser.Context.OwnerId}', TRUE, IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE));";
            deleteCommand += $" AND IF(OwnerId = '{_currentUser.Context.OwnerId}', TRUE, IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE));";
        }
        else if (typeof(TEntity).HasInterface(typeof(IAuditable)))
        {
            sqlCommand += $" AND IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE);";
            deleteCommand += $" AND IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE);";
        }
        
        var entities = (await _connection.QueryAsync<TEntity>(sqlCommand)).ToList();

        if (!entities.Any() || entities.Count == ids.Count)
        {
            throw new BadRequestException(_localizer["repository_data_does_not_exist_or_was_deleted"].Value);
        }
        
        BeforeDelete(entities);
        
        var param = new EntityAuditBase()
        {
            DeletedDate = DateHelper.Now,
            DeletedBy = _currentUser.Context.OwnerId,
        };
        
        await _connection.ExecuteAsync(deleteCommand, param, cancellationToken: cancellationToken);
        
        await ClearCacheWhenChangesAsync(entities.Select(x => (object)x.Id).ToList(), cancellationToken);
        
        return entities;
    }

    public async ValueTask<MySqlBulkCopyResult> BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var table = new System.Data.DataTable(_tableName);
        var properties = entities[0].GetType().GetProperties();
        var ignoreAttributes = BaseAttributes.GetCommonIgnoreAttribute();
        
        // Thêm column vào datatable
        foreach (var property in properties)
        {
            // Ignore các properties có attribute không thêm vào table
            var allowProp = ignoreAttributes.Find(att => Attribute.IsDefined(property, att)) == null;
            if (allowProp)
            {
                table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }
        }
        
        // Thêm value vào từng row datatable
        foreach (var entity in entities)
        {
            var row = table.NewRow();
            var propertiesT = entity.GetPropertyInfos();
            
            foreach (var prop in propertiesT)
            {
                // Ignore các properties có attribute không thêm vào table
                var allowProp = ignoreAttributes.Find(att => Attribute.IsDefined(prop, att)) == null;
                if (allowProp)
                {
                    row[prop.Name] = prop.GetValue(entity) ?? DBNull.Value;
                }
            }
            table.Rows.Add(row);
        }
        
        var result = await _connection.WriteToServerAsync(table, entities, cancellationToken: cancellationToken);
        
        await ClearCacheWhenChangesAsync(null, cancellationToken);
        
        return result;
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


    protected virtual void BeforeSave(IEnumerable<TEntity> entities)
    {
        var batches = entities.ChunkList(1000).ToList();

        batches.ForEach(Action);

        async void Action(List<TEntity> es)
        {
            es.ForEach(entity =>
            {
                entity.Id = Guid.NewGuid();

                if (entity is IAuditable auditable)
                {
                    entity.CreatedBy = _currentUser.Context.OwnerId;
                    entity.CreatedDate = DateHelper.Now;
                    entity.LastModifiedDate = null;
                    entity.LastModifiedBy = null;
                    entity.DeletedDate = null;
                    entity.DeletedBy = null;
                    entity.IsDeleted = false;
                }

                if (entity is IPersonalizeEntity personalize)
                {
                    personalize.OwnerId = _currentUser.Context.OwnerId;
                }
            });

            if (batches.Count() > 1)
            {
                await Task.Delay(69);
            }
        }
    }
    
    protected virtual void BeforeUpdate(TEntity entity)
    {
        if (entity is IAuditable auditable)
        {
            entity.LastModifiedBy = _currentUser.Context.OwnerId;
            entity.LastModifiedDate = DateHelper.Now;
        }
    }

    protected virtual void BeforeDelete(IEnumerable<TEntity> entities)
    {   
        
    }
}
