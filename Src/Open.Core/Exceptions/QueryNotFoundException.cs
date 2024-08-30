namespace Open.Core.Exceptions;

public class QueryNotFoundException : Exception
{
    public QueryNotFoundException() : this(string.Empty)
    {
    }

    public QueryNotFoundException(string message) : base(message)
    {
    }

    public QueryNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected QueryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
