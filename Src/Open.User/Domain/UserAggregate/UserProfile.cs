using Open.Core.SeedWork;
using Open.Core.SeedWork.Interfaces;
using Open.User.Domain.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class UserProfile : EntityBase, IPersonalizeEntity
{
    public Guid OwnerId { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion

}
