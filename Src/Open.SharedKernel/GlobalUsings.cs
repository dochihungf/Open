// System
global using System.Reflection;

// Third-party libraries
global using AutoMapper.Configuration.Annotations;
global using FluentValidation;
global using MassTransit.Internals;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Options;
global using MySqlConnector;
global using MediatR;

// Open namespaces
global using Open.Constants;
global using Open.Core.Exceptions;
global using Open.Core.GuardClauses;
global using Open.Core.Models;
global using Open.Core.Repositories.Dapper;
global using Open.Core.Results;
global using Open.Core.SeedWork;
global using Open.Core.SeedWork.Interfaces;
global using Open.Core.UnitOfWork;
global using Open.Security.Auth;
global using Open.SharedKernel.Attributes;
global using Open.SharedKernel.Caching.Sequence;
global using Open.SharedKernel.Extensions;
global using Open.SharedKernel.Libraries.Helpers;
global using Open.SharedKernel.Libraries.Security;
global using Open.SharedKernel.Libraries.Utilities;
global using Open.SharedKernel.MySQL;
global using Open.SharedKernel.Properties;

// Aliases
global using ValidationException = Open.Core.Exceptions.ValidationException;
global using IgnoreAttribute = Open.SharedKernel.Attributes.IgnoreAttribute;
