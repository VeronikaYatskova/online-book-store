using Application.Abstractions.Contracts.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class FavoriteBooksRepository : RepositoryBase<UserFavoriteBookEntity>, IFavoriteBooksRepository
    {
        public FavoriteBooksRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<UserFavoriteBookEntity>> GetFavouriteBooks(Guid userId) => 
            await FindByCondition(fb => fb.UserId == userId).ToListAsync();

        public async Task AddBookToFavorite(Guid userId, Guid bookId)
        {
            var bookIsAdded = FindByCondition(fb => fb.UserId == userId && fb.BookId == bookId).FirstOrDefault(); 
            if (bookIsAdded is not null)
            {
                throw Exceptions.BookAlreadyExistsInFavorites;
            }

            var bookToAdd = new UserFavoriteBookEntity()
            {
                UserId = userId,
                BookId = bookId,
            };

            await CreateAsync(bookToAdd);
        }

        public void RemoveBookFromFavorite(Guid userId, Guid bookId)
        {
            var bookToDelete = FindByCondition(fb => fb.UserId == userId && fb.BookId == bookId).FirstOrDefault();

            if (bookToDelete is null)
            {
                throw Exceptions.BookNotFound;
            }

            Delete(bookToDelete);
        }
    }
}