using Microsoft.AspNetCore.Http;

namespace Open.Security.Auth;

public interface ICurrentUser
{
    string TokenId { get; }

    ExecutionContext Context { get; }
}


public class ExecutionContext
{
    public string? AccessToken { get; set; }
    public Guid OwnerId { get; set; }
    public string? Username { get; set; }
    public string? Permission { get; set; }
    public HttpContext? HttpContext { get; set; }
}

