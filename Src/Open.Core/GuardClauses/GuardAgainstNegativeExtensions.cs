using System.Runtime.CompilerServices;

namespace Open.Core.GuardClauses;

public static partial class GuardAgainstZeroExtensions
{
    public static int Negative(this IGuardClause guardClause,
        int input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Negative<int>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static long Negative(this IGuardClause guardClause,
        long input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        return Negative<long>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static decimal Negative(this IGuardClause guardClause,
        decimal input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, 
        Func<Exception>? exceptionCreator = null)
    {
        return Negative<decimal>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static float Negative(this IGuardClause guardClause,
        float input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return Negative<float>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static double Negative(this IGuardClause guardClause,
        double input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return Negative<double>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static TimeSpan Negative(this IGuardClause guardClause,
        TimeSpan input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return Negative<TimeSpan>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    private static T Negative<T>(this IGuardClause guardClause,
        T input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null) where T : struct, IComparable
    {
        if (input.CompareTo(default(T)) < 0)
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Required input {parameterName} cannot be negative.", parameterName!);
        }

        return input;
    }
    
    public static int NegativeOrZero(this IGuardClause guardClause,
        int input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return NegativeOrZero<int>(guardClause, input, parameterName, message, exceptionCreator);
    }

    
    public static long NegativeOrZero(this IGuardClause guardClause,
        long input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return NegativeOrZero<long>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static decimal NegativeOrZero(this IGuardClause guardClause,
        decimal input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return NegativeOrZero<decimal>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static float NegativeOrZero(this IGuardClause guardClause,
        float input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return NegativeOrZero<float>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static double NegativeOrZero(this IGuardClause guardClause,
        double input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return NegativeOrZero<double>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    public static TimeSpan NegativeOrZero(this IGuardClause guardClause,
        TimeSpan input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, Func<Exception>? exceptionCreator = null)
    {
        return NegativeOrZero<TimeSpan>(guardClause, input, parameterName, message, exceptionCreator);
    }
    
    /// <summary>
    /// Negative or zero
    /// Âm hoặc bằng 0
    /// </summary>
    private static T NegativeOrZero<T>(this IGuardClause guardClause,
        T input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null, 
        Func<Exception>? exceptionCreator = null) where T : struct, IComparable
    {
        // input.CompareTo()
        // <= 0: input < default(T)
        // = 0: input = default(T)
        // > 0: input > default(T)
        if (input.CompareTo(default(T)) <= 0)
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Required input {parameterName} cannot be zero or negative.", parameterName!);
        }

        return input;
    }
}
