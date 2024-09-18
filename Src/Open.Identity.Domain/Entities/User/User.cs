using Open.Identity.Domain.Enums;
using Open.Identity.Domain.SeedWork;
using Open.Security.Enums;

namespace Open.Identity.Domain.Entities;

public class User : EntityAuditable
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
    public virtual SecretKey SecretKey { get; set; }
    public virtual MFA MFA { get; set; }
    
    public ICollection<OTP> OTPs { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<UserPermission> UserPermissions { get; set; }
    public ICollection<SignInHistory> SignInHistories { get; set; }
    
    #endregion
    
}
