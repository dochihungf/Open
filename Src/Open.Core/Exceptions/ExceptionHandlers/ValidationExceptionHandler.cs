using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Open.Core.Results;

namespace Open.Core.Exceptions.ExceptionHandlers;

public sealed class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }
        
        logger.LogError(validationException, "[{Handler}] Exception occurred: {Message}",
            nameof(ValidationExceptionHandler), validationException.Message);

        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Type = validationException.GetType().Name,
            Title = "Validation failed",
            Detail = "One or more validation errors has occurred"
        };

        if (validationException.Errors != null && validationException.Errors.Any())
        {
            problemDetails.Extensions["errors"] = Result.Invalid(validationException
                .Errors
                .Select(e => new ValidationError(
                    e.Key,
                    e.Value,
                    StatusCodes.Status400BadRequest.ToString(),
                    ValidationSeverity.Info
                )).ToList());
        }
        
        await TypedResults.BadRequest(problemDetails).ExecuteAsync(httpContext);

        return true;
    }
    
}
