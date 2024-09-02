using Open.Core.SeedWork.Interfaces;

namespace Open.Core.SeedWork;

public class EntityBase<TKey> : CoreEntity, IEntityBase<TKey>
{
    [System.ComponentModel.DataAnnotations.Key]
    public TKey Id { get; set; }
    
}

public class EntityBase : EntityBase<Guid>, IEntityBase
{
    
}
