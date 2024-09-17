using Open.Core.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class UserAvatar : PersonalizedEntityAuditBase
{
    public string FileName { get; set; }
    public string Url { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
