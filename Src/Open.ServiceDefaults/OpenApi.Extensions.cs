namespace Open.ServiceDefaults;

public static class OpenApiExtension
{
    /// {
    ///   "OpenApi": {
    ///     "Endpoint: {
    ///         "Name": 
    ///     },
    ///     "Auth": {
    ///         "ClientId": ..,
    ///         "AppName": ..
    ///     }
    ///   }
    /// }
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        app.UseSwagger(c => c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            ArgumentNullException.ThrowIfNull(httpReq);

            swagger.Servers =
            [
                new()
                {
                    Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                    Description = string.Join(
                        " ",
                        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production,
                        nameof(Environment)
                    )
                }
            ];
        }));

        app.UseReDoc(options =>
        {
            app.DescribeApiVersions()
                .Select(desc => $"/swagger/{desc.GroupName}/swagger.json")
                .ToList()
                .ForEach(spec => options.SpecUrl(spec));

            options.EnableUntrustedSpec();
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(setup =>
            {
                var auth = app.Configuration.GetSection(nameof(OpenApi)).Get<OpenApi>()?.Auth;

                foreach (var description in app.DescribeApiVersions())
                {
                    var name = description.GroupName;
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    setup.SwaggerEndpoint(url, name);
                }

                if (auth is not null)
                {
                    setup.DocumentTitle = auth.AppName;
                    setup.OAuthClientId(auth.ClientId);
                    setup.OAuthClientSecret(auth.ClientSecret);
                    setup.OAuthAppName(auth.AppName);
                    setup.OAuthUsePkce();
                    setup.EnableValidator();
                }
            });

            // Add a redirect from the root of the app to the swagger endpoint
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }
        else
        {
            app.MapGet("/", () => Results.Redirect("/api-docs")).ExcludeFromDescription();
        }

        return app;
    }

    public static IHostApplicationBuilder AddDefaultOpenApi(
        this IHostApplicationBuilder builder,
        IApiVersioningBuilder? apiVersioning = default)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        var openApi = configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return builder;
        }

        services.AddEndpointsApiExplorer();

        if (apiVersioning is not null)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddFluentValidationRulesToSwagger();
            services.AddSwaggerGen(options => options.OperationFilter<OpenApiDefaultValues>());
        }

        return builder;
    }
    
}
