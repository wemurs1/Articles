using MediatR;
using Review.Domain.Articles.Events;

namespace Review.Application.Features.Articles.AcceptArticle;

public class PublishArticleApprovedEventHandler(ArticleRepository _articleRepository, IPublishEndpoint _publishEndpoint)
    : INotificationHandler<ArticleAccepted>
{
    public async Task Handle(ArticleAccepted notification, CancellationToken ct)
    {
        var article = await _articleRepository.GetFullArticleById(notification.Article.Id);
        var articleDto = article.Adapt<ArticleDto>();
        await _publishEndpoint.Publish(new ArticleAcceptedForProductionEvent(article.Adapt<ArticleDto>()), ct);
    }
}
