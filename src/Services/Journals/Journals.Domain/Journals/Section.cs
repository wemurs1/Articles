using Blocks.Redis;
using Journals.Domain.Journals.Enums;
using Redis.OM.Modeling;

namespace Journals.Domain.Journals;

[Document(StorageType = StorageType.Json)]
public class Section : Entity
{
    [Indexed]
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<EditorRole> EditorRoles { get; set; } = [];
}
