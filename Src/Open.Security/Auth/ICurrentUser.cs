using Microsoft.AspNetCore.Http;

namespace Open.Security.Auth;

public interface ICurrentUser
{
    string TokenId { get; }

    ExecutionContext Context { get; }
}


public class ExecutionContext
{
    public string? AccessToken { get; set; } = string.Empty;
    public Guid OwnerId { get; set; } = Guid.Empty;
    public string? Username { get; set; } = string.Empty;
    public string? Permission { get; set; } = string.Empty;
    public HttpContext? HttpContext { get; set; }
}

