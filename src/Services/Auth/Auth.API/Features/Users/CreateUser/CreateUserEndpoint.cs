using Articles.Abstractions.Enums;
using Auth.Domain.Events;
using Auth.Domain.Persons;
using Auth.Persistence;
using Auth.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Features.Users.CreateUser;

[Authorize(Roles = Role.USERADMIN)]
[HttpPost("users")]
public class CreateUserEndpoint(PersonRepository _personRepository, AuthDbContext _dbContext, UserManager<User> _userManager)
    : Endpoint<CreateUserCommand, CreateUserResponse>
{
    public override async Task HandleAsync(CreateUserCommand command, CancellationToken ct)
    {
        var person = await _personRepository.GetByEmailASync(command.Email);
        if (person?.User != null) throw new BadRequestException($"User with email {command.Email} already exists");

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

        person ??= await CreatePersonAsync(command, ct);

        var user = Domain.Users.User.Create(command);

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join(" | ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new BadRequestException($"Unable to create user: {errorMessages}");
        }

        person.AssignUser(user);

        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _personRepository.SaveChangesAsync();

        await transaction.CommitAsync(ct);

        await PublishAsync(new UserCreated(user, passwordResetToken), cancellation: ct);

        await Send.OkAsync(new CreateUserResponse(command.Email, user.Id, passwordResetToken), cancellation: ct);
    }

    private async Task<Person> CreatePersonAsync(CreateUserCommand command, CancellationToken ct)
    {
        Person person = Person.Create(command);
        await _personRepository.AddAsync(person);
        await _personRepository.SaveChangesAsync(ct);
        return person;
    }
}
