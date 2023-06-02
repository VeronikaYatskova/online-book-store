using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Services
{
    public interface ITokenService
    {
        string CreateToken(User user, string configToken);
        Task<string> UpdateRefreshToken(User user);
        Task SetRefreshToken(User user);
    }
}
