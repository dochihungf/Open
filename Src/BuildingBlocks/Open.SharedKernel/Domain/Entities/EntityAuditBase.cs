using System.Text.Json.Serialization;
using Open.SharedKernel.Domain.Entities.Interfaces;

namespace Open.SharedKernel.Domain.Entities;

public class EntityAuditBase<TKey> : EntityBase<TKey>, IEntityAuditBase<TKey>
{
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }
    
    [JsonIgnore]
    public bool IsDeleted { get; set; }
}

/// <summary>
/// By default, TKey is Guid
/// </summary>
public class EntityAuditBase : EntityAuditBase<Guid>, IEntityAuditBase
{
}