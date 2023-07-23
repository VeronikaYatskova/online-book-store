using System.Linq.Expressions;
using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>?> FindAllUsersAsync();    
        Task<User?> FindUserByIdAsync(Guid userId);
        Task<User?> FindUserByAsync(Expression<Func<User, bool>> expression);
        Task AddUserAsync(User user);
        void UpdateUser(User user);
        Task SaveUserChangesAsync();
    }
}
