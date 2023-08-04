using Profiles.Domain.Entities;

namespace Profiles.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<User>> GetAuthorsAsync();
    }
}