using Articles.Abstractions;
using Review.Application.Features.Invitations.InviteReviewer;
using Review.Application.Features.Shared;
using Review.Domain.Shared.Enums;

namespace Review.Application.Features.Articles.AcceptArticle;

public partial record AcceptArticleCommand() : ArticleCommand<ArticleActionType, InviteReviewerResponse>
{
    public override ArticleActionType ActionType => ArticleActionType.AcceptInvitation;
}