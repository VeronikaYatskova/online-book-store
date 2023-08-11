using System.Linq.Expressions;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? expression = null)
        {
            return expression is null ? 
                await _dbSet.ToListAsync() :
                await _dbSet.Where(expression).ToListAsync();
        }

        public void LoadRelatedDataWithReference<M>(
            T? entity = null,
            Expression<Func<T, M?>>? expr = null) where M : class
        {
            _dbContext.Entry(entity)
                      .Reference(expr!)
                      .Load();
        }

        public void LoadRelatedDataWithCollection<M>(
            T? entity = null,
            Expression<Func<T, IEnumerable<M>>>? propertyExpression = null) where M : class
        {
            _dbContext.Entry(entity)
                      .Collection(propertyExpression!)
                      .Load();
        }

        public async Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression) =>
            await _dbSet.FirstOrDefaultAsync(expression);
        
        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task UpdateAsync(T entity)
        {
            T? searchValue = await _dbSet.FindAsync(entity);
            
            if (searchValue is not null)
            {
                _dbContext.Entry(searchValue).CurrentValues.SetValues(entity);
            }
        }
    }
}
