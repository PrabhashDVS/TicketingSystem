using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TicketingSystem
{
    public class JwtTokenService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _tokenExpiryMinutes;

        public JwtTokenService(string secretKey, string issuer, string audience, double tokenExpiryMinutes)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _tokenExpiryMinutes = tokenExpiryMinutes;
        }

        public string GenerateToken(string id, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new[]
            {
            new Claim("role", role),
            new Claim("id", id),
            // Add any additional claims as needed
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
