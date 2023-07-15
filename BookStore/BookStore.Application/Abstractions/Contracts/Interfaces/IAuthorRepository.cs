using BookStore.Domain.Entities;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorEntity>> GetAllAsync();
    }
}
