using Articles.Abstractions;
using Auth.Grpc;
using Blocks.Core.Extensions;
using Blocks.Exceptions;
using Review.Domain.Reviewers.Events;
using Review.Domain.Shared.ValueObjects;

namespace Review.Domain.Reviewers;

public partial class Reviewer
{
    public static Reviewer Create(PersonInfo personInfo, IEnumerable<int> journalIds, IArticleAction action)
    {
        var reviewer = new Reviewer
        {
            Id = personInfo.Id,
            UserId = personInfo.UserId,
            Email = EmailAddress.Create(personInfo.Email),
            FirstName = personInfo.FirstName,
            LastName = personInfo.LastName,
            Honourific = personInfo.Honourific,
            Affiliation = personInfo.ProfessionalProfile!.Affiliation
        };

        if (journalIds.IsNotNullOrEmpty())
        {
            reviewer._specialisations = [.. journalIds.Select(journalId => new ReviewerSpecialisation { JournalId = journalId, ReviewerId = reviewer.Id })];
        }
        else
        {
            throw new DomainException("Reviewer must have at least one specialisation");
        }

        var domainEvent = new ReviewerCreated(reviewer, action);
        reviewer.AddDomainEvent(domainEvent);

        return reviewer;
    }

    public void AddSpecialisation(ReviewerSpecialisation specialisation)
    {
        if (!_specialisations.Contains(specialisation)) _specialisations.Add(specialisation);
    }
}
