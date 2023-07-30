using System.Linq.Expressions;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Infrastructure.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet; 

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? expression = null) => 
            expression is null ? 
                await _dbSet.ToListAsync() : 
                await _dbSet.Where(expression).ToListAsync();

        public async Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression) =>
            await _dbSet.Where(expression).FirstOrDefaultAsync();

        public async Task CreateAsync(T entity) =>
            await _dbSet.AddAsync(entity);
        
        public void Update(T entity) =>
            _dbSet.Update(entity);

        public void Delete(T entity) =>
            _dbSet.Remove(entity);

        public async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync(); 
    }
}
