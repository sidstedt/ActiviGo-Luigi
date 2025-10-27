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

            var firstName = user.FirstName ?? string.Empty;
            var lastName = user.LastName ?? string.Empty;
            var fullName = string.Join(" ", new[] { firstName, lastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(fullName) ? (user.UserName ?? user.Email ?? string.Empty) : fullName),
                new Claim(ClaimTypes.GivenName, firstName),
                new Claim(ClaimTypes.Surname, lastName)
            };

            if (roles?.Any() == true)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                claims.Add(new Claim("roles", string.Join(",", roles)));
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
