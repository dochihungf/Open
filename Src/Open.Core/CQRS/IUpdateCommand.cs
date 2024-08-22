using MediatR;

namespace Open.Core.CQRS;

public interface IUpdateCommand<out TResponse> : ICommand<TResponse>;

public interface IUpdateCommand : IUpdateCommand<Unit>;
