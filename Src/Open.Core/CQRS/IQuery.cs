using MediatR;
using Open.ServiceDefaults;

namespace Open.Core.CQRS;

[Authorization]
public interface IQuery<out TResponse> : IRequest<TResponse>;
