namespace Submission.Domain.Entities;

public partial class Article : Entity
{
    public required string Title { get; set; }
    public required string Scope { get; set; }
    public required ArticleType Type { get; set; }
    public ArticleStage Stage { get; internal set; }
    public int JournalId { get; init; }
    public required Journal Journal { get; init; }
    public List<ArticleActor> Actors { get; set; } = [];

    private readonly List<Asset> _assets = [];
    public IReadOnlyList<Asset> Assets => _assets.AsReadOnly();
}
