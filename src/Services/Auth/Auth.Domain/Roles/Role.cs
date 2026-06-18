using Blocks.Domain.Entities;

namespace Auth.Domain.Roles;

public class Role : IdentityRole<int>, IEntity<int>
{
    public required UserRoleType Type { get; set; }
    public required string Description { get; set; }
}
