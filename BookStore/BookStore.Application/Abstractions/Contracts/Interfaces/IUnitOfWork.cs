
using BookStore.Domain.Entities;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryBase<BookEntity> BooksRepository { get; }
        IRepositoryBase<CategoryEntity> CategoryRepository { get; }
        IRepositoryBase<BookAuthorEntity> AuthorsBooksRepository { get; }
        IRepositoryBase<UserBookEntity> UserBooksRepository { get; }
        IRepositoryBase<User> UsersRepository { get; }
        Task SaveChangesAsync();
    }
}
