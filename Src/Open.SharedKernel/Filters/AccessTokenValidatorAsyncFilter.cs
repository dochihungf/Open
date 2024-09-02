using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Ocelot.Infrastructure.Extensions;
using Open.Security.Auth;
using Open.Security.Utilities;
using Open.SharedKernel.Caching.Sequence;
using Open.SharedKernel.Constants;

namespace Open.SharedKernel.Filters;

public class AccessTokenValidatorAsyncFilter : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        if (AuthUtility.EndpointRequiresAuthorize(context))
        {
            var bearerToken = context.HttpContext.Request.Headers[HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(bearerToken.GetValue()))
            {
                var token = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
                var sequenceCaching = context.HttpContext.RequestServices.GetRequiredService<ISequenceCaching>();
                var accessToken = bearerToken.GetValue()[7..];
                var key = BaseCacheKeys.GetRevokeAccessTokenKey(accessToken);
                var isRevoked = !string.IsNullOrEmpty(await sequenceCaching.GetStringAsync(key));

                // If the user is logged out
                if (isRevoked)
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        ContentType = "application/json"
                    };
                    return;
                }
            }
        }
        //else
        //{
        //    context.HttpContext.Request.Headers[HeaderNames.Authorization] = string.Empty;
        //}
        await next();
    }
}
