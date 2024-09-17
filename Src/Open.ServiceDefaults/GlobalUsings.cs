// .NET Core System Libraries
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Globalization;

// ASP.NET Core Libraries
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Http.Json;
global using Microsoft.AspNetCore.Localization;
global using Microsoft.AspNetCore.Localization.Routing;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.AspNetCore.Mvc.ModelBinding;

// Configuration and Hosting
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Primitives;
global using Microsoft.Extensions.Diagnostics.HealthChecks;

// OpenTelemetry Libraries
global using OpenTelemetry;
global using OpenTelemetry.Metrics;
global using OpenTelemetry.Trace;

// Swagger and API Documentation
global using Asp.Versioning;
global using Asp.Versioning.ApiExplorer;
global using Microsoft.OpenApi.Any;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

// MongoDB
global using MongoDB.Bson;

// Custom Libraries
global using Open.Core.Exceptions.ExceptionHandlers;
global using Open.Security.Constants;
global using Open.SharedKernel.ActivityScope;
global using Open.SharedKernel.Logging;

// Rate Limiting
global using AspNetCoreRateLimit;

// FluentValidation
global using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
