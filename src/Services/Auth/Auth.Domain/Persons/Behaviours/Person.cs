using Articles.GrpcContracts.Auth;
using Auth.Domain.Users;

namespace Auth.Domain.Persons;

public partial class Person
{
    public static Person Create(IPersonCreationInfo personInfo)
    {
        var person = new Person
        {
            Email = personInfo.Email,
            FirstName = personInfo.FirstName,
            LastName = personInfo.LastName,
            Gender = personInfo.Gender,
            PictureUrl = personInfo.PictureUrl,
            Honourific = HonourificTitle.FromEnum(personInfo.Honourific!),
            ProfessionalProfile = ProfessionalProfile.Create(personInfo.Position, personInfo.CompanyName, personInfo.Affiliation),
        };

        // todo create domain event

        return person;
    }

    public void AssignUser(User user)
    {
        this.UserId = user.Id;
        this.Email.NormalisedEmail = user.NormalizedEmail!;
    }
}
