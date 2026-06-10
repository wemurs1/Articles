namespace Submission.Domain.Entities;

public partial class Article : AggregateRoot
{
    public required string Title { get; set; }
    public required string Scope { get; set; }
    public required ArticleType Type { get; set; }
    public DateTime? SubmittedOn { get; set; }
    public int? SubmittedById { get; set; }
    public Person? SubmittedBy { get; set; }
    public ArticleStage Stage { get; internal set; }
    public int JournalId { get; init; }
    public required Journal Journal { get; init; }

    private readonly List<Asset> _assets = [];
    public IReadOnlyList<Asset> Assets => _assets.AsReadOnly();

    private readonly List<ArticleActor> _actors = new();
    public IReadOnlyList<ArticleActor> Actors => _actors.AsReadOnly();
}
