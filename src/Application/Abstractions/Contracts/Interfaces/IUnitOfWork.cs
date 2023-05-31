
namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BooksRepository { get; }
        IPublisherRepository PublishersRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IAuthorsBooksRepository AuthorsBooksRepository { get; }
        IFavoriteBooksRepository FavoriteBooksRepository { get; }
        Task SaveChangesAsync();
    }
}
