using System.Runtime.Serialization;

namespace Open.Core.Exception.Types;

public class SqlInjectionException : System.Exception
{
    public SqlInjectionException() : this(string.Empty)
    {
    }

    public SqlInjectionException(string message) : base(message)
    {
    }

    public SqlInjectionException(string message, System.Exception innerException) : base(message, innerException)
    {
    }

    protected SqlInjectionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}