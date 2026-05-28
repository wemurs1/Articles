using Blocks.Core.Extensions;

namespace Submission.Domain.ValueObjects;

public class FileExtensions
{
    public IReadOnlyList<string> Extensions { get; init; } = null!;

    public bool IsValidExtension(string extension)
        // Note: if the list is empty all extensions are allowed
        => Extensions.IsEmpty() || Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
}
