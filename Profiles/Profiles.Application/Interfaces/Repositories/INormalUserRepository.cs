using Profiles.Domain.Entities;

namespace Profiles.Application.Interfaces.Repositories
{
    public interface INormalUserRepository
    {
        Task<IEnumerable<User>> GetNormalUsersAsync();
    }
}