using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IFavoriteBooksRepository
    {
        Task<IEnumerable<UserFavoriteBookEntity>> GetFavouriteBooksAsync(Guid userId);
        Task AddBookToFavoriteAsync(Guid userId, Guid bookId);
        void RemoveBookFromFavorite(Guid userId, Guid bookId);
    }
}
