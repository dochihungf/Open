using Open.Core.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class UserConfig : PersonalizedEntityAuditBase
{
    public string Json { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
