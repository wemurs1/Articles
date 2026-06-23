using FileStorage.Contracts;

namespace Submission.Domain.ValueObjects;

public partial class File
{
    private File() { }

    internal static File CreateFile(FileMetadata fileMetadata, Asset asset, AssetTypeDefinition assetType)
    {
        var fileName = Path.GetFileName(fileMetadata.StoragePath);
        var extension = FileExtension.FromFileName(fileName, assetType);
        var file = new File()
        {
            Name = FileName.Create(asset, extension),
            Extension = extension,
            OriginalName = fileName,
            Size = fileMetadata.FileSize,
            FileServerId = fileMetadata.FileId
        };

        return file;
    }
}
