using Application.Abstractions.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance.DataContext;

namespace Infrastructure.Persistance.Repositories
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
