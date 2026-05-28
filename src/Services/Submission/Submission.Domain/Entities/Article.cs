using Articles.Abstractions.Enums;
using Blocks.Domain.Entities;

namespace Submission.Domain.Entities;

public partial class Article : IEntity
{
    public int Id { get; init; }
    public required string Title { get; set; }
    public required string Scope { get; set; }
    public required ArticleType Type { get; set; }
    public ArticleStage Stage { get; internal set; }
    public int JournalId { get; init; }
    public required Journal Journal { get; init; }
    public List<ArticleActor> Actors { get; set; } = [];
}
