using System.Linq.Expressions;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Domain.Models;
using Auth.Infrastructure.Database.DataContext;

namespace Auth.Infrastructure.Database.Repositories
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Author>? FindAllAuthors() => FindAll();
        
        public Author? FindAuthorBy(Expression<Func<Author, bool>> expression) =>
            FindByCondition(expression)?.FirstOrDefault();

        public Author? FindAuthorById(Guid authorId) =>
            FindByCondition(a => a.Id == authorId)?.FirstOrDefault();
        
        public void AddAuthor(Author author) => Create(author);

        public void UpdateAuthor(Author author) => Update(author);

        public async Task SaveAuthorChangesAsync() => await SaveChangesAsync();
    }
}
