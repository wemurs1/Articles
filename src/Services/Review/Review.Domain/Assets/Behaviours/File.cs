using Articles.Abstractions.Events.Dtos;
using FileStorage.Contracts;

namespace Review.Domain.Assets.ValueObjects;

public partial class File
{
    public static File CreateFile(FileMetadata fileMetadata, Asset asset, AssetTypeDefinition assetType)
    {
        var filename = Path.GetFileName(fileMetadata.StoragePath);
        var extension = FileExtension.FromFileName(filename, assetType);

        var file = new File
        {
            Name = FileName.From(asset, extension),
            Extension = extension,
            OriginalFileName = filename,
            Size = fileMetadata.FileSize,
            FileServerId = fileMetadata.FileId
        };

        return file;
    }
}
