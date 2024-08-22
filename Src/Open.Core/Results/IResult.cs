namespace Open.Core.Results;

public interface IResult
{
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    IEnumerable<ValidationError> ValidationErrors { get; }
    Type ValueType { get; }
    object GetValue();
}
