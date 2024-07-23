namespace Open.SharedKernel.Domain.Entities.Interfaces;

public interface IEntityBase<T> : ICoreEntity
{
    T Id { get; set; }
}

public interface IEntityBase : IEntityBase<Guid>
{
    
}