using Review.Domain.Shared;

namespace Review.Domain.Reviewers;

public partial class Reviewer : Person
{
    private HashSet<ReviewerSpecialisation> _specialisations = [];
    public IReadOnlyCollection<ReviewerSpecialisation> Specialisations => _specialisations;

    public override string TypeDescriminator => nameof(Review);
}
