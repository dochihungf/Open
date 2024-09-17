using System.Diagnostics;

namespace Open.Identity.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log request information
        var watch = Stopwatch.StartNew();
        var request = context.Request;

        // Log request details
        Console.WriteLine($@"Request: {request.Method} {request.Path} at {DateTime.UtcNow}");

        // Call the next middleware in the pipeline
        await _next(context);

        // Log response information
        watch.Stop();
        var response = context.Response;
        Console.WriteLine($@"Response: {response.StatusCode} in {watch.ElapsedMilliseconds}ms at {DateTime.UtcNow}");
    }
}
