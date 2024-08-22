namespace Open.Core.SeedWork.Interfaces;

public interface IEntityBase<TKey> : ICoreEntity
{
    TKey Id { get; }
}

public interface IEntityBase : IEntityBase<Guid>
{
    
}
