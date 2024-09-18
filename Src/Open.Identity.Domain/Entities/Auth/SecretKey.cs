namespace Open.Identity.Domain.Entities;

public class SecretKey : EntityAuditable, IPersonalizeEntity
{
    public string Key { get; set; }
    
    public Guid OwnerId { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion

}
