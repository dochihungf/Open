namespace Open.Identity.Domain.Entities;

public class RefreshToken : PersonalizedEntityAuditBase
{
    public string Token { get; set; }
    public string RefreshTokenValue { get; set; }
    public string CurrentAccessToken { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
    public string CreatedByIp { get; set; }
    
    public DateTime? RevokedDate { get; set; }
    
    public string? RevokedByIp { get; set; }
    
    public string? ReplacedByToken { get; set; }
    
    public string? ReasonRevoked { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
