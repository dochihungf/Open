namespace Open.Core.SeedWork.Interfaces;

public interface IEntityBase<TKey> : ICoreEntity
{
    TKey Id { get; set; }
}

public interface IEntityBase : IEntityBase<Guid>
{
    
}
