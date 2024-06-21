using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Open.Core.Mailing.Abstractions;
using Open.Core.Mailing.MailKit;
using Open.Core.Mailing.Settings;

namespace Open.Core.Mailing;

public static class MailingDependencyInjection
{
    public static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // Options support
        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

        services.AddSingleton<IMailService, MailKitMailService>();

        return services;
    }
}