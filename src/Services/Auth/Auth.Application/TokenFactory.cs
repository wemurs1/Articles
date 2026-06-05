using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Articles.Security;
using Blocks.Core;

namespace Auth.Application;

public class TokenFactory(IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions _jwtSettings = jwtOptions.Value;

    public RefreshToken GenerateRefreshToken(string clientIpAddress)
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rng.GetBytes(randomBytes);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow,
            CreatedByIp = clientIpAddress
        };
    }

    public string GenerateJwtToken(string userId, string fullName, string email, IEnumerable<string> roles, IEnumerable<Claim> additionalClaims)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),

            new Claim(ClaimTypes.NameIdentifier,userId),
            new Claim(ClaimTypes.Name, fullName),
            new Claim(ClaimTypes.Email, email)
        }
        .Concat(roles.Select(r => new Claim(ClaimTypes.Role, r)))
        .Concat(additionalClaims);

        var secretKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            notBefore: _jwtSettings.IssuedAt,
            expires: _jwtSettings.Expiration,
            claims: claims,
            signingCredentials: signingCredentials
        );

        var encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return encodedJwtToken;
    }

}
