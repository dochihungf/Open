namespace Open.Identity.Domain.Constants;

public class TableName
{
    // User
    public const string User = "user";
    public const string UserConfig = "user_config";
    public const string Avatar = "avatar";
    
    // Auth
    public const string Role = "role";
    public const string UserRole = "user_role";
    
    public const string Permission = "permission";
    public const string RolePermission = "role_permission";
    public const string UserPermission = "user_permission";
    
    public const string RefreshToken = "auth_refresh_token";
    
    public const string SignInHistory = "auth_sign_in_history";
    
    public const string SecretKey = "auth_secret_key";
    
    public const string MFA = "auth_mfa";
    public const string OTP = "auth_otp";
}
