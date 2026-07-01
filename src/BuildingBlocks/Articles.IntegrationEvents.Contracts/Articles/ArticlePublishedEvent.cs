using Articles.IntegrationEvents.Contracts.Dtos;

namespace Articles.IntegrationEvents.Contracts;

public record class ArticlePublishedEvent(ArticleDto Article);