using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistance.DataContext;

namespace BookStore.Infrastructure.Persistance.Repositories
{
    public class CategoryRepository : RepositoryBase<CategoryEntity>, ICategoryRepository
    {

        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategoriesAsync() =>
            await FindAllAsync();
    }
}
