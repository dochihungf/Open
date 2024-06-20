using System.Runtime.Serialization;

namespace Open.Core.Exception.Types;

public class CatchableException : System.Exception
{
    public CatchableException() : this(string.Empty)
    {
    }

    public CatchableException(string message) : base(message)
    {
    }

    public CatchableException(string message, System.Exception innerException) : base(message, innerException)
    {
    }

    protected CatchableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}