namespace Review.Domain.Articles;

public class Editor : Reviewer
{
    public override string TypeDescriminator => nameof(Editor);
}
