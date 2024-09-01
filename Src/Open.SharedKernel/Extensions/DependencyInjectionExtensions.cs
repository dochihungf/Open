using System.Globalization;
using System.Text;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Open.Core.GuardClauses;
using Open.Security.Constants;

namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static IServiceCollection AddCoreLocalization(this IServiceCollection services)
    {
        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() };
        });

        return services;
    }
    
    public static IServiceCollection AddCoreAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtOptions =>
        {
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = configuration["Auth:JwtSettings:Issuer"],
                ValidateIssuer = true,
                ValidIssuer = configuration["Auth:JwtSettings:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:JwtSettings:Key"])),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            jwtOptions.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/socket-message"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
        return services;
    }

    public static IServiceCollection AddCoreRateLimit(this IServiceCollection services)
    {
        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.RealIpHeader = HeaderNames.RealIpHeader;
            options.ClientIdHeader = HeaderNames.ClientIdHeader;
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1s",
                    Limit = 8,
                }
            };
        });

        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();

        return services;
    }
    
    public static void UseCoreCors(this IApplicationBuilder app, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredSection("AllowedHosts").Value;
        Guard.Against.NullOrEmpty(origins, nameof(origins));
        
        if (origins.Equals("*"))
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }
        else
        {
            app.UseCors(x => x.WithOrigins(origins.Split(";")).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
        }
    }
    
    public static void UseCoreLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
    }
}
