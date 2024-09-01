using Polly;
using Polly.Retry;

namespace Open.SharedKernel.Caching.Extensions;

public static class PollyExtensions
{
    private static readonly Func<int, TimeSpan> RetryAttemptWaitProvider 
        = retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

    public static AsyncRetryPolicy CreateDefaultPolicy(Action<PolicyBuilder> config)
    {
        var builder = Policy.Handle<TimeoutException>();
        config?.Invoke(builder);
        return builder.WaitAndRetryAsync(3, RetryAttemptWaitProvider);
    }
}
