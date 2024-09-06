namespace Open.Identity.Domain.Entities;

public class UserRole : EntityAuditBase
{
    #region Relationships

    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    
    #endregion
}
