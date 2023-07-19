using System.Linq.Expressions;
using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Repositories
{
    public interface IPublisherRepository
    {
        IEnumerable<Publisher>? FindAllPublishers();    
        Publisher? FindPublisherById(Guid publisherId);
        Publisher? FindPublisherBy(Expression<Func<Publisher, bool>> expression);
        void AddPublisher(Publisher publisher);
        void UpdatePublisher(Publisher upublisher);
        Task SavePublisherChangesAsync();        
    }
}
