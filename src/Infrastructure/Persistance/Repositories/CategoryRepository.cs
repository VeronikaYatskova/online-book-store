using Application.Abstractions.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance.DataContext;

namespace Infrastructure.Persistance.Repositories
{
    public class CategoryRepository : RepositoryBase<CategoryEntity>, ICategoryRepository
    {

        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategoriesAsync() =>
            await FindAllAsync(false);
    }
}
