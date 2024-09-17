namespace Open.Identity.Domain.Entities;

public class OTP : EntityBase, IPersonalizeEntity
{
    public string Code { get; set; }

    public bool IsUsed { get; set; }

    public DateTime ExpiredDate { get; set; }
    
    public DateTime ProvidedDate { get; set; }
    
    public OTPType Type { get; set; } = OTPType.None;
    
    public Guid OwnerId { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
    
}
