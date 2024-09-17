namespace Open.Core.SeedWork.Interfaces;

public interface IEntity<TKey> : ICoreEntity, IDateTracking
{
    TKey Id { get; set; }
}

public interface IEntity : IEntity<Guid>
{
    
}
