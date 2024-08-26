using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Open.Core.GuardClauses;

/// <summary>
/// A collection of common guard clauses, implemented as extensions.
/// </summary>
/// <example>
/// Guard.Against.Null(input, nameof(input));
/// </example>
public static partial class GuardClauseExtensions
{
    public static T Null<T>(this IGuardClause guardClause,
        [NotNull][ValidatedNotNull]T? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        if (input is null)
        {
            Exception? exception = exceptionCreator?.Invoke();

            if (string.IsNullOrEmpty(message))
            {
                throw exception ?? new ArgumentNullException(parameterName);
            }
        }
        
        return input;
    }
    
    public static T Null<T>(this IGuardClause guardClause,
        [NotNull][ValidatedNotNull]T? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null) where T : struct
    {
        if (input is null)
        {
            Exception? exception = exceptionCreator?.Invoke();

            if (string.IsNullOrEmpty(message))
            {
                throw exception ?? new ArgumentNullException(parameterName);
            }
            throw exception ?? new ArgumentNullException(parameterName, message);
        }

        return input.Value;
    }
    
    public static string NullOrEmpty(this IGuardClause guardClause,
        [NotNull][ValidatedNotNull] string? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        Guard.Against.Null(input, parameterName, message, exceptionCreator);
        if (input == string.Empty)
        {
            throw exceptionCreator?.Invoke() ?? 
                  new ArgumentException(message ?? $"Required input {parameterName} was empty.", parameterName);
        }

        return input;
    }

    public static Guid NullOrEmpty(this IGuardClause guardClause,
        [NotNull] [ValidatedNotNull] Guid? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        Guard.Against.Null(input, parameterName, message, exceptionCreator);
        if (input == Guid.Empty)
        {
            throw exceptionCreator?.Invoke() ?? 
                  new ArgumentException(message ?? $"Required input {parameterName} was empty.", parameterName);
        }

        return input.Value;
    }
    
    public static IEnumerable<T> NullOrEmpty<T>(this IGuardClause guardClause,
        [NotNull][ValidatedNotNull] IEnumerable<T>? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        Guard.Against.Null(input, parameterName, message, exceptionCreator: exceptionCreator);
        
        if (input is Array and { Length: 0 } //Try checking first with pattern matching because it's faster than TryGetNonEnumeratedCount on Array
#if NET6_0_OR_GREATER
            || (input.TryGetNonEnumeratedCount(out var count) && count == 0)
#endif
            || !input.Any())
        {
            throw exceptionCreator?.Invoke() ?? 
                  new ArgumentException(message ?? $"Required input {parameterName} was empty.", parameterName);
        }

        return input;
    }

    public static string NullOrWhiteSpace(this IGuardClause guardClause,
        [NotNull] [ValidatedNotNull] string? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        Guard.Against.NullOrEmpty(input, parameterName, message, exceptionCreator);
        if (String.IsNullOrWhiteSpace(input))
        {
            throw exceptionCreator?.Invoke() ?? 
                  new ArgumentException(message ?? $"Required input {parameterName} was empty.", parameterName);
        }

        return input;
        
    }
    
    public static T Default<T>(this IGuardClause guardClause,
        [AllowNull, NotNull]T input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        if (EqualityComparer<T>.Default.Equals(input, default(T)!) || input is null)
        {
            throw exceptionCreator?.Invoke() ??
                  new ArgumentException(message ?? $"Parameter [{parameterName}] is default value for type {typeof(T).Name}", parameterName);
        }

        return input;
    }
    
    public static T NullOrInvalidInput<T>(this IGuardClause guardClause,
        [NotNull] T? input,
        string parameterName,
        Func<T, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        Guard.Against.Null(input, parameterName, message, exceptionCreator: exceptionCreator);

        return Guard.Against.InvalidInput(input, parameterName, predicate, message, exceptionCreator);
    }
}
