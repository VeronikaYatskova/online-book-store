using MediatR;

namespace Application.Features.Book.Commands.AddBookToFavorite
{
    public record AddBookToFavoriteCommand(string userId, string bookId) : IRequest;
}