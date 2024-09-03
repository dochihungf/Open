using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Open.Constants;
using Open.Core.Exceptions;
using Open.Security.Auth;
using Open.Security.Constants;
using Open.SharedKernel.Caching.Sequence;

namespace Open.SharedKernel.Filters;

public class RequiredSecretKeyAttribute : ActionFilterAttribute
{
    private readonly string _keyName;

    public RequiredSecretKeyAttribute(string keyName)
    {
        _keyName = keyName;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var code = context.HttpContext.Request.Headers[HeaderNames.SecretKey].ToString();
        if (string.IsNullOrEmpty(code))
        {
            throw new ForbiddenException();
        }

        var currentUser = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
        var sequenceCaching = context.HttpContext.RequestServices.GetRequiredService<ISequenceCaching>();
        var key = BaseCacheKeys.GetSecretKey(_keyName, currentUser.Context.OwnerId);
        var value = await sequenceCaching.GetStringAsync(key);

        if (!code.Equals(value))
        {
            throw new ForbiddenException();
        }
        await next();
    }
}
