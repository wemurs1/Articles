using System.Security.Claims;
using Auth.Application;
using Auth.Persistence.Repositories;
using Blocks.AspNetCore;
using Blocks.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Features.Users.Login;

[AllowAnonymous]
[HttpPost("login")]
public class LoginEndpoint(PersonRepository _personRepository, UserManager<User> _userManager,SignInManager<User> _signInManager, TokenFactory _tokenFactory)
    : Endpoint<LoginCommand, LoginResponse>
{
    public override async Task HandleAsync(LoginCommand command, CancellationToken ct = default)
    {
        var person = Guard.NotFound(await _personRepository.GetByEmailASync(command.Email));
        var user = Guard.NotFound(person.User);

        var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);
        if (!result.Succeeded) throw new BadRequestException($"Invalid credentials for this user {command.Email}");

        var userRoles = await _userManager.GetRolesAsync(user);

        var jwtToken = _tokenFactory.GenerateJwtToken(user.Id.ToString(), user.Person.FullName, command.Email, userRoles, Array.Empty<Claim>());
        var refreshToken = _tokenFactory.GenerateRefreshToken(HttpContext.GetClientIPAddress());
        user.AddRefreshToken(refreshToken);
        await Send.OkAsync(new LoginResponse(command.Email, jwtToken, refreshToken.Token), ct);
    }
}
