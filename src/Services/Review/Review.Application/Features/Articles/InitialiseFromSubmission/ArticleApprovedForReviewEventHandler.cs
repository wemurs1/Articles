using FileStorage.Contracts;
using Review.Application.FileStorage;
using Review.Persistence;

namespace Review.Application.Features.Articles.InitialiseFromSubmission;

public class ArticleApprovedForReviewEventHandler(
    ReviewDbContext _dbContext,
    ArticleRepository _articleRepository,
    Repository<Journal> _journalRepository,
    Repository<Person> _personRepository,
    AssetTypeDefinitionRepository _assetTypeDefinitionRepository,
    IFileService _fileService,
    FileServiceFactory _fileServiceFactory)
    : IConsumer<ArticleApprovedForReviewEvent>
{
    public async Task Consume(ConsumeContext<ArticleApprovedForReviewEvent> context)
    {
        var articleDto = context.Message.Article;
        if (_articleRepository.Entity.Any(e => e.Id == articleDto.Id)) throw new BadRequestException($"Article({articleDto.Id}) already exists");

        var journal = await GetOrCreateJournal(articleDto);
        var actors = await CreateActors(articleDto);
        var assets = await CreateAssets(articleDto);

        var article = Article.AcceptSubmitted(articleDto, actors, assets);
        journal.AddArticle(article);

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<IEnumerable<Asset>> CreateAssets(ArticleDto articleDto)
    {
        var assets = new List<Asset>();
        foreach (var assetDto in articleDto.Assets)
        {
            var assetTypeDefinition = _assetTypeDefinitionRepository.GetById(assetDto.Type);
            var asset = Asset.CreateFromSubmission(assetDto, assetTypeDefinition, articleDto.Id);

            // todo - download the files from submission and upload to review
            var submissionFileService = _fileServiceFactory(FileStorageType.Submission);
            var (fileStream, fileMetaData) = await submissionFileService.DownloadFileAsync(assetDto.File.FileServerId);
            var uploadRequest = new FileUploadRequest(fileMetaData.StoragePath, fileMetaData.FileName, fileMetaData.ContentType, fileMetaData.FileSize);
            fileMetaData = await _fileService.UploadFileAsync(uploadRequest, fileStream);

            asset.CreateFile(fileMetaData, assetTypeDefinition);

            assets.Add(asset);
        }
        return assets;
    }

    private async Task<IEnumerable<ArticleActor>> CreateActors(ArticleDto articleDto)
    {
        var actors = new List<ArticleActor>();
        foreach (var actorDto in articleDto.Actors)
        {
            var person = await _personRepository.GetByIdAsync(actorDto.Person.Id);
            ArticleActor actor = default!;
            if (actorDto.Role == UserRoleType.AUT || actorDto.Role == UserRoleType.CORAUT)
            {
                person ??= actorDto.Adapt<Author>();
                actor = new ArticleAuthor(actorDto.ContributionAreas)
                {
                    PersonId = person.Id,
                    Person = person,
                    Role = actorDto.Role
                };
            }
            else if (actorDto.Role == UserRoleType.REVED)
            {
                person ??= actorDto.Adapt<Editor>();
                actor = new ArticleActor
                {
                    PersonId = person.Id,
                    Person = person,
                    Role = actorDto.Role
                };
            }
            else
            {
                throw new DomainException($"Unknown role for {actorDto.Person.Email}");
            }
            actors.Add(actor);
        }
        return actors;
    }

    private async Task<Journal> GetOrCreateJournal(ArticleDto articleDto)
    {
        var journal = await _journalRepository.FindByIdAsync(articleDto.Journal.Id);
        if (journal is null)
        {
            journal = articleDto.Journal.Adapt<Journal>();
            await _journalRepository.AddAsync(journal);
        }
        return journal;
    }
}
