using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EmailService.Contracts;
using Blocks.Core.Extensions;

namespace EmailService.Smtp;

public static class MailServiceRegistration
{
    public static IServiceCollection AddSmtpEmailService(this IServiceCollection services, IConfiguration config)
    {
        services.AddAndValidateOptions<EmailOptions>(config);
        services.AddSingleton<IEmailService, SmtpEmailService>();

        return services;
    }
}
