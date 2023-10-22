using System.Linq.Expressions;
using Comments.DAL.Entities;

namespace Comments.DAL.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync(Expression<Func<Comment, bool>>? expression = null);
        Task<Comment?> GetByConditionAsync(Expression<Func<Comment, bool>> expression);
        Task AddAsync(Comment comment);
        Task DeleteAsync(string id);
        Task UpdateAsync(Comment comment);
    }
}
