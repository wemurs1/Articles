namespace Submission.Application.Features.UploadFiles;

public class UploadManuscriptFileCommandHandler(ArticleRepository _articleRepository, AssetTypeDefinitionRepository _assetTypeRepository)
    : IRequestHandler<UploadManuscriptFileCommand, IdResponse>
{
    public async Task<IdResponse> Handle(UploadManuscriptFileCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId);

        var assetType = _assetTypeRepository.GetById(command.AssetType);

        Asset asset = null!;
        if (!assetType.AllowsMultipleAssets) asset = article.Assets.SingleOrDefault(e => e.Type == assetType.Id)!;

        if (asset is null) asset = article.CreateAsset(assetType);

        // todo upload the file

        await _articleRepository.SaveChangesAsync();

        return new IdResponse(asset.Id);
    }
}
