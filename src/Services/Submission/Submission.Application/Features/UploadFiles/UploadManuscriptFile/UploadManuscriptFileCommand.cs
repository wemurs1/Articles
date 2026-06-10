using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Submission.Application.Features.UploadFiles;

public record UploadManuscriptFileCommand : ArticleCommand
{
    /// <summary>
    /// The asset type of the file
    /// </summary>
    [Required]
    public AssetType AssetType { get; set; }

    /// <summary>
    /// The file to be uploaded
    /// </summary>
    [Required]
    public IFormFile File { get; set; } = null!;

    public override ArticleActionType ActionType => ArticleActionType.UploadAsset;
}

public class UploadManuscriptFileCommandValidator : ArticleCommandValidator<UploadManuscriptFileCommand>
{
    public UploadManuscriptFileCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNullWithMessage();

        // todo validate the file size and extension

        RuleFor(r => r.AssetType)
            .Must(IsAssetTypeAllowed)
            .WithMessage(x => $"{x.ActionType} not allowed ");
    }

    private bool IsAssetTypeAllowed(AssetType assetType) => AllowedAssetTypes.Contains(assetType);

    public IReadOnlyCollection<AssetType> AllowedAssetTypes = new HashSet<AssetType> { AssetType.Manuscript };
}