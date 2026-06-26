using Blocks.Domain;
using Review.Domain.Shared.ValueObjects;

namespace Review.Domain.Invitations.Events;

public record class InvitationDeclined(int ArticleId, EmailAddress ReviewerEmail) : IDomainEvent;