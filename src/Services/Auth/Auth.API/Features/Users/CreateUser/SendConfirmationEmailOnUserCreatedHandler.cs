using Auth.Domain.Events;
using Blocks.AspNetCore;
using EmailService.Contracts;
using Flurl;
using Microsoft.Extensions.Options;

namespace Auth.API.Features.Users.CreateUser;

public class SendConfirmationEmailOnUserCreatedHandler(
    IEmailService emailService, IHttpContextAccessor httpContextAccessor, IOptions<EmailOptions> emailOptions)
    : IEventHandler<UserCreated>
{
    public async Task HandleAsync(UserCreated eventModel, CancellationToken ct = default)
    {
        var url = httpContextAccessor.HttpContext?.Request.BaseUrl().AppendPathSegment("password").SetQueryParams(new { eventModel.passwordResetToken });

        var emailMessage = BuildConfirmationEmail(eventModel.user, url, emailOptions.Value.EmailFromAddress);
        await emailService.SendEmailAsync(emailMessage, ct);
    }

    public EmailMessage BuildConfirmationEmail(User user, string resetLink, string fromEmailAddess)
    {
        const string ConfirmationEmail = "Dear {0},<br/>An account has been created for you.<br/>Please set your password using the following link: <br/>{1}";

        return new EmailMessage(
            Subject: "Your Account Has been Created - Set Your Password",
            Content: new Content(ContentType.Html, string.Format(ConfirmationEmail, user.Person.FullName, resetLink)),
            From: new EmailAddress("Articles", fromEmailAddess),
            To: new List<EmailAddress> { new EmailAddress(user.Person.FullName, user.Email!) }
        );
    }
}
