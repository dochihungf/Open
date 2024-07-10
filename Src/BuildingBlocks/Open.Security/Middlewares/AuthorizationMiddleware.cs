using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Open.Security.Attributes;
using Open.Security.Auth;
using Open.Security.Exceptions;
using Open.Security.Services;

namespace Open.Security.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthService _authService;
    private readonly ICurrentUser _currentUser;
    
    public AuthorizationMiddleware(
        RequestDelegate next,
        IAuthService authService, 
        ICurrentUser currentUser)
    {
        _next = next;
        _authService = authService;
        _currentUser = currentUser;
    }
    
    
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint == null)
        {
            await _next(context);
        }
        
        // Lấy thông tin về controller và action từ endpoint.
        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (controllerActionDescriptor != null)
        {
            // Lấy attribute AuthorizationRequestAttribute của controller.
            var controllerAttribute = GetAuthorizationAttribute(controllerActionDescriptor.ControllerTypeInfo);
            if (controllerAttribute != null)
            {
                var allowAnonymous = controllerAttribute.Exponents.Contains(ActionExponent.AllowAnonymous);
                if (!allowAnonymous)
                {
                    if(_currentUser.Context.Permission == null) throw new ForbiddenException(); 
                    
                    var hasPermission = _authService.CheckPermission(controllerAttribute.Exponents, _currentUser.Context.Permission);
                    if (!hasPermission)
                    {
                        throw new ForbiddenException();
                    }
                }
                
            }
            
            // Lấy attribute AuthorizationRequestAttribute của action.
            var actionAttribute = GetAuthorizationAttribute(controllerActionDescriptor.MethodInfo);
            if (actionAttribute != null)
            {
                var allowAnonymous = actionAttribute.Exponents.Contains(ActionExponent.AllowAnonymous);
                if (!allowAnonymous)
                {
                    if(_currentUser.Context.Permission == null) throw new ForbiddenException(); 
                    
                    var hasPermission = _authService.CheckPermission(actionAttribute.Exponents, _currentUser.Context.Permission);
                    if (!hasPermission)
                    {
                        throw new ForbiddenException();
                    }
                }
                
            }
        }
    }
    
    private AuthorizationAttribute? GetAuthorizationAttribute(MemberInfo memberInfo)
    {
        // inherit một cờ (flag) xác định xem liệu phương thức này có nên tìm kiếm các attribute kế thừa từ các lớp cha hay không? (inherit: false là không)
        return (AuthorizationAttribute)memberInfo.GetCustomAttributes(typeof(AuthorizationAttribute), inherit: false).FirstOrDefault()!;
    }
}

public static class AuthorizationMiddlewareExtension
{
    public static IApplicationBuilder UseCoreAuthorization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Microsoft.AspNetCore.Authorization.AuthorizationMiddleware>();
    }
}