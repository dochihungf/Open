using Open.ServiceDefaults;

namespace Open.Core.CQRS;

[Authorization(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public interface IAnonymousQuery<out TResponse> : IQuery<TResponse>;
