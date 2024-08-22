using Open.Core.SeedWork.Interfaces;

namespace Open.Core.SeedWork;

public abstract class PersonalizedEntityBase : EntityBase, IPersonalizeEntity
{
    public Guid OwnerId { get; set; }
}

