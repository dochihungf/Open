using MediatR;

namespace Open.Identity.Apis;

public abstract class BaseService(ILogger logger)
{
    public ILogger Logger { get; } = logger;
}
