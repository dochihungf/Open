namespace Open.Core.Exceptions;

public class BadRequestException : Exception
{
    
    public BadRequestException() : this(string.Empty)
    {
    }

    public BadRequestException(string message) : base(message)
    {
    }
    
    public BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
