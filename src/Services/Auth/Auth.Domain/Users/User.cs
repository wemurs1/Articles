using Blocks.Domain.Entities;

namespace Auth.Domain.Users;

public partial class User : IdentityUser<int>, IEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public string FullName => FirstName + " " + LastName;

    public required Gender Gender { get; set; }
    public HonourificTitle? Honourific { get; set; }
    public ProfessionalProfile? ProfessionalProfile { get; set; }
    public string? PictureUrl { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    private List<UserRole> _userRoles = [];
    public virtual IReadOnlyList<UserRole> UserRoles => _userRoles;
    private List<RefreshToken> _refreshTokens = [];
    public virtual IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens;
}
