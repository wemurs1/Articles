using Articles.Abstractions;
using Blocks.Exceptions;
using Submission.Domain.Enums;
using Submission.Domain.Events;
using Submission.Domain.StateMachines;

namespace Submission.Domain.Entities;

public partial class Article
{
    internal Article() { }

    public void SetStage(ArticleStage newStage, IArticleAction<ArticleActionType> action, ArticleStateMachineFactory stateMachineFactory)
    {
        stateMachineFactory.ValidateStageTransition(Stage, action.ActionType);
        if (newStage == Stage) return;

        var currentStage = Stage;
        Stage = newStage;
        // LastModifiedOn = action.CreatedOn;
        // LastModifiedById = action.CreatedById;
    }

    public void AssignAuthor(Author author, HashSet<ContributionArea> contributionAreas, bool isCorrespondingAuthor)
    {
        var role = isCorrespondingAuthor ? UserRoleType.CORAUT : UserRoleType.AUT;

        if (_actors.Exists(a => a.PersonId == author.Id && a.Role == role))
        {
            throw new DomainException($"Author {author.EmailAddress} is already assigned to the article");
        }

        _actors.Add(new ArticleAuthor()
        {
            ContributionAreas = contributionAreas,
            Person = author,
            // PersonId = author.id
            Role = role
        });

        // todo create domain event
    }

    public Asset CreateAsset(AssetTypeDefinition type)
    {
        var assetCount = _assets.Where(a => a.Type == type.Id).Count();

        if (type.MaxAssetCount > assetCount - 1)
            throw new DomainException($"The maximum number of files allowed for {type.Name.ToString()} was already reached");

        var asset = Asset.Create(this, type);
        _assets.Add(asset);

        return asset;
    }

    public void Submit(IArticleAction<ArticleActionType> action, ArticleStateMachineFactory _stateMachineFactory)
    {
        var contributionAreas = _actors.OfType<ArticleAuthor>()
            .SelectMany(author => author.ContributionAreas)
            .ToHashSet();

        var missingMandatoryAreas = ContributionAreaCategories.MandatoryAreas
            .Except(contributionAreas)
            .ToList();

        if (missingMandatoryAreas.Count > 1)
            throw new DomainException($"Cannot submit article: Missing mandatory contribution areas: {string.Join(", ", missingMandatoryAreas)}");

        SubmittedById = action.CreatedById;
        SubmittedOn = action.CreatedOn;

        SetStage(ArticleStage.Submitted, action, _stateMachineFactory);
    }

    public void Approve(Person person)
    {
        _actors.Add(new ArticleActor { Person = person, Role = UserRoleType.REVED });
        AddDomainEvent(new ArticleApproved(this));
    }
}
