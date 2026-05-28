namespace Submission.Domain.Entities;

public class AssetTypeDefinition: EnumEntity<AssetType>
{
    public required byte MaxFileSizeMB { get; init; }

    public int MaxFileSizeInBytes => (MaxFileSizeMB * 1024 * 1024);

    public required string DefaultFileExtension { get; init; } = default!;
    public required FileExtensions AllowedFileExtensions { get; init; }
}
