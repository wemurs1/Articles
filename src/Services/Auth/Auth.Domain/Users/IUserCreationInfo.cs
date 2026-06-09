using Articules.GrpcContracts.Auth;

namespace Auth.Domain.Users;

public interface IUserCreationInfo : IPersonCreationInfo
{
    string? PhoneNumber { get; }
    IReadOnlyList<IUserRole> UserRoles { get; }
}

public interface IUserRole
{
    UserRoleType Type { get; }
    DateTime? StartDate { get; }
    DateTime? ExpiringDate { get; }
}
