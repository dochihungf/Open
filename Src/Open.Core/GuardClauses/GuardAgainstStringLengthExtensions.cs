using System.Runtime.CompilerServices;

namespace Open.Core.GuardClauses;

public static partial class GuardAgainstZeroExtensions
{
    /// <summary>
    /// String too short
    /// Chuỗi quá ngắn
    /// </summary>
    public static string StringTooShort(this IGuardClause guardClause,
        string input,
        int minLength,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        Guard.Against.NegativeOrZero(minLength, nameof(minLength), exceptionCreator: exceptionCreator);
        if (input.Length < minLength)
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Input {parameterName} with length {input.Length} is too short. Minimum length is {minLength}.", parameterName);
        }
        return input;
    }
    
    /// <summary>
    /// String too long
    /// Chuỗi quá dài
    /// </summary>
    public static string StringTooLong(this IGuardClause guardClause,
        string input,
        int maxLength,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        Guard.Against.NegativeOrZero(maxLength, nameof(maxLength), exceptionCreator: exceptionCreator);
        if (input.Length > maxLength)
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new ArgumentException(message ?? $"Input {parameterName} with length {input.Length} is too long. Maximum length is {maxLength}.", parameterName);
        }
        return input;
    }
}
