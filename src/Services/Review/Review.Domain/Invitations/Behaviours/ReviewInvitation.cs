using Blocks.Exceptions;
using Review.Domain.Invitations.Enums;
using Review.Domain.Invitations.Events;

namespace Review.Domain.Invitations;

public partial class ReviewInvitation
{
    public void Accept()
    {
        if (Status != InvitationStatus.Open) throw new DomainException("Invitation is not open anymore");

        if (ExpiresOn < DateTime.UtcNow) throw new DomainException("Invitation expired");

        AddDomainEvent(new InvitationAccepted(ArticleId, Email));
        Status = InvitationStatus.Accepted;
    }

    public void Decline()
    {
        if (Status != InvitationStatus.Open) throw new DomainException("Invitation is not open anymore");

        AddDomainEvent(new InvitationDeclined(ArticleId, Email));
        Status = InvitationStatus.Declined;
    }
}
