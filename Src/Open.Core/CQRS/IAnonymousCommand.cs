using Open.ServiceDefaults;

namespace Open.Core.CQRS;

[Authorization(new [] { ActionExponent.AllowAnonymous })]
public interface IAnonymousCommand<out TResponse> : ICommand<TResponse>;

[Authorization(new [] { ActionExponent.AllowAnonymous })]
public interface IAnonymousCommand : ICommand;
