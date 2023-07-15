using BookStore.Domain.Entities;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IAuthorsBooksRepository
    {
        Task<IEnumerable<BookAuthorEntity>> GetAllAsync();
    }
}
