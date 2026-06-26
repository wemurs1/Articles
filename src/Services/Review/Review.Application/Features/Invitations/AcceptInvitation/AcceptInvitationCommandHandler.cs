using Auth.Grpc;
using Blocks.EntityFrameworkCore;
using MediatR;
using Review.Domain.Reviewers;
using Review.Persistence;

namespace Review.Application.Features.Invitations.AcceptInvitation;

public class AcceptInvitationCommandHandler(
    ReviewDbContext _dbContext,
    ArticleRepository _articleRepository,
    ReviewInvitationRepository _reviewInvitationRepository,
    ReviewerRepository _reviewerRepository,
    IPersonService _personClient)
    : IRequestHandler<AcceptInvitationCommand, AcceptInvitationResponse>
{
    public async Task<AcceptInvitationResponse> Handle(AcceptInvitationCommand command, CancellationToken ct)
    {
        var article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId);
        var invitation = await _reviewInvitationRepository.GetByTokenOrThrowAsync(command.Token, ct);

        Reviewer? reviewer = default!;
        if (invitation.UserId != null)
        {
            reviewer = (await _reviewerRepository.GetByUserIdAsync(invitation.UserId.Value, ct));
            if (reviewer is null)
            {
                var response = await _personClient.GetPersonByUserIdAsync(new GetPersonByUserIdRequest { UserId = invitation.UserId.Value });
                reviewer = Reviewer.Create(response.PersonInfo, new HashSet<int> { article.JournalId }, command);
                await _reviewerRepository.AddAsync(reviewer);
            }
        }
        else
        {
            reviewer = (await _reviewerRepository.GetByEmailAsync(invitation.Email, ct));
            if (reviewer is null)
            {
                // todo - implement CreateUser gRPC
            }
        }

        invitation.Accept();
        article.AssignReviewer(reviewer, command);

        await _dbContext.SaveChangesAsync(ct);

        return new AcceptInvitationResponse(article.Id, invitation.Id, reviewer.Id);
    }
}
