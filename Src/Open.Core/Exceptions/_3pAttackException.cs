using System.Runtime.Serialization;

namespace Open.Core.Exceptions;

public class _3pAttackException : Exception
{
    public _3pAttackException() : this(string.Empty)
    {
    }
    
    public _3pAttackException(string message) : base(message)
    {
    }

    public _3pAttackException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected _3pAttackException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
