namespace Open.Identity.Domain.Entities;

public class Role : EntityAuditable
{
    public string Code { get; set; }

    public string Name { get; set; }
    
    #region Navigations
    
    public ICollection<UserRole> UserRoles { get; set; }
    
    public ICollection<RolePermission> RolePermissions { get; set; }
    
    #endregion

}
