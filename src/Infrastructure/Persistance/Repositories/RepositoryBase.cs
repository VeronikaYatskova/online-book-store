using System.Linq.Expressions;
using Application.Abstractions.Contracts.Interfaces;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext dbContext;

        public RepositoryBase(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> FindAllAsync() => await dbContext.Set<T>().ToListAsync();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            dbContext.Set<T>().Where(expression);
        
        public async Task<T> CreateAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);

            return entity;
        }

        public void Delete(T entity) => dbContext.Set<T>().Remove(entity);

        public async Task UpdateAsync(T entity)
        {
            T? searchValue = await dbContext.Set<T>().FindAsync(entity);
            
            if (searchValue is not null)
            {
                dbContext.Entry(searchValue).CurrentValues.SetValues(entity);
            }
        }
    }
}
