namespace Open.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : this(string.Empty)
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    
    public NotFoundException(string key, string objectName)
        : base($"Queried object {objectName} was not found, Key: {key}")
    {
    }
    
    public NotFoundException(string key, string objectName, Exception innerException)
        : base($"Queried object {objectName} was not found, Key: {key}", innerException)
    {
    }
}
