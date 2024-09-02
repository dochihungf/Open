namespace Open.Identity.Domain.Entities;

[Table("auth_secret_key")]
public class SecretKey : PersonalizedEntityAuditBase
{
    public string Key { get; set; }
}
