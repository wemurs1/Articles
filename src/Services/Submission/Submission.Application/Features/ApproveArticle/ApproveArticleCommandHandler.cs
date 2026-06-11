using Articles.GrpcContracts.Journals;
using Auth.Grpc;
using Blocks.Exceptions;
using Grpc.Core;

namespace Submission.Application.Features.ApproveArticle;

public class ApproveArticleCommandHandler(
    ArticleRepository _articleRepository, PersonRepository _personRepository, IPersonService _personService, IJournalService _journalService)
    : IRequestHandler<ApproveArticleCommand, IdResponse>
{
    public async Task<IdResponse> Handle(ApproveArticleCommand command, CancellationToken ct = default)
    {
        var article = await _articleRepository.FindByIdOrThrowAsync(command.ArticleId);
        if (!await IsEditorAssignedToJournal(article.JournalId, command.CreatedById))
            throw new BadRequestException($"Editor is not assigned to the Article's Journal ({article.JournalId})");


        Person editor = await GetOrCreatePersonByUserId(command.CreatedById, ct);
        article.Approve(editor!);
        await _articleRepository.SaveChangesAsync(ct);
        return new IdResponse(article.Id);
    }

    private async Task<bool> IsEditorAssignedToJournal(int journalId, int userId)
    {
        var response = await _journalService.IsEditorAssignedToJournalAsync(
                    new IsEditorAssignedToJournalRequest { JournalId = journalId, UserId = userId });
        return response.IsAssigned;
    }

    private async Task<Person> GetOrCreatePersonByUserId(int userId, CancellationToken ct)
    {
        var editor = await _personRepository.GetByUserIdAsync(userId);
        if (editor is null)
        {
            var response = await _personService.GetPersonByUserIdAsync(new GetPersonByUserIdRequest { UserId = userId }, new CallOptions(cancellationToken: ct));
            editor = Person.Create(response.PersonInfo);
            await _personRepository.AddAsync(editor, ct);
        }

        return editor;
    }
}
