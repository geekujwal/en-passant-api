using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lilith.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private const string SecretKey = "your-256-bit-secretyour-256-bit-secretyour-256-bit-secretyour-256-bit-secret"; // You should use a secure key
    private const string Issuer = "example";
    private const string Audience = "example";
    public UserController()
    {
    }

    [HttpGet("login")]
     public string Get()
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "abc1212"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpGet("ping")]
    [Authorize(Roles = "Admin")]
    public StatusCodeResult Ping()
    {
        return new NoContentResult();
    }
}
