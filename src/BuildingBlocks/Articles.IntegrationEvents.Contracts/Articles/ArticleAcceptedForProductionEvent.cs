using Articles.IntegrationEvents.Contracts.Dtos;

namespace Articles.IntegrationEvents.Contracts;

public record ArticleAcceptedForProductionEvent(ArticleDto Article);