namespace Open.User.Domain.UserAggregate;

public class UserProfile
{
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
