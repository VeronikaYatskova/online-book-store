using MediatR;

namespace BookStore.Application.Features.Book.Commands.DeleteBookFromFavorite
{
    public record DeleteBookFromFavoriteCommand(string UserId, string BookId) : IRequest;
}