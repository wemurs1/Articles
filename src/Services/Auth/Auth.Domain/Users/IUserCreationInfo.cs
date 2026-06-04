namespace Auth.Domain.Users;

public interface IUserCreationInfo
{
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    Gender Gender { get; }
    Honourific? Honourific { get; }
    string? PhoneNumber { get; }
    string? PictureUrl { get; }
    string? CompanyName { get; }
    string? Position { get; }
    string? Affiliation { get; }
    IReadOnlyList<IUserRole> UserRoles { get; }
}

public interface IUserRole
{
    UserRoleType Type { get; }
    DateTime? StartDate { get; }
    DateTime? ExpiringDate { get; }
}
