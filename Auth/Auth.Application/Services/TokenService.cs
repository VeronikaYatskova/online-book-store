using Auth.Application.Abstractions.Services;
using Auth.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Application.Services
{
    public class TokenService : ITokenService
    {
        public string CreateToken(User user, string configToken)
        {
            var userId = user.UserGuid;
            var claims = new []
            {
                new Claim("UserId", userId.ToString()),
                new Claim("Role", user.Role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configToken));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }

        public Task SetRefreshToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateRefreshToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}