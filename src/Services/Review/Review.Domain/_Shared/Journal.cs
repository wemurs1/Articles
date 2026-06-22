using Blocks.Domain.Entities;
using Review.Domain.Articles;

namespace Review.Domain.Shared;

public class Journal : Entity<int>
{
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    private readonly List<Article> _articles = [];
    public IReadOnlyList<Article> Articles => _articles;

    public IReadOnlyCollection<ReviewerSpecialisation> Reviewers { get; set; } = new HashSet<ReviewerSpecialisation>();
    public void AddArticle(Article article) => _articles.Add(article);
}
