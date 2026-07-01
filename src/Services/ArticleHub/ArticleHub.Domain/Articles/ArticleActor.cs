namespace ArticleHub.Domain.Articles;

public class ArticleActor
{
    public int ArticleId { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public UserRoleType Role { get; set; }
}
