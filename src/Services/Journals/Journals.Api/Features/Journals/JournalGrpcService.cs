using Articles.GrpcContracts.Journals;
using Blocks.Redis;
using Journals.Domain.Journals;
using ProtoBuf.Grpc;

namespace Journals.Api.Features.Journals;

public class JournalGrpcService(Repository<Journal> _journalRepository) : IJournalService
{
    public async ValueTask<IsEditorAssignToJournalResponse> IsEditorAssignedToJournalAsync(IsEditorAssignedToJournalRequest request, CallContext context = default)
    {
        var journal = await _journalRepository.GetByIdOrThrowAsync(request.JournalId);
        return new IsEditorAssignToJournalResponse { IsAssigned = journal!.ChiefEditorId == request.UserId };
    }
}
