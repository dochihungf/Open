using MediatR;

namespace Open.Core.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : IRequest;
