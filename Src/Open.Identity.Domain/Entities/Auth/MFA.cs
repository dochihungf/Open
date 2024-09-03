using Open.Identity.Domain.Enums;

namespace Open.Identity.Domain.Entities;

[Table(TableName.MFA)]
public class MFA : PersonalizedEntityAuditBase
{
    public MFAType Type { get; set; } = MFAType.None;

    public bool Enabled { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
