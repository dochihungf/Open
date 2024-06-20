using System.Runtime.Serialization;

namespace Open.Core.Exception.Types;

public class AuthorizationException : System.Exception
{
    public string Type { get; set; } = "AUTHORIZATION";
    
    public AuthorizationException() : this(string.Empty)
    {
    }

    public AuthorizationException(string message) : base(message)
    {
    }

    public AuthorizationException(string message, System.Exception innerException) : base(message, innerException)
    {
    }

    protected AuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}