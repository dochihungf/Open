namespace Open.Identity.Domain.Entities;

public class UserConfig : PersonalizedEntityAuditBase
{
    public string Json { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
