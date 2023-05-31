using System.Linq.Expressions;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> FindAllAsync(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
    }
}
