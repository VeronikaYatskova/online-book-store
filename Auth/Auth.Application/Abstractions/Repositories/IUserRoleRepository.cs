using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole> GetUserRoleByIdAsync(Guid roleId);        
    }
}