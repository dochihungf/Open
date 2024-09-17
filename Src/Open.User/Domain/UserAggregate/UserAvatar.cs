using Open.Core.SeedWork;
using Open.Core.SeedWork.Interfaces;
using Open.User.Domain.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class UserAvatar : EntityBase, IPersonalizeEntity
{
    public string FileName { get; set; }
    
    public string Url { get; set; }
    
    public Guid OwnerId { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
    
}
