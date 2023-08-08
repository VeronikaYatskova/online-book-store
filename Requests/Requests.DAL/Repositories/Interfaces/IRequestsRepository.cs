using System.Linq.Expressions;
using Requests.DAL.Models;

namespace Requests.DAL.Repositories.Interfaces
{
    public interface IRequestsRepository
    {
        Task<IEnumerable<Request>> GetAllAsync(Expression<Func<Request, bool>>? expression = null);
        Task<Request> GetByConditionAsync(Expression<Func<Request, bool>> expression);
        Task AddAsync(Request request);
        Task DeleteAsync(string id);
        Task UpdateAsync(Request request);
    }
}
