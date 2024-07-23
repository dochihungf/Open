using Open.SharedKernel.Domain.Entities.Interfaces;

namespace Open.SharedKernel.Domain.Entities;

public abstract class PersonalizedEntityAuditBase : EntityAuditBase, IPersonalizeEntity
{
    public Guid OwnerId { get; set; }
}
