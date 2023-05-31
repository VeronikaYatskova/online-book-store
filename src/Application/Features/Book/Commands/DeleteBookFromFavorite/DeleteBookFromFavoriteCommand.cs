using MediatR;

namespace Application.Features.Book.Commands.DeleteBookFromFavorite
{
    public record DeleteBookFromFavoriteCommand(string userId, string bookId) : IRequest;
}