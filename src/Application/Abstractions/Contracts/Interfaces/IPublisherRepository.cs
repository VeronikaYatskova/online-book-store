using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<PublisherEntity>> GetAllAsync();
        Task<PublisherEntity?> GetByIdAsync(Guid id);
        Task AddPublisherAsync(PublisherEntity book);
    }
}