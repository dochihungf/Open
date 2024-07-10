using Microsoft.Extensions.DependencyInjection;
using Open.Security.Auth;

namespace Open.Security;

public static class SecurityDependencyInjection
{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
}