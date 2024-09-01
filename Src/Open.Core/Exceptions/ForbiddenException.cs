namespace Open.Core.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : this(string.Empty)
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
