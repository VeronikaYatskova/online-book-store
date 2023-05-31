using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IAuthorsBooksRepository
    {
        Task<IEnumerable<BookAuthorEntity>> GetAllAsync();
    }
}
