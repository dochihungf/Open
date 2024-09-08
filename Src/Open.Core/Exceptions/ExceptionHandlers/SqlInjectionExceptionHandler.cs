using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Open.Core.Exceptions.ExceptionHandlers;

public class SqlInjectionExceptionHandler(ILogger<SqlInjectionExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not SqlInjectionException sqlInjectionException)
        {
            return false;
        }
        
        logger.LogError(sqlInjectionException, "SQL Injection Exception occurred: {Message}", sqlInjectionException.Message);
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = sqlInjectionException.GetType().Name,
            Title = "Potential SQL Injection Attack Detected",
            Detail = sqlInjectionException.Message
        };
        
        await TypedResults.Problem(problemDetails).ExecuteAsync(httpContext);

        return true;
    }
}
