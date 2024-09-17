namespace Open.Identity.Domain.Entities;

public class UserPermission : EntityBase
{
    #region Relationships

    public Guid UserId { get; set; }
    public Guid PermissionId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual User User { get; set; }
    public virtual Permission Permission { get; set; }
    
    #endregion
}
