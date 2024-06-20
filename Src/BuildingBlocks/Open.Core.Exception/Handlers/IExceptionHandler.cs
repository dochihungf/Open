namespace Open.Core.Exception.Handlers;

public interface IExceptionHandler
{
    Task PutToDatabaseAsync(System.Exception ex);
}