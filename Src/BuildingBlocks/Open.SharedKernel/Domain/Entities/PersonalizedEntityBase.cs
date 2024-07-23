using Open.SharedKernel.Domain.Entities.Interfaces;

namespace Open.SharedKernel.Domain.Entities;

public abstract class PersonalizedEntityBase : EntityBase, IPersonalizeEntity
{
    public Guid OwnerId { get; set; }
}
