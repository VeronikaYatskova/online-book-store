using Application.Abstractions.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance.DataContext;

namespace Infrastructure.Persistance.Repositories
{
    public class AuthorsBooksRepository : RepositoryBase<BookAuthorEntity>, IAuthorsBooksRepository
    {
        public AuthorsBooksRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<BookAuthorEntity>> GetAllAsync() =>
            await FindAllAsync(false);
    }
}