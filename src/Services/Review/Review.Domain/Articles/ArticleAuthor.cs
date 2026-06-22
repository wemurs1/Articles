using Articles.Abstractions.Enums;

namespace Review.Domain.Articles;

public class ArticleAuthor : ArticleActor
{
    public ArticleAuthor() { }

    public ArticleAuthor(HashSet<ContributionArea> contributionAreas) => _contributionAreas = contributionAreas;

    internal HashSet<ContributionArea> _contributionAreas { get; init; } = [];
    public IReadOnlyCollection<ContributionArea> ContributionAreas => _contributionAreas;
}
