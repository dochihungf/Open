namespace Open.Core.Results.AspNetCore;

[AttributeUsage(AttributeTargets.Method)]
public class ExpectedFailuresAttribute : Attribute
{
    public ExpectedFailuresAttribute(params ResultStatus[] resultStatuses)
    {
        ResultStatuses = resultStatuses;
    }

    public IEnumerable<ResultStatus> ResultStatuses { get; }
}
