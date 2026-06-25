using Auth.Grpc;
using Blocks.EntityFrameworkCore;
using EmailService.Contracts;
using Flurl;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Review.Application.Options;
using Review.Domain.Invitations;
using Review.Persistence;

namespace Review.Application.Features.Invitations.InviteReviewer;

public class InviteReviewerCommandHandler(
    ReviewDbContext _dbContext,
    ArticleRepository _articleRepository,
    ReviewerRepository _reviewerRepository,
    IPersonService _personClient,
    IOptions<AppUrlsOptions> _appUrlOptions,
    IOptions<EmailOptions> _emailOptions,
    IEmailService _emailService)
    : IRequestHandler<InviteReviewerCommand, InviteReviewerResponse>
{
    public async Task<InviteReviewerResponse> Handle(InviteReviewerCommand command, CancellationToken ct)
    {
        var article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId);

        ReviewInvitation invitation = default!;

        if (command.UserId is not null)
        {
            var reviewer = await _reviewerRepository.GetByUserIdAsync(command.UserId.Value, ct);
            if (reviewer is not null)
            {
                invitation = article.InviteReviewer(reviewer, command);
            }
            else
            {
                var response = await _personClient.GetPersonByUserIdAsync(new GetPersonByUserIdRequest { UserId = command.UserId.Value });
                var personInfo = response.PersonInfo;
                invitation = article.InviteReviewer(personInfo.UserId, personInfo.Email, personInfo.FirstName, personInfo.LastName, command);
            }
        }
        else
        {
            invitation = article.InviteReviewer(command.UserId, command.Email, command.FirstName, command.LastName, command);
        }

        await _dbContext.SaveChangesAsync(ct);

        var editor = await _dbContext.Editors.SingleAsync(e => e.UserId == command.CreatedById, ct);

        await _emailService.SendEmailAsync(BuildEmailMessage(article, invitation, editor));

        return new InviteReviewerResponse(article.Id, invitation.Id, invitation.Token.Value);
    }

    private EmailMessage BuildEmailMessage(Article article, ReviewInvitation invitation, Editor editor)
    {
        const string invitationEmail =
        @"Dear Contributor,<br/>
            You have been invited by {0} to review the following article: {1}<br/>
            Please accept of deny, the invitation will expire on {2}<br/>
            If you don't have and account please create one using the following URL: {3}";

        var url = _appUrlOptions.Value.ReviewUIBaseUrl
            .AppendPathSegment($"articles/{invitation.ArticleId}/invitations/{invitation.Token}");

        return new EmailMessage(
            "Review Invitation",
            new Content(ContentType.Html, string.Format(invitationEmail, editor.FullName, article.Title, invitation.ExpiresOn, url)),
            new EmailAddress("articles", _emailOptions.Value.EmailFromAddress),
            new List<EmailAddress> { new EmailAddress(invitation.FullName, invitation.Email) }
        );
    }
}
