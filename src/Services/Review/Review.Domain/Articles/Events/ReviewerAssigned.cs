using Articles.Abstractions;
using Blocks.Domain;
using Review.Domain.Reviewers;

namespace Review.Domain.Articles.Events;

public record class ReviewerAssigned(Article Article, Reviewer Reviewer, IArticleAction Action) : IDomainEvent;