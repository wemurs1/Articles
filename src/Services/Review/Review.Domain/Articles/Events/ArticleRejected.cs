using Articles.Abstractions;
using Blocks.Domain;

namespace Review.Domain.Articles.Events;

public class ArticleRejected : IDomainEvent
{
    public ArticleRejected(Article article, IArticleAction action)
    {
        throw new NotImplementedException();
    }

    public Article Article { get; set; }
}
