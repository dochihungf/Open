using Open.SharedKernel.Domain.Entities.Interfaces;

namespace Open.SharedKernel.Domain.Entities;

public class EntityBase<TKey> : CoreEntity, IEntityBase<TKey>
{
    [System.ComponentModel.DataAnnotations.Key]
    public TKey Id { get; set; }
}

public class EntityBase : EntityBase<Guid>
{
    
}
