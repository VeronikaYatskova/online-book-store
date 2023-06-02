using System.Linq.Expressions;
using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User>? FindAllUsers();    
        User? FindUserById(Guid userId);
        User? FindUserBy(Expression<Func<User, bool>> expression);
        void AddUser(User user);
        void UpdateUser(User user);
        Task SaveUserChangesAsync();
    }
}
