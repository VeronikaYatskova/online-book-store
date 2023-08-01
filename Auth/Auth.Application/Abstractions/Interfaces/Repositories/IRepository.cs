using System.Linq.Expressions;

namespace Auth.Application.Abstractions.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? expression = null);
        Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
