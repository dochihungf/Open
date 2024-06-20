namespace Open.Core.Exception.Types;

public class NotFoundException : System.Exception
{
    public string Type { get; set; } = "NOT_FOUND";
    
    public NotFoundException() { }

    public NotFoundException(string? message) : base(message)
    {
        
    }

    public NotFoundException(string? message, System.Exception? innerException) : base(message, innerException) { }
}
