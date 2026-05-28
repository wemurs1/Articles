namespace Submission.Domain.Entities;

public class ArticleActor
{
    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public UserRoleType Role { get; init; }
    public string TypeDescriminator { get; set; } = null!; // EF descriminator to manage inheritence
}
