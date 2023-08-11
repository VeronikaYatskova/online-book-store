using System.Linq.Expressions;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? expression = null);
        Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
        void LoadRelatedDataWithReference<M>(T? entity, Expression<Func<T, M?>>? expr) where M : class;
        void LoadRelatedDataWithCollection<M>(
            T? entity,
            Expression<Func<T, IEnumerable<M>>>? propertyExpression) where M : class;
    }
}
