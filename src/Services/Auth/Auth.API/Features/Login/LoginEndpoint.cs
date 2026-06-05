using System.Security.Claims;
using Auth.Application;
using Blocks.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Features.Login;

[AllowAnonymous]
[HttpPost("login")]
public class LoginEndpoint(UserManager<User> _userManager, SignInManager<User> _signInManager, TokenFactory _tokenFactory)
    : Endpoint<LoginCommand, LoginResponse>
{
    public override async Task HandleAsync(LoginCommand command, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null) throw new BadRequestException($"User not found {command.Email}");

        var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);
        if (!result.Succeeded) throw new BadRequestException($"Invalid credentials for this user {command.Email}");

        var userRoles = await _userManager.GetRolesAsync(user);

        var jwtToken = _tokenFactory.GenerateJwtToken(user.Id.ToString(), user.FullName, command.Email, userRoles, Array.Empty<Claim>());
        var refreshToken = _tokenFactory.GenerateRefreshToken(HttpContext.GetClientIPAddress());
        user.AddRefreshToken(refreshToken);
        await Send.OkAsync(new LoginResponse(command.Email, jwtToken, refreshToken.Token), ct);
    }
}
