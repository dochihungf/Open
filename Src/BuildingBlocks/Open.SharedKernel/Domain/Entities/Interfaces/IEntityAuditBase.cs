namespace Open.SharedKernel.Domain.Entities.Interfaces;

public interface IEntityAuditBase<TKey> :  IAuditable, IEntityBase<TKey>
{

}

public interface IEntityAuditBase : IEntityAuditBase<Guid>
{
    
}