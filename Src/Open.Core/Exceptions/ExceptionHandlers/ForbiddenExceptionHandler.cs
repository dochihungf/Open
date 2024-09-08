using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Open.Core.Exceptions.ExceptionHandlers;

public class ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException forbiddenException)
        {
            return false;
        }
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Type = forbiddenException.GetType().Name,
            Title = "Access Forbidden",
            Detail = forbiddenException.Message
        };
        
        await TypedResults.Problem(problemDetails).ExecuteAsync(httpContext);
        
        return true;
    }
}
