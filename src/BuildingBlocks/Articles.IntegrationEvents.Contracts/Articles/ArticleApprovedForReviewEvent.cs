using Articles.IntegrationEvents.Contracts.Dtos;

namespace Articles.IntegrationEvents.Contracts;

public record ArticleApprovedForReviewEvent(ArticleDto Article);