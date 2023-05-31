using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetCategoriesAsync();
    }
}