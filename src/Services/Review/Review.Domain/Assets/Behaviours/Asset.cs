using Articles.IntegrationEvents.Contracts.Dtos;
using FileStorage.Contracts;
using Review.Domain.Assets.Enums;
using Review.Domain.Assets.ValueObjects;

namespace Review.Domain.Assets;

public partial class Asset
{
    public static Asset CreateFromSubmission(AssetDto assetDto, AssetTypeDefinition type, int articleId)
    {
        var asset = new Asset
        {
            ArticleId = articleId,
            Name = AssetName.FromAssetType(type),
            Type = type.Id,
            State = AssetState.Uploaded
        };

        return asset;
    }

    public ValueObjects.File CreateFile(FileMetadata fileMetaData, AssetTypeDefinition assetType)
    {
        File = ValueObjects.File.CreateFile(fileMetaData, this, assetType);
        State = AssetState.Uploaded;
        return File;
    }
}
