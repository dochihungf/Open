using System.ComponentModel.DataAnnotations;

namespace Open.Driver.Infrastructure.Master;

public record MySqlRetryOptions
{
    [Required, Range(5, 20)]
    public int MaxRetryCount { get; init; }
    
    [Required, Timestamp]
    public TimeSpan MaxRetryDelay { get; init; }
    
    public int[]? ErrorNumbersToAdd { get; init; }
}
