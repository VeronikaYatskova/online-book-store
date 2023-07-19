using System.Linq.Expressions;
using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        IEnumerable<Author>? FindAllAuthors();    
        Author? FindAuthorById(Guid authorId);
        Author? FindAuthorBy(Expression<Func<Author, bool>> expression);
        void AddAuthor(Author author);
        void UpdateAuthor(Author author);
        Task SaveAuthorChangesAsync();        
    }
}
