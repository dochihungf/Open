namespace Open.Core.Results;

public class ValidationError
{
    public ValidationError()
    {
    }

    public ValidationError(string[] errorMessage) => this.ErrorMessage = errorMessage;

    public ValidationError(
        string identifier,
        string[] errorMessage,
        string errorCode,
        ValidationSeverity severity)
    {
        this.Identifier = identifier;
        this.ErrorMessage = errorMessage;
        this.ErrorCode = errorCode;
        this.Severity = severity;
    }
    public string Identifier { get; set; }
    public string[] ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
    
    public ValidationSeverity Severity  = ValidationSeverity.Error;
}
