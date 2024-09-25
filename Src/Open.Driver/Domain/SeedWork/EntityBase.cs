using Open.Core.SeedWork;
using Open.Core.SeedWork.Interfaces;

namespace Open.Driver.Domain.SeedWork;

public class EntityBase : DomainEntity, IAuditable
{
    public Guid CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    
    public void Delete() => IsDeleted = true;
    
}
