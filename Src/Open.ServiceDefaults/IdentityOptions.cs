namespace Open.ServiceDefaults;

public class IdentityOptions
{
    public string Url { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    
    public bool ValidateIdentityOptions => !string.IsNullOrEmpty(Audience) && !string.IsNullOrEmpty(Url);
}
