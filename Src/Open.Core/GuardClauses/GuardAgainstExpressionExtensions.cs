using System.Runtime.CompilerServices;

namespace Open.Core.GuardClauses;

public static partial class GuardAgainstZeroExtensions
{
    public static T Expression<T>(this IGuardClause guardClause,
        Func<T, bool> func,
        T input,
        string message,
        [CallerArgumentExpression("input")] string? parameterName = null,
        Func<Exception>? exceptionCreator = null)
        where T : struct
    {
        if (func(input))
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message, parameterName!);
        }

        return input;
    }
    
    public static async Task<T> ExpressionAsync<T>(this IGuardClause guardClause,
        Func<T, Task<bool>> func,
        T input,
        string message,
        [CallerArgumentExpression("input")] string? parameterName = null,
        Func<Exception>? exceptionCreator = null)
        where T : struct
    {
        if (await func(input))
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message, parameterName!);
        }

        return input;
    }
}
