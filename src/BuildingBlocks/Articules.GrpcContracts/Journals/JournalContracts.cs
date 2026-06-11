using System.ServiceModel;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace Articles.GrpcContracts.Journals;

[ServiceContract]
public interface IJournalService
{
    [OperationContract]
    ValueTask<IsEditorAssignToJournalResponse> IsEditorAssignedToJournalAsync(IsEditorAssignedToJournalRequest request, CallContext context = default);
}

[ProtoContract]
public class IsEditorAssignedToJournalRequest
{
    [ProtoMember(1)]
    public int JournalId { get; set; } = default;

    [ProtoMember(2)]
    public int UserId { get; set; }
}

[ProtoContract]
public class IsEditorAssignToJournalResponse
{
    [ProtoMember(1)]
    public bool IsAssigned { get; set; }
}
