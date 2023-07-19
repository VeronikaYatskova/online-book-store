using System.Linq.Expressions;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Domain.Models;
using Auth.Infrastructure.Database.DataContext;

namespace Auth.Infrastructure.Database.Repositories
{
    public class PublisherRepository : RepositoryBase<Publisher>, IPublisherRepository
    {
        public PublisherRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Publisher>? FindAllPublishers() => FindAll();

        public Publisher? FindPublisherBy(Expression<Func<Publisher, bool>> expression) =>
            FindByCondition(expression)?.FirstOrDefault();

        public Publisher? FindPublisherById(Guid publisherId) =>
            FindByCondition(p => p.Id == publisherId)?.FirstOrDefault();
        
        public void AddPublisher(Publisher publisher) => Create(publisher);

        public void UpdatePublisher(Publisher publisher) => Update(publisher);

        public async Task SavePublisherChangesAsync() => await SaveChangesAsync();
    }
}
