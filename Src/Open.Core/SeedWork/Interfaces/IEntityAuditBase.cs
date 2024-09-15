namespace Open.Core.SeedWork.Interfaces;

public interface IEntityAuditBase<TKey> :  IAuditable, IEntityBase<TKey>
{

}

public interface IEntityAuditBase : IEntityAuditBase<Guid>, IEntityBase
{
    
}
