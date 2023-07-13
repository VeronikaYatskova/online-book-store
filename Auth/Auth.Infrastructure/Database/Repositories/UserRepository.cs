using System.Linq.Expressions;
using Auth.Application.Abstractions.Repositories;
using Auth.Domain.Models;
using Auth.Infrastructure.Database.DataContext;

namespace Auth.Infrastructure.Database.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<User>? FindAllUsers() => FindAll();

        public User? FindUserById(Guid userGuid) =>
            FindByCondition(u => u.UserGuid == userGuid)?.FirstOrDefault();

        public void AddUser(User user) => 
            Create(user);
        
        public void UpdateUser(User user) =>
            Update(user);

        public User? FindUserBy(Expression<Func<User, bool>> expression) =>
            FindByCondition(expression)?.FirstOrDefault();

        public async Task SaveUserChangesAsync() =>
            await SaveChangesAsync();
    }
}
