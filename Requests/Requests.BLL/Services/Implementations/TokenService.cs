using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Requests.BLL.Services.Interfaces;

namespace Requests.BLL.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenService> _logger;

        public TokenService(
            IHttpContextAccessor httpContextAccessor, ILogger<TokenService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> GetUserIdFromTokenAsync()
        {
            _logger.LogInformation("Got here!");

            var token =  await _httpContextAccessor?.HttpContext?.GetTokenAsync("access_token")!;
            
            if (token is null)
            {
                throw new UnauthorizedAccessException();
            }

            var userId = GetUserId(token);

            return userId;
        }

        private string GetUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userGuid = jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value;
            
            return userGuid;
        }
    }
}
