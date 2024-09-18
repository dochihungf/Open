using Open.Core.SeedWork.Interfaces;

namespace Open.Identity.Domain.SeedWork;

public class EntityAuditable : Entity, IUserTracking, ISoftDelete
{
    public Guid CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
