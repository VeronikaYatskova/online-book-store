using BookStore.Domain.Entities;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetCategoriesAsync();
    }
}