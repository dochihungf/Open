using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Open.Core.Exceptions.ExceptionHandlers;

public class CatchableExceptionHandler(ILogger<CatchableExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ArgumentException argumentException)
        {
            logger.LogWarning(argumentException, "Argument exception occurred: {Message}", argumentException.Message);
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = argumentException.GetType().Name,
                Title = "Invalid Argument",
                Detail = argumentException.Message
            };
            
            await TypedResults.Problem(problemDetails).ExecuteAsync(httpContext);
            
            return true;
        }
        else if (exception is ArgumentNullException argumentNullException)
        {
            logger.LogWarning(argumentNullException, "Argument null exception occurred: {Message}", argumentNullException.Message);
           
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = argumentNullException.GetType().Name,
                Title = "Missing Argument",
                Detail = argumentNullException.Message
            };
            await TypedResults.Problem(problemDetails).ExecuteAsync(httpContext);
            
            return true;
        }
        
        
        return false;
    }
}
