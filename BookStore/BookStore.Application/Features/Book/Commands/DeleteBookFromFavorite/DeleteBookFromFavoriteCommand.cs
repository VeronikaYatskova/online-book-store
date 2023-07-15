using MediatR;

namespace BookStore.Application.Features.Book.Commands.DeleteBookFromFavorite
{
    public record DeleteBookFromFavoriteCommand(string userId, string bookId) : IRequest;
}