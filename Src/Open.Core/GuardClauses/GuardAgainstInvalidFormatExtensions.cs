using System.Text.RegularExpressions;

namespace Open.Core.GuardClauses;

public partial class GuardAgainstZeroExtensions
{
    
    public static string InvalidFormat(this IGuardClause guardClause,
        string input,
        string parameterName,
        string regexPattern,
        string? message = null,
        Func<Exception>? exceptionCreator =  null)
    {
        var m = Regex.Match(input, regexPattern);
        if (!m.Success || input != m.Value)
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Input {parameterName} was not in required format", parameterName);
        }
        
        return input;
    }
    
    public static T InvalidInput<T>(this IGuardClause guardClause, 
        T input, 
        string parameterName, 
        Func<T, bool> predicate, 
        string? message = null,
        Func<Exception>? exceptionCreator =  null)
    {
        if (!predicate(input))
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Input {parameterName} did not satisfy the options", parameterName);
        }

        return input;
    }
    
    public static async Task<T> InvalidInputAsync<T>(this IGuardClause guardClause,
        T input,
        string parameterName,
        Func<T, Task<bool>> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator =  null)
    {
        if (!await predicate(input))
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Input {parameterName} did not satisfy the options", parameterName);
        }

        return input;
    }
}
