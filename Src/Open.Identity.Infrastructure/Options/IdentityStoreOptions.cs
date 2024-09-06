namespace Open.Identity.Infrastructure.Options;

public class IdentityStoreOptions
{
    /// <summary>
    /// Callback to configure the EF DbContext.
    /// </summary>
    /// <value>
    /// The configure database context.
    /// </value>
    public Action<DbContextOptionsBuilder> IdentityDbContext { get; set; }
    
    /// <summary>
    /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
    /// </summary>
    /// <value>
    /// The configure database context.
    /// </value>
    public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }
    
    /// <summary>
    /// Gets or sets the default schema.
    /// </summary>
    /// <value>
    /// The default schema.
    /// </value>
    public string DefaultSchema { get; set; } = null;
    
    public TableConfiguration User { get; set; } = new TableConfiguration("Users");

    public TableConfiguration UserConfig { get; set; } = new TableConfiguration("UserConfigs");
    
    public TableConfiguration Role { get; set; } = new TableConfiguration("Roles");
    
    public TableConfiguration UserRole { get; set; } = new TableConfiguration("UserRoles");
    
    public TableConfiguration Permission { get; set; } = new TableConfiguration("Permissions");
    
    public TableConfiguration RolePermission { get; set; } = new TableConfiguration("RolePermissions");
    
    public TableConfiguration UserPermission { get; set; } = new TableConfiguration("UserPermissions");
    
    public TableConfiguration SecretKey { get; set; } = new TableConfiguration("SecretKeys");
    
    public TableConfiguration RefreshToken { get; set; } = new TableConfiguration("RefreshTokens");
    
    public TableConfiguration MFA { get; set; } = new TableConfiguration("MFA");

    public TableConfiguration OTP { get; set; } = new TableConfiguration("OTPs");

}
