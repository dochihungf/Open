namespace Open.Identity.Domain.Entities;

public class MFA : EntityAuditable, IPersonalizeEntity
{
    public MFAType Type { get; set; } = MFAType.None;

    public bool Enabled { get; set; }
    
    public Guid OwnerId { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}

