using System.Text.Json.Serialization;
using Open.Core.SeedWork.Interfaces;

namespace Open.Core.SeedWork;

public abstract class EntityAuditBase<TKey> : EntityBase<TKey>, IEntityAuditBase<TKey>
{
    [JsonIgnore]
    public bool IsDeleted { get; set; }
        
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public DateTime? DeletedDate { get; set; }

    public Guid? DeletedBy { get; set; }
}

/// <summary>
/// By default, TKey is long
/// </summary>
public class EntityAuditBase : EntityAuditBase<Guid>, IEntityAuditBase
{
}
