/*
   File: JwtTokenService.cs
   Description: This file contains the implementation of the JwtTokenService class, which is responsible for 				generating JWT tokens for user authentication.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/10
*/

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

        /// <summary>
        /// Generates a JWT for the given user ID and role.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="role">The role or permissions associated with the user.</param>
        /// <returns>Returns a JWT containing user claims and roles,
        /// signed with a secret key and with an expiration time set.</returns>
        public string GenerateToken(string id, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new[]
            {
            new Claim("role", role),
            new Claim("id", id),
            new Claim(ClaimTypes.Role, role),
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
