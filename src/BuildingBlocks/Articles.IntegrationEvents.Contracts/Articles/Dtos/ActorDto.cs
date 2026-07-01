using Articles.Abstractions.Enums;

namespace Articles.IntegrationEvents.Contracts.Dtos;

public record class ActorDto(UserRoleType Role, HashSet<ContributionArea> ContributionAreas, PersonDto Person);
