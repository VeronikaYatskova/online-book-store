using System.Linq.Expressions;
using Requests.DAL.Models;

namespace Requests.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByConditionAsync(Expression<Func<User, bool>> expression);
        Task AddUserAsync(User user);
    }
}
