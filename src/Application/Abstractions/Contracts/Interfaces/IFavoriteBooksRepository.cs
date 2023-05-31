using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IFavoriteBooksRepository
    {
        Task<IEnumerable<UserFavoriteBookEntity>> GetFavouriteBooks(Guid userId);
        Task AddBookToFavorite(Guid userId, Guid bookId);
        void RemoveBookFromFavorite(Guid userId, Guid bookId);
    }
}
