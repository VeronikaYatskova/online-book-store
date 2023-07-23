using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;
        private readonly string secretKey;

        public TokenService (
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            secretKey = config["AppSettings:SecretKey"];
        }

        public string CreateToken(User user)
        {
            var userId = user.Id;

            var claims = new []
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Role", user.Role.Name)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(45),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }

        public async Task<string> UpdateRefreshTokenAsync(User user)
        {
            var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refresh-token"];

            if (!object.Equals(user.RefreshToken, refreshToken))
            {
                throw new ArgumentException("Invalid Refresh Token");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                throw new ArgumentException("Token is expired.");
            }

            string token = CreateToken(user);
            await SetRefreshTokenAsync(user);

            return token;
        }

        public async Task<User?> GetUserAsync()
        {
            var guid = await GetInfoAsync();
            if (guid is not null)
            {
                var user = await userRepository.FindUserByIdAsync(Guid.Parse(guid));

                if (user is null)
                {
                    throw new ArgumentException("User is not found...");
                }

                return user;
            }

            return null;
        }

        public async Task SetRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created.ToUniversalTime();
            user.TokenExpires = refreshToken.Expired.ToUniversalTime();

            await userRepository.SaveUserChangesAsync();

            AppendRefreshTokenToCookies(refreshToken);
        }

        private async Task<string?> GetInfoAsync()
        {
            var guid = await GetUserGuidAsync();

            return guid;
        }

        private async Task<string?> GetUserGuidAsync()
        {
            var token = await httpContextAccessor?.HttpContext?.GetTokenAsync("access_token")!;

            if (token is not null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var userGuid = jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value;
                return userGuid;
            }

            return null;
        }
        
        private void AppendRefreshTokenToCookies(RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expired,
            };

            httpContextAccessor.HttpContext.Response.Cookies.Append("refresh-token", refreshToken.Token, cookieOptions);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired = DateTime.Now.AddDays(7),
            };

            return refreshToken;
        }
    }
}
