using Microsoft.IdentityModel.Tokens;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Security
{
    public class JwtGenerator : ITokenGenerator
    {
        private string JwtSecret { get; }

        public JwtGenerator(string secret)
        {
            JwtSecret = secret;
        }

        public string GenerateToken(int id, string username)
        {
            return GenerateToken(id, username, string.Empty);
        }

        public string GenerateToken(int id, string username, string role)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("sub", id.ToString()),
                new Claim("name", username),
            };

            if (!string.IsNullOrWhiteSpace(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecret)), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
