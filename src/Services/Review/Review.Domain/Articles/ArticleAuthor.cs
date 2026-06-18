using Articles.Abstractions.Enums;

namespace Review.Domain.Articles;

public class ArticleAuthor : ArticleActor
{
    internal HashSet<ContributionArea> _contributionAreas { get; init; } = [];
    public IReadOnlyCollection<ContributionArea> ContributionAreas => _contributionAreas;
}
