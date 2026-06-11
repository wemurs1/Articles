using Articles.Abstractions.Enums;

namespace Articles.GrpcContracts.Auth;

public interface IPersonCreationInfo
{
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    Gender Gender { get; }
    Honourific? Honourific { get; }
    string? PictureUrl { get; }
    string? CompanyName { get; }
    string? Position { get; }
    string? Affiliation { get; }
}
