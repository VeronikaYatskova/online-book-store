using BookStore.Domain.Entities;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookEntity>> GetAllAsync();
        Task<BookEntity?> GetByIdAsync(Guid id);
        IEnumerable<BookEntity> GetByName(string name);
        IEnumerable<BookEntity> GetByPublisher(string publisher);
        IEnumerable<BookEntity> GetByISBN10(string isbn10);
        IEnumerable<BookEntity> GetByISBN13(string isbn10);
        Task AddBookAsync(BookEntity book);
        void DeleteBook(BookEntity book);
        Task UpdateBookAsync(BookEntity book);
    }
}
