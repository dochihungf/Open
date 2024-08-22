using MediatR;

namespace Open.Core.CQRS;

public interface IInsertCommand<out TResponse> : ICommand<TResponse>;

public interface IInsertCommand : IInsertCommand<Unit>;
