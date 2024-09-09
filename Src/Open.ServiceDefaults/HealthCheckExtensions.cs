namespace Open.ServiceDefaults;

public static class HealthCheckExtensions
{
    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        var healthChecksConfiguration = builder.Configuration.GetSection("HealthChecks");

        var healthChecksRequestTimeout =
            healthChecksConfiguration.GetValue<TimeSpan?>("RequestTimeout") ?? TimeSpan.FromSeconds(5);
        builder.Services.AddRequestTimeouts(timeouts => timeouts.AddPolicy("HealthChecks", healthChecksRequestTimeout));

        var healthChecksExpireAfter =
            healthChecksConfiguration.GetValue<TimeSpan?>("ExpireAfter") ?? TimeSpan.FromSeconds(10);
        builder.Services.AddOutputCache(caching =>
            caching.AddPolicy("HealthChecks", policy => policy.Expire(healthChecksExpireAfter)));

        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }
}
