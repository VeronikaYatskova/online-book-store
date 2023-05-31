using Application.Abstractions.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class PublisherRepository : RepositoryBase<PublisherEntity>, IPublisherRepository
    {
        public PublisherRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        
        public async Task<IEnumerable<PublisherEntity>> GetAllAsync() =>
            await FindAllAsync(false);

        public async Task<PublisherEntity?> GetByIdAsync(Guid id) =>
            await FindByCondition(b => b.PublisherGuid == id, false).FirstOrDefaultAsync();

        public async Task AddPublisherAsync(PublisherEntity publisher) =>
            await CreateAsync(publisher);
        
    }
}
