﻿using MediatR;

namespace Open.Core.CQRS;

[Authorization(new [] { ActionExponent.View })]
public interface IQuery<out TResponse> : IRequest<TResponse>;
