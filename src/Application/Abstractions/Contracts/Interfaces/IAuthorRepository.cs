using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorEntity>> GetAllAsync();
    }
}
