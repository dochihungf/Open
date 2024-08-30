namespace Open.Core.Exceptions;

public class BadRequestException : Exception
{
    public string Type { get; set; } = "BAD_REQUEST";
    public object Body { get; set; }

    public BadRequestException() : this(string.Empty)
    {
    }

    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, string type) : base(message)
    {
        Type = type;
    }

    public BadRequestException(string message, string type, object body) : base(message)
    {
        Type = type;
        Body = body;
    }

    public BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
