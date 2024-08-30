using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Open.Core.GuardClauses.Exceptions;

namespace Open.Core.GuardClauses;

public static partial class GuardAgainstZeroExtensions
{
    public static T NotFound<T>(this IGuardClause guardClause,
        [NotNull] [ValidatedNotNull] string key,
        [NotNull] [ValidatedNotNull] T? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        Func<Exception>? exceptionCreator = null)
    {
        guardClause.NullOrEmpty(key, nameof(key));

        if (input is null)
        {
            Exception? exception = exceptionCreator?.Invoke();

            throw exception ?? new NotFoundException(key, parameterName!);
        }

        return input;
    }
    
    public static T NotFound<TKey, T>(this IGuardClause guardClause,
        [NotNull][ValidatedNotNull] TKey key,
        [NotNull][ValidatedNotNull]T? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        Func<Exception>? exceptionCreator =  null) where TKey : struct
    {
        guardClause.Null(key, nameof(key));

        if (input is null)
        {
            Exception? exception = exceptionCreator?.Invoke();

            // TODO: Can we safely consider that ToString() won't return null for struct?
            throw exception ?? new NotFoundException(key.ToString()!, parameterName!);
        }

        return input;
    }
}
