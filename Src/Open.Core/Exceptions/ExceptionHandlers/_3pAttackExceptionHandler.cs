using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Open.Core.Exceptions.ExceptionHandlers;

public class _3pAttackExceptionHandler(ILogger<_3pAttackExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not _3pAttackException _3PAttackException)
        {
            return false;
        }
        
        logger.LogError(_3PAttackException, "Third-party attack detected: {Message}", _3PAttackException.Message);
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = _3PAttackException.GetType().Name,
            Title = "Third-Party Attack Detected",
            Detail = "A potential attack from a third-party service was detected. Please check the request details."
        };
        
        await TypedResults.Problem(problemDetails).ExecuteAsync(httpContext);

        return true;
    }
}
