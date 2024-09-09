namespace Open.ServiceDefaults;

public static class LocalizationExtensions
{
    public static IServiceCollection AddDefaultLocalization(this IServiceCollection services)
    {
        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = new IRequestCultureProvider[]
            {
                new RouteDataRequestCultureProvider()
            };
        });

        return services;
    }
    
    public static void UseDefaultLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-us"), new CultureInfo("vi-vn") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-us"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
    }
}
