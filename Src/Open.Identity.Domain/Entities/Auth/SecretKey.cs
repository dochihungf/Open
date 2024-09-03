namespace Open.Identity.Domain.Entities;

[Table(TableName.SecretKey)]
public class SecretKey : PersonalizedEntityAuditBase
{
    public string Key { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
