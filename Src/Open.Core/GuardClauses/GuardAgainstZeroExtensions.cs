using System.Runtime.CompilerServices;

namespace Open.Core.GuardClauses;

public static partial class GuardAgainstZeroExtensions
{
    public static int Zero(this IGuardClause guardClause,
        int input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Zero<int>(guardClause, input, parameterName!, message, exceptionCreator);
    }

    public static long Zero(this IGuardClause guardClause,
        long input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Zero<long>(guardClause, input, parameterName!, message, exceptionCreator);
    }

    public static decimal Zero(this IGuardClause guardClause,
        decimal input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Zero<decimal>(guardClause, input, parameterName!, message, exceptionCreator);
    }

    public static float Zero(this IGuardClause guardClause,
        float input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Zero<float>(guardClause, input, parameterName!, message, exceptionCreator);
    }

    public static double Zero(this IGuardClause guardClause,
        double input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Zero<double>(guardClause, input, parameterName!, message, exceptionCreator);
    }

    public static TimeSpan Zero(this IGuardClause guardClause,
        TimeSpan input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Zero<TimeSpan>(guardClause, input, parameterName!, exceptionCreator: exceptionCreator);
    }

    private static T Zero<T>(this IGuardClause guardClause, T input, string parameterName, string? message = null,
        Func<Exception>? exceptionCreator = null) where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(input, default(T)))
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Required input {parameterName} cannot be zero.",
                parameterName);
        }

        return input;
    }
}
