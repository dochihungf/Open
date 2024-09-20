using MediatR;

namespace Open.Core.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>, ITxRequest;

public interface ICommand : IRequest, ITxRequest;
