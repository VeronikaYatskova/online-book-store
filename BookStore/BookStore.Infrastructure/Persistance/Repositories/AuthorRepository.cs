using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistance.DataContext;

namespace BookStore.Infrastructure.Persistance.Repositories
{
    public class AuthorRepository : RepositoryBase<AuthorEntity>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<AuthorEntity>> GetAllAsync() =>
            await FindAllAsync();
    }
}
