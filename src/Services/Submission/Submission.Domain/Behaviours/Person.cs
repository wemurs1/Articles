using Auth.Grpc;

namespace Submission.Domain.Entities;

public partial class Person
{
    public static Person Create(PersonInfo personInfo)
    {
        return new Person
        {
            Id = personInfo.Id,
            UserId = personInfo.Id,
            EmailAddress = EmailAddress.Create(personInfo.Email),
            FirstName = personInfo.FirstName,
            LastName = personInfo.LastName,
            Title = personInfo.Honourific,
            Affiliation = personInfo.ProfessionalProfile!.Affiliation
        };
    }
}
