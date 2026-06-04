using EmailService.Contracts;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace EmailService.Smtp;

public class SmtpEmailService(IOptions<EmailOptions> emailOptions) : IEmailService
{
    private readonly EmailOptions _emailOptions = emailOptions.Value;

    public async Task<bool> SendEmailAsync(EmailMessage emailMessage, CancellationToken ct = default)
    {
        var message = emailMessage.ToMailKitMessage();

        using var smtpClient = new SmtpClient();

        try
        {
            await smtpClient.ConnectAsync(_emailOptions.Smtp.Host, _emailOptions.Smtp.Port, _emailOptions.Smtp.UseSSL, ct);
            await smtpClient.AuthenticateAsync(_emailOptions.Smtp.Username, _emailOptions.Smtp.Password, ct);
            await smtpClient.SendAsync(message, ct);
        }
        catch (Exception)
        {
            // todo log and rethrow
            return false;
        }
        return true;
    }
}
