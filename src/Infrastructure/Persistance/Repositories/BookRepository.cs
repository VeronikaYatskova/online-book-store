using Application.Abstractions.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class BookRepository : RepositoryBase<BookEntity>, IBookRepository
    {
        public BookRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<BookEntity>> GetAllAsync() => 
            await FindAllAsync(trackChanges: false);
        
        public async Task<BookEntity?> GetByIdAsync(Guid id) => 
            await FindByCondition(b => b.BookGuid == id, false)!.FirstOrDefaultAsync();

        public IEnumerable<BookEntity> GetByName(string name) =>
            FindByCondition(b => b.BookName == name, false);
        
        public IEnumerable<BookEntity> GetByISBN10(string isbn10) =>
            FindByCondition(b => b.ISBN10 == isbn10, false);

        public IEnumerable<BookEntity> GetByISBN13(string isbn13) =>
            FindByCondition(b => b.ISBN13 == isbn13, false);

        public IEnumerable<BookEntity> GetByPublisher(string publisher) =>
            FindByCondition(b => b.Publisher.PublisherName == publisher, false);

        public async Task AddBookAsync(BookEntity book) => 
            await CreateAsync(book);

        public void DeleteBook(BookEntity book) => 
            Delete(book);
        
        public async Task UpdateBookAsync(BookEntity book) => 
            await UpdateAsync(book);
    }
}
