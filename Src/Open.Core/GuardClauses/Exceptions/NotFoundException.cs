namespace Open.Core.GuardClauses.Exceptions;

/// <summary>
/// Represents error that occurs if a queried object by a particular key is null (not found).
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string key, string objectName)
        : base($"Queried object {objectName} was not found, Key: {key}")
    {
    }
    
    public NotFoundException(string key, string objectName, Exception innerException)
        : base($"Queried object {objectName} was not found, Key: {key}", innerException)
    {
    }
}
