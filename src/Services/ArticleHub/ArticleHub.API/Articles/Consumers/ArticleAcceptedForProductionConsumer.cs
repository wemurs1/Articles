using ArticleHub.Domain.Articles;
using ArticleHub.Persistence;
using Articles.Abstractions.Enums;
using Articles.IntegrationEvents.Contracts;
using Articles.IntegrationEvents.Contracts.Dtos;
using Blocks.EntityFrameworkCore;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ArticleHub.API.Articles.Consumers;

public sealed class ArticleAcceptedForProductionConsumer(ArticleHubDbContext _dbContext)
    : IConsumer<ArticleAcceptedForProductionEvent>
{
    public async Task Consume(ConsumeContext<ArticleAcceptedForProductionEvent> ctx)
    {
        var articleDto = ctx.Message.Article;

        // Must already exist for ApprovedForReview
        var article = await _dbContext.Articles.Include(a => a.Actors).SingleOrThowASync(a => a.Id == articleDto.Id, ctx.CancellationToken);

        // Update only fields that change during review
        article.Title = articleDto.Title;
        article.Stage = articleDto.Stage;

        await AddReviewers(articleDto, article);

        await _dbContext.SaveChangesAsync();
    }

    private async Task AddReviewers(ArticleDto articleDto, Article article)
    {
        foreach (var actorDto in articleDto.Actors.Where(a => a.Role == UserRoleType.REV))
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == actorDto.Person.Id);
            if (person == null)
            {
                person = actorDto.Person.Adapt<Person>();
                _dbContext.Persons.Add(person);
            }
            article.Actors.Add(new ArticleActor { ArticleId = article.Id, PersonId = person.Id, Role = actorDto.Role });
        }
    }
}
