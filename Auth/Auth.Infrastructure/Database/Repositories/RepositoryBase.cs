using System.Linq.Expressions;
using Auth.Application.Abstractions.Repositories;
using Auth.Infrastructure.Database.DataContext;

namespace Auth.Infrastructure.Database.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext dbContext;

        public RepositoryBase(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public IQueryable<T>? FindAll() => dbContext.Set<T>();

        public IQueryable<T>? FindByCondition(Expression<Func<T, bool>> expression) =>
            dbContext.Set<T>().Where(expression);

        public void Create(T entity) =>
            dbContext.Set<T>().Add(entity);
        
        public void Update(T entity) =>
            dbContext.Set<T>().Update(entity);

        public void Delete(T entity) =>
            dbContext.Set<T>().Remove(entity);

        public async Task SaveChangesAsync() 
            => await dbContext.SaveChangesAsync(); 
    }
}
