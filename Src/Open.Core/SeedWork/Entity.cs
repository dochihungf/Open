using Open.Core.SeedWork.Interfaces;

namespace Open.Core.SeedWork;

public class Entity<TKey> : CoreEntity, IEntity<TKey>
{
    [System.ComponentModel.DataAnnotations.Key]
    public TKey Id { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public Guid Version { get; set; }
}

public class Entity : Entity<Guid>, IEntity
{
    
}
