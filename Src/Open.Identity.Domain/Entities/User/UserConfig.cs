namespace Open.Identity.Domain.Entities;

[Table(TableName.UserConfig)]
public class UserConfig : PersonalizedEntityBase
{
    public string Json { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}
