using Profiles.Domain.Entities;

namespace Profiles.Application.Interfaces.Repositories
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<User>> GetPublishersAsync();
    }
}
