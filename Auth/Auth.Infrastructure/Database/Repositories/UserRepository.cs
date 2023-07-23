using System.Linq.Expressions;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Domain.Models;
using Auth.Infrastructure.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<User>?> FindAllUsersAsync() => await FindAll().ToListAsync();

        public async Task<User?> FindUserByIdAsync(Guid userGuid) =>
            await FindByCondition(u => u.Id == userGuid).FirstOrDefaultAsync();

        public async Task AddUserAsync(User user) => 
            await CreateAsync(user);
        
        public void UpdateUser(User user) =>
            Update(user);

        public async Task<User?> FindUserByAsync(Expression<Func<User, bool>> expression) =>
            await FindByCondition(expression).FirstOrDefaultAsync();

        public async Task SaveUserChangesAsync() =>
            await SaveChangesAsync();
    }
}
