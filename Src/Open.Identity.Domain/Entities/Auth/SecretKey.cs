namespace Open.Identity.Domain.Entities;

public class SecretKey : PersonalizedEntityAuditBase
{
    public string Key { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
