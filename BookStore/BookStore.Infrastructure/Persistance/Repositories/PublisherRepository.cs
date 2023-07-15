using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Persistance.Repositories
{
    public class PublisherRepository : RepositoryBase<PublisherEntity>, IPublisherRepository
    {
        public PublisherRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        
        public async Task<IEnumerable<PublisherEntity>> GetAllAsync() =>
            await FindAllAsync();

        public async Task<PublisherEntity?> GetByIdAsync(Guid id) =>
            await FindByCondition(b => b.PublisherGuid == id).FirstOrDefaultAsync();

        public async Task AddPublisherAsync(PublisherEntity publisher) =>
            await CreateAsync(publisher);
        
    }
}
