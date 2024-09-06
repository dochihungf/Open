namespace Open.Identity.Infrastructure.Options;

public class IdentityStoreOptions
{
    /// <summary>
    /// Callback to configure the EF DbContext.
    /// </summary>
    /// <value>
    /// The configure database context.
    /// </value>
    public Action<DbContextOptionsBuilder>? IdentityDbContext { get; set; }
    
    /// <summary>
    /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
    /// </summary>
    /// <value>
    /// The configure database context.
    /// </value>
    public Action<IServiceProvider, DbContextOptionsBuilder>? ResolveDbContextOptions { get; set; }
    
    /// <summary>
    /// Gets or sets the default schema.
    /// </summary>
    /// <value>
    /// The default schema.
    /// </value>
    public string DefaultSchema { get; set; } = null;
    
    public TableConfiguration User { get; set; } = new TableConfiguration("user");

    public TableConfiguration UserConfig { get; set; } = new TableConfiguration("use_config");
    
    public TableConfiguration Role { get; set; } = new TableConfiguration("role");
    
    public TableConfiguration UserRole { get; set; } = new TableConfiguration("user_role");
    
    public TableConfiguration Permission { get; set; } = new TableConfiguration("permission");
    
    public TableConfiguration RolePermission { get; set; } = new TableConfiguration("role_permission");
    
    public TableConfiguration UserPermission { get; set; } = new TableConfiguration("user_permission");
    
    public TableConfiguration SecretKey { get; set; } = new TableConfiguration("secret_key");
    
    public TableConfiguration RefreshToken { get; set; } = new TableConfiguration("refresh_token");
    
    public TableConfiguration MFA { get; set; } = new TableConfiguration("mfa");

    public TableConfiguration OTP { get; set; } = new TableConfiguration("otp");
    
    public TableConfiguration SignInHistory { get; set; } = new TableConfiguration("sign_in_history");

}
