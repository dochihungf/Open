namespace Open.Identity.Domain.Entities;

public class RolePermission : EntityAuditBase
{
    #region Relationships

    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual Role Role { get; set; }
    public virtual Permission Permission { get; set; }
    
    #endregion
}
