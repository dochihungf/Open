using System.ComponentModel.DataAnnotations;

namespace Open.Identity.Infrastructure.Options;

public record MySqlRetryOptions
{
    [Required, Range(5, 20)]
    public int MaxRetryCount { get; init; }
    
    [Required, Timestamp]
    public TimeSpan MaxRetryDelay { get; init; }
    
    public int[]? ErrorNumbersToAdd { get; init; }
}
