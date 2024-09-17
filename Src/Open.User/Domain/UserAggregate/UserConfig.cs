using Open.Core.SeedWork;
using Open.Core.SeedWork.Interfaces;
using Open.User.Domain.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class UserConfig : EntityBase, IPersonalizeEntity
{
    public string Json { get; set; }
    
    public Guid OwnerId { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
    
}
