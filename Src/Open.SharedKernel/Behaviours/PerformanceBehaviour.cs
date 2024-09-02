using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Open.Security.Auth;

namespace Open.SharedKernel.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUser _user;
    
    public PerformanceBehaviour(ILogger<TRequest> logger,
        ICurrentUser user)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _user = user;
    }
    
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        
        var response = next();
        
        _timer.Stop();
        
        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.Context.OwnerId == Guid.Empty 
                ? _user.Context.OwnerId.ToString() 
                : $"Anonymous";
            var userName = _user.Context.Username ?? $"Anonymous";
            
            _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName, request);
        }

        return response;
    }
}
