using Open.Identity.Domain.Enums;

namespace Open.Identity.Domain.Entities;

[Table(TableName.OTP)]
public class OTP : PersonalizedEntityAuditBase
{
    public string Code { get; set; }

    public bool IsUsed { get; set; }

    public DateTime ExpiredDate { get; set; }

    
    public DateTime ProvidedDate { get; set; }
    
    public OTPType Type { get; set; } = OTPType.None;
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
