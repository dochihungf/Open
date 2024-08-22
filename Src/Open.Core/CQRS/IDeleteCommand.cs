using MediatR;

namespace Open.Core.CQRS;

public interface IDeleteCommand<out TResponse> : ICommand<TResponse>;

public interface IDeleteCommand : IDeleteCommand<Unit>;
