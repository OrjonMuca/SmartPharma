using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using smartPharmaAPI.Models;

namespace smartPharmaAPI.Services;

public sealed class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime ExpiresAtUtc) CreateToken(AppUser user)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(GetExpiryMinutes());
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetJwtSetting("Key")));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: GetJwtSetting("Issuer"),
            audience: GetJwtSetting("Audience"),
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }

    private string GetJwtSetting(string key)
    {
        return _configuration[$"Jwt:{key}"] ?? throw new InvalidOperationException($"Missing Jwt:{key} configuration.");
    }

    private int GetExpiryMinutes()
    {
        return int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var minutes) ? minutes : 120;
    }
}
