using ActiviGo.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ActiviGo.WebApi.Auth
{
    public interface IJwtTokenService
    {
        string CreateToken(User user, IEnumerable<string>? roles = null);
        string GenerateRefreshToken();
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user, IEnumerable<string>? roles = null)
        {
            var jwt = _configuration.GetSection("Jwt");

            var keyStr = jwt["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var issuer = jwt["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var audience = jwt["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
            var expireMinutes = double.TryParse(jwt["ExpireMinutes"], out var exp) ? exp : 60;

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Guid till string
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? "")
    };

            if (roles?.Any() == true)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
