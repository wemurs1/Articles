using Articles.Abstractions;
using Articles.Abstractions.Enums;
using Blocks.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;
using Submission.Persistence.Repositories;

namespace Submission.Application.Features.CreateArticle;

public class CreateArticleCommandHandler(Repository<Journal> _journalRepository) : IRequestHandler<CreateArticleCommand, IdResponse>
{
    public async Task<IdResponse> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
    {
        var journal = await _journalRepository.FindByIdOrThrowAsync(command.JournalId);

        var article = journal.CreateArticle(command.Title, command.Type, command.Scope);

        await AssignCurrentUserAsAuthor(article, command);

        await _journalRepository.SaveChangesAsync(cancellationToken);
        return new IdResponse(article.Id);
    }

    private async Task AssignCurrentUserAsAuthor(Article article, CreateArticleCommand command, CancellationToken cancellationToken = default)
    {
        var author = await _journalRepository.Context.Authors.SingleOrDefaultAsync(t => t.UserId == command.CreatedById, cancellationToken);
        if (author is not null) article.AssignAuthor(author, [ContributionArea.OriginalDraft], isCorrespondingAuthor: true);
    }
}
