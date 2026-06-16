using Blocks.Domain;

namespace Submission.Domain.Events;

public record class ArticleApproved(Article Article) : IDomainEvent;