using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Open.Core.Exceptions.ExceptionHandlers;

public class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException badRequestException)
        {
            return false;
        }
        
        logger.LogWarning(badRequestException, "Bad request exception occurred: {Message}", badRequestException.Message);
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = badRequestException.GetType().Name,
            Title = "Bad Request",
            Detail = badRequestException.Message
        };
        
        await TypedResults.NotFound(problemDetails).ExecuteAsync(httpContext);

        return true;
    }
}
