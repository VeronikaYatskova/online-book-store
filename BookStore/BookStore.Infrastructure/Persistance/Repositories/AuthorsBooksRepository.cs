using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistance.DataContext;

namespace BookStore.Infrastructure.Persistance.Repositories
{
    public class AuthorsBooksRepository : RepositoryBase<BookAuthorEntity>, IAuthorsBooksRepository
    {
        public AuthorsBooksRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<BookAuthorEntity>> GetAllAsync() =>
            await FindAllAsync();
    }
}