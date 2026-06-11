namespace Submission.Domain.Entities;

public partial class Person : Entity
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string FullName => FirstName + " " + LastName;
    public string? Title { get; init; }
    public required EmailAddress EmailAddress { get; init; }
    public required string Affiliation { get; init; }
    public int? UserId { get; init; }
    public IReadOnlyList<ArticleActor> ArticleActors { get; private set; } = [];
    public string TypeDescriminator { get; set; } = null!; // EF descriminator
}
