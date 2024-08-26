namespace Open.Core.GuardClauses;

public partial class GuardClauseExtensions
{
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
}
