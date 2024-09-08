using Open.Core.Exceptions.ExceptionHandlers;

namespace Open.Identity.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<_3pAttackExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<CatchableExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<SqlInjectionExceptionHandler>();
        services.AddExceptionHandler<ForbiddenExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}
