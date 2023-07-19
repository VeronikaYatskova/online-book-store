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
            return await FindByCondition(r => r.Id == roleId)?.FirstOrDefaultAsync();
        }

        public async Task<Guid> GetUserRoleIdByName(string name)
        {
            var userRole = await FindByCondition(r => r.Name == name).FirstOrDefaultAsync();
            return userRole.Id;
        }
    }
}
