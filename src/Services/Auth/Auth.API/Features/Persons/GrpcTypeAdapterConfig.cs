using Auth.Domain.Persons;
using Auth.Grpc;
using Mapster;

namespace Auth.API.Features.Persons;

public class GrpcTypeAdapterConfig : TypeAdapterConfig
{
    public GrpcTypeAdapterConfig()
    {
        this.NewConfig<Person, PersonInfo>()
            .Map(dest => dest.Honourific, src => src.Honourific == null ? null : src.Honourific.Value)
            .IgnoreNullValues(true);
        this.NewConfig<Domain.Persons.ValueObjects.ProfessionalProfile, ProfessionalProfile>();
    }
}
