namespace Open.Core.Exception.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public Task PutToDatabaseAsync(System.Exception ex)
    {
        throw new NotImplementedException();
    }
}