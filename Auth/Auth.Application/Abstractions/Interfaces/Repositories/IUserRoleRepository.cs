using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole> GetUserRoleByIdAsync(Guid roleId);        
    }
}