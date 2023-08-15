using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Domain.Exceptions;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _userRepository;
        private readonly IOptions<AppSettings> _appSetting;

        public TokenService (
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IRepository<User> userRepository,
            IOptions<AppSettings> appSetting)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _appSetting = appSetting;
        }

        public string CreateToken(User user)
        {
            var claims = new []
            {
                new Claim(TokenClaims.UserIdClaim, user.Id.ToString()),
                new Claim(TokenClaims.UserRoleClaim, user.Role.Name)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.Value.SecretKey));
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
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refresh-token"];

            if (!string.Equals(user.RefreshToken, refreshToken))
            {
                throw new InvalidTokenException(ExceptionMessages.InvalidRefreshTokenMessage);
            }
            else if (user.TokenExpires < DateTime.UtcNow)
            {
                throw new InvalidTokenException(ExceptionMessages.TokenExpiredMessage);
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
                var user = await _userRepository.FindByConditionAsync(u => u.Id == Guid.Parse(guid))! ??
                    throw new NotFoundException();

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

            await _userRepository.SaveChangesAsync();

            AppendRefreshTokenToCookies(refreshToken);
        }

        public string CreateVerificationToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private async Task<string?> GetInfoAsync()
        {
            var guid = await GetUserGuidAsync();

            return guid;
        }

        private async Task<string?> GetUserGuidAsync()
        {
            var token = await _httpContextAccessor?.HttpContext?.GetTokenAsync("access_token")!;

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

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh-token", refreshToken.Token, cookieOptions);
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
