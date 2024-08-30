namespace Open.Core.GuardClauses;

public static partial class GuardAgainstZeroExtensions
{
    public static ReadOnlySpan<char> Empty(this IGuardClause guardClause,
        ReadOnlySpan<char> input,
        string parameterName,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        if (input.Length == 0 || input == string.Empty)
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Required input {parameterName} was empty.", parameterName);
        }
        return input;
    }
    
    public static ReadOnlySpan<char> WhiteSpace(this IGuardClause guardClause,
        ReadOnlySpan<char> input,
        string parameterName,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        if (input.IsWhiteSpace())
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Required input {parameterName} was empty.", parameterName!);
        }

        return input;
    }
}
