using Open.Core.SeedWork.Interfaces;

namespace Open.Core.SeedWork;

public abstract class PersonalizedEntityAuditBase : EntityAuditBase, IPersonalizeEntity
{
    public Guid OwnerId { get; set; }
}
