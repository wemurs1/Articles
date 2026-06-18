using Review.Domain.Shared;

namespace Review.Domain.Articles;

public class Author : Person
{
    public string? Degree { get; set; }
    public string? Discipline { get; set; }
}
