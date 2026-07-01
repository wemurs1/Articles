namespace ArticleHub.Domain.Articles;

public class Journal : Entity<int>
{
    public required string Abbreviation { get; init; }
    public required string Name { get; init; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
