namespace Articles.IntegrationEvents.Contracts.Dtos;

public record class PersonDto(int Id, string FirstName, string LastName, string Email,
    string? Honourific, string? Affiliation, int? UserId, string TypeDescriminator);