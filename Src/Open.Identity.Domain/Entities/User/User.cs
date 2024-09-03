using Open.Identity.Domain.Constants;
using Open.Identity.Domain.Enums;
using Open.Security.Enums;

namespace Open.Identity.Domain.Entities;

[Table(TableName.User)]
public class User : EntityAuditBase
{
    public string Username { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string Salt { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public bool ConfirmedPhone { get; set; }
    
    public string Email { get; set; }
    
    public bool ConfirmedEmail { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public GenderType Gender { get; set; }
    
    public AuthenticatorType AuthenticatorType { get; set; }

    #region Navigations

    public virtual UserConfig UserConfig { get; set; }
    public virtual SecretKey SecretKey { get; set; }
    public virtual OTP Otp { get; set; }
    public virtual MFA Mfa { get; set; }

    #endregion
    
}
