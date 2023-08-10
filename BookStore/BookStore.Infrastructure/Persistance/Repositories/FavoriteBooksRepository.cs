using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.Exceptions;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Persistance.Repositories
{
    public class FavoriteBooksRepository : RepositoryBase<UserFavoriteBookEntity>, IFavoriteBooksRepository
    {
        public FavoriteBooksRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<UserFavoriteBookEntity>> GetFavouriteBooksAsync(Guid userId) => 
            await FindByCondition(fb => fb.UserId == userId).ToListAsync();

        public async Task AddBookToFavoriteAsync(Guid userId, Guid bookId)
        {
            var bookIsAdded = FindByCondition(fb => fb.UserId == userId && fb.BookId == bookId).FirstOrDefault(); 
            if (bookIsAdded is not null)
            {
                throw new NotFoundException(ExceptionMessages.BookAlreadyExistsInFavorites);
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
                throw new NotFoundException(ExceptionMessages.BookNotFound);
            }

            Delete(bookToDelete);
        }
    }
}
