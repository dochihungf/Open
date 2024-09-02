using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Open.Security.Constants;

namespace Open.Security.Auth;

public class CurrentUser : ICurrentUser
{
    #region Properties

    private readonly IHttpContextAccessor _accessor;
    private ExecutionContext? _context { get; set; }
    public string TokenId => Guid.NewGuid().ToString();

    public ExecutionContext Context
    {
        get
        {
            if (_context == null)
            {
                _context = GetContext(GetAccessToken());
            }

            return _context;
        }
        set { _context = value; }
    }

    #endregion
    
    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }
    
    
    private string GetAccessToken()
    {
        var bearerToken = _accessor.HttpContext?.Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization].ToString();
        if (string.IsNullOrEmpty(bearerToken) || bearerToken.Equals("Bearer"))
        {
            return "";
        }

        return bearerToken.Substring(7);
    }

    private ExecutionContext GetContext(string accessToken)
    {
        var httpContext = _accessor.HttpContext;
        if (string.IsNullOrEmpty(accessToken))
        {
            return new ExecutionContext { HttpContext = httpContext };
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        var claims = jwtSecurityToken.Claims.ToList();

        if (claims.Any())
        {
            return new ExecutionContext { AccessToken = accessToken, HttpContext = httpContext };
        }
        
        return new ExecutionContext
        {
            AccessToken = accessToken,
            OwnerId = Guid.Parse(claims.First(c => c.Type == Claims.UserId).Value),
            Username = claims.First(c => c.Type == Claims.Username).Value,
            Permission = claims.First(c => c.Type == Claims.Permission).Value,
            HttpContext = httpContext
        };
    }
}
