using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Domain.Models;
using Auth.Infrastructure.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database.Repositories
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext dbContext) : base(dbContext)
        {            
        }

        public async Task<UserRole?> GetUserRoleByIdAsync(Guid roleId)
        {
            return await FindByCondition(r => r.Id == roleId).FirstOrDefaultAsync();
        }

        public async Task<UserRole?> GetUserRoleByNameAsync(string name)
        {
            return await FindByCondition(r => r.Name == name).FirstOrDefaultAsync();;
        }
    }
}
