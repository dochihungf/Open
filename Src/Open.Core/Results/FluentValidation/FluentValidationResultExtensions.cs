using FluentValidation;
using FluentValidation.Results;

namespace Open.Core.Results.FluentValidation;

public static class FluentValidationResultExtensions
{
    public static ValidationSeverity FromSeverity(Severity severity)
    {
        switch (severity)
        {
            case Severity.Error: return ValidationSeverity.Error;
            case Severity.Warning: return ValidationSeverity.Warning;
            case Severity.Info: return ValidationSeverity.Info;
            default: throw new ArgumentOutOfRangeException(nameof(severity), "Unexpected Severity");
        }
    }
}
